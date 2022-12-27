using CommonCheckers.OrderRealtimeChecker.Models;
using UseCase.Core.Sync.Core;

namespace UseCase.CommonChecker
{
    public class GetOrderCheckerOutputData : IOutputData
    {
        public GetOrderCheckerOutputData(UnitCheckInfoModel unitCheckInfoModel, GetOrderCheckerStatus status)
        {
            UnitCheckInfoModel = unitCheckInfoModel;
            Status = status;
        }

        public UnitCheckInfoModel UnitCheckInfoModel { get; private set; }
        public GetOrderCheckerStatus Status { get; private set; }
    }
}
