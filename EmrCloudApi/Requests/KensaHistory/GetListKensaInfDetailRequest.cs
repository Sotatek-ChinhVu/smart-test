namespace EmrCloudApi.Requests.KensaHistory
{
    public class GetListKensaInfDetailRequest
    {
        public int PtId { get; set; }
        public int SetId { get; set; } = 0;
        public int IraiCd { get; set; } = 0;
        public int IraiCdStart { get; set; } = 0;
        public bool GetGetPrevious { get; set; } = false;
        public bool ShowAbnormalKbn { get; set; } = false;
        public int ItemQuantity { get; set; } = 0;
    }
}
