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
                int removeCounted = 0;

                List<long>? raiinNoList;
                if (inputData.IsRemoveAllLock)
                {
                    raiinNoList = _lockRepository.RemoveAllLock(hpId, userId);
                    raiinNo = 0;
                }
                else if (inputData.IsRemoveAllLockPtId)
                {
                    var removeResult = _lockRepository.RemoveAllLock(hpId, userId, ptId, sinDate, functionCode, tabKey);
                    raiinNoList = removeResult.raiinNoList;
                    removeCounted = removeResult.removedCount;
                    raiinNo = 0;
                }
                else if (inputData.IsRemoveLockWhenLogOut)
                {
                    raiinNoList = _lockRepository.RemoveAllLock(hpId, userId, inputData.LoginKey);
                    raiinNo = 0;
                }
                else
                {
                    var removeResult = _lockRepository.RemoveLock(hpId, functionCode, ptId, sinDate, raiinNo, userId, tabKey);
                    raiinNoList = removeResult.raiinList;
                    removeCounted = removeResult.removedCount;
                    raiinNo = 0;
                }
                if (raiinNoList?.Any() == true)
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
                    return new RemoveLockOutputData(RemoveLockStatus.Successed, responseLockList, removeCounted);
                }
                // if remove lock when logout and don't exist raiinNoList, return true
                if (inputData.IsRemoveLockWhenLogOut)
                {
                    return new RemoveLockOutputData(RemoveLockStatus.Successed, new());
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
