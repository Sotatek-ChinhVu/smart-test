using Domain.Models.Lock;
using UseCase.Lock.Check;

namespace Interactor.Lock
{
    public class CheckLockInteractor : ICheckLockInputPort
    {
        private readonly ILockRepository _lockRepository;
        public CheckLockInteractor(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }

        public CheckLockOutputData Handle(CheckLockInputData inputData)
        {
            try
            {
                string functionCode = inputData.FunctionCode;
                long ptId = inputData.PtId;
                long raiinNo = inputData.RaiinNo;
                int sinDate = inputData.SinDate;
                int hpId = inputData.HpId;
                int userId = inputData.UserId;

                var lockInfList = _lockRepository.GetLock(hpId, functionCode, ptId, sinDate, raiinNo, userId);
                if (!lockInfList.Any())
                {
                    return new CheckLockOutputData(CheckLockStatus.NotLock, new LockModel());
                }

                var lockInf = lockInfList.FirstOrDefault();

                if (lockInf == null || lockInf.IsEmpty)
                {
                    return new CheckLockOutputData(CheckLockStatus.NotLock, new LockModel());
                }
                else if (!string.IsNullOrEmpty(inputData.TabKey) && lockInfList.Exists(item => item.TabKey.Contains(inputData.TabKey)))
                {
                    return new CheckLockOutputData(CheckLockStatus.NotLock, new LockModel());
                }
                return new CheckLockOutputData(CheckLockStatus.Locked, lockInf);
            }
            finally
            {
                _lockRepository.ReleaseResource();
            }
        }
    }
}
