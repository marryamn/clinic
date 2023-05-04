using System.Net;
using Application.AppointmentPay.Queries.GetInsurancePaidAppointmentPay;
using Domain.Models;
using Infrastructure.Repositories.AppointmentPay;
using Infrastructure.Repositories.Insurance;
using Moq;

namespace UnitTest.AppointmentPayTests;

public class GetInsurancePaidAppointmentPayTest
{
    [Fact]
    public async Task GetInsurancePaidAppointmentPayTest_Success()
    {
        var appointmentPays = new List<AppointmentPay>
        {
            new AppointmentPay
            {
                Id = 1, PatientId = 1, AppointmentId = 1, IsPaid = true, PaidTime = DateTime.Today, Price = 1000,
                InsuranceId = 1
            },
            new AppointmentPay
            {
                Id = 1, PatientId = 2, AppointmentId = 1, IsPaid = true, PaidTime = DateTime.Today, Price = 1000,
                InsuranceId = 1
            },
            new AppointmentPay
            {
                Id = 1, PatientId = 1, AppointmentId = 1, IsPaid = false, PaidTime = DateTime.Today, Price = 1000,
                InsuranceId = 1
            },
        };

        var insurances = new List<Insurance>
        {
            new Insurance() { Id = 1, Name = "Iran" },
            new Insurance { Id = 2, Name = "Mellat", },
        };
        var appointmentRepo = new Mock<IAppointmentPayRepository>();
        appointmentRepo
            .Setup(r => r.GetAppointmentList(default))
            .ReturnsAsync(appointmentPays);


        var insuranceRepo = new Mock<IInsuranceRepository>();
        insuranceRepo.Setup(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);


        insuranceRepo.Setup(
            x => x.Get(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync((long id, CancellationToken _) =>
            {
                return new Insurance()
                {
                    Id = insurances.FirstOrDefault(x => x.Id == id)!.Id,
                    Name = insurances.FirstOrDefault(x => x.Id == id)!.Name,
                };
            }
        );
        var query = new GetInsurancePaidAppointmentPayQuery()
        {
            InsuranceId = insurances.First().Id
        };


        var handler = new GetInsurancePaidAppointmentPayQueryHandler(insuranceRepo.Object, appointmentRepo.Object);

        var res = await handler.Handle(query, CancellationToken.None);

        Assert.Equal("Success", res.Message);
        Assert.Equal(HttpStatusCode.OK, res.Status);
        var resData = res.DataAsDataStruct();
        var resInsurance = resData.GetInsuranceDto;
        var oldInsurance = insurances.FirstOrDefault(x => x.Id == query.InsuranceId);
        var appointmentPriceValue =
            appointmentPays.Where(x => x.InsuranceId == query.InsuranceId && x.IsPaid).Sum(x => x.Price);
        Assert.Equal(resInsurance.Id, oldInsurance.Id);
        Assert.Equal(resInsurance.Name, oldInsurance.Name);
        Assert.Equal(appointmentPriceValue, resData.WholePaidPrice);


        insuranceRepo.Verify(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        insuranceRepo.Verify(
            x => x.Get(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task GetInsurancePaidAppointmentPayTest_Notfound()
    {
        var appointmentRepo = new Mock<IAppointmentPayRepository>();

        var insuranceRepo = new Mock<IInsuranceRepository>();


        var query = new GetInsurancePaidAppointmentPayQuery()
        {
            InsuranceId = 3
        };


        var handler = new GetInsurancePaidAppointmentPayQueryHandler(insuranceRepo.Object, appointmentRepo.Object);

        var res = await handler.Handle(query, CancellationToken.None);

        Assert.Equal("شرکت بیمه ای با این مشخصات موجود نیست.", res.Message);
        Assert.Equal(HttpStatusCode.NotFound, res.Status);
        Assert.Null(res.Data);

        insuranceRepo.Verify(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}