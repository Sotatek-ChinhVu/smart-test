using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.KensaHistory;
using UseCase.KensaHistory.UpdateKensaSet;

namespace EmrCloudApi.Presenters.KensaHistory
{
    public class UpdateKensaSetPresenter
    {
        public Response<UpdateKensaSetResponse> Result { get; private set; } = default!;

        public void Complete(UpdateKensaSetOuputData outputData)
        {
            Result = new Response<UpdateKensaSetResponse>
            {
                Data = new UpdateKensaSetResponse(outputData.CheckResult),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpdateKensaSetStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpdateKensaSetStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case UpdateKensaSetStatus.InValidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case UpdateKensaSetStatus.InvalidDataUpdate:
                    Result.Message = ResponseMessage.InputDataNull;
                    break;
            }
        }
    }
}
