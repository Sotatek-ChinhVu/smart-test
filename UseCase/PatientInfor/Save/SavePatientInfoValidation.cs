using System.ComponentModel;

namespace UseCase.PatientInfor.Save
{
    public enum SavePatientInfoValidation
    {
        [Description("HpId property is valid")]
        InvalidHpId,
        [Description("Patient Name property is valid")]
        InvalidName,
        [Description("Patient KanaName property is valid")]
        InvalidKanaName,
    }
}

