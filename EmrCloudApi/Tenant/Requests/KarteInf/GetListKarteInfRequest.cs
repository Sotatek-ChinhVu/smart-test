namespace EmrCloudApi.Tenant.Requests.KarteInfs
{
    public class GetListKarteInfRequest
    {
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
        public int IsDeleted { get; set; }
    }
}
