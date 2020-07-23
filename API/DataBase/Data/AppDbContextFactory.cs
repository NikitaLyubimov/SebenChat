using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

using Infrustructure.Shared;

namespace Infrustructure.Data
{
    public class AppDbContextFactory : DesignTimeDbContextFactory<AppDbContext>
    {
        protected override AppDbContext CreateNewInstance(DbContextOptions<AppDbContext> options)
        {
            return new AppDbContext(options);
        }
    }
}
