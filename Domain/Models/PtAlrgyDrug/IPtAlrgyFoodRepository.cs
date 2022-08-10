namespace Domain.Models.PtAlrgyDrug
{
    public interface IPtAlrgyDrugRepository
    {
        List<PtAlrgyDrugModel> GetList(long ptId);
    }
}
