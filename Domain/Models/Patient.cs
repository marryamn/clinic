using Domain.Common;
using Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using DbContext = SoftDeletes.Core.DbContext;

namespace Domain.Models;

[EntityTypeConfiguration(typeof(PatientConfiguration))]

public class Patient:ModelExtension
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public List<Appointment> Appointments { get; set; }
    public List<Insurance> Insurances { get; set; }
    public List<AppointmentPay> AppointmentPays { get; set; }

    public override async Task OnSoftDeleteAsync(DbContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        await context.RemoveAsync(Appointments, cancellationToken);
        await context.RemoveAsync(Insurances, cancellationToken);
        await context.RemoveAsync(AppointmentPays, cancellationToken);
    }

    public override void OnSoftDelete(DbContext context)
    {
        context.RemoveAsync(Appointments);
        context.RemoveAsync(Insurances);
        context.RemoveAsync(AppointmentPays);
    }

    public override async Task LoadRelationsAsync(DbContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        await context.Entry(this)
            .Collection(x => x.Appointments)
            .LoadAsync(cancellationToken);
        await context.Entry(this)
            .Collection(x => x.Insurances)
            .LoadAsync(cancellationToken);
        await context.Entry(this)
            .Collection(x => x.AppointmentPays)
            .LoadAsync(cancellationToken);
    }

    public override void LoadRelations(DbContext context)
    {
        context.Entry(this)
            .Collection(x => x.Appointments)
            .Load();
        context.Entry(this)
            .Collection(x => x.Insurances)
            .Load();
        context.Entry(this)
            .Collection(x => x.AppointmentPays)
            .Load();
    }
}