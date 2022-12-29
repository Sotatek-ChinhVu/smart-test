using Domain.Common;

namespace Domain.Models.RaiinKbn;

public interface IRaiinKbnRepository : IRepositoryBase
{
    void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int grpId, int kbnCd, int userId);

    bool SoftDelete(int hpId, long ptId, int sinDate, long raiinNo, int grpId);

    List<RaiinKbnModel> GetRaiinKbns(int hpId, long ptId, long raiinNo, int sinDate);

    List<RaiinKbnModel> InitDefaultByRsv(int hpId, int frameID, List<RaiinKbnModel> raiinKbns);
}
