using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Patient;

public class PatientRepository:IPatientRepository
{
    public PatientRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; set; }
    public async Task<bool> Exists(long id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Patients.AnyAsync(x => x.Id == id, cancellationToken );
    }

    public async Task<Domain.Models.Patient?> Get(long id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Patients.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public async Task<Domain.Models.Patient> Add(Domain.Models.Patient patient,
        CancellationToken cancellationToken = default)
    {
        await DbContext.AddAsync(patient, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return patient;
    }
}