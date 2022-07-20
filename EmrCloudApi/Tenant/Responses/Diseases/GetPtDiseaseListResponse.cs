using Domain.Models.Diseases;

namespace EmrCloudApi.Tenant.Responses.Diseases
{
    public class GetPtDiseaseListResponse
    {
        public List<PtDiseaseModel> DiseaseList { get; set; } = new List<PtDiseaseModel>();
    }
}
