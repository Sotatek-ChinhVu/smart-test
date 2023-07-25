using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.GetList;

public class GetListFlowSheetInputData : IInputData<GetListFlowSheetOutputData>
{
    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public bool IsHolidayOnly { get; private set; }

    public bool IsRaiinListMstOnly { get; private set; }

    public int HolidayFrom { get; private set; }

    public int HolidayTo { get; private set; }

    public int PageIndex { get; private set; }

    public int PageSize { get; private set; }

    public Dictionary<string, string> SortData { get; private set; }

    public GetListFlowSheetInputData(int hpId, long ptId, int sinDate, long raiinNo, bool isHolidayOnly, int holidayFrom, int holidayTo, bool isRaiinListMstOnly, int pageIndex, int pageSize, Dictionary<string, string> sortData)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        IsHolidayOnly = isHolidayOnly;
        HolidayFrom = holidayFrom;
        HolidayTo = holidayTo;
        IsRaiinListMstOnly = isRaiinListMstOnly;
        PageIndex = pageIndex;
        PageSize = pageSize;
        SortData = sortData;
    }
}
