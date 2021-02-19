using Internship.SftpService.Service.Jobs.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Internship.SftpService.Service.Extentions
{
    public static class JobExtention
    {
        public static IServiceCollection AddJob<T>(
            this IServiceCollection services, 
            JobConfiguration jobConfiguration) where T : IJob
        {
            services.AddQuartz(q =>  
            {
                // Use a Scoped container to create jobs.
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                // Create a "key" for the job
                var jobKey = new JobKey(jobConfiguration.JobKey);

                // Register the job with the DI container
                q.AddJob<T>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey) // link to the DownloadFilesJob
                    .WithIdentity(jobConfiguration.WithIdentity) // give the trigger a unique name
                    .StartAt(jobConfiguration.StartAt)
                    .WithCronSchedule(jobConfiguration.CronSchedule));
            });

            // Add the Quartz.NET hosted service
            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);
            
            return services;
        }
    }
}