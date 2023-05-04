using Application.AppointmentPay.Queries.GetAppointmentList;
using Application.Common;
using Application.Common.Response;
using Application.User.Queries.GetUser;
using Infrastructure.Common.Pagination;
using Infrastructure.Repositories.AppointmentPay;
using Infrastructure.Repositories.Patient;
using MediatR;

namespace Application.AppointmentPay.Queries.GetNotCompleteInstallment;

public class GetNotCompleteInstallmentQueryHandler:AbstractRequestHandler<GetNotCompleteInstallmentQuery, StdResponse<GetNotCompleteInstallmentDto>>
{
private IAppointmentPayRepository AppointmentPayRepository { get; }
private IPatientRepository PatientRepository { get; }

public GetNotCompleteInstallmentQueryHandler(IAppointmentPayRepository appointmentPayRepository,IPatientRepository patientRepository )
{
    AppointmentPayRepository=appointmentPayRepository;
    PatientRepository = patientRepository;
   
}


    public override async Task<StdResponse<GetNotCompleteInstallmentDto>> Handle(GetNotCompleteInstallmentQuery request,
        CancellationToken _)
    {
        var dd = await AppointmentPayRepository.GetAppointmentList(_);
        if (!await PatientRepository.Exists(request.PatientId, _))
        {
            return NotFoundMsg<GetNotCompleteInstallmentDto>("کاربری با این مشخصات موجود نیست.");
        }

        var notPaidAppointmentPays = await AppointmentPayRepository.GetPatientInstallment(PatientId: request.PatientId,
            IsPaid: request.IsPaid, request.Page,
            request.PageSize, _);


        var patient = await PatientRepository.Get(request.PatientId, _);

       
        return Ok(new GetNotCompleteInstallmentDto()
        {
            GetAppointmentLists = new PaginationModel<GetAppointmentListDto>()
            {
                PageCount = notPaidAppointmentPays.PageCount,
                CurrentPage = notPaidAppointmentPays.CurrentPage,
                CurrentPageSize = notPaidAppointmentPays.CurrentPageSize,
                Total = notPaidAppointmentPays.Total,
                List = notPaidAppointmentPays.List.Select(x => new GetAppointmentListDto() {
                    Id = x.Id,
                    Price = x.Price,
                    IsPaid = x.IsPaid,
                    PaidTime = x.PaidTime
                })
            },
            NotCompleteInstallmentCount = notPaidAppointmentPays.Total,
            GetUserDto = new GetUserDto()
            {
                Id = patient.Id,
                Name = patient.Name,
                Phone = patient.Phone
            }
        });
    }
}