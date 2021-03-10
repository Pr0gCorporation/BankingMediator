using System;

namespace Internship.SftpService.Service.Jobs.Configuration
{
    public class JobConfiguration
    {
        public string JobKey { get; set; }
        public string WithIdentity { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public string CronSchedule { get; set; }

        public JobConfiguration(string jobKey, 
            string withIdentity, 
            DateTimeOffset startAt, 
            string cronSchedule)
        {
            JobKey = jobKey;
            WithIdentity = withIdentity;
            StartAt = startAt;
            CronSchedule = cronSchedule;
        }

        public JobConfiguration()
        {
            
        }
    }  
}