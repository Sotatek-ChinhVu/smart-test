using Domain.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.VisitingList.ReceptionComment;

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
            if (inputData.RaiinNo <= 0)
            {
                return new GetReceptionCommentOutputData(new List<ReceptionModel>(), GetReceptionCommentStatus.InvalidRaiinNo);
            }

            var listData = _receptionRepository.GetReceptionComments(inputData.RaiinNo);
            if (listData == null || listData.Count == 0)
            {
                return new GetReceptionCommentOutputData(new(), GetReceptionCommentStatus.NoData);
            }
            return new GetReceptionCommentOutputData(listData, GetReceptionCommentStatus.Success);
        }
    }
}
