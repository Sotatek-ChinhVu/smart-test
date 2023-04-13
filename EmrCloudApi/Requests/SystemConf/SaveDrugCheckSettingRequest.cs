namespace EmrCloudApi.Requests.SystemConf;

public class SaveDrugCheckSettingRequest
{
    public int CheckDrugSameName { get; set; }

    public bool StrainCheckSeibun { get; set; }

    public bool StrainCheckPurodoragu { get; set; }

    public bool StrainCheckRuiji { get; set; }

    public bool StrainCheckKeito { get; set; }

    public int AgentCheckSetting { get; set; }

    public bool DosageDrinkingDrugSetting { get; set; }

    public bool DosageDrugAsOrderSetting { get; set; }

    public bool DosageOtherDrugSetting { get; set; }

    public double DosageRatioSetting { get; set; }

    public bool AllergyMedicineSeibun { get; set; }

    public bool AllergyMedicinePurodoragu { get; set; }

    public bool AllergyMedicineRuiji { get; set; }

    public bool AllergyMedicineKeito { get; set; }

    public int FoodAllergyLevelSetting { get; set; }

    public int DiseaseLevelSetting { get; set; }

    public int KinkiLevelSetting { get; set; }

    public int DosageMinCheckSetting { get; set; }

    public int AgeLevelSetting { get; set; }
}
