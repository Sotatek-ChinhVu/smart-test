using CommonCheckers.OrderRealtimeChecker.Models;
using UseCase.CommonChecker;

namespace EmrCloudApi.Responses.PatientInfor
{
    public class OrderRealtimeCheckerResponse
    {
        public OrderRealtimeCheckerResponse(List<UnitCheckInfoModel> unitCheckInfoModel, GetOrderCheckerStatus status)
        {
            UnitCheckInfoModel = unitCheckInfoModel;
            Status = status;
        }

        public List<UnitCheckInfoModel> UnitCheckInfoModel { get; private set; }


        public GetOrderCheckerStatus Status { get; private set; }
    }
}
