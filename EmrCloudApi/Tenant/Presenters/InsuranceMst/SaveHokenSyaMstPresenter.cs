using EmrCloudApi.Tenant.Responses.InsuranceMst;
using EmrCloudApi.Tenant.Responses;
using UseCase.InsuranceMst.SaveHokenSyaMst;
using EmrCloudApi.Tenant.Constants;

namespace EmrCloudApi.Tenant.Presenters.InsuranceMst
{
    public class SaveHokenSyaMstPresenter : ISaveHokenSyaMstOutputPort
    {
        public Response<SaveHokenSyaMstResponse> Result { get; private set; } = new Response<SaveHokenSyaMstResponse>();
        public void Complete(SaveHokenSyaMstOutputData outputData)
        {
            Result.Data = new SaveHokenSyaMstResponse()
            {
                State = outputData.Status
            };
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(SaveHokenSyaMstStatus status) => status switch
        {
            SaveHokenSyaMstStatus.Successful => ResponseMessage.Success,
            SaveHokenSyaMstStatus.InvalidHpID => ResponseMessage.InvalidHpId,
            SaveHokenSyaMstStatus.InvalidHokenSyaNo => ResponseMessage.InvalidHokenSyaNo,
            SaveHokenSyaMstStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
