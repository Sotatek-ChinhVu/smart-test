using EmrCloudApi.Tenant.Responses.OrdInf;

namespace EmrCloudApi.Tenant.Responses.OrdInfs
{
    public class ValidationOrdInfListResponse
    {
        public ValidationOrdInfListResponse(List<ValidationOrdInfListItemResponse> validations)
        {
            Validations = validations;
        }

        public List<ValidationOrdInfListItemResponse> Validations { get; private set; }
    }
}
