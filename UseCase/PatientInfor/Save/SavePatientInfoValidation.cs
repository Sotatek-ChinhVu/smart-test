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
        [Description("The date of death must be indicated when the patient is dead")]
        InvalidRequiredDeathDate,
        [Description("HomePost property is valid")]
        InvalidHomePost,
        [Description("HomeAddress1 property is valid")]
        InvalidHomeAddress1,
        [Description("HomeAddress2 property is valid")]
        InvalidHomeAddress2,
        [Description("Tel1 property is valid")]
        InvalidTel1,
        [Description("Tel2 property is valid")]
        InvalidTel2,
        [Description("Email property is valid")]
        InvalidEmail,
        [Description("Setanusi property is valid")]
        InvalidSetanusi,
        [Description("Zokugara property is valid")]
        InvalidZokugara,
        [Description("Job property is valid")]
        InvalidJob,
        [Description("RenrakuName property is valid")]
        InvalidRenrakuName,
        [Description("RenrakuPost property is valid")]
        InvalidRenrakuPost,
        [Description("RenrakuAddress1 property is valid")]
        InvalidRenrakuAddress1,
        [Description("RenrakuAddress2 property is valid")]
        InvalidRenrakuAddress2,
        [Description("RenrakuTel property is valid")]
        InvalidRenrakuTel,
        [Description("RenrakuMemo property is valid")]
        InvalidRenrakuMemo,
        [Description("OfficeName property is valid")]
        InvalidOfficeName,
        [Description("OfficePost property is valid")]
        InvalidOfficePost,
        [Description("OfficeAddress1 property is valid")]
        InvalidOfficeAddress1,
        [Description("OfficeAddress2 property is valid")]
        InvalidOfficeAddress2,
        [Description("OfficeTel property is valid")]
        InvalidOfficeOfficeTel,
        [Description("OfficeOfficeMemo property is valid")]
        InvalidOfficeOfficeMemo
    }
}

