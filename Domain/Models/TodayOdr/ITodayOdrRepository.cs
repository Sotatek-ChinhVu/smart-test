using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;

namespace Domain.Models.TodayOdr
{
    public interface ITodayOdrRepository
    {
        bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId);
    }
}
