using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.NextOrder;
using UseCase.NextOrder.Check;

namespace EmrCloudApi.Presenters.NextOrder
{
    public class CheckUpsertNextOrderPresenter : ICheckUpsertNextOrderOutputPort
    {
        public Response<CheckUpsertNextOrderResponse> Result { get; private set; } = new Response<CheckUpsertNextOrderResponse>();
        public void Complete(CheckUpsertNextOrderOutputData outputData)
        {
            Result.Data = new CheckUpsertNextOrderResponse(outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(CheckUpsertNextOrderStatus status) => status switch
        {
            CheckUpsertNextOrderStatus.Valid => ResponseMessage.Valid,
            CheckUpsertNextOrderStatus.InValid => ResponseMessage.InValid,
            _ => string.Empty
        };
    }
}
