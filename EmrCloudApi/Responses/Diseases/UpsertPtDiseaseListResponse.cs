namespace EmrCloudApi.Responses.Diseases
{
    public class UpsertPtDiseaseListResponse
    {
        public UpsertPtDiseaseListResponse(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; private set; }
    }
}
