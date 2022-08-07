namespace Domain.Models.PtCmtInf;

public interface IPtCmtInfRepository
{
    void Upsert(long ptId, string text);
}
