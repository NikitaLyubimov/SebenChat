using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



using Infrustructure.Identity;
using Core.Interfaces.Gateways.Reposytories;
using Core.Domain.Entities;
using Core.DTO.GatewayResponces.Repositories;
using Core.DTO;

namespace Infrustructure.Data.Repositories
{
    public class UserReposytory : BaseReposytory<User, AppDbContext>, IUserReposytory
    {
        private UserManager<AppUser> _uManager;

        public UserReposytory(UserManager<AppUser> manager, AppDbContext db) : base(db)
        {
            _uManager = manager;
        }

        public Task<bool> CheckPassword(User user, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ConfirmEmail(User user)
        {
            var appUser = await _uManager.FindByIdAsync(user.IdentityId);
            appUser.EmailConfirmed = true;
            var result = await _uManager.UpdateAsync(appUser);
            return result.Succeeded;
        }

        public async Task<CreateUserResponce> Create(string firstName, string secondName, string email, string userName, string password)
        {
            var appUser = new AppUser { Email = email, UserName = userName };
            var identityResult = await _uManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded)
                return new CreateUserResponce(appUser.Id, false, identityResult.Errors.Select(e => new Error(e.Code, e.Description)));
            var user = new User(firstName, secondName, userName, email, appUser.Id);
            
            _db.Users.Add(user);
            await _db.SaveChangesAsync();


            return new CreateUserResponce(appUser.Id, identityResult.Succeeded, identityResult.Succeeded ? null : identityResult.Errors.Select(e => new Error(e.Code, e.Description)));
        }

        public async Task<User> FindByName(string userName)
        {
            var appUser = await _uManager.FindByNameAsync(userName);
            var user = await _db.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(x => x.IdentityId == appUser.Id);
            return appUser == null ? null : user; 

        }

        public async Task<User> GetByIdentityId(string id)
        {
            var user = await _db.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(x => x.IdentityId == id);
            return user == null ? null : user;
        }
    }
}
