using Dapper;
using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain;
using HRSolution.Infrastructure.Domain.Email;
using HRSolution.Infrastructure.DTOs.Email;
using HRSolution.Utilities.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


namespace HRSolution.Core.Handler
{

    public class EmailingService : IEmailService
    {
       
        private readonly IConfiguration _config;
        private readonly ILogger<EmailingService> _logger;
        private readonly IHostingEnvironment _env;
        private readonly ISysSettingService _systemSettingManager;
        private readonly IEmailLog _emailLog;
        protected string _connectionString;

        public static bool _mailSent;



        public EmailingService(ILogger<EmailingService> logger, IConfiguration config,
          IConfiguration iConfig, IHostingEnvironment env, ISysSettingService systemSettingManager, IEmailLog emailLog)
        {
           
            _logger = logger;
            _config = config;
           _emailLog = emailLog;
            _connectionString = config.GetConnectionString("DefaultConnection");
        }


        public async Task<EmailTemplateDTO> GetEmailTemplateByCode(string Code)
        {
            EmailTemplateDTO model = new EmailTemplateDTO();

            try
            {
               
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    var sql = $"select * from EmailTemplate where [Code] = '{Code}'";
                    var existingRecord = await conn.QueryAsync<EmailTemplate>(sql);

                    if (existingRecord.Any())
                    {

                        var output = existingRecord.Select(model => new EmailTemplateDTO
                        {
                            Subject = model.Subject,
                            MailBody = model.MailBody,
                            Code = model.Code,
                            Description = model.Description,

                        })
                        .FirstOrDefault();
                        return output;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return model;
            }
        }


        #region Emailing  

        public async Task<bool> PrepareEmailLog(string EmailTemplateCode, string emailTo, string cc
      , string bcc,List<EmailTokenDTO> emailTokens, string createdBy, DateTime SendDate, bool EmailScheduler)
        {
            string status = "";

            EmailLogDTO EmailLogDTO = new EmailLogDTO();
            try
            {
                var emailTemplate = await GetEmailTemplateByCode(EmailTemplateCode);


                StringBuilder sbEmailBody = new StringBuilder();

                sbEmailBody.Append(emailTemplate.MailBody);

                foreach (var token in emailTokens)
                {
                    sbEmailBody = sbEmailBody.Replace(token.Token, token.TokenValue);
                }

                EmailLog newEmailLog = new EmailLog
                {
                    Sender = _config["Sender"],
                    Receiver = emailTo,
                    CC = string.IsNullOrEmpty(cc) ? null : cc,
                    BCC = string.IsNullOrEmpty(bcc) ? null : bcc,
                    MessageBody = sbEmailBody.ToString(),
                    Subject = emailTemplate.Subject,
                    CreatedBy = createdBy
                };


                await _emailLog.AddAsync(newEmailLog);

                return true;



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return false;
            }
        }


        public async Task<bool> SendEmail(EmailLogDTO emailModel, bool IsEmailScheduler, string createdBy)
        {
            string _smtpusername = "";
            string _smtppassword = "";
            string _smtpHost = "";

            string _smtpEnableSsl = "";
            string _SenderId = "";
            string _smtpPort = "";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var sysSql = $"select * from SystemSetting where [LookUpCode] = {CommonResponseMessage.SMTP_LOOKUP.ToUpper()}";
                var setupResponse = await conn.QueryAsync<SystemSetting>(sysSql);

                //var setupResponse = aPPContext.SystemSettings.Where(a => a.LookUpCode.ToUpper() == CommonResponseMessage.SMTP_LOOKUP.ToUpper());

                if (setupResponse.Count() > 0)
                {


                    foreach (var response in setupResponse)
                    {

                        if (response.ItemName.ToUpper() == CommonResponseMessage._smtpusername.ToUpper())
                            _smtpusername = response.ItemValue;

                        if (response.ItemName.ToUpper() == CommonResponseMessage._smtppassword.ToUpper())
                            _smtppassword = response.ItemValue;


                        if (response.ItemName.ToUpper() == CommonResponseMessage._smtpHost.ToUpper())
                            _smtpHost = response.ItemValue;

                        if (response.ItemName.ToUpper() == CommonResponseMessage._smtpPort.ToUpper())
                            _smtpPort = response.ItemValue;


                        if (response.ItemName.ToUpper() == CommonResponseMessage._smtpEnableSsl.ToUpper())
                            _smtpEnableSsl = response.ItemValue;

                        if (response.ItemName.ToUpper() == CommonResponseMessage._SenderId.ToUpper())
                            _SenderId = response.ItemValue;
                    }


                    EmailLog email = new EmailLog
                    {
                        Receiver = emailModel.Receiver,
                        CC = emailModel.CC,
                        BCC = emailModel.BCC,
                        Subject = emailModel.Subject,
                        MessageBody = emailModel.MessageBody,
                        CreatedDate = DateTime.Now,
                        DateToSend = DateTime.Now,
                        Sender = _SenderId,
                        IsSent = false,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = createdBy

                    };



                    try
                    {

                        // Wrap up the Logo Image Here


                        var message = new MailMessage
                        {
                            Sender = new MailAddress(email.Sender),
                            From = new MailAddress(email.Sender),
                            Subject = email.Subject,
                            Priority = MailPriority.High,
                            IsBodyHtml = true,
                            Body = HttpUtility.HtmlDecode(emailModel.MessageBody),
                        };

                        var emailToCol = email.Receiver.Split(',');
                        if (emailToCol.Count() == 0)
                        {
                            throw new ArgumentException("Email to not supplied");
                        }

                        foreach (string emailTo in emailToCol)
                        {
                            if (Regex.IsMatch(emailTo, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                            {
                                message.To.Add(new MailAddress(emailTo));
                            }
                        }

                        if (!string.IsNullOrEmpty(emailModel.CC))
                        {
                            var ccCol = emailModel.CC.Split(',');
                            foreach (string cc in ccCol)
                            {
                                if (Regex.IsMatch(cc, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                                {
                                    message.CC.Add(new MailAddress(cc));
                                }

                            }
                        }

                        if (!string.IsNullOrEmpty(emailModel.BCC))
                        {
                            var bccCol = emailModel.BCC.Split(',');
                            foreach (string bcc in bccCol)
                            {
                                if (Regex.IsMatch(bcc, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                                {
                                    message.CC.Add(new MailAddress(bcc));
                                }

                            }
                        }



                        var smtpClient = new SmtpClient
                        {
                            Host = _smtpHost,
                            Port = Convert.ToInt32(_smtpPort),
                            EnableSsl = Convert.ToBoolean(_smtpEnableSsl),
                            DeliveryMethod = 0,
                            UseDefaultCredentials = false,
                            Credentials = new System.Net.NetworkCredential(_smtpusername, _smtppassword),

                        };


                        smtpClient.Send(message);
                    }
                    catch (Exception ex)
                    {
                        //log mail if this is not email scheduler email
                        if (!IsEmailScheduler)
                        {
                            await _emailLog.AddAsync(email);
                        }

                        return false;
                    }

                    email.IsSent = true;
                    if (!IsEmailScheduler)
                    {
                        await _emailLog.AddAsync(email);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //public void ProcessPendingEmails()
        //{



        //    try
        //    {
        //        var cc = _configuration["cc"];
        //        var bcc = "";
        //        var allUnsentMail = _context.EmailLogs.Where(x => x.IsSent == false && x.IsDeleted == false).ToList();
        //        bool EmailScheduler = false;
        //        foreach (var item in allUnsentMail)
        //        {
        //            if (item != null)
        //            {
        //                EmailLogDTO newEmailLog = new EmailLogDTO
        //                {
        //                    Receiver = item.Receiver,
        //                    CC = string.IsNullOrEmpty(cc) ? null : cc,
        //                    BCC = string.IsNullOrEmpty(bcc) ? null : bcc,
        //                    MessageBody = item.MessageBody,
        //                    Subject = item.Subject,
        //                };
        //                var isSent = SendEmail(newEmailLog, item.CreatedBy);
        //                var emailLog = _context.EmailLogs.Where(x => x.ID == item.ID).FirstOrDefault();
        //                if (isSent)
        //                {
        //                    //update email log

        //                    emailLog.IsSent = true;
        //                    emailLog.DateSent = DateTime.Now;

        //                }
        //                else
        //                {
        //                    emailLog.Retires = emailLog.Retires + 1;
        //                }

        //                _context.SaveChanges();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //    }
        //    finally
        //    {
        //        //  db.Dispose();
        //    }
        //}


        #endregion

    }

}
