using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;

namespace MyJobs
{
    public class MyJob : IJob
    {
        private ILog Log { get; }

        public MyJob(ILog log) =>
            Log = log ?? throw new ArgumentNullException(nameof(log));

        public Task Execute(IJobExecutionContext context) =>
            Task.Run(() => Log.Info("Hi from MyJob...................."));
    }
}
