using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses;
using UseCase.MainMenu.DeleteKensaInf;

namespace EmrCloudApi.Presenters.MainMenu;

public class DeleteKensaInfPresenter : IDeleteKensaInfOutputPort
{
    public Response<DeleteKensaInfResponse> Result { get; private set; } = new();

    public void Complete(DeleteKensaInfOutputData output)
    {
        Result.Data = new DeleteKensaInfResponse(output.Status == DeleteKensaInfStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(DeleteKensaInfStatus status) => status switch
    {
        DeleteKensaInfStatus.Successed => ResponseMessage.Success,
        DeleteKensaInfStatus.Failed => ResponseMessage.Failed,
        DeleteKensaInfStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        DeleteKensaInfStatus.InvalidIraiCd => ResponseMessage.InvalidIraiCd,
        _ => string.Empty
    };
}