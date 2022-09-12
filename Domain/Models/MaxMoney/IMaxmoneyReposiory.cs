namespace Domain.Models.MaxMoney
{
    public interface IMaxmoneyReposiory
    {
        List<MaxMoneyModel> GetListMaxMoney(long ptId, int hpId);
    }
}
