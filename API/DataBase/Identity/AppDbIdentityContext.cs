using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace Infrustructure.Identity
{
    public class AppDbIdentityContext : IdentityDbContext<AppUser>
    {
        public AppDbIdentityContext(DbContextOptions<AppDbIdentityContext> options) : base(options) { }
    }
}
