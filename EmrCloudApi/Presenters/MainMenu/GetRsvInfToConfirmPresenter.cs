using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MainMenu;
using UseCase.MainMenu.RsvInfToConfirm;

namespace EmrCloudApi.Presenters.MainMenu
{
    public class GetRsvInfToConfirmPresenter : IGetRsvInfToConfirmOutputPort
    {
        public Response<GetRsvInfToConfirmResponse> Result { get; private set; } = new();

        public void Complete(GetRsvInfToConfirmOutputData outputData)
        {
            Result.Data = new GetRsvInfToConfirmResponse(outputData.RsvInfToConfirms);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetRsvInfToConfirmStatus status) => status switch
        {
            GetRsvInfToConfirmStatus.Successed => ResponseMessage.Success,
            GetRsvInfToConfirmStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
