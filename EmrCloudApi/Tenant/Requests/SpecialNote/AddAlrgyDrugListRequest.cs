namespace EmrCloudApi.Tenant.Requests.SpecialNote
{
    public class AddAlrgyDrugListRequest
    {
        public List<AddAlrgyDrugListItem> AlgrgyDrugs { get; set; } = new();
    }
}
