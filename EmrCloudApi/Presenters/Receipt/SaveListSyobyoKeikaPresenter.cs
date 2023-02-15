using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SaveListSyobyoKeika;

namespace EmrCloudApi.Presenters.Receipt;

public class SaveListSyobyoKeikaPresenter : ISaveListSyobyoKeikaOutputPort
{
    public Response<SaveListSyobyoKeikaResponse> Result { get; private set; } = new();

    public void Complete(SaveListSyobyoKeikaOutputData outputData)
    {
        Result.Data = new SaveListSyobyoKeikaResponse(outputData.Status == SaveListSyobyoKeikaStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveListSyobyoKeikaStatus status) => status switch
    {
        SaveListSyobyoKeikaStatus.Successed => ResponseMessage.Success,
        SaveListSyobyoKeikaStatus.Failed => ResponseMessage.Failed,
        SaveListSyobyoKeikaStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveListSyobyoKeikaStatus.InvalidSinYm => ResponseMessage.InvalidSinYm,
        SaveListSyobyoKeikaStatus.InvalidSeqNo => ResponseMessage.InvalidSeqNo,
        SaveListSyobyoKeikaStatus.InvalidSinDay => ResponseMessage.InvalidSinDay,
        SaveListSyobyoKeikaStatus.InvalidKeika => ResponseMessage.InvalidKeika,
        _ => string.Empty
    };
}
