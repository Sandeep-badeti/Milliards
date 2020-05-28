using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Services
{
    interface ILogService
    {
        public void LogException(string ErrorMessage);
        public void LogException(Exception ex);
    }
}
