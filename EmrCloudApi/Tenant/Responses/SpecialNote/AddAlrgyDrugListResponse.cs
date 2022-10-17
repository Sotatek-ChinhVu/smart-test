namespace EmrCloudApi.Tenant.Responses.SpecialNote
{
    public class AddAlrgyDrugListResponse
    {
        public AddAlrgyDrugListResponse(List<AddAlrgyDrugListItemResponse> validations)
        {
            Validations = validations;
        }

        public List<AddAlrgyDrugListItemResponse> Validations { get; private set; }
    }
}
