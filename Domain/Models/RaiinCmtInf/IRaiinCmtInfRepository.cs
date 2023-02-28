using Domain.Common;

namespace Domain.Models.RaiinCmtInf;

public interface IRaiinCmtInfRepository : IRepositoryBase
{
    void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int cmtKbn, string text, int userId);

    string GetRaiinCmtByPtId(int hpId, long ptId, int sindate, long raiinNo);
}
