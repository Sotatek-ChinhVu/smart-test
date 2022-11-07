namespace EmrCloudApi.Tenant.Requests.PatientInfor.PtKyuseiInf
{
    public class GetPtKyuseiInfRequest
    {
        public long PtId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
