using Application.AppointmentPay.Queries.GetInsurancePaidAppointmentPay;
using Application.AppointmentPay.Queries.GetNotCompleteInstallment;
using Application.Common.Response;

namespace Application.AppointmentPay;

public interface IAppointmentPayService
{
    public Task<StdResponse<GetNotCompleteInstallmentDto>> GetNotCompleteInstallment(
        GetNotCompleteInstallmentQuery query, CancellationToken _
    );

    public Task<StdResponse<GetInsurancePaidAppointmentPayDto>> GetInsurancePaidAppointmentPay(
        GetInsurancePaidAppointmentPayQuery query, CancellationToken _
    );
}