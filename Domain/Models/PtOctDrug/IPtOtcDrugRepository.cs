namespace Domain.Models.PtOtcDrug
{
    public interface IPtOtcDrugRepository
    {
        List<PtOtcDrugModel> GetList(long ptId);
    }
}
