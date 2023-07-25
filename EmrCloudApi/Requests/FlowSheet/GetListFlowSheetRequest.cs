namespace EmrCloudApi.Requests.FlowSheet
{
    public class GetListFlowSheetRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public bool IsHolidayOnly { get; set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public Dictionary<string, string> SortData { get; private set; } = new();
    }
}
