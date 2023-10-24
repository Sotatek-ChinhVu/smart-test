using EmrCloudApi.Constants;
using EmrCloudApi.Responses.RaiinListSetting;
using EmrCloudApi.Responses;
using UseCase.RaiinListSetting.GetRaiiinListSetting;

namespace EmrCloudApi.Presenters.RaiinListSetting
{
    public class GetRaiiinListSettingPresenter : IGetRaiiinListSettingOutputPort
    {
        public Response<GetRaiiinListSettingResponse> Result { get; private set; } = new Response<GetRaiiinListSettingResponse>();

        public void Complete(GetRaiiinListSettingOutputData outputData)
        {
            Result.Data = new GetRaiiinListSettingResponse(outputData.Data, outputData.GrpIdMax, outputData.SortNoMax);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(GetRaiiinListSettingStatus status) => status switch
        {
            GetRaiiinListSettingStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            GetRaiiinListSettingStatus.Successful => ResponseMessage.Success,
            GetRaiiinListSettingStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
