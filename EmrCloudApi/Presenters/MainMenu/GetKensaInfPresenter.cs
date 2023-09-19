using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MainMenu.Dto;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses;
using UseCase.MainMenu.GetKensaInf;

namespace EmrCloudApi.Presenters.MainMenu;

public class GetKensaInfPresenter : IGetKensaInfOutputPort
{
    public Response<GetKensaInfResponse> Result { get; private set; } = new();

    public void Complete(GetKensaInfOutputData output)
    {
        Result.Data = new GetKensaInfResponse(output.KensaInfModelList.Select(item => new KensaInfDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetKensaInfStatus status) => status switch
    {
        GetKensaInfStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}