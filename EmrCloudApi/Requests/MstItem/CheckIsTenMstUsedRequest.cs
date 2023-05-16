namespace EmrCloudApi.Requests.MstItem
{
    public class CheckIsTenMstUsedRequest
    {
        public string ItemCd { get; set; } = string.Empty;

        public int StartDate { get; set; }

        public int EndDate { get; set; }
    }
}
