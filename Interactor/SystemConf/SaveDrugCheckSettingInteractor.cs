using Domain.Models.SystemConf;
using UseCase.SystemConf;
using UseCase.SystemConf.SaveDrugCheckSetting;

namespace Interactor.SystemConf;

public class SaveDrugCheckSettingInteractor : ISaveDrugCheckSettingInputPort
{
    private readonly ISystemConfRepository _systemConfRepository;

    public SaveDrugCheckSettingInteractor(ISystemConfRepository systemConfRepository)
    {
        _systemConfRepository = systemConfRepository;
    }

    public SaveDrugCheckSettingOutputData Handle(SaveDrugCheckSettingInputData inputData)
    {
        try
        {

            return new SaveDrugCheckSettingOutputData(SaveDrugCheckSettingStatus.Successed);
        }
        finally
        {
            _systemConfRepository.ReleaseResource();
        }
    }

    private (List<SystemConfModel> systemConfigList, SaveDrugCheckSettingStatus status) ValidateInput(DrugCheckSettingItem input)
    {
        List<SystemConfModel> result = new();

        //set data for CheckDrugSameName
        if (input.CheckDrugSameName < 0 || input.CheckDrugSameName > 2)
        {
            return (result, SaveDrugCheckSettingStatus.InvalidCheckDrugSameName);
        }
        result.Add(ConvertToSystemConfModel(2027, 4, input.CheckDrugSameName));

        //set data for StrainCheckSeibun
        result.Add(ConvertToSystemConfModel(2024, 0, input.StrainCheckSeibun ? 1 : 0));

        //set data for StrainCheckPurodoragu
        result.Add(ConvertToSystemConfModel(2024, 1, input.StrainCheckPurodoragu ? 1 : 0));

        //set data for StrainCheckRuiji
        result.Add(ConvertToSystemConfModel(2024, 2, input.StrainCheckRuiji ? 1 : 0));

        //set data for StrainCheckKeito
        result.Add(ConvertToSystemConfModel(2024, 3, input.StrainCheckKeito ? 1 : 0));

        //set data for AgentCheckSetting
        if (input.AgentCheckSetting < 0 || input.AgentCheckSetting > 3)
        {
            return (result, SaveDrugCheckSettingStatus.InvalidAgentCheckSetting);
        }
        result.Add(ConvertToSystemConfModel(2025, 0, input.AgentCheckSetting));

        //set data for DosageDrinkingDrugSetting
        result.Add(ConvertToSystemConfModel(2023, 2, input.DosageDrinkingDrugSetting ? 1 : 0));

        //set data for DosageDrugAsOrderSetting
        result.Add(ConvertToSystemConfModel(2023, 3, input.DosageDrugAsOrderSetting ? 1 : 0));

        //set data for DosageOtherDrugSetting
        result.Add(ConvertToSystemConfModel(2023, 4, input.DosageOtherDrugSetting ? 1 : 0));

        //set data for DosageRatioSetting
        result.Add(ConvertToSystemConfModel(2023, 0, input.DosageRatioSetting));

        //set data for AllergyMedicineSeibun
        result.Add(ConvertToSystemConfModel(2026, 0, input.AllergyMedicineSeibun ? 1 : 0));

        //set data for AllergyMedicinePurodoragu
        result.Add(ConvertToSystemConfModel(2026, 1, input.AllergyMedicinePurodoragu ? 1 : 0));

        //set data for AllergyMedicineRuiji
        result.Add(ConvertToSystemConfModel(2026, 2, input.AllergyMedicineRuiji ? 1 : 0));

        //set data for AllergyMedicineKeito
        result.Add(ConvertToSystemConfModel(2026, 3, input.AllergyMedicineKeito ? 1 : 0));

        //set data for FoodAllergyLevelSetting


        //set data for DiseaseLevelSetting


        //set data for KinkiLevelSetting


        //set data for DosageMinCheckSetting


        //set data for AgeLevelSetting

        return (result, SaveDrugCheckSettingStatus.ValidateSuccess);
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

    private SystemConfModel ConvertToSystemConfModel(int grpCd, int grpEdaNo, double val)
    {
        return new SystemConfModel(grpCd, grpEdaNo, val);
    }
}
