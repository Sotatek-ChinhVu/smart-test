using Domain.Common;
using Domain.Models.RaiinListMst;

namespace Domain.Models.FlowSheet
{
    public interface IFlowSheetRepository : IRepositoryBase
    {
        List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo, ref long totalCount);

        Dictionary<long, List<RaiinListInfModel>> GetRaiinListInf(int hpId, long ptId);

        Dictionary<int, List<RaiinListInfModel>> GetRaiinListInfForNextOrder(int hpId, long ptId);

        List<RaiinListMstModel> GetRaiinListMsts(int hpId);

        bool SaveHolidayMst(HolidayModel holiday, int userId);

        List<HolidayDto> GetHolidayMst(int hpId, int holidayFrom, int holidayTo);

        void UpsertTag(List<FlowSheetModel> inputDatas, int hpId, int userId);

        void UpsertCmt(List<FlowSheetModel> inputDatas, int hpId, int userId);

        List<(int date, string tooltip)> GetTooltip(int hpId, long ptId, int sinDate, int startDate, int endDate, bool isAll);
    }
}
