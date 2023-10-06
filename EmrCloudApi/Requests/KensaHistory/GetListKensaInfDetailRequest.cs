namespace EmrCloudApi.Requests.KensaHistory
{
    public class GetListKensaInfDetailRequest
    {
        public int PtId { get; set; }
        public int SetId { get; set; } = 0;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
