using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using UseCase.SystemConf.SaveDrugCheckSetting;

namespace EmrCloudApi.Presenters.SytemConf;

public class SaveDrugCheckSettingPresenter : ISaveDrugCheckSettingOutputPort
{
    public Response<SaveDrugCheckSettingResponse> Result { get; private set; } = new();

    public void Complete(SaveDrugCheckSettingOutputData outputData)
    {
        Result.Data = new SaveDrugCheckSettingResponse(outputData.Status == SaveDrugCheckSettingStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveDrugCheckSettingStatus status) => status switch
    {
        SaveDrugCheckSettingStatus.Successed => ResponseMessage.Success,
        SaveDrugCheckSettingStatus.Failed => ResponseMessage.Failed,
        SaveDrugCheckSettingStatus.InvalidCheckDrugSameName => ResponseMessage.InvalidCheckDrugSameName,
        SaveDrugCheckSettingStatus.InvalidAgentCheckSetting => ResponseMessage.InvalidAgentCheckSetting,
        SaveDrugCheckSettingStatus.InvalidDosageRatioSetting => ResponseMessage.InvalidDosageRatioSetting,
        SaveDrugCheckSettingStatus.InvalidFoodAllergyLevelSetting => ResponseMessage.InvalidFoodAllergyLevelSetting,
        SaveDrugCheckSettingStatus.InvalidDiseaseLevelSetting => ResponseMessage.InvalidDiseaseLevelSetting,
        SaveDrugCheckSettingStatus.InvalidKinkiLevelSetting => ResponseMessage.InvalidKinkiLevelSetting,
        SaveDrugCheckSettingStatus.InvalidDosageMinCheckSetting => ResponseMessage.InvalidDosageMinCheckSetting,
        SaveDrugCheckSettingStatus.InvalidAgeLevelSetting => ResponseMessage.InvalidAgeLevelSetting,
        _ => string.Empty
    };
}