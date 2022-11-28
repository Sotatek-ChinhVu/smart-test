namespace EmrCloudApi.Requests.DrugDetail
{
    public class GetDrugDetailDataRequest
    {
        public int SelectedIndexOfMenuLevel { get; set; }

        public int Level { get; set; }

        public string DrugName { get; set; } = string.Empty;

        public string ItemCd { get; set; } = string.Empty;

        public string YJCode { get; set; } = string.Empty;
    }
}
