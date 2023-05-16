using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.CheckIsTenMstUsed;

namespace EmrCloudApi.Presenters.MstItem
{
    public class CheckIsTenMstUsedPresenter : ICheckIsTenMstUsedOutputPort
    {
        public Response<CheckIsTenMstUsedResponse> Result { get; private set; } = new();

        public void Complete(CheckIsTenMstUsedOutputData output)
        {
            Result.Data = new CheckIsTenMstUsedResponse(output.Status);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(CheckIsTenMstUsedStatus status) => status switch
        {
            CheckIsTenMstUsedStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            CheckIsTenMstUsedStatus.InvalidItemCd => ResponseMessage.InvalidItemCd,
            _ => string.Empty
        };
    }
}
