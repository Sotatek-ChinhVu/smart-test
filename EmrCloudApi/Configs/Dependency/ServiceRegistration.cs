using UseCase.Core.Dependency;

namespace EmrCloudApi.Configs.Dependency
{
    public class ServiceRegistration : IServiceRegistration
    {
        private readonly IServiceCollection collection;

        public ServiceRegistration(IServiceCollection collection)
        {
            this.collection = collection;
        }

        public void AddTransient<T>() where T : class
        {
            collection.AddTransient<T>();
        }

        public IServiceProvider BuildServiceProvider()
        {
            return collection.BuildServiceProvider();
        }
    }
}
