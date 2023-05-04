using Infrastructure.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AppointmentPay;

public class AppointmentPayRepository:IAppointmentPayRepository
{
    public AppointmentPayRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; set; }
    public async Task<bool> Exists(long id, CancellationToken cancellationToken = default)
    {
        return await DbContext.AppointmentPays.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Domain.Models.AppointmentPay?> Get(long id, CancellationToken cancellationToken = default)
    {
        return await DbContext.AppointmentPays.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    

    public async Task<List<Domain.Models.AppointmentPay>> GetAppointmentList(
        CancellationToken cancellationToken = default)
    {
        return await DbContext.AppointmentPays
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }


    public async Task<PaginationModel<Domain.Models.AppointmentPay>> GetPatientInstallment(long patientId,bool isPaied, int? page,
        int? pageSize, CancellationToken cancellationToken = default)
    {
        return await DbContext.AppointmentPays
            .Where(x=>x.PatientId==patientId && x.IsPaid==isPaied)
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .UsePaginationAsync(page, pageSize, cancellationToken);
    }
}