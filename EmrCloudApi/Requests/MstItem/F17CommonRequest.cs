namespace EmrCloudApi.Requests.MstItem
{
    public class F17CommonRequest
    {

        public F17CommonRequest(string kensaStdItemCd, string itemCd)
        {
            KensaStdItemCd = kensaStdItemCd;
            ItemCd = itemCd;
        }

        public string KensaStdItemCd { get; set; } = string.Empty;

        public string ItemCd { get; set; } = string.Empty;
    }
}
