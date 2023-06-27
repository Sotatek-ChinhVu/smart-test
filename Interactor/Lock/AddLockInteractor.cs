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
                    var responseLockList = _lockRepository.GetResponseLockModel(hpId, sinDate);
                    return new AddLockOutputData(AddLockStatus.Successed, responseLockList);
                }
                else
                {
                    return new AddLockOutputData(AddLockStatus.Existed, new());
                }
            }
            finally
            {
                _lockRepository.ReleaseResource();
            }
        }
    }
}
