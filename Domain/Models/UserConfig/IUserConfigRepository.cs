namespace Domain.Models.UserConfig
{
    public interface IUserConfigRepository
    {
        UserConfigModel? Get(int userId, int groupCd, int grpItemCd);
        List<UserConfigModel> GetList(int groupCd, int grpItemCd);
        List<UserConfigModel> GetList(int hpId, int groupCd, int grpItemCd, int userId);
        List<UserConfigModel> GetList(int hpId, int groupCd, int userId);
        List<UserConfigModel> GetList(int groupCd);
        List<UserConfigModel> GetListFT(int userId, int fromGrpCd, int toGrpCd);

    }
}
