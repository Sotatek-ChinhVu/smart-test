using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MainMenu.Dto;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses;
using UseCase.MainMenu.GetKensaCenterMstList;

namespace EmrCloudApi.Presenters.MainMenu;

public class GetKensaCenterMstListPresenter : IGetKensaCenterMstListOutputPort
{
    public Response<GetKensaCenterMstListResponse> Result { get; private set; } = new();

    public void Complete(GetKensaCenterMstListOutputData output)
    {
        Result.Data = new GetKensaCenterMstListResponse(output.KensaCenterMstList.Select(item => new KensaCenterMstDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetKensaCenterMstListStatus status) => status switch
    {
        GetKensaCenterMstListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
