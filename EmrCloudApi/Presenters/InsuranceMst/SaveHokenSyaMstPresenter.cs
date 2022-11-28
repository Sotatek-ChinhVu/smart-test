using EmrCloudApi.Responses.InsuranceMst;
using EmrCloudApi.Responses;
using UseCase.InsuranceMst.SaveHokenSyaMst;
using EmrCloudApi.Constants;

namespace EmrCloudApi.Presenters.InsuranceMst
{
    public class SaveHokenSyaMstPresenter : ISaveHokenSyaMstOutputPort
    {
        public Response<SaveHokenSyaMstResponse> Result { get; private set; } = new Response<SaveHokenSyaMstResponse>();
        public void Complete(SaveHokenSyaMstOutputData outputData)
        {
            Result.Data = new SaveHokenSyaMstResponse(outputData.Status);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
            if (outputData.Status == SaveHokenSyaMstStatus.Failed)
                Result.Message += $".{outputData.Message}";
        }

        private string GetMessage(SaveHokenSyaMstStatus status) => status switch
        {
            SaveHokenSyaMstStatus.Successful => ResponseMessage.Success,
            SaveHokenSyaMstStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
