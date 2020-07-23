using System;
using System.Collections.Generic;
using System.Text;


using Infrustructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrustructure.Identity
{
    public class AppIdentityDbFactory : DesignTimeDbContextFactory<AppDbIdentityContext> 
    {
        protected override AppDbIdentityContext CreateNewInstance(DbContextOptions<AppDbIdentityContext> options)
        {
            return new AppDbIdentityContext(options);
        }
    }
}
