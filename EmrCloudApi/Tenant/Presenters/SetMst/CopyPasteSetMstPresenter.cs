using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using UseCase.SetMst.CopyPasteSetMst;

namespace EmrCloudApi.Tenant.Presenters.SetMst
{
    public class CopyPasteSetMstPresenter : ICopyPasteSetMstOutputPort
    {
        public Response<CopyPasteSetMstResponse> Result { get; private set; } = new Response<CopyPasteSetMstResponse>();

        public void Complete(CopyPasteSetMstOutputData output)
        {
            Result.Data = new CopyPasteSetMstResponse(output.Status == CopyPasteSetMstStatus.Successed);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(CopyPasteSetMstStatus status) => status switch
        {
            CopyPasteSetMstStatus.Successed => ResponseMessage.Success,
            CopyPasteSetMstStatus.Failed => ResponseMessage.Failed,
            CopyPasteSetMstStatus.InvalidLevel => ResponseMessage.InvalidLevel,
            CopyPasteSetMstStatus.InvalidCopySetCd => ResponseMessage.InvalidCopySetCd,
            CopyPasteSetMstStatus.InvalidPasteSetCd => ResponseMessage.InvalidPasteSetCd,
            CopyPasteSetMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            CopyPasteSetMstStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            _ => string.Empty
        };
    }
}
