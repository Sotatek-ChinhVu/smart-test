namespace EmrCloudApi.Requests.MstItem
{
    public class GetListKensaIjiSettingRequest
    {
        public string KeyWords { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public bool IsExpired { get; set; }
    }
}
