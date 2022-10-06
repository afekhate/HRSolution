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
using static HRSolution.Infrastructure.ApiResponse.StaffResponse;

namespace HRSolution.Background.GetBankStaffs
{
    public class GetStaffs
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public class FetchStaffs : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                int addedStaffs = 0;

                try
                {
                    var ADbaseUrl = ConfigHelper.GetCurrentSettings("Baseurl", "ADService").appSettingValue;
                    var result = await MiddleWare.IRestGeturlAsync(ADbaseUrl + WebApiAddress.GetAllGlobusStaffs);
                    var response = JsonConvert.DeserializeObject<StaffResponseRoot>(result.Content);

                    if (response == null)
                    {
                        Console.WriteLine(CommonResponseMessage.InvalidOperation);
                    }
                    else if (response.Message != CommonResponseMessage.Successful)
                    {
                        Console.WriteLine(CommonResponseMessage.RecordNotFetched);
                    }
                    else
                    {
                        var localApibaseUrl = ConfigHelper.GetCurrentSettings("localApibaseUrl", "General").appSettingValue;

                        if (response.users.value.Count != 0)
                        {
                            foreach (var staff in response.users.value.Where(x => x.jobTitle != null))
                            {
                                if(staff.employeeId == null)
                                {
                                    staff.employeeId = "0"; //Exco
                                }
                                var getStaff = await MiddleWare.IRestGetLocal(localApibaseUrl + WebApiAddress.GetStaffById + staff.mailNickname);
                                var checkExist = JsonConvert.DeserializeObject<ApiResult<BranchDTO>>(getStaff.Content).Result;
                                if (checkExist == null)
                                {
                                    
                                    var addStaff = await MiddleWare.IRestPostLocal(localApibaseUrl + WebApiAddress.AddStaffApi, staff);

                                    if (addStaff.IsSuccessful == true)
                                    {
                                        Console.WriteLine(CommonResponseMessage.StaffAdded, staff.employeeId);
                                        addedStaffs++;
                                    }
                                    else
                                    {
                                        Console.WriteLine(CommonResponseMessage.GenericError, staff.employeeId);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(CommonResponseMessage.StaffExist, staff.employeeId);
                                }
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Console.WriteLine(CommonResponseMessage.OperationComplete, DateTime.Now.ToString(), addedStaffs);

                await Task.CompletedTask;


                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();
            }

        }
    }
}

