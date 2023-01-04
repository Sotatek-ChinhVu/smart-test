using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Santei;
using UseCase.Santei.GetListSanteiInf;

namespace EmrCloudApi.Presenters.Santei;

public class GetListSanteiInfPresenter : IGetListSanteiInfOutputPort
{
    public Response<GetListSanteiInfResponse> Result { get; private set; } = new();

    public void Complete(GetListSanteiInfOutputData outputData)
    {
        Result.Data = new GetListSanteiInfResponse(outputData.ListSanteiInfs, outputData.AlertTermCombobox, outputData.KisanKbnCombobox, outputData.ListByomeis);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetListSanteiInfStatus status) => status switch
    {
        GetListSanteiInfStatus.Successed => ResponseMessage.Success,
        GetListSanteiInfStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
