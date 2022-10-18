using Domain.Models.OrdInfDetails;
using Domain.Models.TodayOdr;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetCheckDisease
{
    public class GetCheckDiseaseOutputData : IOutputData
    {
        public GetCheckDiseaseOutputData(List<OrdInfDetailModel> drugOrders, List<CheckedDiseaseModel> byomeis, GetCheckDiseaseStatus status)
        {
            DrugOrders = drugOrders;
            Byomeis = byomeis;
            Status = status;
        }

        public List<OrdInfDetailModel> DrugOrders { get; private set; }
        public List<CheckedDiseaseModel> Byomeis { get; private set; }
        public GetCheckDiseaseStatus Status { get; private set; }
    }
}
