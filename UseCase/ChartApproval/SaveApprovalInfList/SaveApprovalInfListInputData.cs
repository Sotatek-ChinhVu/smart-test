using Domain.Models.ChartApproval;
using UseCase.Core.Sync.Core;

namespace UseCase.ChartApproval.SaveApprovalInfList
{
    public class SaveApprovalInfListInputData : IInputData<SaveApprovalInfListOutputData>
    {
        public SaveApprovalInfListInputData(List<ApprovalInfModel> approvalInfs, int hpId, int userId)
        {
            ApprovalInfs = approvalInfs;
            HpId = hpId;
            UserId = userId;
        }

        public List<ApprovalInfModel> ApprovalInfs { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }
    }
}