using Domain.Models.OrdInfs;
using Domain.Models.Reception;
using UseCase.OrdInfs.GetHeaderInf;

namespace Interactor.OrdInfs
{
    public class GetHeaderInfInteractor : IGetHeaderInfInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IReceptionRepository _receptionRepository;

        public GetHeaderInfInteractor(IOrdInfRepository ordInfRepository, IReceptionRepository receptionRepository)
        {
            _ordInfRepository = ordInfRepository;
            _receptionRepository = receptionRepository;
        }

        public GetHeaderInfOutputData Handle(GetHeaderInfInputData inputData)
        {
            try
            {
                if (inputData.RaiinNo <= 0)
                {
                    return new GetHeaderInfOutputData(GetHeaderInfStatus.InvalidRaiinNo);
                }
                if (inputData.HpId <= 0)
                {
                    return new GetHeaderInfOutputData(GetHeaderInfStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new GetHeaderInfOutputData(GetHeaderInfStatus.InvalidPtId);

                }
                if (inputData.SinDate <= 0)
                {
                    return new GetHeaderInfOutputData(GetHeaderInfStatus.InvalidSinDate);
                }

                var raiinNo = _receptionRepository.Get(inputData.RaiinNo);
                var odrInf = _ordInfRepository.GetHeaderInfo(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);

                if (odrInf.HpId == 0 && odrInf.PtId == 0 && odrInf.SinDate == 0 && odrInf.RaiinNo == 0 && raiinNo.HpId == 0 && raiinNo.PtId == 0 && raiinNo.RaiinNo == 0 && raiinNo.SinDate == 0)
                {
                    return new GetHeaderInfOutputData(GetHeaderInfStatus.NoData);
                }

                return new GetHeaderInfOutputData(raiinNo.SyosaisinKbn, raiinNo.JikanKbn, raiinNo.HokenPid, raiinNo.SanteiKbn, raiinNo.TantoId, raiinNo.KaId, raiinNo.UketukeTime, raiinNo.SinStartTime, raiinNo.SinEndTime, odrInf, GetHeaderInfStatus.Successed);
            }
            catch
            {
                return new GetHeaderInfOutputData(GetHeaderInfStatus.Failed);
            }
        }
    }
}
