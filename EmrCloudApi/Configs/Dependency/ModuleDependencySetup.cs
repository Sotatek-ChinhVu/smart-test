using Domain.Models.User;
using Infrastructure.Repositories;
using Interactor.User;
using UseCase.Core.Builder;
using UseCase.User.Create;

namespace EmrCloudApi.Configs.Dependency
{
    public class ModuleDependencySetup : IDependencySetup
    {
        public void Run(IServiceCollection services)
        {
            SetupRepositories(services);
            SetupUseCase(services);
        }

        private void SetupRepositories(IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
        }

        private void SetupUseCase(IServiceCollection services)
        {
            var registration = new ServiceRegistration(services);
            var busBuilder = new SyncUseCaseBusBuilder(registration);

            //User
            busBuilder.RegisterUseCase<CreateUserInputData, CreateUserInteractor>();

            var bus = busBuilder.Build(); ;
            services.AddSingleton(bus);
        }
    }
}
