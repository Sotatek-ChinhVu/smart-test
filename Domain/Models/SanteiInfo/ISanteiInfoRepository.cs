namespace Domain.Models.SanteiInfo
{
    public interface ISanteiInfoRepository
    {
        List<SanteiInfoModel> GetList(int hpId, long ptId, int sinDate);
    }
}
