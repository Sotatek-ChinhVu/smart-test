using SuperAdminAPI.BackgroundService;
using SuperAdminAPI.Services;

namespace SuperAdminAPI.ScheduleTask
{
    public class TaskScheduleDeleteJunkFileS3 : ScheduledProcessor
    {
        public TaskScheduleDeleteJunkFileS3(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }
        protected override string Schedule => "0 0 * * *"; // Run every day at midnight

        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            IDeleteJunkFileS3Service deleteJunkFile = scopeServiceProvider.GetRequiredService<IDeleteJunkFileS3Service>();

            deleteJunkFile.DeleteJunkFileS3();

            return Task.CompletedTask;
        }
    }
}
