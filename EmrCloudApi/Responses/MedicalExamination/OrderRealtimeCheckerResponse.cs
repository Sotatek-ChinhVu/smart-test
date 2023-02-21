using CommonChecker.Models;
using UseCase.CommonChecker;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class OrderRealtimeCheckerResponse
    {
        public OrderRealtimeCheckerResponse(List<ErrorInfoModel> errorInfoModels, GetOrderCheckerStatus status)
        {
            ErrorInfoModels = errorInfoModels;
            Status = status;
        }

        public List<ErrorInfoModel> ErrorInfoModels { get; private set; }

        public GetOrderCheckerStatus Status { get; private set; }
    }
}
