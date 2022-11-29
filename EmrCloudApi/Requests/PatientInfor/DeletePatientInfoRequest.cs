namespace EmrCloudApi.Requests.PatientInfor
{
    public class DeletePatientInfoRequest
    {
        public DeletePatientInfoRequest(int hpId, long ptId)
        {
            HpId = hpId;
            PtId = ptId;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
    }
}
