using CommonChecker.Models.OrdInf;
using UseCase.CommonChecker;

namespace EmrCloudApi.Responses.PatientInfor
{
    public class OrderRealtimeCheckerResponse
    {
        public OrderRealtimeCheckerResponse(OrdInfoModel ordInfoModel, GetOrderCheckerStatus status)
        {
            OrdInfoModel = ordInfoModel;
            Status = status;
        }

        public OrdInfoModel OrdInfoModel { get; private set; }
        public GetOrderCheckerStatus Status { get; private set; }
    }
}
