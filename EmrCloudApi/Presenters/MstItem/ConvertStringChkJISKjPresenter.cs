using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.ConvertStringChkJISKj;

namespace EmrCloudApi.Presenters.MstItem;

public class ConvertStringChkJISKjPresenter : IConvertStringChkJISKjOutputPort
{
    public Response<ConvertStringChkJISKjResponse> Result { get; private set; } = new();

    public void Complete(ConvertStringChkJISKjOutputData output)
    {
        Result.Data = new ConvertStringChkJISKjResponse(output);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(ConvertStringChkJISKjStatus status) => status switch
    {
        ConvertStringChkJISKjStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
