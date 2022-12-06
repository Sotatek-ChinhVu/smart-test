using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetSelectiveComment;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetSelectiveCommentPresenter : IGetSelectiveCommentOutputPort
    {
        public Response<GetSelectiveCommentResponse> Result { get; private set; } = default!;

        public void Complete(GetSelectiveCommentOutputData outputData)
        {
            Result = new Response<GetSelectiveCommentResponse>
            {
                Data = new GetSelectiveCommentResponse(outputData.Comments),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case GetSelectiveCommentStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case GetSelectiveCommentStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case GetSelectiveCommentStatus.InvalidSindate:
                    Result.Message = ResponseMessage.InvalidSinDate;
                    break;
                case GetSelectiveCommentStatus.InvalidItemCds:
                    Result.Message = ResponseMessage.InvalidItemCd;
                    break;
                case GetSelectiveCommentStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
            }
        }
    }
}
