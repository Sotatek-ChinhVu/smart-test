using Domain.Models.Diseases;

namespace EmrCloudApi.Responses.Diseases
{
    public class GetPtDiseaseListResponse
    {
        public List<PtDiseaseModel> DiseaseList { get; set; } = new List<PtDiseaseModel>();
    }
}
