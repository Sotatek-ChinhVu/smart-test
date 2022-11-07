namespace EmrCloudApi.Tenant.Requests.ReceptionInsurance
{
    public class ReceptionInsuranceRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public bool IsShowExpiredReception { get; set; }
    }
}
