namespace Domain.Models.MaxMoney
{
    public interface IMaxmoneyReposiory
    {
        List<LimitListModel> GetListLimitModel(long ptId, int hpId);
        MaxMoneyInfoHokenModel GetInfoHokenMoney(int hpId, long ptId, int kohiId, int sinYm);
        bool SaveMaxMoney(List<LimitListModel> dataInputs, int hpId, long ptId, int kohiId, int sinYm);
    }
}
