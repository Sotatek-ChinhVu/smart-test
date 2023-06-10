namespace EmrCloudApi.Requests.OrdInfs
{
    public class ValidationInputItemRequest
    {
        public int SinDate { get; set; }
        public List<OdrInfInputItem> OdrInfs { get; set; } = new();
    }
}
