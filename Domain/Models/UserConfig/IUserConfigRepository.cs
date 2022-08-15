namespace Domain.Models.UserConfig
{
    public interface IUserConfigRepository
    {
        List<UserConfigModel> GetList(int groupCd, int grpItemCd);
        List<UserConfigModel> GetList(int hpId, int groupCd, int grpItemCd, int userId);
        List<UserConfigModel> GetList(int hpId, int groupCd, int userId);
        List<UserConfigModel> GetList(int groupCd);
    }
}
