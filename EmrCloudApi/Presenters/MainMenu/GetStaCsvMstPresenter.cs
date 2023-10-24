using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses;
using UseCase.MainMenu.GetKensaIrai;
using EmrCloudApi.Responses.MainMenu.Dto;
using UseCase.MainMenu.GetStaCsvMstModel;

namespace EmrCloudApi.Presenters.MainMenu;

public class GetStaCsvMstPresenter : IGetStaCsvMstOutputPort
{
    public Response<GetStaCsvMstResponse> Result { get; private set; } = new();

    public void Complete(GetStaCsvMstOutputData output)
    {
        Result.Data = new GetStaCsvMstResponse(output.StaCsvMstModels);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetStaCsvMstStatus status) => status switch
    {
        GetStaCsvMstStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

