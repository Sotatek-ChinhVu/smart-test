using Domain.Models.SystemConf;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.SystemConf;
using UseCase.SystemConf.SaveDrugCheckSetting;

namespace Interactor.SystemConf;

public class SaveDrugCheckSettingInteractor : ISaveDrugCheckSettingInputPort
{
    private readonly ISystemConfRepository _systemConfRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveDrugCheckSettingInteractor(ITenantProvider tenantProvider, ISystemConfRepository systemConfRepository)
    {
        _systemConfRepository = systemConfRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveDrugCheckSettingOutputData Handle(SaveDrugCheckSettingInputData inputData)
    {
        try
        {
            var validateResult = ValidateInput(inputData.DrugCheckSetting);
            if (validateResult.Status != SaveDrugCheckSettingStatus.ValidateSuccess)
            {
                return new SaveDrugCheckSettingOutputData(validateResult.Status);
            }
            if (_systemConfRepository.SaveSystemConfigList(inputData.HpId, inputData.UserId, validateResult.SystemConfigList))
            {
                return new SaveDrugCheckSettingOutputData(SaveDrugCheckSettingStatus.Successed);
            }
            return new SaveDrugCheckSettingOutputData(SaveDrugCheckSettingStatus.Failed);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _systemConfRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }

    private (List<SystemConfModel> SystemConfigList, SaveDrugCheckSettingStatus Status) ValidateInput(DrugCheckSettingItem input)
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
        if (input.DosageRatioSetting < 1 || input.DosageRatioSetting > 9.999)
        {
            return (new(), SaveDrugCheckSettingStatus.InvalidDosageRatioSetting);
        }
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
        if (input.FoodAllergyLevelSetting < 0 || input.FoodAllergyLevelSetting > 3)
        {
            return (new(), SaveDrugCheckSettingStatus.InvalidFoodAllergyLevelSetting);
        }
        result.Add(ConvertToSystemConfModel(2027, 0, input.FoodAllergyLevelSetting));

        //set data for DiseaseLevelSetting
        if (input.DiseaseLevelSetting < 0 || input.DiseaseLevelSetting > 3)
        {
            return (new(), SaveDrugCheckSettingStatus.InvalidDiseaseLevelSetting);
        }
        result.Add(ConvertToSystemConfModel(2027, 2, input.DiseaseLevelSetting));

        //set data for KinkiLevelSetting
        if (input.KinkiLevelSetting < 0 || input.KinkiLevelSetting > 4)
        {
            return (new(), SaveDrugCheckSettingStatus.InvalidKinkiLevelSetting);
        }
        result.Add(ConvertToSystemConfModel(2027, 1, input.KinkiLevelSetting));

        //set data for DosageMinCheckSetting
        if (input.DosageMinCheckSetting < 0 || input.DosageMinCheckSetting > 1)
        {
            return (new(), SaveDrugCheckSettingStatus.InvalidDosageMinCheckSetting);
        }
        result.Add(ConvertToSystemConfModel(2023, 1, input.DosageMinCheckSetting));

        //set data for AgeLevelSetting
        if (input.AgeLevelSetting < 0 || input.AgeLevelSetting > 10)
        {
            return (new(), SaveDrugCheckSettingStatus.InvalidAgeLevelSetting);
        }
        result.Add(ConvertToSystemConfModel(2027, 3, input.AgeLevelSetting));

        return (result, SaveDrugCheckSettingStatus.ValidateSuccess);
    }

    private SystemConfModel ConvertToSystemConfModel(int grpCd, int grpEdaNo, double val)
    {
        return new SystemConfModel(grpCd, grpEdaNo, val);
    }
}
