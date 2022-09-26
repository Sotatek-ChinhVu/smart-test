using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.PatientInfor.PatientComment
{
    public enum GetPatientCommentStatus : byte
    {
        Success = 1,
        InvalidHpId = 2,
        InvalidPtId = 3,
        Failed = 4
    }
}
