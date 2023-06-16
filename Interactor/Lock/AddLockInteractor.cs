using Domain.Models.Lock;
using UseCase.Lock.Add;

namespace Interactor.Lock
{
    public class AddLockInteractor : IAddLockInputPort
    {
        private readonly ILockRepository _lockRepository;
        public AddLockInteractor(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }

        public AddLockOutputData Handle(AddLockInputData inputData)
        {
            try
            {
                string functionCode = inputData.FunctionCode;
                long ptId = inputData.PtId;
                long raiinNo = inputData.RaiinNo;
                int sinDate = inputData.SinDate;
                int hpId = inputData.HpId;
                int userId = inputData.UserId;

                bool result = _lockRepository.AddLock(hpId, functionCode, ptId, sinDate, raiinNo, userId, inputData.Token);
                if (result)
                {
                    return new AddLockOutputData(AddLockStatus.Successed, new LockModel());
                }
                else
                {
                    var lockInfList = _lockRepository.GetLock(hpId, functionCode, ptId, sinDate, raiinNo, userId);
                    return new AddLockOutputData(AddLockStatus.Existed, lockInfList.FirstOrDefault() ?? new LockModel());
                }
            }
            finally
            {
                _lockRepository.ReleaseResource();
            }
        }
    }
}
