namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class DeletePatientInfoRequest
    {
        public DeletePatientInfoRequest(long ptId)
        {
            PtId = ptId;
        }
        public long PtId { get; private set; }
    }
}
