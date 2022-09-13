using Domain.Models.ReceptionComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.VisitingList.ReceptionComment;

namespace Interactor.ReceptionComment
{
    public class GetReceptionCommentInteractor : IGetReceptionCommentInputPort
    {
        private readonly IReceptionCommentRepository _receptionCommentRepository;
        public GetReceptionCommentInteractor(IReceptionCommentRepository receptionCommentRepository)
        {
            _receptionCommentRepository = receptionCommentRepository;
        }
        public GetReceptionCommentOutputData Handle(GetReceptionCommentInputData inputData)
        {
            if (inputData.RaiinNo <= 0)
            {
                return new GetReceptionCommentOutputData(new List<ReceptionCommentModel>(), GetReceptionCommentStatus.InvalidRaiinNo);
            }

            var listData = _receptionCommentRepository.GetReceptionComments(inputData.RaiinNo);
            if (listData == null || listData.Count == 0)
            {
                return new GetReceptionCommentOutputData(new(), GetReceptionCommentStatus.NoData);
            }
            return new GetReceptionCommentOutputData(listData, GetReceptionCommentStatus.Success);
        }
    }
}
