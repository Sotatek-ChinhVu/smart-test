namespace EmrCloudApi.Requests.ReceptionInsurance
{
    public class ReceptionInsuranceRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public bool IsShowExpiredReception { get; set; }
    }
}
