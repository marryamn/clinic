namespace Infrastructure.Repositories.Patient;

public interface IPatientRepository
{
    public Task<bool> Exists(long id, CancellationToken cancellationToken = default);

    public Task<Domain.Models.Patient?> Get(long id, CancellationToken cancellationToken = default);
    
    public Task<Domain.Models.Patient> Add(Domain.Models.Patient patient,
        CancellationToken cancellationToken = default);
}