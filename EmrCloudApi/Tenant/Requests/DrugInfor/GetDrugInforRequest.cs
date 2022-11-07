namespace EmrCloudApi.Tenant.Requests.DrugInfor
{
    public class GetDrugInforRequest
    {
        public int HpId { get; set; }

        public int SinDate { get; set; }

        public string ItemCd { get; set; } = string.Empty;
    }
}
