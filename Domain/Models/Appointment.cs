using Domain.Common;
using Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using DbContext = SoftDeletes.Core.DbContext;

namespace Domain.Models;

[EntityTypeConfiguration(typeof(AppointmentConfiguration))]

public class Appointment:ModelExtension
{
    public long DoctorId { get; set; }
    public long PatientId { get; set; }
    public Doctor Doctor { get; set; }
    public Patient Patient { get; set; }
    public bool IsPayComplete { get; set; }
    public long Price { get; set; }
    public bool HasInstallment { get; set; }
    public DateTime AppointmentTime { get; set; }
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