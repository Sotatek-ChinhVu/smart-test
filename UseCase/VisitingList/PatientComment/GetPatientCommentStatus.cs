using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.VisitingList.PatientComment
{
    public enum GetPatientCommentStatus : byte
    {
        Success = 1,
        InvalidData = 2,
        NoData = 3
    }
}
