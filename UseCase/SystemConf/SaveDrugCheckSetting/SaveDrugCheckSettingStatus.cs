namespace UseCase.SystemConf.SaveDrugCheckSetting;

public enum SaveDrugCheckSettingStatus : byte
{
    ValidateSuccess = 0,
    Successed,
    Failed,
    InvalidCheckDrugSameName,
    InvalidAgentCheckSetting,
    InvalidDosageRatioSetting,
    InvalidFoodAllergyLevelSetting,
    InvalidDiseaseLevelSetting,
    InvalidKinkiLevelSetting,
    InvalidDosageMinCheckSetting,
    InvalidAgeLevelSetting
}
