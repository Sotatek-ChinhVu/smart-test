namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class SearchEmptyIdResquest
    {
        public long PtNum { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}