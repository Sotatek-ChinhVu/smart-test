using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetPtByoMei
{
    public class GetPtByoMeiOutputData : IOutputData
    {
        public GetPtByoMeiOutputData(List<PtDiseaseDto> ptDiseaseDtos, GetPtByoMeiStatus status)
        {
            PtDiseaseDtos = ptDiseaseDtos;
            Status = status;
        }

        public List<PtDiseaseDto> PtDiseaseDtos { get; private set; }
        public GetPtByoMeiStatus Status { get; private set; }
    }
}
