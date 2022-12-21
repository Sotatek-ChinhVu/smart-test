using Domain.Models.OrdInfs;
using UseCase.OrdInfs.GetMaxRpNo;

namespace Interactor.OrdInfs
{
    public class GetMaxRpNoInteractor : IGetMaxRpNoInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        public GetMaxRpNoInteractor(IOrdInfRepository ordInfRepository)
        {
            _ordInfRepository = ordInfRepository;
        }

        public GetMaxRpNoOutputData Handle(GetMaxRpNoInputData inputData)
        {
            try
            {

                if (inputData.RaiinNo <= 0)
                {
                    return new GetMaxRpNoOutputData(0, GetMaxRpNoStatus.InvalidRaiinNo);
                }
                if (inputData.HpId <= 0)
                {
                    return new GetMaxRpNoOutputData(0, GetMaxRpNoStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new GetMaxRpNoOutputData(0, GetMaxRpNoStatus.InvalidPtId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetMaxRpNoOutputData(0, GetMaxRpNoStatus.InvalidSinDate);
                }
                var maxRpNo = _ordInfRepository.GetMaxRpNo(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);
                return new GetMaxRpNoOutputData(maxRpNo, GetMaxRpNoStatus.Successed);
            }
            catch
            {
                return new GetMaxRpNoOutputData(0, GetMaxRpNoStatus.Failed);
            }
            finally
            {
                _ordInfRepository.ReleaseResource();
            }
        }
    }
}
