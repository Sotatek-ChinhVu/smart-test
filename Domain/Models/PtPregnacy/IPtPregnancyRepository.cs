namespace Domain.Models.PtPregnancy
{
    public interface IPtPregnancyRepository
    {
        List<PtPregnancyModel> GetList(long ptId, int hpId, int sinDate);
    }
}
