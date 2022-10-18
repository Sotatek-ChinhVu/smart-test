using Domain.Models.TodayOdr;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination
{
    public class GetByomeiOfCheckDiseaseResponse
    {
        public GetByomeiOfCheckDiseaseResponse(List<CheckedDiseaseModel> byomeis)
        {
            Byomeis = byomeis;
        }

        public List<CheckedDiseaseModel> Byomeis { get; private set; }
    }
}
