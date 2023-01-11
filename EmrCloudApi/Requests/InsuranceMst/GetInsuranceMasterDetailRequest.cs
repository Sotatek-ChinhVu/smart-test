namespace EmrCloudApi.Requests.InsuranceMst
{
    public class GetInsuranceMasterDetailRequest
    {
        public int FHokenNo { get; set; }

        public int FHokenSbtKbn { get; set; }

        public bool IsJitan { get; set; }

        public bool IsTaken { get; set; }
    }
}
