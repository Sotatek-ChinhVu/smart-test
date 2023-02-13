using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetPtByoMei
{
    public class GetPtByoMeiOutputData : IOutputData
    {
        public GetPtByoMeiOutputData(List<PtByomeiModel> ptByomeiModels, GetPtByoMeiStatus status)
        {
            PtByomeiModels = ptByomeiModels;
            Status = status;
        }

        public List<PtByomeiModel> PtByomeiModels { get; private set; }
        public GetPtByoMeiStatus Status { get; private set; }
    }
}
