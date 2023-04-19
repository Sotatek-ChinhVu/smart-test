using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.GetSetDataTenMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetSetDataTenMstPresenter : IGetSetDataTenMstOutputPort
    {
        public Response<GetSetDataTenMstResponse> Result { get; private set; } = default!;

        public void Complete(GetSetDataTenMstOutputData outputData)
        {
            Result = new Response<GetSetDataTenMstResponse>()
            {
                Data = new GetSetDataTenMstResponse(outputData.SetData),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSetDataTenMstStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetSetDataTenMstStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case GetSetDataTenMstStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
