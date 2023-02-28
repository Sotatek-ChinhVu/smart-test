using CommonChecker.Models;
using UseCase.Core.Sync.Core;

namespace UseCase.CommonChecker
{
    public class GetOrderCheckerOutputData : IOutputData
    {
        public GetOrderCheckerOutputData(List<ErrorInfoModel> errorInfoModels, GetOrderCheckerStatus status)
        {
            ErrorInfoModels = errorInfoModels;
            Status = status;
        }

        public List<ErrorInfoModel> ErrorInfoModels { get; private set; }

        public GetOrderCheckerStatus Status { get; private set; }
    }
}
