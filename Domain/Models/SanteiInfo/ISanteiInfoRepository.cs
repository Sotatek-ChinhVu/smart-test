namespace Domain.Models.SanteiInfo
{
    public interface ISanteiInfoRepository
    {
        IEnumerable<SanteiInfoModel> GetList(int hpId, long ptId, int sinDate);
    }
}
