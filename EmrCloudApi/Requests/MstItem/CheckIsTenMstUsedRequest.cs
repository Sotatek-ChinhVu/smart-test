namespace EmrCloudApi.Requests.MstItem
{
    public class CheckIsTenMstUsedRequest
    {
        public CheckIsTenMstUsedRequest(string itemCd, int startDate, int endDate)
        {
            ItemCd = itemCd;
            StartDate = startDate;
            EndDate = endDate;
        }

        public string ItemCd { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }
    }
}
