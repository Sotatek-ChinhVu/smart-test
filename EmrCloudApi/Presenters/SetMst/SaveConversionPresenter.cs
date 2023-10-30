using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using UseCase.SuperSetDetail.SaveConversion;

namespace EmrCloudApi.Presenters.SetMst;

public class SaveConversionPresenter : ISaveConversionOutputPort
{
    public Response<SaveConversionResponse> Result { get; private set; } = new();

    public void Complete(SaveConversionOutputData output)
    {
        Result.Data = new SaveConversionResponse(output.Status == SaveConversionStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveConversionStatus status) => status switch
    {
        SaveConversionStatus.Successed => ResponseMessage.Success,
        SaveConversionStatus.Failed => ResponseMessage.Failed,
        SaveConversionStatus.InvalidConversionItemCd => ResponseMessage.InvalidConversionItemCd,
        SaveConversionStatus.InvalidSourceItemCd => ResponseMessage.InvalidSourceItemCd,
        SaveConversionStatus.InvalidDeleteConversionItemCd => ResponseMessage.InvalidDeleteConversionItemCd,
        _ => string.Empty
    };
}
