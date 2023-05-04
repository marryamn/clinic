using Application.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.AppointmentPay.Queries.GetNotCompleteInstallment;

public class GetNotCompleteInstallmentQuery:IRequest<StdResponse<GetNotCompleteInstallmentDto>>
{
    [FromQuery]public long PatientId { get; set; }
    [FromQuery]public bool IsPaid { get; set; }
    [FromQuery] public int? Page { get; set; }
    [FromQuery] public int? PageSize { get; set; }
}