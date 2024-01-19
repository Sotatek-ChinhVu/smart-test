using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using UseCase.Yousiki.CreateYuIchiFile;

namespace EmrCloudApi.Presenters.Yousiki;

public class CreateYuIchiFilePresenter : ICreateYuIchiFileOutputPort
{
    public Response<CreateYuIchiFileResponse> Result { get; private set; } = new();
    public void Complete(CreateYuIchiFileOutputData outputData)
    {
        Result.Data = new CreateYuIchiFileResponse(outputData.MessageType, outputData.ConfirmMessage);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(CreateYuIchiFileStatus status) => status switch
    {
        CreateYuIchiFileStatus.Successed => ResponseMessage.Success,
        CreateYuIchiFileStatus.Failed => ResponseMessage.Failed,
        CreateYuIchiFileStatus.InvalidCreateYuIchiFileSinYm => ResponseMessage.InvalidCreateYuIchiFileSinYm,
        CreateYuIchiFileStatus.InvalidCreateYuIchiFileCheckedOption => ResponseMessage.InvalidCreateYuIchiFileCheckedOption,
        CreateYuIchiFileStatus.CreateYuIchiFileSuccessed => ResponseMessage.CreateYuIchiFileSuccessed,
        CreateYuIchiFileStatus.CreateYuIchiFileFailed => ResponseMessage.CreateYuIchiFileFailed,
        _ => string.Empty
    };
}

