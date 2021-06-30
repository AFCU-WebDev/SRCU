using SRCUBagTracking.Repository.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SRCUBagTracking.Models
{
    public class SendEmail : IDisposable
    {
        private ISRCURepository _srcuRepositoryInterface;

        // fake ID
        [Key]
        public Guid? ID { get; set; }

        //Parameters
        public string MailServer { 
            get {
                var mailserver = "Mail.applefcu.org";
                return mailserver;
            }
        }

        public string From
        {
            get {
                //var fromaddress = "fnamdarian@applefcu.org";
                var fromaddress = "srcu@applefcu.org";
                return fromaddress;
            }
        }
        public string To { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public SendEmail()
        {
            this._srcuRepositoryInterface = new SRCURepository(new SRCUDbContext());
        }

        public int DeliverEmail(string to, string cc, string subject, string body)
        {
            this.To = to;
            this.Subject = subject;
            this.CC = cc;
            this.Body = body;
            return _srcuRepositoryInterface.DeliverEmail(this);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}