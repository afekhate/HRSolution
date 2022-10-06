using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain.Email
{
    public class EmailTemplate : BaseObject
    {

        [StringLength(500)]
        public string Subject { get; set; }
        public string MailBody { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
