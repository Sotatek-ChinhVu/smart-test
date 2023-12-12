using Domain.Common;

namespace Domain.Models.PtCmtInf;

public interface IPtCmtInfRepository : IRepositoryBase
{
    void Upsert(int hpId, long ptId, string text, int userId);

    List<PtCmtInfModel> GetList(long ptId, int hpId);

    PtCmtInfModel GetPtCmtInfo(int hpId, long ptId);
}
