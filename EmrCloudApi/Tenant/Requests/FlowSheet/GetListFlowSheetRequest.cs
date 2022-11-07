namespace EmrCloudApi.Tenant.Requests.FlowSheet
{
    public class GetListFlowSheetRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public int StartIndex { get; set; }

        public int Count { get; set; }

        public bool IsHolidayOnly { get; set; }

        public string Sort { get; set; } = string.Empty;
    }
}
