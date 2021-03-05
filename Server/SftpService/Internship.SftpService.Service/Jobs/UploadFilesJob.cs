using System.Text;
using System.Threading.Tasks;
using Internship.SftpService.Service.SFTPActions.UploadFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace Internship.SftpService.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class UploadFilesJob : IJob
    {
        private readonly IServerFileUploadable _uploadable;
        private readonly HostBuilderContext _hostBuilderContext;

        public UploadFilesJob(IServerFileUploadable uploadable, HostBuilderContext hostBuilderContext)
        {
            _uploadable = uploadable;
            _hostBuilderContext = hostBuilderContext;
        }
        
        public Task Execute(IJobExecutionContext context)
        {
            var configuration = _hostBuilderContext.Configuration;
            _uploadable.Upload(configuration.GetValue<string>("PathConfig:UploadFiles:To"), 
                configuration.GetValue<string>("PathConfig:UploadFiles:From"), 
                configuration.GetValue<bool>("PathConfig:UploadFiles:RemoveAfter"));
            return Task.CompletedTask;
        }
    }
}