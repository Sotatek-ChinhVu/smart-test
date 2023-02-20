using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.GetPtByoMei;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetPtByoMeiPresenter : IGetPtByoMeiOutputPort
    {
        public Response<GetPtByoMeiResponse> Result { get; private set; } = new();
        public void Complete(GetPtByoMeiOutputData outputData)
        {
            Result.Data = new GetPtByoMeiResponse(outputData.PtDiseaseModels);
            Result.Message = GetMessage(outputData.PtDiseaseModels);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            GetPtByoMeiStatus.Successed => ResponseMessage.Success,
            GetPtByoMeiStatus.Failed => ResponseMessage.Failed,
            GetPtByoMeiStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
