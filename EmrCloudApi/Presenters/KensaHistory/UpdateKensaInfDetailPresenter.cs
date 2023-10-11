using EmrCloudApi.Constants;
using EmrCloudApi.Responses.KensaHistory;
using EmrCloudApi.Responses;
using UseCase.KensaHistory.UpdateKensaSet;
using UseCase.KensaHistory.UpdateKensaInfDetail;

namespace EmrCloudApi.Presenters.KensaHistory
{
    public class UpdateKensaInfDetailPresenter
    {
        public Response<UpdateKensaInfDetailResponse> Result { get; private set; } = default!;

        public void Complete(UpdateKensaInfDetailOutputData outputData)
        {
            Result = new Response<UpdateKensaInfDetailResponse>
            {
                Data = new UpdateKensaInfDetailResponse(outputData.CheckResult),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case UpdateKensaInfDetailStatus.Successed:
                    Result.Message = ResponseMessage.Success;
                    break;
                case UpdateKensaInfDetailStatus.InValidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case UpdateKensaInfDetailStatus.InValidUserId:
                    Result.Message = ResponseMessage.InvalidUserId;
                    break;
                case UpdateKensaInfDetailStatus.InvalidDataUpdate:
                    Result.Message = ResponseMessage.InputDataNull;
                    break;
            }
        }
    }
}
