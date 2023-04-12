using Domain.Common;

namespace Domain.Models.MonshinInf
{
    public interface IMonshinInforRepository : IRepositoryBase
    {
        bool SaveList(List<MonshinInforModel> monshinInforModels, int userId);

        List<MonshinInforModel> GetMonshinInfor(int hpId, long ptId, long raiinNo, bool isDeleted, bool isGetAll = true);
    }
}
