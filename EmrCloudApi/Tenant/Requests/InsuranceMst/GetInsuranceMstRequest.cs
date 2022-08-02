namespace EmrCloudApi.Tenant.Requests.Insurance
{
    public class GetInsuranceMstRequest
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int SinDate { get; set; }

        public int HokenId { get; set; }
    }
}