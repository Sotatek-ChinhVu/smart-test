namespace EmrCloudApi.Tenant.Requests.SpecialNote
{
    public class AddAlrgyDrugListRequest
    {
        public List<AddAlrgyDrugListItemRequest> AlgrgyDrugs { get; set; } = new();
    }
}
