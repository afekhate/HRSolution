using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.DTOs.Email
{
    public class EmailAttachmentDTO
    {
        public long EmailLogId { get; set; }
        public string FolderOnServer { get; set; }
        public string FileNameOnServer { get; set; }
        public string EmailFileName { get; set; }
    }
}
