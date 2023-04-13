using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses;
using UseCase.MstItem.DeleteOrRecoverTenMst;

namespace EmrCloudApi.Presenters.MstItem
{
    public class DeleteOrRecoverTenMstPresenter : DeleteOrRecoverTenMstOutputPort
    {
        public Response<DeleteOrRecoverTenMstResponse> Result { get; private set; } = default!;

        public void Complete(DeleteOrRecoverTenMstOutputData outputData)
        {
            Result = new Response<DeleteOrRecoverTenMstResponse>()
            {
                Data = new DeleteOrRecoverTenMstResponse(outputData.Status),
                Status = (int)outputData.Status
            };
            switch (outputData.Status)
            {
                case DeleteOrRecoverTenMstStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case DeleteOrRecoverTenMstStatus.Successful:
                    Result.Message = ResponseMessage.Success;
                    break;
                case DeleteOrRecoverTenMstStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case DeleteOrRecoverTenMstStatus.InvalidItemCd:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
            }
        }
    }
}
