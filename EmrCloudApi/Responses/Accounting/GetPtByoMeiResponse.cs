using Domain.Models.Diseases;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetPtByoMeiResponse
    {
        public GetPtByoMeiResponse(List<PtDiseaseModel> ptDiseaseModels)
        {
            PtDiseaseModels = ptDiseaseModels;
        }

        public List<PtDiseaseModel> PtDiseaseModels { get; private set; }
    }
}
