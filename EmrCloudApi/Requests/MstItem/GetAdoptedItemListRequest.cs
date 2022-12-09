namespace EmrCloudApi.Requests.MstItem
{
    public class GetAdoptedItemListRequest
    {
        public int SinDate { get; set; }

        public List<string> ItemCds { get; set; } = new();
    }
}
