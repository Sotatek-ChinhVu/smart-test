using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using UseCase.SetMst.GetToolTip;

namespace EmrCloudApi.Presenters.SetMst
{
    public class GetSetMstToolTipPresenter : IGetSetMstToolTipOutputPort
    {
        public Response<GetSetMstToolTipResponse> Result { get; private set; } = default!;

        public void Complete(GetSetMstToolTipOutputData outputData)
        {
            Result = new Response<GetSetMstToolTipResponse>()
            {
                Data = new GetSetMstToolTipResponse(outputData.SetList),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSetMstToolTipStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetSetMstToolTipStatus.InvalidSetCd:
                    Result.Message = ResponseMessage.InvalidSetCd;
                    break;
                case GetSetMstToolTipStatus.NoData:
                    Result.Message = ResponseMessage.GetSetListNoData;
                    break;
                case GetSetMstToolTipStatus.Successed:
                    Result.Message = ResponseMessage.GetSetListSuccessed;
                    break;
                case GetSetMstToolTipStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
