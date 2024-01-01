namespace SuperAdminAPI.BackgroundService
{
    public class RunProcessor : BackgroundService
    {
        protected override Task Process()
        {
           return new Task(()=> Console.WriteLine("run"));
        }
    }
}
