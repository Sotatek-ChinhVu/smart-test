using Domain.Common;

namespace Domain.Models.ApprovalInfo
{
    public interface IApprovalInfRepository : IRepositoryBase
    {
        List<ApprovalInfModel> GetList(int hpId, int starDate, int endDate, int kaId, int tantoId);
    }
}
