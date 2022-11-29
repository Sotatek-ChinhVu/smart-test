using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.ApprovalInfo.UpdateApprovalInfList;

public enum UpdateApprovalInfListStatus
{
    Success = 1,
    ApprovalInfoListInputNoData,
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
    Failed,
}
