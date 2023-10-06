using Domain.Models.Lock;
using UseCase.Lock.GetLockInf;

namespace Interactor.Lock
{
    public class GetLockInfInteractor : IGetLockInfInputPort
    {
        private readonly ILockRepository _lockRepository;

        public GetLockInfInteractor(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }
        public GetLockInfOutputData Handle(GetLockInfInputData inputData)
        {
            try
            {
                var data = _lockRepository.GetLockInfModels(inputData.HpId);
                if (data.Count == 0)
                {
                    return new GetLockInfOutputData(new(), GetLockInfStatus.NoData);
                }
                else
                {
                    return new GetLockInfOutputData(data, GetLockInfStatus.Successful);
                }
            }
            finally
            {
                _lockRepository.ReleaseResource();
            }
        }
    }
}
