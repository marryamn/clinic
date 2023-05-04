using Application.AppointmentPay.Queries.GetAppointmentList;
using Application.User.Queries.GetUser;
using Infrastructure.Common.Pagination;

namespace Application.AppointmentPay.Queries.GetNotCompleteInstallment;

public class GetNotCompleteInstallmentDto
{
    public  GetUserDto GetUserDto { get; set; }
    public int NotCompleteInstallmentCount { get; set; }
    public PaginationModel<GetAppointmentListDto> GetAppointmentLists { get; set; }
}