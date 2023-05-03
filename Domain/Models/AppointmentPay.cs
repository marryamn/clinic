using Domain.Common;
using Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using DbContext = SoftDeletes.Core.DbContext;

namespace Domain.Models;

[EntityTypeConfiguration(typeof(AppointmentPayConfiguration))]

public class AppointmentPay:ModelExtension
{
    public long AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
    public long PatientId { get; set; }
    public Patient Patient { get; set; }
    public long Price { get; set; }
    public bool IsPaid { get; set; }
    public DateTime PaidTime { get; set; }
    public long? InsuranceId { get; set; }
    public Insurance Insurance { get; set; }
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