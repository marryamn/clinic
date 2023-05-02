using Mc2.CrudTest.Domain.Server.Configurations;
using Microsoft.EntityFrameworkCore;
using DbContext = SoftDeletes.Core.DbContext;

namespace Mc2.CrudTest.Domain.Server.Models;
[EntityTypeConfiguration(typeof(CustomerConfiguration))]

public class Customer :SoftDeletes.ModelTools.ModelExtension
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? BankAccountNumber { get; set; }
    public override Task OnSoftDeleteAsync(DbContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.CompletedTask;
    }

    public override void OnSoftDelete(DbContext context)
    {
    }

    public override Task LoadRelationsAsync(DbContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.CompletedTask;
    }

    public override void LoadRelations(DbContext context)
    {
    }
}