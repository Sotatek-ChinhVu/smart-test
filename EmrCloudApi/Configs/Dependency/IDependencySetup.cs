namespace EmrCloudApi.Configs.Dependency
{
    public interface IDependencySetup
    {
        void Run(IServiceCollection services);
    }
}
