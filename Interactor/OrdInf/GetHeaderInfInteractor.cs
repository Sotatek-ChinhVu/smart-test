using Domain.Models.OrdInfs;
using Domain.Models.RainListTag;
using Domain.Models.Reception;
using UseCase.OrdInfs.GetHeaderInf;

namespace Interactor.OrdInfs
{
    public class GetHeaderInfInteractor : IGetHeaderInfInputPort
    {
        private readonly IOrdInfRepository _ordInfRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly IRaiinListTagRepository _raiinListTagRepository;

        public GetHeaderInfInteractor(IOrdInfRepository ordInfRepository, IReceptionRepository receptionRepository, IRaiinListTagRepository raiinListTagRepository)
        {
            _ordInfRepository = ordInfRepository;
            _receptionRepository = receptionRepository;
            _raiinListTagRepository = raiinListTagRepository;
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

                var reception = _receptionRepository.Get(inputData.RaiinNo);
                var odrInf = _ordInfRepository.GetHeaderInfo(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);
                var raiinTag = _raiinListTagRepository.Get(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);

                if (odrInf.HpId == 0 && odrInf.PtId == 0 && odrInf.SinDate == 0 && odrInf.RaiinNo == 0 && reception.HpId == 0 && reception.PtId == 0 && reception.RaiinNo == 0 && reception.SinDate == 0)
                {
                    return new GetHeaderInfOutputData(GetHeaderInfStatus.NoData);
                }

                return new GetHeaderInfOutputData(reception.SyosaisinKbn, reception.JikanKbn, reception.HokenPid, reception.SanteiKbn, reception.TantoId, reception.KaId, reception.UketukeTime, reception.SinStartTime, reception.SinEndTime, raiinTag.TagNo, odrInf, GetHeaderInfStatus.Successed);
            }
            catch
            {
                return new GetHeaderInfOutputData(GetHeaderInfStatus.Failed);
            }
        }
    }
}
