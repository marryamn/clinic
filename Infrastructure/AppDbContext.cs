using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : SoftDeletes.Core.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {
        }
        
    }
}