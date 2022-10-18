
using Domain.Models.OrdInfDetails;
using Domain.Models.TodayOdr;

namespace EmrCloudApi.Tenant.Responses.MedicalExamination
{
    public class GetCheckDiseaseResponse
    {
        public GetCheckDiseaseResponse(List<OrdInfDetailModel> drugOrders, List<CheckedDiseaseModel> byomeis)
        {
            DrugOrders = drugOrders;
            Byomeis = byomeis;
        }

        public List<OrdInfDetailModel> DrugOrders { get; private set; }
        public List<CheckedDiseaseModel> Byomeis { get; private set; }
    }
}
