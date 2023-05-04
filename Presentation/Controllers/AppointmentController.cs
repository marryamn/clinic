using Application.AppointmentPay;
using Application.AppointmentPay.Queries.GetInsurancePaidAppointmentPay;
using Application.AppointmentPay.Queries.GetNotCompleteInstallment;
using Application.Common.Response;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;


public class AppointmentController:ControllerExtension
{
    public AppointmentController(IAppointmentPayService appointmentPayService)
    {
        AppointmentPayService = appointmentPayService;
    }

    private IAppointmentPayService AppointmentPayService { get; }

    [HttpGet("appointmentPay/notCompletedInstallment")]
    public async Task<ActionResult<StdResponse<GetNotCompleteInstallmentDto>>> GetNotCompletedInstallment(
        [FromQuery] GetNotCompleteInstallmentQuery query,
        CancellationToken _
    )
    {
        return FormatResponse(await AppointmentPayService.GetNotCompleteInstallment(query, _));
    }
    
    [HttpGet("appointmentPay/InsurancePaid")]
    public async Task<ActionResult<StdResponse<GetInsurancePaidAppointmentPayDto>>> GetInsurancePaidAppointmentPay(
        [FromQuery] GetInsurancePaidAppointmentPayQuery query,
        CancellationToken _
    )
    {
        return FormatResponse(await AppointmentPayService.GetInsurancePaidAppointmentPay(query, _));
    }
}