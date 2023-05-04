using System.Net;
using Application.AppointmentPay.Queries.GetNotCompleteInstallment;
using Domain.Models;
using Infrastructure.Common.Pagination;
using Infrastructure.Repositories.AppointmentPay;
using Infrastructure.Repositories.Patient;
using Moq;

namespace UnitTest.AppointmentPayTests;

public class GetNotCompleteInstallmentTest
{
    [Fact]
    public async Task GetNotCompleteInstallmentTest_Success()
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

        var patients = new List<Patient>
        {
            new Patient() { Id = 1, Name = "maryam amani", Phone = "954548785" },
            new Patient() { Id = 2, Name = "new User", Phone = "9545255785" },
        };
        var appointmentRepo = new Mock<IAppointmentPayRepository>();
        appointmentRepo
            .Setup(r => r.GetAppointmentList(default))
            .ReturnsAsync(appointmentPays);


        var patientRepo = new Mock<IPatientRepository>();
        patientRepo.Setup(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);


        patientRepo.Setup(
            x => x.Get(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync((long id, CancellationToken _) =>
            {
                return new Patient()
                {
                    Id = patients.FirstOrDefault(x => x.Id == id)!.Id,
                    Name = patients.FirstOrDefault(x => x.Id == id)!.Name,
                    Phone = patients.FirstOrDefault(x => x.Id == id)!.Phone,
                    InsuranceId = patients.FirstOrDefault(x => x.Id == id)!.InsuranceId
                };
            }
        );


        var query = new GetNotCompleteInstallmentQuery()
        {
            PatientId = 1,
            IsPaid = false,
            Page = 1,
            PageSize = 10
        };

        appointmentRepo.Setup(
            x => x.GetPatientInstallment(
                query.PatientId, false, It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<
                    CancellationToken>())
        ).ReturnsAsync((long _, bool _, int? page, int? pageSize, CancellationToken _) =>
            {
                return new PaginationModel<AppointmentPay>
                {
                    PageCount = (int)Math.Ceiling(appointmentPays.Count / (double)pageSize!),
                    CurrentPage = page!.Value,
                    CurrentPageSize = pageSize.Value,
                    Total = appointmentPays.Count(x => x.PatientId == query.PatientId && x.IsPaid == query.IsPaid),
                    List = appointmentPays.Where(x => x.PatientId == query.PatientId && x.IsPaid == query.IsPaid)
                        .Skip((page.Value - 1) * pageSize.Value)
                        .Take(pageSize.Value)
                };
            }
        );


        var handler = new GetNotCompleteInstallmentQueryHandler(appointmentRepo.Object, patientRepo.Object);

        var res = await handler.Handle(query, CancellationToken.None);

        Assert.Equal("Success", res.Message);
        Assert.Equal(HttpStatusCode.OK, res.Status);
        var resData = res.DataAsDataStruct();
        Assert.Equal(resData.NotCompleteInstallmentCount,
            appointmentPays.Count(x => x.PatientId == query.PatientId && x.IsPaid == query.IsPaid));
        var resPatient = resData.GetUserDto;
        var oldPatient = patients.FirstOrDefault(x => x.Id == query.PatientId);
        if (oldPatient != null) Assert.Equal(resPatient.Id, oldPatient.Id);
        if (oldPatient != null) Assert.Equal(resPatient.Name, oldPatient.Name);
        if (oldPatient != null) Assert.Equal(resPatient.Phone, oldPatient.Phone);
        var resAppointment = resData.GetAppointmentLists.List.ToList();
        var oldAppointment = appointmentPays.Where(x => x.PatientId == query.PatientId && x.IsPaid == query.IsPaid)
            .ToList();
        for (var i = 0; i < resAppointment.Count(); i++)
        {
            Assert.Equal(resAppointment[i].Id, oldAppointment[i].Id);
            Assert.Equal(resAppointment[i].IsPaid, oldAppointment[i].IsPaid);
            Assert.Equal(resAppointment[i].PaidTime, oldAppointment[i].PaidTime);
            Assert.Equal(resAppointment[i].Price, oldAppointment[i].Price);
        }
    }

    [Fact]
    public async Task GetNotCompleteInstallmentTest_Notfound()
    {
        var appointmentRepo = new Mock<IAppointmentPayRepository>();

        var patientRepo = new Mock<IPatientRepository>();

        var query = new GetNotCompleteInstallmentQuery()
        {
            PatientId = 3,
            IsPaid = false,
            Page = 1,
            PageSize = 10
        };


        var handler = new GetNotCompleteInstallmentQueryHandler(appointmentRepo.Object, patientRepo.Object);

        var res = await handler.Handle(query, CancellationToken.None);

        Assert.Equal("کاربری با این مشخصات موجود نیست.", res.Message);
        Assert.Equal(HttpStatusCode.NotFound, res.Status);
        Assert.Null(res.Data);

        patientRepo.Verify(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}