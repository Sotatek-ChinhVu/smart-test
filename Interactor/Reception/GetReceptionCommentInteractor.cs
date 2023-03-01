using Domain.Models.Reception;
using UseCase.Reception.ReceptionComment;

namespace Interactor.Reception
{
    public class GetReceptionCommentInteractor : IGetReceptionCommentInputPort
    {
        private readonly IReceptionRepository _receptionRepository;
        public GetReceptionCommentInteractor(IReceptionRepository receptionCommentRepository)
        {
            _receptionRepository = receptionCommentRepository;
        }
        public GetReceptionCommentOutputData Handle(GetReceptionCommentInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetReceptionCommentOutputData(new(), GetReceptionCommentStatus.InvalidHpId);
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new GetReceptionCommentOutputData(new(), GetReceptionCommentStatus.InvalidRaiinNo);
                }

                var data = _receptionRepository.GetReceptionComments(inputData.HpId, inputData.RaiinNo);

                return new GetReceptionCommentOutputData(new ReceptionDto(
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
                    ), GetReceptionCommentStatus.Success);
            }
            catch (Exception)
            {
                return new GetReceptionCommentOutputData(GetReceptionCommentStatus.Failed);
            }
            finally
            {
                _receptionRepository.ReleaseResource();
            }
        }
    }
}
