using EmrCloudApi.Constants;
using EmrCloudApi.Responses.PtGroupMst;
using EmrCloudApi.Responses;
using UseCase.PtGroupMst.CheckAllowDelete;

namespace EmrCloudApi.Presenters.PtGroupMst
{
    public class CheckAllowDeleteGroupMstPresenter : ICheckAllowDeleteGroupMstOutputPort
    {
        public Response<CheckAllowDeleteGroupMstResponse> Result { get; private set; } = new Response<CheckAllowDeleteGroupMstResponse>();

        public void Complete(CheckAllowDeleteGroupMstOutputData outputData)
        {
            Result.Data = new CheckAllowDeleteGroupMstResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(CheckAllowDeleteGroupMstStatus status) => status switch
        {
            CheckAllowDeleteGroupMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            _ => string.Empty
        };
    }
}
