using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Schema;
using UseCase.Schema.SaveListFileTodayOrder;

namespace EmrCloudApi.Presenters.Schema;

public class SaveListFilePresenter : ISaveListFileTodayOrderOutputPort
{
    public Response<SaveListFileResponse> Result { get; private set; } = new();

    public void Complete(SaveListFileTodayOrderOutputData outputData)
    {
        Result.Data = new SaveListFileResponse(outputData.ListKarteFile);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveListFileTodayOrderStatus status) => status switch
    {
        SaveListFileTodayOrderStatus.Successed => ResponseMessage.Success,
        SaveListFileTodayOrderStatus.Failed => ResponseMessage.Failed,
        SaveListFileTodayOrderStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveListFileTodayOrderStatus.InvalidFileImage => ResponseMessage.InvalidFileImage,
        SaveListFileTodayOrderStatus.InvalidSetCd => ResponseMessage.InvalidSetCd,
        SaveListFileTodayOrderStatus.InvalidTypeUpload => ResponseMessage.InvalidTypeUpload,
        _ => string.Empty
    };
}
