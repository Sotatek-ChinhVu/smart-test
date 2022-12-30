using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.KarteInf;
using UseCase.KarteInf.ConvertTextToRichText;

namespace EmrCloudApi.Presenters.KarteInf;

public class ConvertTextToRichTextPresenter : IConvertTextToRichTextOutputPort
{
    public Response<ConvertTextToRichTextResponse> Result { get; private set; } = new();

    public void Complete(ConvertTextToRichTextOutputData outputData)
    {
        Result.Data = new ConvertTextToRichTextResponse(outputData.Status == ConvertTextToRichTextStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(ConvertTextToRichTextStatus status) => status switch
    {
        ConvertTextToRichTextStatus.Successed => ResponseMessage.Success,
        ConvertTextToRichTextStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}
