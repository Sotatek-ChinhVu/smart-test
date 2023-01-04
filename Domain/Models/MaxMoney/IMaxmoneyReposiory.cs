using Domain.Common;

namespace Domain.Models.MaxMoney
{
    public interface IMaxmoneyReposiory : IRepositoryBase
    {
        List<LimitListModel> GetListLimitModel(long ptId, int hpId);
        MaxMoneyInfoHokenModel GetInfoHokenMoney(int hpId, long ptId, int kohiId, int sinYm);
        bool SaveMaxMoney(List<LimitListModel> dataInputs, int hpId, long ptId, int kohiId, int sinYm, int userId);
    }
}
