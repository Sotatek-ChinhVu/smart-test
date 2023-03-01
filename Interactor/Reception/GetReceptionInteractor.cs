using Domain.Models.Reception;
using UseCase.Reception.Get;

namespace Interactor.Reception
{
    public class GetReceptionInteractor : IGetReceptionInputPort
    {
        private readonly IReceptionRepository _receptionRepository;
        public GetReceptionInteractor(IReceptionRepository receptionRepository)
        {
            _receptionRepository = receptionRepository;
        }

        public GetReceptionOutputData Handle(GetReceptionInputData inputData)
        {
            try
            {
                if (inputData.RaiinNo <= 0)
                {
                    return new GetReceptionOutputData(new(), GetReceptionStatus.InvalidRaiinNo);
                }

                var receptionModel = _receptionRepository.Get(inputData.RaiinNo);
                if (receptionModel.HpId == 0 && receptionModel.PtId == 0 && receptionModel.SinDate == 0 && receptionModel.RaiinNo == 0)
                {
                    return new GetReceptionOutputData(new(), GetReceptionStatus.ReceptionNotExisted);
                }

                return new GetReceptionOutputData(new ReceptionDto(
                        receptionModel.HpId,
                        receptionModel.PtId,
                        receptionModel.SinDate,
                        receptionModel.RaiinNo,
                        receptionModel.OyaRaiinNo,
                        receptionModel.HokenPid,
                        receptionModel.SanteiKbn,
                        receptionModel.Status,
                        receptionModel.IsYoyaku,
                        receptionModel.YoyakuTime,
                        receptionModel.YoyakuId,
                        receptionModel.UketukeSbt,
                        receptionModel.UketukeTime,
                        receptionModel.UketukeId,
                        receptionModel.UketukeNo,
                        receptionModel.SinStartTime,
                        receptionModel.SinEndTime,
                        receptionModel.KaikeiTime,
                        receptionModel.KaikeiId,
                        receptionModel.KaId,
                        receptionModel.TantoId,
                        receptionModel.SyosaisinKbn,
                        receptionModel.JikanKbn,
                        receptionModel.Comment
                    ), GetReceptionStatus.Successed);
            }
            finally
            {
                _receptionRepository.ReleaseResource();
            }
        }
    }
}
