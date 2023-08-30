using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Domain.Models.Online;

public class PtInfConfirmationModel
{
    public PtInfConfirmationModel(string name, string value, string compareValue, int nameBasicInfoCheck, int kanaNameBasicInfoCheck, int genderBasicInfoCheck, int birthDayBasicInfoCheck, int addressBasicInfoCheck, int postcodeBasicInfoCheck, int seitaiNushiBasicInfoCheck)
    {
        AttributeName = name;
        CurrentValue = value.AsString();
        XmlValue = compareValue.AsString();
        CanCheck = !string.IsNullOrEmpty(XmlValue);
        switch (name)
        {
            case PtInfOQConst.HOME_ADDRESS:
                IsReflect = string.IsNullOrEmpty(XmlValue) || HenkanJ.Instance.ToFullsize(CurrentValueDisplay.AsString()).Equals(HenkanJ.Instance.ToFullsize(XmlValueDisplay.AsString()));
                break;
            default:
                IsReflect = string.IsNullOrEmpty(XmlValue) || CurrentValueDisplay.AsString().Equals(XmlValueDisplay.AsString());
                break;
        }

        NameBasicInfoCheck = nameBasicInfoCheck;
        KanaNameBasicInfoCheck = kanaNameBasicInfoCheck;
        GenderBasicInfoCheck = genderBasicInfoCheck;
        BirthDayBasicInfoCheck = birthDayBasicInfoCheck;
        AddressBasicInfoCheck = addressBasicInfoCheck;
        PostcodeBasicInfoCheck = postcodeBasicInfoCheck;
        SeitaiNushiBasicInfoCheck = seitaiNushiBasicInfoCheck;
    }

    public bool IgnoreConfig { get; private set; }

    public string AttributeName { get; private set; }

    public string CurrentValue { get; private set; }

    public string XmlValue { get; private set; }

    public bool CanCheck { get; private set; }

    public bool IsDefaultSetting { get; private set; } = true;

    public bool IsReflect { get; private set; }

    public int NameBasicInfoCheck { get; private set; }

    public int KanaNameBasicInfoCheck { get; private set; }

    public int GenderBasicInfoCheck { get; private set; }

    public int BirthDayBasicInfoCheck { get; private set; }

    public int AddressBasicInfoCheck { get; private set; }

    public int PostcodeBasicInfoCheck { get; private set; }

    public int SeitaiNushiBasicInfoCheck { get; private set; }

    public string CurrentValueDisplay
    {
        get
        {
            switch (AttributeName)
            {
                case PtInfOQConst.HOME_POST:
                    {
                        if (!string.IsNullOrEmpty(CurrentValue) && CurrentValue.Length > 3)
                        {
                            return CurrentValue.Substring(0, 3) + "-" + CurrentValue.Substring(3);
                        }
                        break;
                    }
                case PtInfOQConst.SEX:
                    return ConvertGender(CurrentValue.AsInteger());
                case PtInfOQConst.BIRTHDAY:
                    if (CIUtil.CheckSDate(CurrentValue))
                    {
                        return CIUtil.SDateToShowWSDate(CurrentValue.AsInteger());
                    }
                    return string.Empty;
            }
            return CurrentValue;
        }
    }

    public string XmlValueDisplay
    {
        get
        {
            switch (AttributeName)
            {
                case PtInfOQConst.SEX:
                    return ConvertGender(XmlValue.AsInteger(), true);
                case PtInfOQConst.BIRTHDAY:
                    if (CIUtil.CheckSDate(XmlValue))
                    {
                        return CIUtil.SDateToShowWSDate(XmlValue.AsInteger());
                    }
                    return string.Empty;
                default:
                    if (string.IsNullOrEmpty(XmlValue))
                    {
                        return "(情報なし)";
                    }
                    break;
            }
            return XmlValue;
        }
    }

    public string Reflect
    {
        get
        {
            if (!CanCheck) return string.Empty;
            switch (AttributeName)
            {
                case PtInfOQConst.KANJI_NAME:
                    {
                        if (!IsReflect && NameBasicInfoCheck == 1 && IsDefaultSetting)
                        {
                            return string.Empty;
                        }
                        break;
                    }
                case PtInfOQConst.KANA_NAME:
                    {
                        if (!IsReflect && KanaNameBasicInfoCheck == 1 && IsDefaultSetting)
                        {
                            return string.Empty;
                        }
                        break;
                    }
                case PtInfOQConst.SEX:
                    {
                        if (!IsReflect && GenderBasicInfoCheck == 1 && IsDefaultSetting)
                        {
                            return string.Empty;
                        }
                        break;
                    }
                case PtInfOQConst.BIRTHDAY:
                    {
                        if (!IsReflect && BirthDayBasicInfoCheck == 1 && IsDefaultSetting)
                        {
                            return string.Empty;
                        }
                        break;
                    }
                case PtInfOQConst.HOME_ADDRESS:
                    {
                        if (!IsReflect && AddressBasicInfoCheck == 1 && IsDefaultSetting)
                        {
                            return string.Empty;
                        }
                        break;
                    }
                case PtInfOQConst.HOME_POST:
                    {
                        if (!IsReflect && PostcodeBasicInfoCheck == 1 && IsDefaultSetting)
                        {
                            return string.Empty;
                        }
                        break;
                    }
                case PtInfOQConst.SETANUSI:
                    {
                        if (!IsReflect && SeitaiNushiBasicInfoCheck == 1 && IsDefaultSetting)
                        {
                            return string.Empty;
                        }
                        break;
                    }
            }
            return IsReflect ? string.Empty : "✓";
        }
    }

    public bool IsVisible
    {
        get
        {
            switch (AttributeName)
            {
                case PtInfOQConst.KANJI_NAME:
                    return NameBasicInfoCheck != 0;
                case PtInfOQConst.KANA_NAME:
                    return KanaNameBasicInfoCheck != 0;
                case PtInfOQConst.SEX:
                    return IgnoreConfig || GenderBasicInfoCheck != 0;
                case PtInfOQConst.BIRTHDAY:
                    return IgnoreConfig || BirthDayBasicInfoCheck != 0;
                case PtInfOQConst.HOME_ADDRESS:
                    return AddressBasicInfoCheck != 0;
                case PtInfOQConst.HOME_POST:
                    return PostcodeBasicInfoCheck != 0;
                case PtInfOQConst.SETANUSI:
                    return SeitaiNushiBasicInfoCheck != 0;
                default:
                    return true;
            }
        }
    }

    private string ConvertGender(int gender, bool isXml = false)
    {
        if (gender == 1)
        {
            return "男";
        }
        else if (gender == 2)
        {
            return "女";
        }
        if (isXml && gender != 3)
        {
            return "(情報なし)";
        }
        return string.Empty;
    }
}
