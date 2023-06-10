using Domain.Common;

namespace Domain.Models.ChartApproval
{
    public interface IApprovalInfRepository : IRepositoryBase
    {
        List<ApprovalInfModel> GetList(int hpId, int starDate, int endDate, int kaId, int tantoId);

        bool SaveApprovalInfs(List<ApprovalInfModel> approvalInfs, int hpId, int userId);

        bool NeedApprovalInf(int hpId, int startDate, int departmentId, int tantoId);

        void UpdateApproveInf(int hpId, long ptId, int sinDate, long raiinNo, int userId);
    }
}
