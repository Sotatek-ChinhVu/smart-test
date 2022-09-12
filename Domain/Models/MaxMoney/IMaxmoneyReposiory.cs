namespace Domain.Models.MaxMoney
{
    public interface IMaxmoneyReposiory
    {
        List<LimitListModel> GetListLimitModel(long ptId, int hpId);
    }
}
