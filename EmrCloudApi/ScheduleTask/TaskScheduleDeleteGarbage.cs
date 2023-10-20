using EmrCloudApi.BackgroundService;
using EmrCloudApi.Services;

namespace EmrCloudApi.ScheduleTask
{
    public class TaskScheduleDeleteGarbage : ScheduledProcessor
    {

        public TaskScheduleDeleteGarbage(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

#if DEBUG
        protected override string Schedule => "*/1 * * * *"; // every 1 min 
#else
        protected override string Schedule => "*/60 * * * *"; // every 60 min 
#endif

        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            ISystemStartDbService systemStartDbService = scopeServiceProvider.GetRequiredService<ISystemStartDbService>();

            systemStartDbService.DeleteAndUpdateData();

            return Task.CompletedTask;
        }
    }
}
