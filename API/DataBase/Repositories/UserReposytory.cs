using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


using DataBase.DTO;
using DataBase.Interfaces;
using DataBase.Entities;
using DataBase.Identity;
using System.Linq;

namespace DataBase.Repositories
{
    public class UserReposytory : BaseReposytory<User, AppDbContext>, IUserReposytory
    {
        private UserManager<AppUser> _uManager;

        public UserReposytory(UserManager<AppUser> manager, AppDbContext db) : base(db)
        {
            _uManager = manager;
        }

        public async Task<CreateUserResponce> Create(string firstName, string secondName, string email, string userName, string password)
        {
            var appUser = new AppUser { Email = email, UserName = userName };
            var identityResult = await _uManager.CreateAsync(appUser, password);

            if (!identityResult.Succeeded)
                return new CreateUserResponce(appUser.Id,userName, false, identityResult.Errors.Select(err => new Error(err.Code, err.Description)));
            var user = new User(firstName, secondName, userName, email, appUser.Id);
            
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            

            return new CreateUserResponce(appUser.Id,userName, identityResult.Succeeded, identityResult.Succeeded ? null : identityResult.Errors.Select(err => new Error(err.Code, err.Description)));
        }

        public async Task<User> FindByName(string userName)
        {
            var appUser = await _uManager.FindByNameAsync(userName);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.IdentityId == appUser.Id);
            return appUser == null ? null : user; 

        }
    }
}
