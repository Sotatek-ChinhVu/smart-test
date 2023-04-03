using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.Delete;

namespace EmrCloudApi.Presenters.Reception
{
    public class DeleteReceptionPresenter : IDeleteReceptionOutputPort
    {
        public Response<DeleteReceptionResponse> Result { get; private set; } = default!;

        public void Complete(DeleteReceptionOutputData outputData)
        {
            Result = new Response<DeleteReceptionResponse>()
            {
                Data = new DeleteReceptionResponse(outputData.Status == DeleteReceptionStatus.Successed),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case DeleteReceptionStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case DeleteReceptionStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case DeleteReceptionStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case DeleteReceptionStatus.InvalidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case DeleteReceptionStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
