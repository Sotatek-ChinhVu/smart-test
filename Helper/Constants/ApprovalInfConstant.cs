using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Constants
{
    public static class ApprovalInfConstant
    {
        public enum ValidationStatus
        {
            InvalidHpId,
            InvalidId,
            InvalidIsDeleted,
            InvalidRaiinNo,
            InvalidSeqNo,
            InvalidPtId,
            InvalidSinDate,
            InvalidCreateMachine,
            InvalidCreateId,
            InvalidUpdateMachine,
            InvalidUpdateId,
            ApprovalInfListInvalidNoExistedId,
            ApprovalInfListExistedInputData,
            ApprovalInfListInvalidNoExistedRaiinNo,
            Valid
        }
    }
}
