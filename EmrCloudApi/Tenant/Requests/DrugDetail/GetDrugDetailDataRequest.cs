namespace EmrCloudApi.Tenant.Requests.DrugDetail
{
    public class GetDrugDetailDataRequest
    {
        public int SelectedIndexOfChildrens { get; set; }

        public int SelectedIndexOfLevel0 { get; set; }

        public string DrugName { get; set; } = string.Empty;

        public string ItemCd { get; set; } = string.Empty;

        public string YJCode { get; set; } = string.Empty;
    }
}
