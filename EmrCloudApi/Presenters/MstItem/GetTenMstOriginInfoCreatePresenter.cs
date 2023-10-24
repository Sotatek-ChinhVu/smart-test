using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetTenMstOriginInfoCreate;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetTenMstOriginInfoCreatePresenter : IGetTenMstOriginInfoCreateOutputPort
    {
        public Response<GetTenMstOriginInfoCreateResponse> Result { get; private set; } = default!;

        public void Complete(GetTenMstOriginInfoCreateOutputData outputData)
        {
            Result = new Response<GetTenMstOriginInfoCreateResponse>()
            {
                Data = new GetTenMstOriginInfoCreateResponse(outputData.ItemCd, outputData.JihiSbt, outputData.TenMstOriginModel),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetTenMstOriginInfoCreateStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetTenMstOriginInfoCreateStatus.InvalidTypeItem:
                    Result.Message = ResponseMessage.InvalidTypeItem;
                    break;
                case GetTenMstOriginInfoCreateStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
