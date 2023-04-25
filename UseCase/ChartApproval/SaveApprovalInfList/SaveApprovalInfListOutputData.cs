using Domain.Models.ChartApproval;
using Helper.Constants;
using UseCase.Core.Sync.Core;

namespace UseCase.ChartApproval.SaveApprovalInfList
{
    public class SaveApprovalInfListOutputData : IOutputData
    {
        public SaveApprovalInfListOutputData(SaveApprovalInfStatus status)
        {
            Status = status;
        }

        public SaveApprovalInfStatus Status { get; private set; }
    }
}