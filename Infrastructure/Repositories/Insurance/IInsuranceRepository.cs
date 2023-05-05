namespace Infrastructure.Repositories.Insurance;

public interface IInsuranceRepository
{
    public Task<bool> Exists(long id, CancellationToken cancellationToken = default);

    public Task<Domain.Models.Insurance?> Get(long id, CancellationToken cancellationToken = default);
    
    public Task<Domain.Models.Insurance> Add(Domain.Models.Insurance insurance,
        CancellationToken cancellationToken = default);
}