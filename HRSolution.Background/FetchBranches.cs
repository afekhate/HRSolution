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

namespace HRSolution.Background.GetBankBranches
{
    public class GetBranches
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
       
        public class FetchBranches : IJob
        {          
            public async Task Execute(IJobExecutionContext context)
            {
                int addedBranches = 0;

                try
                {
                    var EnquirybaseUrl = ConfigHelper.GetCurrentSettings("Baseurl", "EnquiryService").appSettingValue;
                    var result = await MiddleWare.IRestGeturlAsync(EnquirybaseUrl + WebApiAddress.GetAllGlobusBranches);
                    var response = JsonConvert.DeserializeObject<BranchResponseRoot>(result.Content);

                    if (response == null)
                    {
                        Console.WriteLine(CommonResponseMessage.InvalidOperation);
                    }
                    else if (response.ResponseCode != "00")
                    {
                        Console.WriteLine(CommonResponseMessage.RecordNotFetched);
                    }
                    else
                    {                       
                        var localApibaseUrl = ConfigHelper.GetCurrentSettings("localApibaseUrl", "General").appSettingValue;

                        if (response.Result.branches.Count != 0)
                        {
                            foreach(var branch in response.Result.branches)
                            {
                                var branchToInt = Convert.ToInt32(branch.BranchCode);
                                var getBranch = await MiddleWare.IRestGetLocal(localApibaseUrl + WebApiAddress.GetBranchByCode + branchToInt);
                                var checkExist = JsonConvert.DeserializeObject<ApiResult<BranchDTO>>(getBranch.Content).Result;
                                if (checkExist == null)
                                {
                                    var addBranch = await MiddleWare.IRestPostLocal(localApibaseUrl + WebApiAddress.AddBranchApi, branch);
               
                                    if (addBranch.IsSuccessful == true)
                                    {
                                        Console.WriteLine(CommonResponseMessage.BranchAdded, branch.BranchCode);
                                        addedBranches++;
                                    }
                                    else
                                    {
                                        Console.WriteLine(CommonResponseMessage.GenericError, branch.BranchCode);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(CommonResponseMessage.BranchExist, branch.BranchCode);
                                }
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Console.WriteLine(CommonResponseMessage.OperationComplete, DateTime.Now.ToString(), addedBranches);

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

