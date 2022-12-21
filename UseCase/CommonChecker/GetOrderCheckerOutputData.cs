using CommonChecker.Models.OrdInf;
using UseCase.Core.Sync.Core;
using UseCase.DrugDetailData.Get;

namespace UseCase.CommonChecker
{
    public class GetOrderCheckerOutputData : IOutputData
    {
        public GetOrderCheckerOutputData(OrdInfoModel ordInfoModels, GetOrderCheckerStatus status)
        {
            OrdInfoModels = ordInfoModels;
            Status = status;
        }

        public OrdInfoModel OrdInfoModels { get; private set; }
        public GetOrderCheckerStatus Status { get; private set; }
    }
}
