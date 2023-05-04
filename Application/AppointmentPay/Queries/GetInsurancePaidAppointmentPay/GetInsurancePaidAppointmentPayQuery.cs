using Application.Common.Response;
using MediatR;

namespace Application.AppointmentPay.Queries.GetInsurancePaidAppointmentPay;

public class GetInsurancePaidAppointmentPayQuery:IRequest<StdResponse<GetInsurancePaidAppointmentPayDto>>
{
    public long InsuranceId { get; set; }
}