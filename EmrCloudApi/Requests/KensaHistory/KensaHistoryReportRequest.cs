namespace EmrCloudApi.Requests.KensaHistory
{
    public class KensaHistoryReportRequest
    {
        public int UserId {  get; set; }
        public long PtId { get; set; }
        public int SetId { get; set; } = 0;
        public int IraiDate { get; set; } = 0;
        public int StartDate { get; set; } = 0;
        public int EndDate { get; set; } = 0;
        public bool ShowAbnormalKbn { get; set; } = false;
        public int SinDate { get; set; }
    }
}
