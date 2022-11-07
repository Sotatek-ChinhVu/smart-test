namespace EmrCloudApi.Tenant.Requests.DrugDetail
{
    public class GetDrugDetailRequest
    {
        public int HpId { get; set; }

        public int SinDate { get; set; }

        public string ItemCd { get; set; } = string.Empty;
    }
}
