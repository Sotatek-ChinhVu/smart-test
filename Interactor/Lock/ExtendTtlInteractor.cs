using Domain.Models.Lock;
using UseCase.Lock.ExtendTtl;

namespace Interactor.Lock
{
    public class ExtendTtlLockInteractor : IExtendTtlLockInputPort
    {
        private readonly ILockRepository _lockRepository;
        public ExtendTtlLockInteractor(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }

        public ExtendTtlLockOutputData Handle(ExtendTtlLockInputData inputData)
        {
            try
            {
                string functionCode = inputData.FunctionCode;
                long ptId = inputData.PtId;
                long raiinNo = inputData.RaiinNo;
                int sinDate = inputData.SinDate;
                int hpId = inputData.HpId;
                int userId = inputData.UserId;

                bool result = _lockRepository.ExtendTtl(hpId, functionCode, ptId, sinDate, raiinNo, userId);

                return new ExtendTtlLockOutputData(result ? ExtendTtlLockStatus.Successed : ExtendTtlLockStatus.Failed);
            }
            finally
            {
                _lockRepository.ReleaseResource();
            }
        }
    }
}
