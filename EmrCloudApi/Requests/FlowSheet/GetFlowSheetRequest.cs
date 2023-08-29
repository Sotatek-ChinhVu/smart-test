namespace EmrCloudApi.Requests.FlowSheet
{
    public class GetFlowSheetTooltipRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public bool IsAll { get; set; }
    }
}
