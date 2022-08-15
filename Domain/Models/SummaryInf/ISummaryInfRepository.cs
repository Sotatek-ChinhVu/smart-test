namespace Domain.Models.SummaryInf
{
    public interface ISummaryInfRepository
    {
        List<SummaryInfModel> GetList(int hpId, long ptId);
    }
}
