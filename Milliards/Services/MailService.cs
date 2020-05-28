using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Milliards.Services
{
    public class MailService : IMailService
    {
        private IConfiguration _iconfiguration;
        public MailService(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        public void SendMail(string Message, string InnerException, string StackTrace)
        {
            string MailMessage = string.Empty;
            MailMessage += (!string.IsNullOrEmpty(Message)) ? _iconfiguration["MESSAGE"] + Message + _iconfiguration["DOUBLE_BR"] : "";
            MailMessage += (!string.IsNullOrEmpty(InnerException)) ? _iconfiguration["INNEREXCEPTION"] + InnerException + _iconfiguration["DOUBLE_BR"] : "";
            MailMessage += (!string.IsNullOrEmpty(StackTrace)) ? _iconfiguration["STACKTRACE"] + StackTrace + _iconfiguration["DOUBLE_BR"] : "";
            SendMail(MailMessage);
        }
        public void SendMail(string Message)
        {
            string SenderMailId = string.Empty,
                RecipientMailId = string.Empty,
                MailSubject = string.Empty,
                Host = string.Empty,
                PortNo = string.Empty,
                SenderMailPWD = string.Empty;

            AuthService AuthServiceObj = new AuthService(_iconfiguration);
            SenderMailId = AuthServiceObj.Decrypt(_iconfiguration["senderMailId"]);
            RecipientMailId = _iconfiguration["recipientMailId"];
            MailSubject = _iconfiguration["mailSubject"];
            Host = _iconfiguration["host"];
            PortNo = _iconfiguration["portNo"];
            SenderMailPWD = AuthServiceObj.Decrypt(_iconfiguration["senderMailPWD"]);

            try
            {
                MailMessage MessageObj = new MailMessage();
                foreach (string ToAddress in RecipientMailId.Split(','))
                {
                    MessageObj.To.Add(ToAddress);
                }
                MessageObj.From = new MailAddress(SenderMailId);
                MessageObj.Body = Message;
                MessageObj.IsBodyHtml = true;
                MessageObj.Subject = MailSubject;
                SmtpClient smtpObj = new SmtpClient(Host, int.Parse(PortNo));
                smtpObj.UseDefaultCredentials = false;
                smtpObj.Credentials = new System.Net.NetworkCredential(SenderMailId, SenderMailPWD);
                smtpObj.EnableSsl = true;
                smtpObj.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpObj.Send(MessageObj);
            }
            catch (Exception Ex)
            {
                LogService LogService = new LogService(_iconfiguration);
                LogService.LogException(Ex);
            }
        }
    }
}
