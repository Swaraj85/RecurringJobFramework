using System;
using MyJobs;
using Quartz;

namespace Framework.WindowsService
{
    public class MyService
    {
        private IScheduler Scheduler { get; }

        public MyService(IScheduler scheduler) => 
            Scheduler = scheduler ?? throw new ArgumentNullException(nameof(Scheduler));

        public void OnStart()
        {
            Scheduler.Start();

            IJobDetail job = JobBuilder
                .Create<MyJob>()
                .WithIdentity(typeof(MyJob).Name, SchedulerConstants.DefaultGroup)
                .Build();

            ITrigger trigger = TriggerBuilder
                .Create()
                .WithIdentity("simpletrigger", SchedulerConstants.DefaultGroup)
                .ForJob(job)
                .StartNow()
                .WithSimpleSchedule(x =>
                    x.WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            Scheduler.ScheduleJob(job, trigger);
        } 

        public void OnPaused() => Scheduler.PauseAll();

        public void OnContinue() => Scheduler.ResumeAll();

        public void OnStop() => Scheduler.Shutdown();
    }
}