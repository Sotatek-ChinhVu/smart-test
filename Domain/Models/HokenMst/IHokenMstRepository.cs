namespace Domain.Models.HokenMst
{
    public interface IHokenMstRepository
    {
        HokenMasterModel GetHokenMaster(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int sinDate);
        List<HokenMasterModel> CheckExistHokenEdaNo(int hokenNo);
    }
}
