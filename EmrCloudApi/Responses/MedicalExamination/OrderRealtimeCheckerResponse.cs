using CommonChecker.Models;
using UseCase.CommonChecker;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class OrderRealtimeCheckerResponse
    {
        public OrderRealtimeCheckerResponse(List<ErrorInfoModel> errorInfoModels, string weightInfo, string weightDateInfo, string heightInfo, string heightDateInfo, GetOrderCheckerStatus status)
        {
            ErrorInfoModels = errorInfoModels;
            WeightInfo = weightInfo;
            WeightDateInfo = weightDateInfo;
            HeightInfo = heightInfo;
            HeightDateInfo = heightDateInfo;
            Status = status;
        }

        public List<ErrorInfoModel> ErrorInfoModels { get; private set; }

        public string WeightInfo { get; private set; }

        public string WeightDateInfo { get; private set; }

        public string HeightInfo { get; private set; }

        public string HeightDateInfo { get; private set; }

        public GetOrderCheckerStatus Status { get; private set; }
    }
}
