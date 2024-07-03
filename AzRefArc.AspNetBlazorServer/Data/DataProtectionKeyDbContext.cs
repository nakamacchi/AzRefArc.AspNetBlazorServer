using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AzRefArc.AspNetBlazorServer.Data
{
    public class DataProtectionKeyDbContext : DbContext, IDataProtectionKeyContext
    {
        public DataProtectionKeyDbContext(DbContextOptions<DataProtectionKeyDbContext> options) : base(options)
        {
        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
    }
}
