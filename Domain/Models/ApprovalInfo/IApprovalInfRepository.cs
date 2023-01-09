using Domain.Common;

namespace Domain.Models.ApprovalInfo
{
    public interface IApprovalInfRepository : IRepositoryBase
    {
        List<ApprovalInfModel> GetList(int hpId, int starDate, int endDate, int kaId, int tantoId);

        void UpdateApprovalInfs(List<ApprovalInfModel> approvalInfs, int userId);

        bool CheckExistedId(List<int> ids);

        bool CheckExistedRaiinNo(List<long> raiinNos);
    }
}
