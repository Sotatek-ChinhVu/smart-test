using System.ComponentModel;

namespace UseCase.PatientInfor.Save
{
    public enum SavePatientInfoValidation
    {
        [Description("{0} property is required")]
        PropertyIsRequired,
        [Description("{0} property is invalid")]
        PropertyIsInvalid
    }
}