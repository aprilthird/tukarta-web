using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace TUKARTA.PE.DATA.Context
{
    public class TuKartaDbContextFactory : IDesignTimeDbContextFactory<TuKartaDbContext>
    {
        public TuKartaDbContextFactory()
        {
        }

        public TuKartaDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TuKartaDbContext>();

            builder.UseSqlServer(
                //DataConnectionString
                "Server=localhost;Database=TuKarta.DB;Trusted_Connection=True;MultipleActiveResultSets=true",
                //"Server=tukarta.database.windows.net;Database=TuKarta.DB;User ID=tukarta;Password=Admin.123;MultipleActiveResultSets=true;",
                opts => 
                {
                    opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                    opts.UseNetTopologySuite();
                });

            return new TuKartaDbContext(builder.Options);
        }
    }
}
