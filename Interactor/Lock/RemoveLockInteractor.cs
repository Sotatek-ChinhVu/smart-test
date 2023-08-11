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
                string tabKey = inputData.TabKey;

                List<long> raiinNoList;
                if (inputData.IsRemoveAllLock)
                {
                    raiinNoList = _lockRepository.RemoveAllLock(hpId, userId);
                    raiinNo = 0;
                }
                else if (inputData.IsRemoveAllLockPtId)
                {
                    raiinNoList = _lockRepository.RemoveAllLock(hpId, userId, ptId, sinDate, functionCode, tabKey);
                    raiinNo = 0;
                }
                else if (inputData.IsRemoveLockWhenLogOut)
                {
                    raiinNoList = _lockRepository.RemoveAllLock(hpId, userId, inputData.LoginKey);
                    raiinNo = 0;
                }
                else
                {
                    raiinNoList = _lockRepository.RemoveLock(hpId, functionCode, ptId, sinDate, raiinNo, userId, tabKey);
                    raiinNo = 0;
                }
                if (raiinNoList.Any())
                {
                    List<ResponseLockModel> responseLockList;
                    if (inputData.IsRemoveLockWhenLogOut)
                    {
                        responseLockList = _lockRepository.GetResponseLockModel(hpId, raiinNoList);
                    }
                    else
                    {
                        responseLockList = _lockRepository.GetResponseLockModel(hpId, ptId, sinDate, raiinNo);
                    }
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
