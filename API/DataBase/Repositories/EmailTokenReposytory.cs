using System;
using System.Collections.Generic;
using System.Text;

using DataBase.Entities;
using DataBase;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories
{
    public class EmailTokenReposytory : BaseReposytory<EmailConfirmToken, AppDbContext>
    {
        public EmailTokenReposytory(AppDbContext db) : base(db) { }

        public async Task<EmailConfirmToken> GetToken(string token)
        {
            var confToken = await _db.EmailConfirmTokens.FirstOrDefaultAsync(x => x.Token == token);
            return confToken == null ? null : confToken; 

        }


    }
}
