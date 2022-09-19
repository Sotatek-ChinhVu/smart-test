using Domain.Models.Reception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.VisitingList.ReceptionComment
{
    public class GetReceptionCommentOutputData : IOutputData
    {
        public List<ReceptionModel> ReceptionComments { get; private set; }

        public GetReceptionCommentStatus Status { get; private set; }

        public GetReceptionCommentOutputData(List<ReceptionModel> receptionComments, GetReceptionCommentStatus status)
        {
            ReceptionComments = receptionComments;
            Status = status;
        }
    }
}
