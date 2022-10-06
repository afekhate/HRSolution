
using Quartz;
using Quartz.Impl;
using static HRSolution.Background.GetBankBranches.GetBranches;
using static HRSolution.Background.GetBankBranches.MailSender;
using static HRSolution.Background.GetBankStaffs.GetStaffs;

namespace FetchBankBranches
{
    class Program
    {
        static async Task Main(string[] args)
        {


            #region General Initialization
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            #endregion

            #region First Job
            IJobDetail firstJob = JobBuilder.Create<FetchBranches>()
               .WithIdentity("Fetch Branches", "HR Solution group")
               .Build();

            ITrigger firstTrigger = TriggerBuilder.Create()
                             .WithIdentity("Fetch Branches Trigger", "HR Solution group")
                             //.StartNow()
                             .WithSimpleSchedule(x => x.WithIntervalInHours(48).RepeatForever())
                             //.WithCronSchedule("*/5 * * * * ")
                             .Build();

            #endregion

            #region Second Job

            IJobDetail secondJob = JobBuilder.Create<FetchStaffs>()
                           .WithIdentity("Fetch Staffs", "HR Solution group")
                           .Build();

            ITrigger secondTrigger = TriggerBuilder.Create()
                             .WithIdentity("secondTrigger")
                             //.StartNow()
                             .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
                             //.WithCronSchedule(" * * * *")
                             .Build();
            #endregion

            #region Third Job

            IJobDetail thirdJob = JobBuilder.Create<SendMails>()
                           .WithIdentity("Send Mail", "HR Solution group")
                           .Build();

            ITrigger thirdTrigger = TriggerBuilder.Create()
                             .WithIdentity("thirdTrigger")
                             //.StartNow()
                             .WithSimpleSchedule(x => x.WithIntervalInSeconds(300000000).RepeatForever())
                             //.WithCronSchedule(" * * * *")
                             .Build();
            #endregion


            await scheduler.ScheduleJob(firstJob, firstTrigger);
            await scheduler.ScheduleJob(secondJob, secondTrigger);
            await scheduler.ScheduleJob(thirdJob, thirdTrigger);


            #region Single Implementation Example
            //ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

            //IScheduler scheduler = await schedulerFactory.GetScheduler();
            //await scheduler.Start();


            //IJobDetail job = JobBuilder.Create<FetchBranches>()
            //        .WithIdentity("Fetch Branches", "HR Solution group")
            //        .Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("Fetch Branches Trigger", "HR Solution group")
            //    .WithSimpleSchedule(x => x.WithIntervalInHours(24).RepeatForever())
            //    .Build();

            //await scheduler.ScheduleJob(job, trigger);
            //Console.ReadLine();

            #endregion

            Console.ReadLine();



        }
    }


}