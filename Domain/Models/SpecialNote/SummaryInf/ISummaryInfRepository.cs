namespace Domain.Models.SpecialNote.SummaryInf
{
    public interface ISummaryInfRepository
    {
        SummaryInfModel GetList(int hpId, long ptId);
    }
}
