namespace EmrCloudApi.Tenant.Responses.Diseases
{
    public class UpsertPtDiseaseListResponse
    {
        public UpsertPtDiseaseListResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}
