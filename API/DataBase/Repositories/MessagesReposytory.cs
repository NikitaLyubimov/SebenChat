using System;
using System.Collections.Generic;
using System.Text;

using DataBase.Entities;
using DataBase;

namespace DataBase.Repositories
{
    public class MessagesReposytory : BaseReposytory<Message, AppDbContext>
    {
        public MessagesReposytory(AppDbContext db) : base(db) { }
    }
}
