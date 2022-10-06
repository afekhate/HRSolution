using Quartz;
using Quartz.Impl;

using System.Threading.Tasks;
using System.Data;

using System.Text;
using System.Configuration;
using Microsoft.Extensions.Logging;
using HRSolution.Utilities.Common;
using HRSolution.Infrastructure.DTOs.GStaff;
using VatPay.Utilities.Common;
using Newtonsoft.Json;
using NLog;
using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain.GStaff;
using HRSolution.Infrastructure.Domain.Branches;
using HRSolution.Infrastructure.DTOs.Branches;
using HRSolution.Infrastructure.ApiResponse;
using HRSolution.Infrastructure.DTOs.Email;
using HRSolution.Core.Handler;
using System.Data.SqlClient;
using HRSolution.Infrastructure.Domain;
using Dapper;
using HRSolution.Infrastructure.Domain.Email;
using System.Net.Mail;
using System.Web;
using System.Text.RegularExpressions;

namespace HRSolution.Background.GetBankBranches
{
    public class MailSender
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        
       
        public class SendMails : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                int addedEmail = 0;

                try
                {
                    var localApibaseUrl = ConfigHelper.GetCurrentSettings("localApibaseUrl", "General").appSettingValue;
                    var getEmails = await MiddleWare.IRestGetLocal(localApibaseUrl + WebApiAddress.GetUnsentMails);
                    var unSentMails = JsonConvert.DeserializeObject<ApiResult<List<EmailLogDTO>>>(getEmails.Content).Result;
                    if (unSentMails.Count != 0)
                    {
                        foreach (var email in unSentMails)
                        {
                            var senderOutput = await SendEmail(email, false,"Admin");
                            if(senderOutput == true)
                            {
                                //Update

                                string _connectionString = ConfigHelper.GetCurrentSettings("DefaultConnection", "ConnectionStrings").appSettingValue;
                                using (SqlConnection conn = new SqlConnection(_connectionString))
                                {
                                    var sysSql = $"update EmailLog set IsSent = 1 , DateSent = '{DateTime.Now}' where Id = {email.Id}";
                                    var entity = await conn.QueryAsync<EmailLog>(sysSql);
                                }

                                Console.WriteLine(CommonResponseMessage.MailSent, email.Subject, email.Receiver,DateTime.Now);
                                addedEmail++;
                            }
                            else
                            {
                                Console.WriteLine(CommonResponseMessage.MailNotSent, email.Subject, email.Receiver);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(CommonResponseMessage.MailUnavailable);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Console.WriteLine(CommonResponseMessage.OperationComplete, DateTime.Now.ToString(), addedEmail);

                await Task.CompletedTask;


                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();
            }




        }



        public static async Task<bool> SendEmail(EmailLogDTO emailModel, bool IsEmailScheduler, string createdBy)
        {
            string _smtpusername = "";
            string _smtppassword = "";
            string _smtpHost = "";

            string _smtpEnableSsl = "";
            string _SenderId = "";
            string _smtpPort = "";

            var _connectionString = ConfigHelper.GetCurrentSettings("DefaultConnection", "ConnectionStrings").appSettingValue;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var sysSql = $"select * from SystemSetting where [LookUpCode] = '{CommonResponseMessage.SMTP_LOOKUP.ToUpper()}'";
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

                        if (response.ItemName.ToUpper() == CommonResponseMessage._emailFrom.ToUpper())
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
                        CreatedBy = createdBy,
                        HasAttachment = emailModel.HasAttachment,
                        EmailAttachments = emailModel.EmailAttachments

                    };



                    try
                    {

                        // Wrap up the Logo Image Here


                        var message = new MailMessage
                        {
                            Sender = new MailAddress(_SenderId),
                            From = new MailAddress(_SenderId),
                            Subject = email.Subject,
                            Priority = MailPriority.High,
                            IsBodyHtml = true,
                            Body = HttpUtility.HtmlDecode(emailModel.MessageBody),
                        };


                        //Get Attachment From DB

                        if(email.HasAttachment == true && email.EmailAttachments.Count > 0)
                        {
                            foreach (var attach in email.EmailAttachments)
                            {
                                Attachment attachment = new Attachment(attach.FolderOnServer + attach.FileNameOnServer);
                                message.Attachments.Add(attachment);
                            }
                        }
                       

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


                        await smtpClient.SendMailAsync(message);

                       

                    }
                    catch (Exception ex)
                    {
                        //log mail if this is not email scheduler email
                        if (!IsEmailScheduler)
                        {
                            //await _emailLog.AddAsync(email);
                        }

                        return false;
                    }

                    email.IsSent = true;
                    if (!IsEmailScheduler)
                    {
                        //await _emailLog.AddAsync(email);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}

