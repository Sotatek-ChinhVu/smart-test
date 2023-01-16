using Domain.Common;
using Domain.Models.RaiinListMst;

namespace Domain.Models.FlowSheet
{
    public interface IFlowSheetRepository : IRepositoryBase
    {
        List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo, int startIndex, int count, string sort, ref long totalCount);

        List<RaiinListInfModel> GetRaiinListInf(int hpId, long ptId);

        List<RaiinListMstModel> GetRaiinListMsts(int hpId);

        List<HolidayModel> GetHolidayMst(int hpId, int holidayFrom, int holidayTo);

        void UpsertTag(List<FlowSheetModel> inputDatas, int hpId, int userId);

        void UpsertCmt(List<FlowSheetModel> inputDatas, int hpId, int userId);
    }
}
