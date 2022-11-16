using Domain.Models.KarteInfs;
using Domain.Models.OrdInfs;

namespace Domain.Models.TodayOdr
{
    public interface ITodayOdrRepository
    {
        bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId);

        int MonthsAfterExcludeHoliday(int hpId, int baseDate, int term);

        double SanteiCount(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, List<string> itemCds, List<int> santeiKbns, List<int> hokenKbns);

        List<DensiSanteiKaisuModel> FindDensiSanteiKaisuList(int hpId, List<string> itemCds, int minSinDate, int maxSinDate);
    }
}
