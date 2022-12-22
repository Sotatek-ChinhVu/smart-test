using Domain.Common;

namespace Domain.Models.MonshinInf
{
    public interface IMonshinInforRepository : IRepositoryBase
    {
        bool SaveList(List<MonshinInforModel> monshinInforModels, int userId);

        public List<MonshinInforModel> MonshinInforModels(int hpId, long ptId, int sinDate, bool isDeleted);
    }
}
 