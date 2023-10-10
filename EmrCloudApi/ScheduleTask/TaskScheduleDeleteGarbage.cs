using AspNetCoreSchedulerDemo.BackgroundService;
using EmrCloudApi.Services;
using Helper.Common;
using Interactor.SystemStartDbs;
using UseCase.Core.Sync;
using UseCase.Document.GetDocCategoryDetail;
using UseCase.SystemStartDbs;

namespace AspNetCoreSchedulerDemo.ScheduleTask
{
    public class TaskScheduleDeleteGarbage : ScheduledProcessor
    {

        public TaskScheduleDeleteGarbage(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule => "*/60 * * * *"; // every 1 min 

        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            ISystemStartDbService systemStartDbService = scopeServiceProvider.GetRequiredService<ISystemStartDbService>();

            systemStartDbService.DeleteAndUpdateData();

            return Task.CompletedTask;
        }
    }
}
