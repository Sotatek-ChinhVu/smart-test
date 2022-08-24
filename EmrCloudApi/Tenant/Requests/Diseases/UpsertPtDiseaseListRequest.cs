namespace EmrCloudApi.Tenant.Requests.Diseases

{
    public class UpsertPtDiseaseListRequest
    {
        public List<UpsertPtDiseaseListItem> PtDiseases { get; set; } = new List<UpsertPtDiseaseListItem>();
    }
}
