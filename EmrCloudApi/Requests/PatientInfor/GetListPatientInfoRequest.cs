namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class GetListPatientInfoRequest
    {
        public long PtId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
