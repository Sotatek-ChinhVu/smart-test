using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetListKensaIjiSetting;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetListKensaIjiSettingPresenter : IGetListKensaIjiSettingOutputPort
    {
        public Response<GetListKensaIjiSettingResponse> Result { get; private set; } = default!;
        public void Complete(GetListKensaIjiSettingOutputData outputData)
        {
            Result = new Response<GetListKensaIjiSettingResponse>()
            {
                Data = new GetListKensaIjiSettingResponse(outputData.KensaIjiSettingModels),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetListKensaIjiSettingStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetListKensaIjiSettingStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetListKensaIjiSettingStatus.NoData:
                    Result.Message = ResponseMessage.NoData;
                    break;
            }
        }
    }
}
