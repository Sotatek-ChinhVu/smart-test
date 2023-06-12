namespace EmrCloudApi.Requests.MonshinInfor
{
    public class GetMonshinInforListRequest
    {
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsGetAll { get; set; }
    }
}
