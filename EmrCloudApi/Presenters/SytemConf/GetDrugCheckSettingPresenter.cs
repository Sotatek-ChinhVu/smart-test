using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using UseCase.SystemConf.GetDrugCheckSetting;

namespace EmrCloudApi.Presenters.SytemConf;

public class GetDrugCheckSettingPresenter : IGEtDrugCheckSettingOutputPort
{
    public Response<GetDrugCheckSettingResponse> Result { get; private set; } = new();

    public void Complete(GetDrugCheckSettingOutputData outputData)
    {
        Result.Data = new GetDrugCheckSettingResponse(outputData.DrugCheckSettingData);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetDrugCheckSettingStatus status) => status switch
    {
        GetDrugCheckSettingStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

