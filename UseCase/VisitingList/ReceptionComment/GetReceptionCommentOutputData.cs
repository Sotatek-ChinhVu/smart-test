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
        public ReceptionModel ReceptionModel { get; private set; }

        public GetReceptionCommentStatus Status { get; private set; }

        public GetReceptionCommentOutputData(ReceptionModel receptionModel, GetReceptionCommentStatus status)
        {
            ReceptionModel = receptionModel;
            Status = status;
        }

        public GetReceptionCommentOutputData(GetReceptionCommentStatus status)
        {
            ReceptionModel = new ReceptionModel();
            Status = status;
        }
    }
}
