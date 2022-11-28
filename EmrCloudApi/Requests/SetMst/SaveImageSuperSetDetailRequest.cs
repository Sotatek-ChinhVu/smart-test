namespace EmrCloudApi.Requests.SetMst
{
    public class SaveImageSuperSetDetailRequest
    {
        public int HpId { get; set; }
        public int SetCd { get; set; }
        public int Position { get; set; }
        public string OldImage { get; set; } = string.Empty;
    }
}
