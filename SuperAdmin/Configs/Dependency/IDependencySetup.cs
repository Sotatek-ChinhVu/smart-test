namespace SuperAdmin.Configs.Dependency
{
    public interface IDependencySetup
    {
        void Run(IServiceCollection services);
    }
}
