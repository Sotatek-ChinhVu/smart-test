using Domain.Common;
using Domain.Models.Diseases;
using Domain.Models.KarteInfs;
using Domain.Models.MstItem;
using Domain.Models.NextOrder;
using Domain.Models.OrdInfs;
using Domain.Models.RaiinKubunMst;

namespace Domain.Models.TodayOdr
{
    public interface ITodayOdrRepository : IRepositoryBase
    {
        bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId);

        int MonthsAfterExcludeHoliday(int hpId, int baseDate, int term);

        double SanteiCount(int hpId, long ptId, int startDate, int endDate, int sinDate, long raiinNo, List<string> itemCds, List<int> santeiKbns, List<int> hokenKbns);

        List<DensiSanteiKaisuModel> FindDensiSanteiKaisuList(int hpId, List<string> itemCds, int minSinDate, int maxSinDate);

        List<(string, string, List<CheckedDiseaseModel>)> GetCheckDiseases(int hpId, int sinDate, List<PtDiseaseModel> todayByomeis, List<OrdInfModel> todayOdrs);

        List<(int, int, List<Tuple<string, string, long>>)> GetAutoAddOrders(int hpId, long ptId, int sinDate, List<Tuple<int, int, string>> addingOdrList, List<Tuple<int, int, string, double>> currentOdrList);

        List<OrdInfModel> AutoAddOrders(int hpId, int userId, int sinDate, List<Tuple<int, int, string, int, int>> addingOdrList, List<Tuple<int, int, string, long>> autoAddItems);

        Dictionary<string, string> CheckNameChanged(List<OrdInfModel> odrInfModelList);

        (int type, string itemName, int lastDaySanteiRiha, string rihaItemName) GetValidGairaiRiha(int hpId, int ptId, long raiinNo, int sinDate, int syosaiKbn, List<OrdInfModel> allOdrInf);

        (double systemSetting, bool isExistYoboItemOnly) GetValidJihiYobo(int hpId, int syosaiKbn, int sinDate, List<OrdInfModel> allOrder);

        List<RaiinKbnModel> InitDefaultByTodayOrder(List<RaiinKbnModel> raiinKbns, List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns, List<RaiinKbnItemModel> raiinKbnItemCds, List<OrdInfModel> todayOrds);

        Dictionary<string, bool> ConvertInputItemToTodayOdr(int hpId, int sinDate, Dictionary<string, string> detailInfs);

        List<OrdInfModel> FromNextOrderToTodayOrder(int hpId, int sinDate, long raiinNo, int userId, List<RsvkrtOrderInfModel> rsvkrtOdrInfModels);

        List<(int type, string message, int odrInfPosition, int odrInfDetailPosition, TenItemModel tenItemMst, double suryo)> AutoCheckOrder(int hpId, int sinDate, long ptId, List<OrdInfModel> odrInfs);

        List<(int position, OrdInfModel odrInfModel)> ChangeAfterAutoCheckOrder(int hpId, int sinDate, int userId, long raiinNo, long ptId, List<OrdInfModel> odrInfs, List<Tuple<int, string, int, int, TenItemModel, double>> targetItems);
    }
}
