using System.Net;
using Application.AppointmentPay.Queries.GetNotCompleteInstallment;
using Application.Common.Response;
using AssertLibrary;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories.AppointmentPay;
using Infrastructure.Repositories.Patient;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BDDTest.Steps;

[Binding]
public sealed class GetNotCompleteInstallmentStep
{
    private IAppointmentPayRepository AppointmentPayRepository { get; set; }
    private IPatientRepository PatientRepository { get; set; }
    private DbContextOptions<AppDbContext> DbContextOptions { get; set; }

    private readonly ScenarioContext _scenarioContext;


    public GetNotCompleteInstallmentStep(ScenarioContext scenarioContext)
    {
        DbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "CustomersDataBase")
            .Options;
        _scenarioContext = scenarioContext;
    }

    [Given(@"the following patient exist in the database")]
    public void GivenTheFollowingPatientExistInTheDatabase(Table table)
    {
        using var context = new AppDbContext(DbContextOptions);
        PatientRepository = new PatientRepository(context);
        var patients = table.CreateSet<Patient>();

        foreach (Patient patient in patients)
        {
            PatientRepository.Add(new Patient()
            {
                Id = patient.Id,
                Name = patient.Name,
                Phone = patient.Phone,
                InsuranceId = patient.InsuranceId
            });
        }

        _scenarioContext.Add("patient", patients);
    }

    [Given(@"the following appointmentPay items  in database")]
    public void GivenTheFollowingAppointmentExistInTheDatabase(Table table)
    {
        using var context = new AppDbContext(DbContextOptions);
        this.AppointmentPayRepository = new AppointmentPayRepository(context);
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

    [When(@"I Send Request to GetNotCompleteInstallment with this input")]
    public async Task WhenSendCorrectRequest(Table table)
    {
        var input = table.CreateSet<GetNotCompleteInstallmentQuery>().First();

        await using var context = new AppDbContext(DbContextOptions);
        AppointmentPayRepository = new AppointmentPayRepository(context);
        PatientRepository = new PatientRepository(context);


        var queryHandler = new GetNotCompleteInstallmentQueryHandler(AppointmentPayRepository, PatientRepository);
        var response = await queryHandler.Handle(
            new GetNotCompleteInstallmentQuery()
                { PatientId = input.PatientId, IsPaid = input.IsPaid, Page = input.Page, PageSize = input.PageSize },
            CancellationToken.None);
        _scenarioContext.Add("GetResponse", response.Data);
        _scenarioContext.Add("input", input);
    }

    [Then(@"the GetNotCompleteInstallment data is correct")]
    public void ThenGetNotCompleteInstallmentDataIsCorrect()
    {
        var response = _scenarioContext.Get<GetNotCompleteInstallmentDto>("GetResponse");
        var appointmentPays = _scenarioContext.Get<List<AppointmentPay>>("appointmentPay");
        var patients = _scenarioContext.Get<List<Patient>>("patient");
        var input = _scenarioContext.Get<GetNotCompleteInstallmentQuery>("input");

        var resPatient = patients.FirstOrDefault(x => x.Id == input.PatientId);
        var resAppointments = appointmentPays.Where(x => x.PatientId == input.PatientId && x.IsPaid == input.IsPaid)
            .ToList();
        response.NotCompleteInstallmentCount.ShouldBe(1);
        Assert.IsEqual(response.GetUserDto.Id, resPatient!.Id);
        Assert.IsEqual(response.GetUserDto.Name, resPatient.Name);
        Assert.IsEqual(response.GetUserDto.Phone, resPatient.Phone);
        var responseAppointmentPayList = response.GetAppointmentLists.List.ToList();
        for (int i = 0; i < responseAppointmentPayList.Count(); i++)
        {
            Assert.IsEqual(responseAppointmentPayList[i].Id, resAppointments[i].Id);
            Assert.IsEqual(responseAppointmentPayList[i].IsPaid, resAppointments[i].IsPaid);
            Assert.IsEqual(responseAppointmentPayList[i].PaidTime, resAppointments[i].PaidTime);
            Assert.IsEqual(responseAppointmentPayList[i].Price, resAppointments[i].Price);
        }
    }

    [When(@"I Send Request to GetNotCompleteInstallment with this Not Correct input")]
    public async Task WhenISendNotCorrectRequest(Table table)
    {
        var input = table.CreateSet<GetNotCompleteInstallmentQuery>().First();

        await using var context = new AppDbContext(DbContextOptions);
        AppointmentPayRepository = new AppointmentPayRepository(context);
        PatientRepository = new PatientRepository(context);


        var queryHandler = new GetNotCompleteInstallmentQueryHandler(AppointmentPayRepository, PatientRepository);
        var response = await queryHandler.Handle(
            new GetNotCompleteInstallmentQuery()
                { PatientId = input.PatientId, IsPaid = input.IsPaid, Page = input.Page, PageSize = input.PageSize },
            CancellationToken.None);
        _scenarioContext.Add("GetResponse", response);
        _scenarioContext.Add("input", input);
    }
    
    [Then(@"the GetNotCompleteInstallment data is not correct")]
    public void ThenGetNotCompleteInstallmentDataIsNotCorrect()
    {
        var response = _scenarioContext.Get<StdResponse<GetNotCompleteInstallmentDto>>("GetResponse");

        Assert.IsEqual( response.Status,HttpStatusCode.NotFound);
        Assert.IsEqual(response.Message,"کاربری با این مشخصات موجود نیست.");
    }
}