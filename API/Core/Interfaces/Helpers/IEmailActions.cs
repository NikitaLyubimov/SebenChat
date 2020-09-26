using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Helpers
{
    public interface IEmailActions
    {
        Task SendMessage(string receiverEmail,long userId);
    }
}
