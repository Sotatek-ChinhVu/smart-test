using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.LastDayInformation;
using UseCase.LastDayInformation.SaveSettingLastDayInfoList;

namespace EmrCloudApi.Presenters.LastDayInformation
{
    public class SaveSettingLastDayInfoListPresenter : ISaveSettingLastDayInfoListOutputPort
    {
        public Response<SaveSettingLastDayInfoListResponse> Result { get; private set; } = default!;

        public void Complete(SaveSettingLastDayInfoListOutputData outputData)
        {
            Result = new Response<SaveSettingLastDayInfoListResponse>()
            {
                Data = new SaveSettingLastDayInfoListResponse(outputData.Status == SaveSettingLastDayInfoListStatus.Successed),
                Message = GetMessage(outputData.Status),
                Status = (int)outputData.Status
            };
        }

        private static string GetMessage(SaveSettingLastDayInfoListStatus status) => status switch
        {
            SaveSettingLastDayInfoListStatus.Successed => ResponseMessage.Success,
            SaveSettingLastDayInfoListStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
