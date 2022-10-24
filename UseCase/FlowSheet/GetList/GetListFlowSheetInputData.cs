using UseCase.Core.Sync.Core;

namespace UseCase.FlowSheet.GetList
{
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

        public int StartIndex { get; private set; }

        public int Count { get; private set; }

        public string Sort { get; private set; }

        public GetListFlowSheetInputData(int hpId, long ptId, int sinDate, long raiinNo, bool isHolidayOnly, int holidayFrom, int holidayTo, bool isRaiinListMstOnly, int startIndex, int count, string sort)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            IsHolidayOnly = isHolidayOnly;
            HolidayFrom = holidayFrom;
            HolidayTo = holidayTo;
            IsRaiinListMstOnly = isRaiinListMstOnly;
            StartIndex = startIndex;
            Count = count;
            Sort = sort;
        }
    }
}
