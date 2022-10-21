namespace EmrCloudApi.Tenant.Requests.SwapHoken
{
    public class SaveSwapHokenRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int HokenIdBefore { get; set; }
        public int HokenIdAfter { get; set; }
        public int HokenPidBefore { get; set; }
        public int HokenPidAfter { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
    }
}
