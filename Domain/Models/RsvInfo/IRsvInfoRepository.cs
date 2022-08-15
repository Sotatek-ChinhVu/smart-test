namespace Domain.Models.RsvInfo
{
    public interface IRsvInfoRepository
    {
        List<RsvInfoModel> GetList(int hpId, long ptId, int sinDate);
    }
}
