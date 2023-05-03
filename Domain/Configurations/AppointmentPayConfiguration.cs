using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations;

public class AppointmentPayConfiguration: AbstractModelMap<AppointmentPay>
{
    public override void Configure(EntityTypeBuilder<AppointmentPay> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Appointment)
            .WithMany(x => x.AppointmentPays)
            .HasForeignKey(x => x.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Insurance)
            .WithMany(x => x.AppointmentPays)
            .HasForeignKey(x => x.InsuranceId);
        builder.HasOne(x => x.Patient)
            .WithMany(x => x.AppointmentPays)
            .HasForeignKey(x => x.PatientId);
            
    }  
}