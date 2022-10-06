using HRSolution.Infrastructure.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Core.Contract
{
    public interface IEmailService
    {
        Task<EmailTemplateDTO> GetEmailTemplateByCode(string Code);
        Task<bool> PrepareEmailLog(string EmailTemplateCode, string emailTo, string cc
      , string bcc, List<EmailTokenDTO> emailTokens, string createdBy, DateTime SendDate, bool EmailScheduler);
        Task<bool> SendEmail(EmailLogDTO newEmailLog, bool EmailScheduler, string createdBy);
      
        //void ProcessPendingEmails();
    }
}
