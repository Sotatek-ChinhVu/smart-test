using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Receipt;
using EmrCloudApi.Responses;
using UseCase.Receipt.CheckExistsReceInf;

namespace EmrCloudApi.Presenters.Receipt;

public class CheckExistsReceInfPresenter : ICheckExistsReceInfOutputPort
{
    public Response<CheckExistsReceInfResponse> Result { get; private set; } = new();

    public void Complete(CheckExistsReceInfOutputData outputData)
    {
        Result.Data = new CheckExistsReceInfResponse(outputData.IsExisted);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(CheckExistsReceInfStatus status) => status switch
    {
        CheckExistsReceInfStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
