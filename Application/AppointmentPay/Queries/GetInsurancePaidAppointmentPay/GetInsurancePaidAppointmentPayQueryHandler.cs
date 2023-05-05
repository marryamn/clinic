using Application.AppointmentPay.Insurance.Queries.GetInsurance;
using Application.Common;
using Application.Common.Response;
using Infrastructure.Repositories.AppointmentPay;
using Infrastructure.Repositories.Insurance;
using Infrastructure.Repositories.Patient;
using MediatR;

namespace Application.AppointmentPay.Queries.GetInsurancePaidAppointmentPay;

public class GetInsurancePaidAppointmentPayQueryHandler: AbstractRequestHandler<GetInsurancePaidAppointmentPayQuery,
    StdResponse<GetInsurancePaidAppointmentPayDto>>
{
    private IAppointmentPayRepository AppointmentPayRepository { get; }
    private IInsuranceRepository InsuranceRepository { get; }
    

    public GetInsurancePaidAppointmentPayQueryHandler(IInsuranceRepository insuranceRepository,
        IAppointmentPayRepository appointmentPayRepository)
    {
        InsuranceRepository = insuranceRepository;
        AppointmentPayRepository = appointmentPayRepository;
        
    }


    public override async Task<StdResponse<GetInsurancePaidAppointmentPayDto>> Handle(GetInsurancePaidAppointmentPayQuery request, CancellationToken _)
    {

        if (!await InsuranceRepository.Exists(request.InsuranceId, _))
        {
            return NotFoundMsg<GetInsurancePaidAppointmentPayDto>("شرکت بیمه ای با این مشخصات موجود نیست.");
        }

        var insurance = await InsuranceRepository.Get(request.InsuranceId, _);
        var appointmentPays = await AppointmentPayRepository.GetAppointmentList(_);
        var price = appointmentPays.Where(x=>x.InsuranceId==request.InsuranceId && x.IsPaid).Sum(x => x.Price);

        return Ok(new GetInsurancePaidAppointmentPayDto()
        {
            GetInsuranceDto = new GetInsuranceDto()
            {
                Id = insurance.Id,
                Name = insurance.Name
            },
            WholePaidPrice = price
        });

    }
}