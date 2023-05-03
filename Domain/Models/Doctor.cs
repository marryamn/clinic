using Domain.Common;
using Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using DbContext = SoftDeletes.Core.DbContext;

namespace Domain.Models;

[EntityTypeConfiguration(typeof(DoctorConfiguration))]

public class Doctor:ModelExtension
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Degree { get; set; }
    public bool IsExpert { get; set; }
    public List<Appointment> Appointments { get; set; }

    public override async Task OnSoftDeleteAsync(DbContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        await context.RemoveAsync(Appointments, cancellationToken);
    }

    public override void OnSoftDelete(DbContext context)
    {
        context.RemoveAsync(Appointments);
    }

    public override async Task LoadRelationsAsync(DbContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        await context.Entry(this)
            .Collection(x => x.Appointments)
            .LoadAsync(cancellationToken);    }

    public override void LoadRelations(DbContext context)
    {
        context.Entry(this)
            .Collection(x => x.Appointments)
            .Load();
    }
}