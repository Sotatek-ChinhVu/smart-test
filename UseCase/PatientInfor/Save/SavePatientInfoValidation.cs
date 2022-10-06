using System.ComponentModel;

namespace UseCase.PatientInfor.Save
{
    public enum SavePatientInfoValidation
    {
        [Description("HpId property is valid")]
        InvalidHpId,
        [Description("Name property is valid")]
        InvalidName,
        [Description("KanaName property is valid")]
        InvalidKanaName,
    }
}

