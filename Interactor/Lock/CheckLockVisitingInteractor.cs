using Domain.Models.Lock;
using UseCase.Lock.Get;

namespace Interactor.Lock
{
    public class CheckLockVisitingInteractor : ICheckLockVisitingInputPort
    {
        private readonly ILockRepository _lockRepository;
        public CheckLockVisitingInteractor(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }
        public CheckLockVisitingOutputData Handle(CheckLockVisitingInputData inputData)
        {
            try
            {
                var status = _lockRepository.GetVisitingLockStatus(inputData.HpId, inputData.UserId, inputData.PtId, inputData.FunctionCode);
                if (!status)
                {
                    return new CheckLockVisitingOutputData(CheckLockVisitingStatus.Locked);
                }

                return new CheckLockVisitingOutputData(CheckLockVisitingStatus.None);
            }
            finally
            {
                _lockRepository.ReleaseResource();
            }
        }
    }
}
