using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations;

public class InsuranceConfiguration: AbstractModelMap<Insurance>
{
    public override void Configure(EntityTypeBuilder<Insurance> builder)
    {
        base.Configure(builder);

        
    }  
}