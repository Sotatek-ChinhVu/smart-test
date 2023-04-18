using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UserConf;
using UseCase.User.GetUserConfList;
using UseCase.UserConf.UserSettingParam;

namespace EmrCloudApi.Presenters.UserConf
{
    public class GetUserConfigParamPresenter : IGetUserConfigParamOutputPort
    {
        public Response<GetUserConfigParamResponse> Result { get; private set; } = new();

        public void Complete(GetUserConfigParamOutputData outputData)
        {
            Result.Data = new GetUserConfigParamResponse(outputData.Param);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }


        private string GetMessage(GetUserConfigParamStatus status) => status switch
        {
            GetUserConfigParamStatus.Successed => ResponseMessage.Success,
            GetUserConfigParamStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
