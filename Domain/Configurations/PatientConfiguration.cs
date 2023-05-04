using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations;

public class PatientConfiguration: AbstractModelMap<Patient>
{
    public override void Configure(EntityTypeBuilder<Patient> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Insurance)
            .WithMany(x => x.Patients)
            .HasForeignKey(x => x.InsuranceId)
            ;
    }  
}