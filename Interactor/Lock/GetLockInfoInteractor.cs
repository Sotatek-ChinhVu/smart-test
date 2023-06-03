using Domain.Models.Lock;
using UseCase.Lock.Get;

namespace Interactor.Lock
{
    public class GetLockInfoInteractor : IGetLockInfoInputPort
    {
        private readonly ILockRepository _lockRepository;
        public GetLockInfoInteractor(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }

        public GetLockInfoOutputData Handle(GetLockInfoInputData inputData)
        {
            try
            {
                var lockInfs = _lockRepository.GetLockInfo(inputData.HpId, inputData.PtId, inputData.ListFunctionCdB, inputData.SinDate, inputData.RaiinNo);

                if (lockInfs.Any())
                {
                    return new GetLockInfoOutputData(lockInfs, GetLockInfoStatus.Successed);
                }

                return new GetLockInfoOutputData(new(), GetLockInfoStatus.NoData);
            }
            finally
            {
                _lockRepository.ReleaseResource();
            }

        }
    }
}
