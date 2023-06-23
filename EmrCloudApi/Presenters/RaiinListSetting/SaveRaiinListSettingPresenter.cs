using EmrCloudApi.Constants;
using EmrCloudApi.Responses.RaiinListSetting;
using EmrCloudApi.Responses;
using UseCase.RaiinListSetting.SaveRaiinListSetting;

namespace EmrCloudApi.Presenters.RaiinListSetting
{
    public class SaveRaiinListSettingPresenter : ISaveRaiinListSettingOutputPort
    {
        public Response<SaveRaiinListSettingResponse> Result { get; private set; } = new Response<SaveRaiinListSettingResponse>();

        public void Complete(SaveRaiinListSettingOutputData outputData)
        {
            Result.Data = new SaveRaiinListSettingResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(SaveRaiinListSettingStatus status) => status switch
        {
            SaveRaiinListSettingStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveRaiinListSettingStatus.Successful => ResponseMessage.Success,
            SaveRaiinListSettingStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            SaveRaiinListSettingStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
