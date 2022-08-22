namespace Domain.Models.PhysicalInfo
{
    public interface IPhysicalInfoRepository
    {
        List<PhysicalInfoModel> GetList(int hpId, long ptId);
    }
}
