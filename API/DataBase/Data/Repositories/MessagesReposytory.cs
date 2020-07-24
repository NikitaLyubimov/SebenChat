using System;
using System.Collections.Generic;
using System.Text;

using Core.Domain.Entities;
using Core.Interfaces.Gateways.Reposytories;

namespace Infrustructure.Data.Repositories
{
    public class MessagesReposytory : BaseReposytory<Message, AppDbContext>, IMessageReposytory
    {
        public MessagesReposytory(AppDbContext db) : base(db) { }
    }
}
