using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Services
{
    interface IMailService
    {
        void SendMail(string Message, string InnerException, string StackTrace);
        void SendMail(string Message);
    }
}
