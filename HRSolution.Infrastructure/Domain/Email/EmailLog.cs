using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain.Email
{
    public class EmailLog : BaseObject
    {
        public EmailLog()
        {
            EmailAttachments = new HashSet<EmailAttachment>();
        }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string? CC { get; set; }
        public string? BCC { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public bool HasAttachment { get; set; } = false;
        public bool IsSent { get; set; } = false;
        public int Retires { get; set; } = 0;
        public string? DateSent { get; set; }
        public DateTime? DateToSend { get; set; } = DateTime.Now;

        public virtual ICollection<EmailAttachment> EmailAttachments { get; set; }

    }
}
