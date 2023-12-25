using UseCase.Core.Sync;
using UseCase.SuperAdmin.DeleteJunkFileS3;

namespace SuperAdminAPI.Services
{
    public class DeleteJunkFileS3Service : IDeleteJunkFileS3Service
    {
        private readonly UseCaseBus _bus;

        public DeleteJunkFileS3Service(UseCaseBus bus)
        {
            _bus = bus;
        }

        public void DeleteJunkFileS3()
        {
            var input = new DeleteJunkFileS3InputData();
            var output = _bus.Handle(input);
        }
    }
}
