namespace EmrCloudApi.Requests.MonshinInfor
{
    public class GetMonshinInforListRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
