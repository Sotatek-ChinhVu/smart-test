namespace EmrCloudApi.Requests.Receipt
{
    public class GetMedicalDetailsRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int SinYm { get; set; }
        public int HokenId { get; set; }
    }
}
