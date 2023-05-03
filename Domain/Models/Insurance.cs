using Domain.Common;
using Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using DbContext = SoftDeletes.Core.DbContext;

namespace Domain.Models;

[EntityTypeConfiguration(typeof(InsuranceConfiguration))]

public class Insurance:ModelExtension
{
    public long PatientId { get; set; }
    public Patient Patient { get; set; }
    public string Name { get; set; }
    public List<AppointmentPay> AppointmentPays { get; set; }
    public override async Task OnSoftDeleteAsync(DbContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        await context.RemoveAsync(AppointmentPays, cancellationToken);
    }

    public override void OnSoftDelete(DbContext context)
    {
        context.RemoveAsync(AppointmentPays);

    }

    public override async Task LoadRelationsAsync(DbContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        await context.Entry(this)
            .Collection(x => x.AppointmentPays)
            .LoadAsync(cancellationToken);
    }

    public override void LoadRelations(DbContext context)
    {
        context.Entry(this)
            .Collection(x => x.AppointmentPays)
            .Load();
    }
}