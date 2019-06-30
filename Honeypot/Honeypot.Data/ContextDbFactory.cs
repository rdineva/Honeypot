using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Honeypot.Data
{
    public class ContextDbFactory : IDesignTimeDbContextFactory<HoneypotDbContext>
    {
        HoneypotDbContext IDesignTimeDbContextFactory<HoneypotDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HoneypotDbContext>();

            optionsBuilder.UseSqlServer(@"Server=DESKTOP-6P48I7L\SQLEXPRESS;
                                          Database=Honeypot;
                                          Trusted_Connection=True;
                                          MultipleActiveResultSets=true");

            return new HoneypotDbContext(optionsBuilder.Options);
        }
    }
}