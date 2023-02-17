using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SaveListSyobyoKeika;

namespace EmrCloudApi.Presenters.Receipt;

public class SaveSyobyoKeikaListPresenter : ISaveSyobyoKeikaListOutputPort
{
    public Response<SaveSyobyoKeikaListResponse> Result { get; private set; } = new();

    public void Complete(SaveSyobyoKeikaListOutputData outputData)
    {
        Result.Data = new SaveSyobyoKeikaListResponse(outputData.Status == SaveSyobyoKeikaListStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveSyobyoKeikaListStatus status) => status switch
    {
        SaveSyobyoKeikaListStatus.Successed => ResponseMessage.Success,
        SaveSyobyoKeikaListStatus.Failed => ResponseMessage.Failed,
        SaveSyobyoKeikaListStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveSyobyoKeikaListStatus.InvalidSinYm => ResponseMessage.InvalidSinYm,
        SaveSyobyoKeikaListStatus.InvalidSeqNo => ResponseMessage.InvalidSeqNo,
        SaveSyobyoKeikaListStatus.InvalidSinDay => ResponseMessage.InvalidSinDay,
        SaveSyobyoKeikaListStatus.InvalidKeika => ResponseMessage.InvalidKeika,
        _ => string.Empty
    };
}
