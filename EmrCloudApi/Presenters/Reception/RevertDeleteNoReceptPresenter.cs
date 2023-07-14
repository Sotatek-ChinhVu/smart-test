using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.Delete;
using UseCase.Reception.RevertDeleteNoRecept;

namespace EmrCloudApi.Presenters.Reception
{
    public class RevertDeleteNoReceptPresenter : IRevertDeleteNoReceptOutputPort
    {
        public Response<RevertDeleteNoReceptResponse> Result { get; private set; } = default!;

        public void Complete(RevertDeleteNoReceptOutputData outputData)
        {
            Result = new Response<RevertDeleteNoReceptResponse>()
            {
                Data = new RevertDeleteNoReceptResponse(outputData.Status == RevertDeleteNoReceptStatus.Success),
                Status = (byte)outputData.Status
            };
            switch (outputData.Status)
            {
                case RevertDeleteNoReceptStatus.InvalidRaiinNo:
                    Result.Message = ResponseMessage.InvalidRaiinNo;
                    break;
                case RevertDeleteNoReceptStatus.InvalidHpId:
                    Result.Message = ResponseMessage.InvalidHpId;
                    break;
                case RevertDeleteNoReceptStatus.Failed:
                    Result.Message = ResponseMessage.Failed;
                    break;
                case RevertDeleteNoReceptStatus.Success:
                    Result.Message = ResponseMessage.Success;
                    break;
            }
        }
    }
}
