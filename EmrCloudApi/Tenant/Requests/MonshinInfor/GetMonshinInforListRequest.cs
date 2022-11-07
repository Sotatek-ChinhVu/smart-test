namespace EmrCloudApi.Tenant.Requests.MonshinInfor
{
    public class GetMonshinInforListRequest
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
