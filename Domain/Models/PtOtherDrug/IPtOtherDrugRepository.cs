namespace Domain.Models.PtOtherDrug
{
    public interface IPtOtherDrugRepository
    {
        List<PtOtherDrugModel> GetList(long ptId);
    }
}
