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

                List<long> result;
                if (inputData.IsRemoveAllLock)
                {
                    result = _lockRepository.RemoveAllLock(hpId, userId);
                }
                else if (inputData.IsRemoveAllLockPtId)
                {
                    result = _lockRepository.RemoveAllLock(hpId, userId, ptId, sinDate, functionCode);
                }
                else
                {
                    result = _lockRepository.RemoveLock(hpId, functionCode, ptId, sinDate, raiinNo, userId);
                }
                if (result.Any())
                {
                    var responseLockList = _lockRepository.GetResponseLockModel(hpId, sinDate);
                    return new RemoveLockOutputData(RemoveLockStatus.Successed, responseLockList);
                }
                return new RemoveLockOutputData(RemoveLockStatus.Failed, new());
            }
            finally
            {
                _lockRepository.ReleaseResource();
            }
        }
    }
}
