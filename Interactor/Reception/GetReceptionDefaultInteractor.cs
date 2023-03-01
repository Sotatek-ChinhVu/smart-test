using Domain.Models.Reception;
using UseCase.Reception.GetReceptionDefault;

namespace Interactor.Reception
{
    public class GetReceptionDefaultInteractor : IGetReceptionDefaultInputPort
    {
        private readonly IReceptionRepository _receptionRepository;
        public GetReceptionDefaultInteractor(IReceptionRepository receptionRepository)
        {
            _receptionRepository = receptionRepository;
        }

        public GetReceptionDefaultOutputData Handle(GetReceptionDefaultInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new GetReceptionDefaultOutputData(new(), GetReceptionDefaultStatus.InvalidHpId);
                }

                if (inputData.PtId < 0)
                {
                    return new GetReceptionDefaultOutputData(new(), GetReceptionDefaultStatus.InvalidPtId);
                }

                if (inputData.Sindate < 0)
                {
                    return new GetReceptionDefaultOutputData(new(), GetReceptionDefaultStatus.InvalidSindate);
                }

                if (inputData.DefaultDoctorSetting < 0)
                {
                    return new GetReceptionDefaultOutputData(new(), GetReceptionDefaultStatus.InvalidDefautDoctorSetting);
                }

                var data = _receptionRepository.GetDataDefaultReception(inputData.HpId, inputData.PtId, inputData.Sindate, inputData.DefaultDoctorSetting);
                return new GetReceptionDefaultOutputData(new ReceptionDto(
                        data.HpId,
                        data.PtId,
                        data.SinDate,
                        data.RaiinNo,
                        data.OyaRaiinNo,
                        data.HokenPid,
                        data.SanteiKbn,
                        data.Status,
                        data.IsYoyaku,
                        data.YoyakuTime,
                        data.YoyakuId,
                        data.UketukeSbt,
                        data.UketukeTime,
                        data.UketukeId,
                        data.UketukeNo,
                        data.SinStartTime,
                        data.SinEndTime,
                        data.KaikeiTime,
                        data.KaikeiId,
                        data.KaId,
                        data.TantoId,
                        data.SyosaisinKbn,
                        data.JikanKbn,
                        data.Comment
                    ), GetReceptionDefaultStatus.Successed);
            }
            catch
            {
                return new GetReceptionDefaultOutputData(new(), GetReceptionDefaultStatus.Failed);
            }
            finally
            {
                _receptionRepository.ReleaseResource();
            }
        }
    }
}
