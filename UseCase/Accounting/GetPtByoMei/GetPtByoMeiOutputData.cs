using Domain.Models.Diseases;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetPtByoMei
{
    public class GetPtByoMeiOutputData : IOutputData
    {
        public GetPtByoMeiOutputData(List<PtDiseaseModel> ptDiseaseModels, GetPtByoMeiStatus status)
        {
            PtDiseaseModels = ptDiseaseModels;
            Status = status;
        }

        public List<PtDiseaseModel> PtDiseaseModels { get; private set; }
        public GetPtByoMeiStatus Status { get; private set; }
    }
}
