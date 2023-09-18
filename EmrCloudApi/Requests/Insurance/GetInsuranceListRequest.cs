namespace EmrCloudApi.Requests.Insurance
{
    public class GetInsuranceListRequest
    {
        public long PtId { get; set; }

        public int SinDate { get; set; }

        public byte SortType { get; set; }
    }
}