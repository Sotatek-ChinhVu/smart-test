namespace EmrCloudApi.Requests.KensaHistory
{
    public class KensaHistoryReportRequest
    {
        public int HpId {  get; set; }
        public int UserId {  get; set; }
        public int PtId { get; set; }
        public int SetId { get; set; } = 0;
        public int IraiCd { get; set; } = 0;
        public int SeikyuYm { get; set; } = 0;
        public int StartDate { get; set; } = 0;
        public int EndDate { get; set; } = 0;
        public bool ShowAbnormalKbn { get; set; } = false;
        public int ItemQuantity { get; set; } = 0;
    }
}
