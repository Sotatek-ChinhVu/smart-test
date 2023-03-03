using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetSinMei
{
    public class GetSinMeiOutputData : IOutputData
    {
        public GetSinMeiOutputData(List<SinMeiModel> sinMeiModels, GetSinMeiStatus status)
        {
            SinMeiModels = sinMeiModels;
            Status = status;
        }

        public List<SinMeiModel> SinMeiModels { get; set; }
        public GetSinMeiStatus Status { get; private set; }
    }
}
