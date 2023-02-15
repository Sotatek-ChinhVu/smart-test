using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SaveListSyoukiInf;

namespace EmrCloudApi.Presenters.Receipt;

public class SaveListSyoukiInfPresenter : ISaveListSyoukiInfOutputPort
{
    public Response<SaveListSyoukiInfResponse> Result { get; private set; } = new();

    public void Complete(SaveListSyoukiInfOutputData outputData)
    {
        Result.Data = new SaveListSyoukiInfResponse(outputData.Status == SaveListSyoukiInfStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveListSyoukiInfStatus status) => status switch
    {
        SaveListSyoukiInfStatus.Successed => ResponseMessage.Success,
        SaveListSyoukiInfStatus.Failed => ResponseMessage.Failed,
        SaveListSyoukiInfStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveListSyoukiInfStatus.InvalidSinYm => ResponseMessage.InvalidSinYm,
        SaveListSyoukiInfStatus.InvalidSeqNo => ResponseMessage.InvalidSeqNo,
        SaveListSyoukiInfStatus.InvalidSyoukiKbn => ResponseMessage.InvalidSyoukiKbn,
        _ => string.Empty
    };
}