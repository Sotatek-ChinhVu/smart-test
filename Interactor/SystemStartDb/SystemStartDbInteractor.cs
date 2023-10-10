using Domain.Models.SystemStartDb;
using Helper.Common;
using UseCase.SystemStartDbs;

namespace Interactor.SystemStartDbs
{
    public class SystemStartDbInteractor : ISystemStartDbInputPort
    {
        private readonly ISystemStartDbRepository _systemStartDbRepository;

        public SystemStartDbInteractor(ISystemStartDbRepository systemStartDbRepository)
        {
            _systemStartDbRepository = systemStartDbRepository;
        }

        public SystemStartDbOutputData Handle(SystemStartDbInputData inputData)
        {
            try
            {
                _systemStartDbRepository.DeleteAndUpdateData(inputData.DelDate);

                return new SystemStartDbOutputData(true);
            } catch(Exception ex)
            {
                return new SystemStartDbOutputData(false);
            }
            finally
            {
                _systemStartDbRepository.ReleaseResource();
            }
        }
    }
}
