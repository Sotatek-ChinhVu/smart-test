using Domain.Models.Diseases;

namespace EmrCloudApi.Tenant.Responses.Diseases
{
    public class GetPtDiseaseListResponse
    {
        public List<PtDisease> DiseaseList { get; set; } = new List<PtDisease>();
    }
}
