using Domain.Models.RaiinListMst;

namespace Domain.Models.FlowSheet
{
    public interface IFlowSheetRepository
    {
        List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo, int startIndex, int count, ref long totalCount);

        List<RaiinListMstModel> GetRaiinListMsts(int hpId);

        List<HolidayModel> GetHolidayMst(int hpId, int holidayFrom, int holidayTo);

        void Upsert(List<FlowSheetModel> inputDatas);
    }
}
