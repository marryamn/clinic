using Infrastructure.Common.Pagination;

namespace Infrastructure.Repositories.AppointmentPay;

public interface IAppointmentPayRepository
{
   

    public Task<bool> Exists(long id, CancellationToken cancellationToken = default);

    public Task<Domain.Models.AppointmentPay?> Get(long id, CancellationToken cancellationToken = default);

    public Task<PaginationModel<Domain.Models.AppointmentPay>> GetPatientInstallment(long PatientId,bool IsPaid, int? page,
        int? pageSize,
        CancellationToken cancellationToken = default);
    
    public Task<List<Domain.Models.AppointmentPay>> GetAppointmentList(CancellationToken cancellationToken = default);

    public Task<Domain.Models.AppointmentPay> Add(Domain.Models.AppointmentPay appointmentPay,
        CancellationToken cancellationToken = default);

   

}