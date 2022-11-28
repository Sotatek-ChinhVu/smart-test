namespace EmrCloudApi.Requests.Insurance
{
    public class GetInsuranceListRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
    }
}