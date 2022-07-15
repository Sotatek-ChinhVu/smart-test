using Domain.Models.Diseases;

namespace EmrCloudApi.Tenant.Responses.Diseases
{
    public class GetDiseaseListResponse
    {
        public List<Disease> DiseaseList { get; set; } = new List<Disease>();
    }
}
