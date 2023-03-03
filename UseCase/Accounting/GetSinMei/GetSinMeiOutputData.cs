using Domain.Models.Accounting;
using UseCase.Accounting.GetPtByoMei;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetSinMei
{
    public class GetSinMeiOutputData : IOutputData
    {
        public GetSinMeiOutputData(List<SinMeiModel> sinMeiModels, GetPtByoMeiStatus status)
        {
            SinMeiModels = sinMeiModels;
            Status = status;
        }

        public List<SinMeiModel> SinMeiModels { get; set; }
        public GetPtByoMeiStatus Status { get; private set; }
    }
}
