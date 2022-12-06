namespace Domain.Models.ApprovalInfo
{
    public interface IApprovalInfRepository
    {
        List<ApprovalInfModel> GetList(int hpId, int starDate, int endDate, int kaId, int tantoId);

        void UpdateApprovalInfs(List<ApprovalInfModel> approvalInfs);

        bool CheckExistedId(List<int> ids);

        bool CheckExistedRaiinNo(List<long> raiinNos);
    }
}
