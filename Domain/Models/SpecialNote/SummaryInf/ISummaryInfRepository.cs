namespace Domain.Models.SpecialNote.SummaryInf
{
    public interface ISummaryInfRepository
    {
        SummaryInfModel Get(int hpId, long ptId);
    }
}
