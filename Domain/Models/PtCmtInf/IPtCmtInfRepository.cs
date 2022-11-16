namespace Domain.Models.PtCmtInf;

public interface IPtCmtInfRepository
{
    void Upsert(long ptId, string text, int userId);
    List<PtCmtInfModel> GetList(long ptId, int hpId);
}
