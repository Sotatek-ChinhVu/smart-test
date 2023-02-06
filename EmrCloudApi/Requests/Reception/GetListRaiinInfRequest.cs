namespace EmrCloudApi.Tenant.Requests.Reception
{
    public class GetListRaiinInfRequest
    {
        public long PtId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}