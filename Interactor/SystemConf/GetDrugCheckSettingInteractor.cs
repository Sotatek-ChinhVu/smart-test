using Domain.Models.SystemConf;
using UseCase.SystemConf;
using UseCase.SystemConf.GetDrugCheckSetting;

namespace Interactor.SystemConf;

public class GetDrugCheckSettingInteractor : IGetDrugCheckSettingInputPort
{
    private readonly ISystemConfRepository _systemConfRepository;

    public GetDrugCheckSettingInteractor(ISystemConfRepository systemConfRepository)
    {
        _systemConfRepository = systemConfRepository;
    }

    public GetDrugCheckSettingOutputData Handle(GetDrugCheckSettingInputData inputData)
    {
        try
        {
            List<int> systemConfigCodeList = new List<int> { 2023, 2024, 2025, 2026, 2027 };
            var systemConfigList = _systemConfRepository.GetList(inputData.HpId, systemConfigCodeList);
            return new GetDrugCheckSettingOutputData(ConvertSystemConfigToModel(systemConfigList), GetDrugCheckSettingStatus.Successed);
        }
        finally
        {
            _systemConfRepository.ReleaseResource();
        }
    }

    private DrugCheckSettingItem ConvertSystemConfigToModel(List<SystemConfModel> systemConfigList)
    {
        int checkDrugSameName = (int)GetConfigValue(systemConfigList, 2027, 4, 0);
        bool strainCheckSeibun = GetConfigValue(systemConfigList, 2024, 0, 1) != 0;
        bool strainCheckPurodoragu = GetConfigValue(systemConfigList, 2024, 1, 1) != 0;
        bool strainCheckRuiji = GetConfigValue(systemConfigList, 2024, 2, 1) != 0;
        bool strainCheckKeito = GetConfigValue(systemConfigList, 2024, 3, 1) != 0;
        int agentCheckSetting = (int)GetConfigValue(systemConfigList, 2025, 0, 3);
        bool dosageDrinkingDrugSetting = GetConfigValue(systemConfigList, 2023, 2, 1) != 0;
        bool dosageDrugAsOrderSetting = GetConfigValue(systemConfigList, 2023, 3, 1) != 0;
        bool dosageOtherDrugSetting = GetConfigValue(systemConfigList, 2023, 4, 1) != 0;
        double dosageRatioSetting = GetConfigValue(systemConfigList, 2023, 0, 1);
        bool allergyMedicineSeibun = GetConfigValue(systemConfigList, 2026, 0, 1) != 0;
        bool allergyMedicinePurodoragu = GetConfigValue(systemConfigList, 2026, 1, 1) != 0;
        bool allergyMedicineRuiji = GetConfigValue(systemConfigList, 2026, 2, 1) != 0;
        bool allergyMedicineKeito = GetConfigValue(systemConfigList, 2026, 3, 1) != 0;
        int foodAllergyLevelSetting = (int)GetConfigValue(systemConfigList, 2027, 0, 3);
        int diseaseLevelSetting = (int)GetConfigValue(systemConfigList, 2027, 2, 3);
        int kinkiLevelSetting = (int)GetConfigValue(systemConfigList, 2027, 1, 4);
        int dosageMinCheckSetting = (int)GetConfigValue(systemConfigList, 2023, 1, 0);
        int ageLevelSetting = (int)GetConfigValue(systemConfigList, 2027, 3, 10);


        return new DrugCheckSettingItem(
                   checkDrugSameName,
                   strainCheckSeibun,
                   strainCheckPurodoragu,
                   strainCheckRuiji,
                   strainCheckKeito,
                   agentCheckSetting,
                   dosageDrinkingDrugSetting,
                   dosageDrugAsOrderSetting,
                   dosageOtherDrugSetting,
                   dosageRatioSetting,
                   allergyMedicineSeibun,
                   allergyMedicinePurodoragu,
                   allergyMedicineRuiji,
                   allergyMedicineKeito,
                   foodAllergyLevelSetting,
                   diseaseLevelSetting,
                   kinkiLevelSetting,
                   dosageMinCheckSetting,
                   ageLevelSetting);
    }

    private double GetConfigValue(List<SystemConfModel> systemConfigList, int grpCd, int grpEdaNo, double defaultValue = 0)
    {
        if (systemConfigList == null)
        {
            return defaultValue;
        }
        var systemConfig = systemConfigList.FirstOrDefault(u => u.GrpCd == grpCd && u.GrpEdaNo == grpEdaNo);
        return systemConfig == null ? defaultValue : systemConfig.Val;
    }
}
