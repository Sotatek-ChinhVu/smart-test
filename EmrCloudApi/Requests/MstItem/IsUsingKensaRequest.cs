namespace EmrCloudApi.Requests.MstItem
{
    public class IsUsingKensaRequest
    {
        public string KensaItemCd { get; set; } = string.Empty;

        public List<string> ItemCds { get; set; } = new List<string>();
    }
}
