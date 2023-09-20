namespace EmrCloudApi.Requests.MstItem
{
    public class F17CommonRequest
    {
        public string UsingKensaItemCd { get; set; } = string.Empty;

        public List<string> UsingItemCds { get; set; } = new List<string>();

        public string kensaStdItemCd { get; set; } = string.Empty;
    }
}
