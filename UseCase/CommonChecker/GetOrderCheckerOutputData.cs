using CommonChecker.Models.OrdInf;
using UseCase.Core.Sync.Core;

namespace UseCase.CommonChecker
{
    public class GetOrderCheckerOutputData : IOutputData
    {
        public GetOrderCheckerOutputData(OrdInfoModel ordInfoModels)
        {
            OrdInfoModels = ordInfoModels;
        }

        public OrdInfoModel OrdInfoModels { get; private set; }
    }
}
