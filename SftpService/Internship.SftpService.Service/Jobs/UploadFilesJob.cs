using System.Threading.Tasks;
using Internship.SftpService.Service.SFTPAccess;
using Quartz;

namespace Internship.SftpService.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class UploadFilesJob : IJob
    {
        private readonly IServerFileUploadable _uploadable;

        public UploadFilesJob(IServerFileUploadable uploadable)
        {
            _uploadable = uploadable;
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            _uploadable.Upload("upload/in/", "./downloads/", false);
            return Task.CompletedTask;
        }
    }
}