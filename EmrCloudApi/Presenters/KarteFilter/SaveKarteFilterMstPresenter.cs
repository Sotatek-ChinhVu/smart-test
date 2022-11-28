using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.KarteFilter;
using UseCase.KarteFilter.SaveListKarteFilter;

namespace EmrCloudApi.Presenters.KarteFilter;

public class SaveKarteFilterMstPresenter : ISaveKarteFilterOutputPort
{
    public Response<SaveKarteFilterMstResponse> Result { get; private set; } = new Response<SaveKarteFilterMstResponse>();

    public void Complete(SaveKarteFilterOutputData outputData)
    {
        Result.Data = new SaveKarteFilterMstResponse(outputData.Status == SaveKarteFilterStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveKarteFilterStatus status) => status switch
    {
        SaveKarteFilterStatus.Successed => ResponseMessage.Success,
        SaveKarteFilterStatus.Failed => ResponseMessage.Failed,
        SaveKarteFilterStatus.GetSetListNoData => ResponseMessage.GetSetListNoData,
        _ => string.Empty
    };
}
