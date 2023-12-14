using Domain.Common;

namespace Domain.Models.PtCmtInf;

public interface IPtCmtInfRepository : IRepositoryBase
{
    /// <summary>
    /// using hpId to get data
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="text"></param>
    /// <param name="userId"></param>
    void Upsert(int hpId, long ptId, string text, int userId);

    List<PtCmtInfModel> GetList(long ptId, int hpId);

    PtCmtInfModel GetPtCmtInfo(int hpId, long ptId);
}
