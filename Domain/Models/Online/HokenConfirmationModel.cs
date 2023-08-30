using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using System.Xml.Serialization;
using UseCase.Online.QualificationConfirmation;

namespace Domain.Models.Online;

public class HokenConfirmationModel
{
    public HokenConfirmationModel(string name, string value, string compareValue, int age = -1, int sinDate = 0, string confirmationResult = "")
    {
        AttributeName = name;
        Age = age;
        SinDate = sinDate;
        CurrentValue = value.AsString();
        var response = new XmlSerializer(typeof(QCXmlMsgResponse)).Deserialize(new StringReader(confirmationResult)) as QCXmlMsgResponse;

        if (response != null && response.MessageBody.ResultList != null && response.MessageBody.ResultList.ResultOfQualificationConfirmation.Length == 0)
        {
            ResultOfQualificationConfirmation = response.MessageBody.ResultList.ResultOfQualificationConfirmation[0];
        }

        if (AttributeName == HokenConfOnlQuaConst.KOGAKU_KBN)
        {
            XmlValue = GetXmlValue().AsString();
        }
        else
        {
            XmlValue = compareValue.AsString().Trim();
        }

        // Reflect
        IsReflect = "(情報なし)".Equals(XmlValueDisplay) || CurrentValueDisplay.AsString().Equals(XmlValueDisplay.AsString());
        if (AttributeName.Equals(HokenConfOnlQuaConst.END_DATE) && !IsReflect)
        {
            IsReflect = value.AsInteger() == 99999999 && string.IsNullOrEmpty(XmlValue);
        }
        else if (AttributeName == HokenConfOnlQuaConst.HOKENSYA_NO)
        {
            IsReflect = CurrentValueDisplay.AsString().Trim().Equals(XmlValueDisplay.AsString().Trim());
        }
        else if (AttributeName == HokenConfOnlQuaConst.KIGO
                || AttributeName == HokenConfOnlQuaConst.BANGO)
        {
            IsReflect = HenkanJ.Instance.ToHalfsize(CurrentValueDisplay.AsString()).Replace("/", "").Trim()
                        .Equals(HenkanJ.Instance.ToHalfsize(XmlValueDisplay.AsString()).Replace("/", ""));
        }
        else if (AttributeName == HokenConfOnlQuaConst.KOGAKU_KBN)
        {
            IsReflect = GetIsReflect(CurrentValue.AsInteger(), CurrentValueDisplay);
        }

        // Can check 
        if (string.IsNullOrEmpty(XmlValue)
            || AttributeName == HokenConfOnlQuaConst.HOKENSYA_NO
            || AttributeName == HokenConfOnlQuaConst.KIGO
            || AttributeName == HokenConfOnlQuaConst.BANGO
            || AttributeName == HokenConfOnlQuaConst.EDANO)
        {
            CanCheck = false;
        }

        if (AttributeName == HokenConfOnlQuaConst.KOGAKU_KBN)
        {
            CanCheck = !IsReflect;
        }
    }

    public int SinDate { get; private set; }

    public string AttributeName { get; private set; }

    public string CurrentValue { get; private set; }

    public string XmlValue { get; private set; }

    public bool CanCheck { get; private set; }

    public bool IsReflect { get; private set; }

    public int Age { get; private set; }

    public ResultOfQualificationConfirmation ResultOfQualificationConfirmation { get; private set; }

    public string CurrentValueDisplay
    {
        get
        {
            switch (AttributeName)
            {
                case HokenConfOnlQuaConst.HONKE:
                    return HonkeKbnToString(CurrentValue.AsInteger());
                case HokenConfOnlQuaConst.KOFU_DATE:
                case HokenConfOnlQuaConst.START_DATE:
                case HokenConfOnlQuaConst.END_DATE:
                    if (CIUtil.CheckSDate(CurrentValue))
                    {
                        return CIUtil.SDateToShowWSDate(CurrentValue.AsInteger());
                    }
                    return string.Empty;
                case HokenConfOnlQuaConst.KOGAKU_KBN:
                    if (Age >= 0)
                    {
                        return KogakuKbnToLimitConsFlgConverter.KogakuKbnToDisplay(CurrentValue.AsInteger(), Age);
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
            string returnValue = string.Empty;
            switch (AttributeName)
            {
                case HokenConfOnlQuaConst.HONKE:
                    string honke = HonkeKbnToString(XmlValue.AsInteger());
                    if (!string.IsNullOrEmpty(honke))
                    {
                        returnValue = honke;
                    }
                    break;
                case HokenConfOnlQuaConst.KOFU_DATE:
                case HokenConfOnlQuaConst.START_DATE:
                case HokenConfOnlQuaConst.END_DATE:
                    if (CIUtil.CheckSDate(XmlValue))
                    {
                        returnValue = CIUtil.SDateToShowWSDate(XmlValue.AsInteger());
                    }
                    break;
                case HokenConfOnlQuaConst.KOGAKU_KBN:
                    if (Age >= 0)
                    {
                        returnValue = GetXmlDisplay(CurrentValueDisplay);
                    }
                    break;
                default:
                    if (!string.IsNullOrEmpty(XmlValue))
                    {
                        returnValue = XmlValue;
                    }
                    break;
            }

            if (string.IsNullOrEmpty(returnValue) && AttributeName != HokenConfOnlQuaConst.KIGO && AttributeName != HokenConfOnlQuaConst.CREDENTIAL)
            {
                returnValue = "(情報なし)";
            }

            return returnValue;
        }
    }

    public string Reflect
    {
        get
        {
            return IsReflect ? string.Empty : "✓";
        }
    }

    private string HonkeKbnToString(int honkeKbn)
    {
        switch (honkeKbn)
        {
            case 1:
                return "本人";
            case 2:
                return "家族";
            default:
                return "";
        }
    }

    public bool CheckDefaultValue()
    {
        return false;
    }

    #region private function
    public string GetXmlDisplay(string currentKogakuKbnDisplay, bool isMyNumberProcess = false)
    {
        string limitCertificateFlag = ResultOfQualificationConfirmation?.LimitApplicationCertificateRelatedInfo?.LimitApplicationCertificateClassificationFlag ?? string.Empty;
        if (string.IsNullOrEmpty(limitCertificateFlag))
        {
            string InsurerNumber = ResultOfQualificationConfirmation?.InsurerNumber ?? string.Empty;
            if (!string.IsNullOrEmpty(InsurerNumber) && CIUtil.IsNumberic(InsurerNumber) && InsurerNumber.Length == 8 && InsurerNumber.StartsWith("39"))
            {
                string insuredPartialContributionRatio = ResultOfQualificationConfirmation?.InsuredPartialContributionRatio ?? string.Empty;
                if (!string.IsNullOrEmpty(insuredPartialContributionRatio))
                {
                    switch (insuredPartialContributionRatio)
                    {
                        case "030":
                            if (isMyNumberProcess)
                            {
                                return "情報なし(26）";
                            }
                            if (currentKogakuKbnDisplay == "26 現役Ⅲ" || currentKogakuKbnDisplay == "27 現役Ⅱ" || currentKogakuKbnDisplay == "28 現役Ⅰ")
                            {
                                return "(情報なし)";
                            }
                            return "26 現役Ⅲ";
                        case "020":
                            if (isMyNumberProcess)
                            {
                                return "情報なし(41）";
                            }
                            if (currentKogakuKbnDisplay == "41 一般Ⅱ")
                            {
                                return "(情報なし)";
                            }
                            return "41 一般Ⅱ";
                        case "010":
                            if (isMyNumberProcess)
                            {
                                return "情報なし(0）";
                            }
                            if (currentKogakuKbnDisplay == "" || currentKogakuKbnDisplay == "0 一般" || currentKogakuKbnDisplay == "4 低所Ⅱ" || currentKogakuKbnDisplay == "5 低所Ⅰ")
                            {
                                return "(情報なし)";
                            }
                            return "0 一般";
                    }
                }
            }
            string elderlyRecipientCertificateInfo = ResultOfQualificationConfirmation?.ElderlyRecipientCertificateInfo?.ElderlyRecipientContributionRatio ?? string.Empty;
            if (!string.IsNullOrEmpty(elderlyRecipientCertificateInfo))
            {
                switch (elderlyRecipientCertificateInfo)
                {
                    case "030":
                        if (isMyNumberProcess)
                        {
                            return "情報なし(26）";
                        }
                        if (currentKogakuKbnDisplay == "26 現役Ⅲ" || currentKogakuKbnDisplay == "27 現役Ⅱ" || currentKogakuKbnDisplay == "28 現役Ⅰ")
                        {
                            return "(情報なし)";
                        }
                        return "26 現役Ⅲ";
                    case "020":
                    case "010":
                        if (isMyNumberProcess)
                        {
                            return "情報なし(0）";
                        }
                        if (currentKogakuKbnDisplay == "" || currentKogakuKbnDisplay == "0 一般" || currentKogakuKbnDisplay == "4 低所Ⅱ" || currentKogakuKbnDisplay == "5 低所Ⅰ")
                        {
                            return "(情報なし)";
                        }
                        return "0 一般";
                }
            }
            return "(情報なし)";
        }
        return KogakuKbnToLimitConsFlgConverter.KogakuKbnClassToDisplay(limitCertificateFlag, Age);
    }

    public string GetXmlValue()
    {
        string limitCertificateFlag = ResultOfQualificationConfirmation?.LimitApplicationCertificateRelatedInfo?.LimitApplicationCertificateClassificationFlag ?? string.Empty;
        if (string.IsNullOrEmpty(limitCertificateFlag))
        {
            string InsurerNumber = ResultOfQualificationConfirmation?.InsurerNumber ?? string.Empty;
            if (!string.IsNullOrEmpty(InsurerNumber) && CIUtil.IsNumberic(InsurerNumber) && InsurerNumber.Length == 8 && InsurerNumber.StartsWith("39"))
            {
                string insuredPartialContributionRatio = ResultOfQualificationConfirmation?.InsuredPartialContributionRatio ?? string.Empty;
                if (!string.IsNullOrEmpty(insuredPartialContributionRatio))
                {
                    switch (insuredPartialContributionRatio)
                    {
                        case "030":
                            return "26";
                        case "020":
                            return "41";
                        case "010":
                            return "0";
                    }
                }
            }

            string elderlyRecipientCertificateInfo = ResultOfQualificationConfirmation?.ElderlyRecipientCertificateInfo?.ElderlyRecipientContributionRatio ?? string.Empty;
            if (!string.IsNullOrEmpty(elderlyRecipientCertificateInfo))
            {
                switch (elderlyRecipientCertificateInfo)
                {
                    case "030":
                        return "26";
                    case "020":
                    case "010":
                        return "0";
                }
            }
            return "0";
        }
        return limitCertificateFlag;
    }

    public bool GetIsReflect(int currentKogakuKbn, string currentKogakuKbnDisplay)
    {
        string limitCertificateFlag = ResultOfQualificationConfirmation?.LimitApplicationCertificateRelatedInfo?.LimitApplicationCertificateClassificationFlag ?? string.Empty;
        if (string.IsNullOrEmpty(limitCertificateFlag))
        {
            string InsurerNumber = ResultOfQualificationConfirmation?.InsurerNumber ?? string.Empty;
            if (!string.IsNullOrEmpty(InsurerNumber) && CIUtil.IsNumberic(InsurerNumber) && InsurerNumber.Length == 8 && InsurerNumber.StartsWith("39"))
            {
                string insuredPartialContributionRatio = ResultOfQualificationConfirmation?.InsuredPartialContributionRatio ?? string.Empty;
                if (!string.IsNullOrEmpty(insuredPartialContributionRatio))
                {
                    switch (insuredPartialContributionRatio)
                    {
                        case "030":
                            if (currentKogakuKbnDisplay == "26 現役Ⅲ" || currentKogakuKbnDisplay == "27 現役Ⅱ" || currentKogakuKbnDisplay == "28 現役Ⅰ")
                            {
                                return true;
                            }
                            return false;
                        case "020":
                            if (currentKogakuKbnDisplay == "41 一般Ⅱ")
                            {
                                return true;
                            }
                            return false;
                        case "010":
                            if (currentKogakuKbnDisplay == "" || currentKogakuKbnDisplay == "0 一般" || currentKogakuKbnDisplay == "4 低所Ⅱ" || currentKogakuKbnDisplay == "5 低所Ⅰ")
                            {
                                return true;
                            }
                            return false;
                    }
                }
            }
            string elderlyRecipientCertificateInfo = ResultOfQualificationConfirmation?.ElderlyRecipientCertificateInfo?.ElderlyRecipientContributionRatio ?? string.Empty;
            if (!string.IsNullOrEmpty(elderlyRecipientCertificateInfo))
            {
                switch (elderlyRecipientCertificateInfo)
                {
                    case "030":
                        if (currentKogakuKbnDisplay == "26 現役Ⅲ" || currentKogakuKbnDisplay == "27 現役Ⅱ" || currentKogakuKbnDisplay == "28 現役Ⅰ")
                        {
                            return true;
                        }
                        return false;
                    case "020":
                    case "010":
                        if (currentKogakuKbnDisplay == "" || currentKogakuKbnDisplay == "0 一般" || currentKogakuKbnDisplay == "4 低所Ⅱ" || currentKogakuKbnDisplay == "5 低所Ⅰ")
                        {
                            return true;
                        }
                        return false;
                }
            }

            return true;
        }
        if (currentKogakuKbnDisplay == "" && GetXmlDisplay(currentKogakuKbnDisplay) == "0 一般")
        {
            return true;
        }
        return currentKogakuKbnDisplay == GetXmlDisplay(currentKogakuKbnDisplay);
    }
    #endregion
}
