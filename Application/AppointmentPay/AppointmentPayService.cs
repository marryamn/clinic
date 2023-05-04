using Application.AppointmentPay.Queries.GetInsurancePaidAppointmentPay;
using Application.AppointmentPay.Queries.GetNotCompleteInstallment;
using Application.Common.Response;
using MediatR;

namespace Application.AppointmentPay;

public class AppointmentPayService:IAppointmentPayService
{
    public AppointmentPayService(IMediator mediator)
    {
        Mediator = mediator;
    }

    private IMediator Mediator { get; }
    public async Task<StdResponse<GetNotCompleteInstallmentDto>> GetNotCompleteInstallment(GetNotCompleteInstallmentQuery query, CancellationToken _)
    {
        return await Mediator.Send(query, _);
    }

    public async Task<StdResponse<GetInsurancePaidAppointmentPayDto>> GetInsurancePaidAppointmentPay(GetInsurancePaidAppointmentPayQuery query, CancellationToken _)
    {
        return await Mediator.Send(query, _);
    }

   
}