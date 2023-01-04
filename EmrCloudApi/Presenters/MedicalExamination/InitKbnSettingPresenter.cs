using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using UseCase.MedicalExamination.InitKbnSetting;

namespace EmrCloudApi.Presenters.MedicalExamination
{
    public class InitKbnSettingPresenter : IInitKbnSettingOutputPort
    {
        public Response<InitKbnSettingResponse> Result { get; private set; } = default!;

        public void Complete(InitKbnSettingOutputData outputData)
        {
            Result = new Response<InitKbnSettingResponse>()
            {
                Data = new InitKbnSettingResponse(outputData.RaiinKbnModels),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case InitKbnSettingStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case InitKbnSettingStatus.InvalidWindowType:
                    Result.Message = ResponseMessage.InvalidWindowType;
                    break;
                case InitKbnSettingStatus.InvalidFrameId:
                    Result.Message = ResponseMessage.InvalidFrameId;
                    break;
                case InitKbnSettingStatus.InvalidPtId:
                    Result.Message = ResponseMessage.InvalidPtId;
                    break;
                case InitKbnSettingStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case InitKbnSettingStatus.InvalidSinDate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case InitKbnSettingStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case InitKbnSettingStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
