using Domain.Models.Lock;
using UseCase.Lock.Remove;

namespace Interactor.Lock
{
    public class RemoveLockInteractor : IRemoveLockInputPort
    {
        private readonly ILockRepository _lockRepository;
        public RemoveLockInteractor(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }

        public RemoveLockOutputData Handle(RemoveLockInputData inputData)
        {
            try
            {
                string functionCode = inputData.FunctionCode;
                long ptId = inputData.PtId;
                long raiinNo = inputData.RaiinNo;
                int sinDate = inputData.SinDate;
                int hpId = inputData.HpId;
                int userId = inputData.UserId;

                bool result;
                if (inputData.IsRemoveAllLock)
                {
                    result = _lockRepository.RemoveAllLock(hpId, userId);
                }
                else if (inputData.IsRemoveAllLockPtId)
                {
                    result = _lockRepository.RemoveAllLock(hpId, userId, ptId, sinDate);
                }
                else
                {
                    result = _lockRepository.RemoveLock(hpId, functionCode, ptId, sinDate, raiinNo, userId);
                }
                return new RemoveLockOutputData(result ? RemoveLockStatus.Successed : RemoveLockStatus.Failed);
            }
            finally
            {
                _lockRepository.ReleaseResource();
            }
        }
    }
}
