using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SaveListSyoukiInf;

namespace EmrCloudApi.Presenters.Receipt;

public class SaveSyoukiInfListPresenter : ISaveSyoukiInfListOutputPort
{
    public Response<SaveSyoukiInfListResponse> Result { get; private set; } = new();

    public void Complete(SaveSyoukiInfListOutputData outputData)
    {
        Result.Data = new SaveSyoukiInfListResponse(outputData.Status == SaveSyoukiInfListStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveSyoukiInfListStatus status) => status switch
    {
        SaveSyoukiInfListStatus.Successed => ResponseMessage.Success,
        SaveSyoukiInfListStatus.Failed => ResponseMessage.Failed,
        SaveSyoukiInfListStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveSyoukiInfListStatus.InvalidSinYm => ResponseMessage.InvalidSinYm,
        SaveSyoukiInfListStatus.InvalidSeqNo => ResponseMessage.InvalidSeqNo,
        SaveSyoukiInfListStatus.InvalidSyoukiKbn => ResponseMessage.InvalidSyoukiKbn,
        SaveSyoukiInfListStatus.InvalidHokenId => ResponseMessage.InvalidHokenId,
        _ => string.Empty
    };
}