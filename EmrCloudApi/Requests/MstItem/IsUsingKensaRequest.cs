namespace EmrCloudApi.Requests.MstItem
{
    public class IsUsingKensaRequest
    {
        public string KensaItemCd { get; set; } = string.Empty;

        public List<string> itemCds { get; set; } = new List<string>();
    }
}
