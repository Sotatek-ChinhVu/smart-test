namespace Domain.Models.RaiinKubunMst
{
    public interface IRaiinKubunMstRepository
    {
        List<RaiinKubunMstModel> GetList(bool isDeleted);

        List<RaiinKubunMstModel> LoadDataKubunSetting(int HpId);

        List<string> SaveDataKubunSetting(List<RaiinKubunMstModel> raiinKubunMstModels);

        List<string> GetListColumnName(int hpId);
    }
}
