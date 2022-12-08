namespace EmrCloudApi.Requests.MstItem
{
    public class UpdateAdoptedItemListRequest
    {
        public int ValueAdopted { get; set; }

        public int SinDate { get; set; }

        public List<string> ItemCds { get; set; } = new();
    }
}
