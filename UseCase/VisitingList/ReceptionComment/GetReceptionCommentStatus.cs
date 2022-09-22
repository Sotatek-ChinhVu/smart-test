using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.VisitingList.ReceptionComment
{
    public enum GetReceptionCommentStatus : byte
    {
        Success = 1,
        InvalidRaiinNo = 2,
        InvalidHpId = 3,
        Failed = 4
    }
}
