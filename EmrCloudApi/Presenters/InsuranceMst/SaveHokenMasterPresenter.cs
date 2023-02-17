using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.InsuranceMst;
using UseCase.InsuranceMst.SaveHokenMaster;

namespace EmrCloudApi.Presenters.InsuranceMst
{
    public class SaveHokenMasterPresenter : ISaveHokenMasterOutputPort
    {
        public Response<SaveHokenMasterResponse> Result { get; private set; } = new Response<SaveHokenMasterResponse>();
        public void Complete(SaveHokenMasterOutputData outputData)
        {
            Result.Data = new SaveHokenMasterResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = outputData.Message ?? GetMessage(outputData.Status);
        }

        private string GetMessage(SaveHokenMasterStatus status) => status switch
        {
            SaveHokenMasterStatus.Successful => ResponseMessage.Success,
            SaveHokenMasterStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveHokenMasterStatus.Failed => ResponseMessage.Failed,
            SaveHokenMasterStatus.Exception => ResponseMessage.ExceptionError,
            _ => string.Empty
        };
    }
}
