namespace EmrCloudApi.Requests.KensaHistory
{
    public class GetListKensaInfDetailRequest
    {
        public int PtId { get; set; }
        public int SetId { get; set; } = 0;
        public int IraiDate { get; set; } = 0;
        public int StartDate { get; set; } = 0;
        public bool ShowAbnormalKbn { get; set; } = false;
        public int ItemQuantity { get; set; } = 0;
    }
}
