namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class SearchEmptyIdResquest
    {
        public int HpId { get; set; }
        public long PtNum { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}