namespace Application.AppointmentPay.Queries.GetAppointmentList;

public class GetAppointmentListDto
{
    public long Id { get; set; }
    public long Price { get; set; }
    public bool IsPaid { get; set; }
    public DateTime PaidTime { get; set; }
}