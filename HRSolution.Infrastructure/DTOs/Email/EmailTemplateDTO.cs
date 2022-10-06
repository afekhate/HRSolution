using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.DTOs.Email
{
    public class EmailTemplateDTO
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string MailBody { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
