using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Services
{
    public class LogService : ILogService
    {
        private IConfiguration _iconfiguration;
        public LogService(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        public void LogException(string ErrorMessage)
        {
            List<string> errList = new List<string>();
            errList.Add(_iconfiguration["BLANKLINE"]);
            errList.Add(DateTime.Now.ToString(_iconfiguration["DATETIMEFORMAT_MM/DD/YYYY"]));
            errList.Add(_iconfiguration["BLANKLINE"]);
            errList.Add(ErrorMessage);
            if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + _iconfiguration["ErrorFilePath"]))
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\" + _iconfiguration["ErrorFilePath"]);
            System.IO.File.AppendAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + _iconfiguration["ErrorFilePath"] + "\\" + _iconfiguration["ERRORLOGFILENAME"], errList);

        }
        public void LogException(Exception ex)
        {
            List<string> errList = new List<string>();
            errList.Add(_iconfiguration["BLANKLINE"]);
            errList.Add(DateTime.Now.ToString(_iconfiguration["DATETIMEFORMAT_MM/DD/YYYY"]));
            errList.Add(_iconfiguration["BLANKLINE"]);
            errList.Add(ex != null && ex.InnerException != null ? (ex.InnerException.Message ?? (ex != null ? ex.Message : "")) : (ex != null ? ex.Message : ""));
            if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + _iconfiguration["ErrorFilePath"]))
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\" + _iconfiguration["ErrorFilePath"]);
            System.IO.File.AppendAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + _iconfiguration["ErrorFilePath"] + "\\" + _iconfiguration["ERRORLOGFILENAME"], errList);
        }
    }
}
