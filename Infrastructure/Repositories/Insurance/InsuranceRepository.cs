using Infrastructure.Repositories.Patient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Insurance;

public class InsuranceRepository:IInsuranceRepository
{
    public InsuranceRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; set; }
    public async Task<bool> Exists(long id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Insurances.AnyAsync(x => x.Id == id, cancellationToken );
    }

    public async Task<Domain.Models.Insurance?> Get(long id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Insurances.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}