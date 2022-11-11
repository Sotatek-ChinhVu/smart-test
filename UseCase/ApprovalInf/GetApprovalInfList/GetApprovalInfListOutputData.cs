using Domain.Models.ApprovalInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.ApprovalInf.GetApprovalInfList
{
    public class GetApprovalInfListOutputData : IOutputData
    {
        public List<ApprovalInfModel> ApprovalInfList { get; private set; }
        public GetApprovalInfListStatus Status { get; private set; }
        public GetApprovalInfListOutputData(List<ApprovalInfModel> approvalInfList, GetApprovalInfListStatus status)
        {
            ApprovalInfList = approvalInfList;
            Status = status;
        }
    }
}
