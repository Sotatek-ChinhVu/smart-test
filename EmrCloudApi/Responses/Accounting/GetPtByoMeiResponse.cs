using Domain.Models.Diseases;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetPtByoMeiResponse
    {
        public GetPtByoMeiResponse(List<PtDiseaseDto> ptDiseaseDtos)
        {
            PtDiseaseDtos = ptDiseaseDtos;
        }

        public List<PtDiseaseDto> PtDiseaseDtos { get; private set; }
    }
}
