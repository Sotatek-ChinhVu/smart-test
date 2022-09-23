using Domain.Models.OrdInfs;
using UseCase.OrdInfs.GetHeaderInf;

namespace Interactor.OrdInfs
{
    public class GetHeaderInfInteractor : IGetHeaderInfInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        public GetHeaderInfInteractor(IOrdInfRepository ordInfRepository)
        {
            _ordInfRepository = ordInfRepository;
        }

        public GetHeaderInfOutputData Handle(GetHeaderInfInputData inputData)
        {
            try
            {
                if (inputData.RaiinNo <= 0)
                {
                    return new GetHeaderInfOutputData(new OrdInfModel(), GetHeaderInfStatus.InvalidRaiinNo);
                }
                if (inputData.HpId <= 0)
                {
                    return new GetHeaderInfOutputData(new OrdInfModel(), GetHeaderInfStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new GetHeaderInfOutputData(new OrdInfModel(), GetHeaderInfStatus.InvalidPtId);

                }
                if (inputData.SinDate <= 0)
                {
                    return new GetHeaderInfOutputData(new OrdInfModel(), GetHeaderInfStatus.InvalidSinDate);
                }

                var result = _ordInfRepository.GetHeaderInfo(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);

                if (result.HpId == 0 && result.PtId == 0 && result.SinDate == 0 && result.RaiinNo == 0)
                {
                    return new GetHeaderInfOutputData(new OrdInfModel(), GetHeaderInfStatus.NoData);
                }

                return new GetHeaderInfOutputData(result, GetHeaderInfStatus.Successed);
            }
            catch
            {
                return new GetHeaderInfOutputData(new OrdInfModel(), GetHeaderInfStatus.Failed);
            }
        }
    }
}
