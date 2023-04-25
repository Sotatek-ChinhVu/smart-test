using Domain.Common;

namespace Domain.Models.ChartApproval
{
    public interface IApprovalInfRepository : IRepositoryBase
    {
        List<ApprovalInfModel> GetList(int hpId, int starDate, int endDate, int kaId, int tantoId);

        bool SaveApprovalInfs(List<ApprovalInfModel> approvalInfs, int hpId, int userId);
    }
}
