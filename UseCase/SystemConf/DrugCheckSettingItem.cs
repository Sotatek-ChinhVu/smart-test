namespace UseCase.SystemConf;

public class DrugCheckSettingItem
{
    public DrugCheckSettingItem(int checkDrugSameName, bool strainCheckSeibun, bool strainCheckPurodoragu, bool strainCheckRuiji, bool strainCheckKeito, int agentCheckSetting, bool dosageDrinkingDrugSetting, bool dosageDrugAsOrderSetting, bool dosageOtherDrugSetting, double dosageRatioSetting, bool allergyMedicineSeibun, bool allergyMedicinePurodoragu, bool allergyMedicineRuiji, bool allergyMedicineKeito, int foodAllergyLevelSetting, int diseaseLevelSetting, int kinkiLevelSetting, int dosageMinCheckSetting, int ageLevelSetting)
    {
        CheckDrugSameName = checkDrugSameName;
        StrainCheckSeibun = strainCheckSeibun;
        StrainCheckPurodoragu = strainCheckPurodoragu;
        StrainCheckRuiji = strainCheckRuiji;
        StrainCheckKeito = strainCheckKeito;
        AgentCheckSetting = agentCheckSetting;
        DosageDrinkingDrugSetting = dosageDrinkingDrugSetting;
        DosageDrugAsOrderSetting = dosageDrugAsOrderSetting;
        DosageOtherDrugSetting = dosageOtherDrugSetting;
        DosageRatioSetting = dosageRatioSetting;
        AllergyMedicineSeibun = allergyMedicineSeibun;
        AllergyMedicinePurodoragu = allergyMedicinePurodoragu;
        AllergyMedicineRuiji = allergyMedicineRuiji;
        AllergyMedicineKeito = allergyMedicineKeito;
        FoodAllergyLevelSetting = foodAllergyLevelSetting;
        DiseaseLevelSetting = diseaseLevelSetting;
        KinkiLevelSetting = kinkiLevelSetting;
        DosageMinCheckSetting = dosageMinCheckSetting;
        AgeLevelSetting = ageLevelSetting;
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
