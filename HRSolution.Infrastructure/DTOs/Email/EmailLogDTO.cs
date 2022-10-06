using HRSolution.Infrastructure.Domain.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.DTOs.Email
{
    public class EmailLogDTO
    {

        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string? CC { get; set; }
        public string? BCC { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public bool HasAttachment { get; set; }
        public bool IsSent { get; set; }
        public int Retires { get; set; } = 0;
        public string? DateSent { get; set; }
        public DateTime? DateToSend { get; set; } = DateTime.Now;

        public List<EmailAttachment>? EmailAttachments { get; set; }
    }
}
