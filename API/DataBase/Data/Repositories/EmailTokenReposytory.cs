using System;
using System.Collections.Generic;
using System.Text;

using Core.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrustructure.Data.Repositories
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
