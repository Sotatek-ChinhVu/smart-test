using Domain.Models.Diseases;
using Domain.Models.KarteInfs;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;

namespace Domain.Models.TodayOdr
{
    public interface ITodayOdrRepository
    {
        bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, List<KarteInfModel> karteInfModels);

        (List<OrdInfDetailModel>, List<CheckedDiseaseModel>) GetCheckDiseases(int hpId, int sinDate, List<PtDiseaseModel> todayByomeis, List<OrdInfModel> todayOdrs);

        List<CheckedDiseaseModel> GetByomeisOfCheckDiseases(bool isGridStyle, int hpId, string itemCd, int sinDate, List<CheckedDiseaseModel> checkedDiseases);
    }
}
