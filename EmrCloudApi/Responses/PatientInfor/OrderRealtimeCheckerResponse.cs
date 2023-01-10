using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Models;
using UseCase.CommonChecker;

namespace EmrCloudApi.Responses.PatientInfor
{
    public class OrderRealtimeCheckerResponse
    {
        public OrderRealtimeCheckerResponse(List<UnitCheckInfoModel> unitCheckInfoModel, List<ErrorInfoModel> errorInfoModels, GetOrderCheckerStatus status)
        {
            UnitCheckInfoModel = unitCheckInfoModel;
            ErrorInfoModels = errorInfoModels;
            Status = status;
        }

        public List<UnitCheckInfoModel> UnitCheckInfoModel { get; private set; }

        public List<ErrorInfoModel> ErrorInfoModels { get; private set; }

        public GetOrderCheckerStatus Status { get; private set; }
    }
}
