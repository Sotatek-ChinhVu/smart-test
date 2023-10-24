using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.HokenMst;
using UseCase.HokenMst.GetHokenMst;

namespace EmrCloudApi.Presenters.HokenMst
{
    public class GetHokenMstPresenter : IGetHokenMstOutputPort
    {
        public Response<GetHokenMstResponse> Result { get; private set; } = new Response<GetHokenMstResponse>();
        public void Complete(GetHokenMstOutputData outputData)
        {
            Result.Data = new GetHokenMstResponse(outputData.HokenInfModels, outputData.KohiModelWithFutansyanos, outputData.KohiModels);
            Result.Status = (int)outputData.Status;
            Result.Message = GetMessage(outputData.Status);
        }

        private string GetMessage(GetHokenMstStatus status) => status switch
        {
            GetHokenMstStatus.Successed => ResponseMessage.Success,
            GetHokenMstStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
