using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using UseCase.Online.UpdateOnlineConsents;

namespace EmrCloudApi.Presenters.Online;

public class UpdateOnlineConsentsPresenter : IUpdateOnlineConsentsOutputPort
{
    public Response<UpdateOnlineConsentsResponse> Result { get; private set; } = new();

    public void Complete(UpdateOnlineConsentsOutputData output)
    {
        Result.Data = new UpdateOnlineConsentsResponse(output.Status == UpdateOnlineConsentsStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateOnlineConsentsStatus status) => status switch
    {
        UpdateOnlineConsentsStatus.Successed => ResponseMessage.Success,
        UpdateOnlineConsentsStatus.Failed => ResponseMessage.Failed,
        UpdateOnlineConsentsStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        UpdateOnlineConsentsStatus.InvalidXmlFile => ResponseMessage.InvalidXmlFile,
        UpdateOnlineConsentsStatus.InvalidOnlineConfirmationDate => ResponseMessage.InvalidOnlineConfirmationDate,
        _ => string.Empty
    };
}