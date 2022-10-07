namespace EmrCloudApi.Tenant.Responses.KarteInf
{
    public class ValidationKarteInfListResponse
    {
        public ValidationKarteInfListResponse(List<ValidationKarteInfListItemResponse> validations)
        {
            Validations = validations;
        }

        public List<ValidationKarteInfListItemResponse> Validations { get; private set; }
    }
}
