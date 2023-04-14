using UseCase.SystemConf;

namespace EmrCloudApi.Responses.SystemConf;

public class GetDrugCheckSettingResponse
{
    public GetDrugCheckSettingResponse(DrugCheckSettingItem output)
    {
        CheckDrugSameName = output.CheckDrugSameName;
        StrainCheckSeibun = output.StrainCheckSeibun;
        StrainCheckPurodoragu = output.StrainCheckPurodoragu;
        StrainCheckRuiji = output.StrainCheckRuiji;
        StrainCheckKeito = output.StrainCheckKeito;
        AgentCheckSetting = output.AgentCheckSetting;
        DosageDrinkingDrugSetting = output.DosageDrinkingDrugSetting;
        DosageDrugAsOrderSetting = output.DosageDrugAsOrderSetting;
        DosageOtherDrugSetting = output.DosageOtherDrugSetting;
        DosageRatioSetting = output.DosageRatioSetting;
        AllergyMedicineSeibun = output.AllergyMedicineSeibun;
        AllergyMedicinePurodoragu = output.AllergyMedicinePurodoragu;
        AllergyMedicineRuiji = output.AllergyMedicineRuiji;
        AllergyMedicineKeito = output.AllergyMedicineKeito;
        FoodAllergyLevelSetting = output.FoodAllergyLevelSetting;
        DiseaseLevelSetting = output.DiseaseLevelSetting;
        KinkiLevelSetting = output.KinkiLevelSetting;
        DosageMinCheckSetting = output.DosageMinCheckSetting;
        AgeLevelSetting = output.AgeLevelSetting;
    }

    public int CheckDrugSameName { get; private set; }

    public bool StrainCheckSeibun { get; private set; }

    public bool StrainCheckPurodoragu { get; private set; }

    public bool StrainCheckRuiji { get; private set; }

    public bool StrainCheckKeito { get; private set; }

    public int AgentCheckSetting { get; private set; }

    public bool DosageDrinkingDrugSetting { get; private set; }

    public bool DosageDrugAsOrderSetting { get; private set; }

    public bool DosageOtherDrugSetting { get; private set; }

    public double DosageRatioSetting { get; private set; }

    public bool AllergyMedicineSeibun { get; private set; }

    public bool AllergyMedicinePurodoragu { get; private set; }

    public bool AllergyMedicineRuiji { get; private set; }

    public bool AllergyMedicineKeito { get; private set; }

    public int FoodAllergyLevelSetting { get; private set; }

    public int DiseaseLevelSetting { get; private set; }

    public int KinkiLevelSetting { get; private set; }

    public int DosageMinCheckSetting { get; private set; }

    public int AgeLevelSetting { get; private set; }
}
