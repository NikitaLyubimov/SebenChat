
using Core.Interfaces.Gateways.Reposytories;
using Core.Domain.Entities;
using System.Threading.Tasks;

namespace Infrustructure.Data.Repositories
{
    public class PublickKeyReposytory : BaseReposytory<PublicKey, AppDbContext>, IPublickKeyReposytory
    {
        public PublickKeyReposytory(AppDbContext db) : base(db) { }

        public async Task<bool> Create(long userId, string keyValue)
        {
            try
            {
                var newPublickKey = new PublicKey(userId, keyValue);
                _db.Set<PublicKey>().Add(newPublickKey);
                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
