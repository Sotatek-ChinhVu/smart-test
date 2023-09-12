using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses;
using UseCase.MainMenu.GetKensaIrai;
using EmrCloudApi.Responses.MainMenu.Dto;

namespace EmrCloudApi.Presenters.MainMenu;

public class GetKensaIraiPresenter : IGetKensaIraiOutputPort
{
    public Response<GetKensaIraiResponse> Result { get; private set; } = new();

    public void Complete(GetKensaIraiOutputData output)
    {
        Result.Data = new GetKensaIraiResponse(output.KensaIraiList.Select(item => new KensaIraiDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetKensaIraiStatus status) => status switch
    {
        GetKensaIraiStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

