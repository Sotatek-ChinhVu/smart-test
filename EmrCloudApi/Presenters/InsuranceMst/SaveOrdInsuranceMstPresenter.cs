using EmrCloudApi.Constants;
using EmrCloudApi.Responses.InsuranceMst;
using EmrCloudApi.Responses;
using UseCase.InsuranceMst.SaveOrdInsuranceMst;

namespace EmrCloudApi.Presenters.InsuranceMst
{
    public class SaveOrdInsuranceMstPresenter : ISaveOrdInsuranceMstOutputPort
    {
        public Response<SaveOrdInsuranceMstResponse> Result { get; private set; } = default!;

        public void Complete(SaveOrdInsuranceMstOutputData outputData)
        {
            Result = new Response<SaveOrdInsuranceMstResponse>()
            {
                Data = new SaveOrdInsuranceMstResponse(outputData.Status),
                Status = (int)outputData.Status,
                Message = GetMessage(outputData.Status)
            };
        }

        private string GetMessage(SaveOrdInsuranceMstStatus status) => status switch
        {
            SaveOrdInsuranceMstStatus.Successful => ResponseMessage.Success,
            SaveOrdInsuranceMstStatus.Failed => ResponseMessage.Failed,
            SaveOrdInsuranceMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            SaveOrdInsuranceMstStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            _ => string.Empty
        };
    }
}
