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
                    return new GetReceptionCommentOutputData(new ReceptionModel(), GetReceptionCommentStatus.InvalidHpId);
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new GetReceptionCommentOutputData(new ReceptionModel(), GetReceptionCommentStatus.InvalidRaiinNo);
                }

                var Data = _receptionRepository.GetReceptionComments(inputData.HpId, inputData.RaiinNo);

                return new GetReceptionCommentOutputData(Data, GetReceptionCommentStatus.Success);
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
