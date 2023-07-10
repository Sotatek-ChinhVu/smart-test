namespace EmrCloudApi.Requests.FlowSheet
{
    public class GetListFlowSheetRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public long RaiinNo { get; set; }

        public bool IsHolidayOnly { get; set; }
    }
}
