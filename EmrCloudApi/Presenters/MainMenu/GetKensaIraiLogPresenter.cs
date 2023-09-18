using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses;
using UseCase.MainMenu.GetKensaIraiLog;
using EmrCloudApi.Responses.MainMenu.Dto;

namespace EmrCloudApi.Presenters.MainMenu;

public class GetKensaIraiLogLogPresenter : IGetKensaIraiLogOutputPort
{
    public Response<GetKensaIraiLogResponse> Result { get; private set; } = new();

    public void Complete(GetKensaIraiLogOutputData output)
    {
        Result.Data = new GetKensaIraiLogResponse(output.KensaIraiLogList.Select(item => new KensaIraiLogDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetKensaIraiLogStatus status) => status switch
    {
        GetKensaIraiLogStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

