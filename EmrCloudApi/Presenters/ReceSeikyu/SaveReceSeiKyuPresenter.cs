using EmrCloudApi.Constants;
using EmrCloudApi.Responses.ReceSeikyu;
using EmrCloudApi.Responses;
using UseCase.ReceSeikyu.Save;

namespace EmrCloudApi.Presenters.ReceSeikyu
{
    public class SaveReceSeiKyuPresenter : ISaveReceSeiKyuOutputPort
    {
        public Response<SaveReceSeiKyuResponse> Result { get; private set; } = new Response<SaveReceSeiKyuResponse>();

        public void Complete(SaveReceSeiKyuOutputData output)
        {
            Result.Data = new SaveReceSeiKyuResponse(output.Status);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(SaveReceSeiKyuStatus status) => status switch
        {
            SaveReceSeiKyuStatus.Successful => ResponseMessage.Success,
            SaveReceSeiKyuStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveReceSeiKyuStatus.Failed => ResponseMessage.Failed,
            SaveReceSeiKyuStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            _ => string.Empty
        };
    }
}
