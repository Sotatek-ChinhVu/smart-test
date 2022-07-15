namespace EmrCloudApi.Tenant.Requests.Diseases

{
    public class GetDiseaseListRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        // public ByomeiViewType RequestFrom
    }
}
