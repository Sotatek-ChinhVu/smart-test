using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetDrugAction;

namespace EmrCloudApi.Presenters.MstItem
{
    public class GetDrugActionPresenter : IGetDrugActionOutputPort
    {
        public Response<GetDrugActionResponse> Result { get; private set; } = new();

        public void Complete(GetDrugActionOutputData outputData)
        {
            Result.Data = new GetDrugActionResponse(outputData.DrugInfo);

            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            GetDrugActionStatus.Successed => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
