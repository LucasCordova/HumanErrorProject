using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HumanErrorProject.Ui.Services
{
    public class HangFireJobService : IHangFireJobService
    {
        protected class UserJob
        {
            public string Name { get; set; }
            public IList<string> JobIds { get; set; } = new List<string>();
        }

        protected IList<UserJob> UserJobs;
        protected Mutex Mutex;

        public HangFireJobService()
        {
            UserJobs = new List<UserJob>();
            Mutex = new Mutex();
        }


        public string GetParentOrDefault(string name)
        {
            Mutex.WaitOne();
            var user = UserJobs.SingleOrDefault(u => u.Name.Equals(name));
            string jobId = null;
            if (user != null)
            {
                jobId = user.JobIds.Last();
            }
            Mutex.ReleaseMutex();
            return jobId;
        }

        public void AddJob(string name, string jobId)
        {
            Mutex.WaitOne();

            var user = UserJobs.SingleOrDefault(u => u.Name.Equals(name));
            if (user == null)
            {
                UserJobs.Add(new UserJob()
                {
                    Name = name,
                    JobIds = new List<string>()
                    {
                        jobId
                    }
                });
            }
            else
            {
                user.JobIds.Add(jobId);
            }

            Mutex.ReleaseMutex();
        }

        public void Remove(string name)
        {
            Mutex.WaitOne();

            var user = UserJobs.SingleOrDefault(u => u.Name.Equals(name));

            if (user != null)
            {
                user.JobIds.Remove(user.JobIds.First());

                if (!user.JobIds.Any())
                {
                    UserJobs.Remove(user);
                }
            }
            Mutex.ReleaseMutex();
        }
    }
}
