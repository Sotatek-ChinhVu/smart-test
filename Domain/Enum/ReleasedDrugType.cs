namespace Domain.Enum
{
    public enum ReleasedDrugType
    {
        None = 6,
        Unchangeable = 3,
        CommonName = 13,
        CommonName_DoNotChangeTheDosageForm = 14,
        CommonName_DoesNotChangeTheContentStandard = 15,
        CommonName_DoesNotChangeTheContentStandardOrDosageForm = 16,
        Changeable = 0,
        Changeable_DoNotChangeTheDosageForm = 7,
        Changeable_DoesNotChangeTheContentStandard = 8,
        Changeable_DoesNotChangeTheContentStandardOrDosageForm = 9
    }
}
