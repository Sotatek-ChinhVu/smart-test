namespace EmrCloudApi.Constants;

// Reference: https://wiki.sotatek.com/display/SMAR/1.1+Auto+refresh+code
public static class FunctionCodes
{
    public const string ReceptionChanged = "ReceptionChanged";
    public const string PatientInfChanged = "PatientInfChanged";
    public const string MedicalChanged = "MedicalChanged";
    public const string LockChanged = "LockChanged";
    public const string SupserSetSaveChanged = "SupserSetSaveChanged";
    public const string SupserSetReorderChanged = "SupserSetReorderChanged";
    public const string SuperCopyPasteChanged = "SuperCopyPasteChanged";
    public const string AccountDueChanged = "AccountDueChanged";
    public const string DeletePtInfChanged = "DeletePtInfChanged";
}
