using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetDefaultPrecautions;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetDefaultPrecautionsPresenter : IGetDefaultPrecautionsOutputPort
    {
        public Response<GetDefaultPrecautionsResponse> Result { get; private set; } = new();
        public void Complete(GetDefaultPrecautionsOutputData outputData)
        {
            Result.Data = new GetDefaultPrecautionsResponse(outputData.DrugInfo);

            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            GetDefaultPrecautionsStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
