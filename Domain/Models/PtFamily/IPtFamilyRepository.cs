namespace Domain.Models.PtFamily
{
    public interface IPtFamilyRepository
    {
        List<PtFamilyModel> GetList(long ptId, int hpId);
    }
}
