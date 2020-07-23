using System;
using System.Collections.Generic;
using System.Text;

using Core.Domain.Entities;

namespace Infrustructure.Data.Repositories
{
    public class MessagesReposytory : BaseReposytory<Message, AppDbContext>
    {
        public MessagesReposytory(AppDbContext db) : base(db) { }
    }
}
