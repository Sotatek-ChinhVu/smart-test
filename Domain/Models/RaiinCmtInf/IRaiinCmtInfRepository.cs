namespace Domain.Models.RaiinCmtInf;

public interface IRaiinCmtInfRepository
{
    void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int cmtKbn, string text, int userId);
}
