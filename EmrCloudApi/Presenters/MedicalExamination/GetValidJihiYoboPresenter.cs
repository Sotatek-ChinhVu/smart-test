using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.CheckedItemName;
using UseCase.MedicalExamination.GetValidJihiYobo;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class GetValidJihiYoboPresenter : IGetValidJihiYoboOutputPort
    {
        public Response<GetValidJihiYoboResponse> Result { get; private set; } = default!;

        public void Complete(GetValidJihiYoboOutputData outputData)
        {
            Result = new Response<GetValidJihiYoboResponse>()
            {
                Data = new GetValidJihiYoboResponse(outputData.SystemSetting, outputData.IsExistYoboItemOnly),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetValidJihiYoboStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetValidJihiYoboStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetValidJihiYoboStatus.InvalidSyosaiKbn:
                    Result.Message = ResponseMessage.RaiinInfTodayOdrInvalidSyosaiKbn;
                    break;
                case GetValidJihiYoboStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case GetValidJihiYoboStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
