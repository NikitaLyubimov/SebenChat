using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.EntityFrameworkCore;

using Core.Domain.Entities;
using Core.Interfaces.Gateways.Reposytories;


namespace Infrustructure.Data.Repositories
{
    public class EmailTokenReposytory : BaseReposytory<EmailConfirmToken, AppDbContext>, IEmailTokenReposytory
    {
        public EmailTokenReposytory(AppDbContext db) : base(db) { }

        public async Task<EmailConfirmToken> GetToken(string token)
        {
            var confToken = await _db.EmailConfirmTokens.FirstOrDefaultAsync(x => x.Token == token);
            return confToken == null ? null : confToken; 

        }


    }
}
