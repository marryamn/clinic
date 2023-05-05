using System.Net;
using Application.AppointmentPay.Queries.GetInsurancePaidAppointmentPay;
using Application.Common.Response;
using AssertLibrary;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories.AppointmentPay;
using Infrastructure.Repositories.Insurance;
using Microsoft.EntityFrameworkCore;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BDDTest.Steps;

[Binding]
public sealed class GetInsurancePaidAppointmentPay
{
    private IAppointmentPayRepository AppointmentPayRepository { get; set; }
    private IInsuranceRepository InsuranceRepository { get; set; }
    private DbContextOptions<AppDbContext> DbContextOptions { get; set; }

    private readonly ScenarioContext _scenarioContext;


    public GetInsurancePaidAppointmentPay(ScenarioContext scenarioContext
        )
    {
        DbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "CustomersDataBase")
            .Options;
        _scenarioContext = scenarioContext;
    }

    [Given(@"the following insurance exist in the database")]
    public void GivenTheFollowingInsuranceExistInTheDatabase(Table table)
    {
        using var context = new AppDbContext(DbContextOptions);
        InsuranceRepository = new InsuranceRepository(context);
        var insurances = table.CreateSet<Insurance>();

        foreach (Insurance insurance in insurances)
        {
            InsuranceRepository.Add(new Insurance()
            {
                Id = insurance.Id,
                Name = insurance.Name,
            });
        }

        _scenarioContext.Add("insurance", insurances);
    }


    [Given(@"the following appointmentPay items  in database again")]
    public void GivenTheFollowingAppointmentExistInTheDatabase(Table table)
    {
        using var context = new AppDbContext(DbContextOptions);
        AppointmentPayRepository = new AppointmentPayRepository(context);
        var appointmentPays = table.CreateSet<AppointmentPay>();

        foreach (AppointmentPay appointmentPay in appointmentPays)
        {
            AppointmentPayRepository.Add(new AppointmentPay()
            {
                Id = appointmentPay.Id,
                PatientId = appointmentPay.PatientId,
                IsPaid = appointmentPay.IsPaid,
                PaidTime = appointmentPay.PaidTime,
                Price = appointmentPay.Price,
                InsuranceId = appointmentPay.InsuranceId
            });
        }

        _scenarioContext.Add("appointmentPay", appointmentPays);
    }


    [When(@"I Send Request to GetInsurancePaidAppointmentPay with this input")]
    public async Task GetInsurancePaidAppointmentPayWithCorrectInput(Table table)
    {
        var input = table.CreateSet<GetInsurancePaidAppointmentPayQuery>().First();

        await using var context = new AppDbContext(DbContextOptions);
        AppointmentPayRepository = new AppointmentPayRepository(context);
        InsuranceRepository = new InsuranceRepository(context);


        var queryHandler =
            new GetInsurancePaidAppointmentPayQueryHandler(InsuranceRepository, AppointmentPayRepository);
        var response = await queryHandler.Handle(
            new GetInsurancePaidAppointmentPayQuery()
                { InsuranceId = input.InsuranceId },
            CancellationToken.None);
        _scenarioContext.Add("GetResponse", response.Data);
        _scenarioContext.Add("input", input);
    }

    [Then(@"the GetInsurancePaidAppointmentPay data is correct")]
    public void ThenTheGetInsurancePaidAppointmentPayDataIsCorrect()
    {
        var response = _scenarioContext.Get<GetInsurancePaidAppointmentPayDto>("GetResponse");
        var appointmentPays = _scenarioContext.Get<List<AppointmentPay>>("appointmentPay");
        var insurances = _scenarioContext.Get<List<Insurance>>("insurance");
        var input = _scenarioContext.Get<GetInsurancePaidAppointmentPayQuery>("input");
        Console.WriteLine(appointmentPays);

        var resInsurance = insurances.FirstOrDefault(x => x.Id == input.InsuranceId);
        var resWholePaidPrice = appointmentPays.Where(x => x.InsuranceId == input.InsuranceId && x.IsPaid)
            .Sum(x => x.Price);

        Assert.IsEqual(response.GetInsuranceDto.Id, resInsurance!.Id);
        Assert.IsEqual(response.GetInsuranceDto.Name, resInsurance.Name);
        Assert.IsEqual(response.WholePaidPrice, resWholePaidPrice);
        Console.WriteLine();
    }


    [When(@"I Send Request to GetInsurancePaidAppointmentPay with this not Correct input")]
    public async Task GetInsurancePaidAppointmentPayWithNotCorrectInput(Table table)
    {
        var input = table.CreateSet<GetInsurancePaidAppointmentPayQuery>().First();

        await using var context = new AppDbContext(DbContextOptions);
        AppointmentPayRepository = new AppointmentPayRepository(context);
        InsuranceRepository = new InsuranceRepository(context);


        var queryHandler =
            new GetInsurancePaidAppointmentPayQueryHandler(InsuranceRepository, AppointmentPayRepository);
        var response = await queryHandler.Handle(
            new GetInsurancePaidAppointmentPayQuery()
                { InsuranceId = input.InsuranceId },
            CancellationToken.None);
        _scenarioContext.Add("GetResponse", response);
        _scenarioContext.Add("input", input);
    }


    [Then(@"the GetInsurancePaidAppointmentPay data is not correct")]
    public void ThenTheGetInsurancePaidAppointmentPayIsNotCorrect()
    {
        var response = _scenarioContext.Get<StdResponse<GetInsurancePaidAppointmentPayDto>>("GetResponse");

        Assert.IsEqual( response.Status,HttpStatusCode.NotFound);
        Assert.IsEqual(response.Message,"شرکت بیمه ای با این مشخصات موجود نیست.");
    }
}