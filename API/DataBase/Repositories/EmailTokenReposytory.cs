using System;
using System.Collections.Generic;
using System.Text;

using DataBase.Entities;
using DataBase;

namespace DataBase.Repositories
{
    public class EmailTokenReposytory : BaseReposytory<EmailConfirmToken, AppDbContext>
    {
        public EmailTokenReposytory(AppDbContext db) : base(db) { }
    }
}
