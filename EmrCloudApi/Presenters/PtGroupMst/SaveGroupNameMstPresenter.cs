using EmrCloudApi.Responses.PtGroupMst;
using EmrCloudApi.Responses;
using UseCase.PtGroupMst.SaveGroupNameMst;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.PtGroupMst
{
    public class SaveGroupNameMstPresenter : ISaveGroupNameMstOutputPort
    {
        public Response<SaveGroupNameMstResponse> Result { get; private set; } = new Response<SaveGroupNameMstResponse>();

        public void Complete(SaveGroupNameMstOutputData outputData)
        {
            Result.Data = new SaveGroupNameMstResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(SaveGroupNameMstStatus status) => status switch
        {
            SaveGroupNameMstStatus.Successful => ResponseMessage.Success,
            SaveGroupNameMstStatus.Failed => ResponseMessage.Failed,
            SaveGroupNameMstStatus.Exception => ResponseMessage.ExceptionError,
            SaveGroupNameMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveGroupNameMstStatus.InvalidSortNoGrpIdGroupMst => ResponseMessage.InvalidSortNoGrpIdGroupMst,
            _ => string.Empty 
        };
    }
}