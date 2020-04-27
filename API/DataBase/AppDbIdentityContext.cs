using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

using DataBase.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class AppDbIdentityContext : IdentityDbContext<AppUser>
    {
        public AppDbIdentityContext(DbContextOptions<AppDbIdentityContext> options) : base(options) { }
    }
}
