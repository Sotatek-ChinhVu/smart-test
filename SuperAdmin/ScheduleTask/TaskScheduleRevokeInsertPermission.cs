using SuperAdminAPI.BackgroundService;
using SuperAdminAPI.Services;

namespace EmrCloudApi.ScheduleTask
{
    public class TaskScheduleRevokeInsertPermission : ScheduledProcessor
    {

        public TaskScheduleRevokeInsertPermission(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

#if DEBUG
        protected override string Schedule => "*/60 * * * *"; // every 60 min 
#else
        protected override string Schedule => "*/60 * * * *"; // every 60 min 
#endif

        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            ISystemStartDbService systemStartDbService = scopeServiceProvider.GetRequiredService<ISystemStartDbService>();

            systemStartDbService.RevokeInsertPermission();

            return Task.CompletedTask;
        }
    }
}
