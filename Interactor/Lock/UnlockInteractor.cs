using Domain.Models.Lock;
using UseCase.Lock.Unlock;

namespace Interactor.Lock
{
    public class UnlockInteractor : IUnlockInputPort
    {
        private readonly ILockRepository _lockRepository;

        public UnlockInteractor(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }
        public UnlockOutputData Handle(UnlockInputData inputData)
        {
            try
            {
                var data = _lockRepository.Unlock(inputData.HpId, inputData.UserId, inputData.LockModels, inputData.ManagerKbn);
                if (data)
                {
                    return new UnlockOutputData(UnlockStatus.Success);
                }
                else
                {
                    return new UnlockOutputData(UnlockStatus.Failed);
                }

            }
            finally
            {
                _lockRepository.ReleaseResource();
            }
        }
    }
}
