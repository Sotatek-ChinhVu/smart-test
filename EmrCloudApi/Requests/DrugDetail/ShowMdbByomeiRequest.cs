namespace EmrCloudApi.Requests.DrugDetail
{
    public class ShowMdbByomeiRequest
    {
        public string ItemCd { get; set; } = string.Empty;

        public int Level { get; private set; }

        public string DrugName { get; set; } = string.Empty;

        public string YJCode { get; set; } = string.Empty;
    }
}
