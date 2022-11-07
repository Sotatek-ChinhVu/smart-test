namespace EmrCloudApi.Tenant.Requests.PatientInfor
{
    public class GetByIdRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int RaiinNo { get; set; }
    }
}