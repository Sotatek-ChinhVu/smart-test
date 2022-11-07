namespace EmrCloudApi.Tenant.Requests.Diseases
{
    public class GetPtDiseaseListRequest
    {
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int HokenId { get; set; }
        public int RequestFrom { get; set; }

    }
}
