namespace Domain.Models.ApprovalInfo
{
    public interface IApprovalInfRepository
    {
        List<ApprovalInfModel> GetList(int hpId, int starDate, int endDate, int kaId, int tantoId);
    }
}
