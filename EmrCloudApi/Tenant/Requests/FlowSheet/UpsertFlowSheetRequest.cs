namespace EmrCloudApi.Tenant.Requests.FlowSheet
{
    public class UpsertFlowSheetRequest
    {
        public long RainNo { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int TagNo { get; set; }

        public int CmtKbn { get; set; }

        public string Text { get; set; } = string.Empty;
        public int SeqNo { get; set; }
    }
}