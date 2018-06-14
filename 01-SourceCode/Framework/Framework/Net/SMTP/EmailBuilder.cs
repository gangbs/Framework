using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class EmailBuilder
    {
        private SmtpClient client;
        private MailMessage msg;
        readonly List<MailAddress> toAddress;
        readonly MailAddress fromAddress;
        readonly string subject;
        readonly string content;

        public EmailBuilder(string toAddress, string subject, string content)
        {
            this.client = new SmtpClient();
            this.msg = new MailMessage();
            this.toAddress = new List<MailAddress>();
            this.fromAddress = new MailAddress(ConfigurationManager.AppSettings["EmailFromAddress"]);
            this.toAddress.Add(new MailAddress(toAddress));
            this.subject = subject;
            this.content = content;
        }

        public EmailBuilder(List<string> toAddress, string subject, string content)
        {
            this.client = new SmtpClient();
            this.msg = new MailMessage();
            this.toAddress = new List<MailAddress>();
            this.fromAddress = new MailAddress(ConfigurationManager.AppSettings["EmailFromAddress"]);
            foreach (var ads in toAddress)
            {
                this.toAddress.Add(new MailAddress(ads));
            }
            this.subject = subject;
            this.content = content;
        }

        public EmailBuilder(List<MailAddress> toAddress, string subject, string content)
        {
            this.client = new SmtpClient();
            this.msg = new MailMessage();
            this.toAddress = new List<MailAddress>();
            this.fromAddress = new MailAddress(ConfigurationManager.AppSettings["EmailFromAddress"]);
            this.toAddress = toAddress;
            this.subject = subject;
            this.content = content;
        }

        public EmailBuilder(string fromAddress, List<string> toAddress, string subject, string content)
        {
            this.client = new SmtpClient();
            this.msg = new MailMessage();
            this.toAddress = new List<MailAddress>();
            this.fromAddress = new MailAddress(fromAddress);
            foreach (var ads in toAddress)
            {
                this.toAddress.Add(new MailAddress(ads));
            }
            this.subject = subject;
            this.content = content;
        }

        public EmailBuilder(MailAddress fromAddress, List<MailAddress> toAddress, string subject, string content)
        {
            this.client = new SmtpClient();
            this.msg = new MailMessage();
            this.toAddress = new List<MailAddress>();
            this.fromAddress = fromAddress;
            this.toAddress = toAddress;
            this.subject = subject;
            this.content = content;
        }

        private void SetHost()
        {
            this.client.Host = ConfigurationManager.AppSettings["EmailHost"];
            this.client.Port = int.Parse(ConfigurationManager.AppSettings["EmailPort"]);
        }

        private void SetCredential()
        {
            string userName = ConfigurationManager.AppSettings["EmailUserName"];
            string pwd = ConfigurationManager.AppSettings["EmailPassword"];
            this.client.Credentials = new NetworkCredential(userName, pwd);
            //this.client.EnableSsl = true;
        }

        private void SetMailFromTo()
        {
            this.msg.From = this.fromAddress;
            foreach (var ads in toAddress)
            {
                this.msg.To.Add(ads);
            }
        }

        private void SetMailContent()
        {
            this.msg.Subject = this.subject;
            this.msg.Body = this.content;
            this.msg.IsBodyHtml = true;
            this.msg.Priority = MailPriority.Normal;
        }

        public SmtpClient Construct()
        {
            SetHost();
            SetCredential();
            SetMailFromTo();
            SetMailContent();
            return this.client;
        }

        public bool SendEmail()
        {
            bool flag = false;
            try
            {
                this.client.Send(this.msg);
                flag = true;
            }
            catch (Exception e)
            {
                flag = false;
            }
            return flag;
        }

    }
}
