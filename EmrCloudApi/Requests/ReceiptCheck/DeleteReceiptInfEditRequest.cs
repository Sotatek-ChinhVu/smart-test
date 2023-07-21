namespace EmrCloudApi.Requests.ReceiptCheck
{
    public class DeleteReceiptInfEditRequest
    {
        public int PtId { get; set; }

        public int SeikyuYm { get; set; }

        public int SinYm { get; set; }

        public int HokenId { get; set; }
    }
}
