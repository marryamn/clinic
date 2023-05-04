using Application.AppointmentPay.Insurance.Queries.GetInsurance;

namespace Application.AppointmentPay.Queries.GetInsurancePaidAppointmentPay;

public class GetInsurancePaidAppointmentPayDto
{
    public GetInsuranceDto GetInsuranceDto { get; set; }
    public long WholePaidPrice { get; set; }
}