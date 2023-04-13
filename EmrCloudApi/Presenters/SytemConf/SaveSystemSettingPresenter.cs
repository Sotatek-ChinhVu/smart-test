using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using UseCase.SystemConf.SaveSystemSetting;

namespace EmrCloudApi.Presenters.SytemConf
{
    public class SaveSystemSettingPresenter : ISaveSystemSettingOutputPort
    {
        public Response<SaveSystemSettingResponse> Result { get; private set; } = new();

        public void Complete(SaveSystemSettingOutputData outputData)
        {
            Result.Data = new SaveSystemSettingResponse(outputData.Status == SaveSystemSettingStatus.Successed);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(SaveSystemSettingStatus status) => status switch
        {
            SaveSystemSettingStatus.Successed => ResponseMessage.Success,
            SaveSystemSettingStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
