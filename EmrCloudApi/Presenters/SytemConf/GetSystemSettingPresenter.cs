using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using UseCase.SystemConf.SystemSetting;

namespace EmrCloudApi.Presenters.SytemConf
{
    public class GetSystemSettingPresenter : IGetSystemSettingOutputPort
    {
        public Response<GetSystemSettingResponse> Result { get; private set; } = new();

        public void Complete(GetSystemSettingOutputData outputData)
        {
            Result.Data = new GetSystemSettingResponse(outputData.RoudouMst, outputData.HpInfs, outputData.SystemConfMenus);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetSystemSettingStatus status) => status switch
        {
            GetSystemSettingStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
