using Domain.Converter;
using Domain.Models.Online.QualificationConfirmation;
using Helper.Common;

namespace Domain.Models.Online
{
    public class OnlineKogakuHelper
    {
        private ResultOfQualificationConfirmation _resultOfQualification;
        private int Age;
        public OnlineKogakuHelper(ResultOfQualificationConfirmation resultOfQualification, int age)
        {
            _resultOfQualification = resultOfQualification;
            Age = age;
        }

        public bool IsLimitCertificate()
        {
            return !string.IsNullOrEmpty(_resultOfQualification.LimitApplicationCertificateRelatedInfo?.LimitApplicationCertificateClassificationFlag);
        }

        public string GetXmlDisplay(string currentKogakuKbnDisplay, bool isMyNumberProcess = false)
        {
            string limitCertificateFlag = _resultOfQualification.LimitApplicationCertificateRelatedInfo?.LimitApplicationCertificateClassificationFlag;
            if (string.IsNullOrEmpty(limitCertificateFlag))
            {
                string InsurerNumber = _resultOfQualification.InsurerNumber;
                if (!string.IsNullOrEmpty(InsurerNumber) && CIUtil.IsNumberic(InsurerNumber) && InsurerNumber.Length == 8 && InsurerNumber.StartsWith("39"))
                {
                    string insuredPartialContributionRatio = _resultOfQualification.InsuredPartialContributionRatio;
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
                string elderlyRecipientCertificateInfo = _resultOfQualification.ElderlyRecipientCertificateInfo?.ElderlyRecipientContributionRatio;
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
            string limitCertificateFlag = _resultOfQualification.LimitApplicationCertificateRelatedInfo?.LimitApplicationCertificateClassificationFlag;
            if (string.IsNullOrEmpty(limitCertificateFlag))
            {
                string InsurerNumber = _resultOfQualification.InsurerNumber;
                if (!string.IsNullOrEmpty(InsurerNumber) && CIUtil.IsNumberic(InsurerNumber) && InsurerNumber.Length == 8 && InsurerNumber.StartsWith("39"))
                {
                    string insuredPartialContributionRatio = _resultOfQualification.InsuredPartialContributionRatio;
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

                string elderlyRecipientCertificateInfo = _resultOfQualification.ElderlyRecipientCertificateInfo?.ElderlyRecipientContributionRatio;
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

        public bool IsReflect(int currentKogakuKbn, string currentKogakuKbnDisplay)
        {
            string limitCertificateFlag = _resultOfQualification.LimitApplicationCertificateRelatedInfo?.LimitApplicationCertificateClassificationFlag;
            if (string.IsNullOrEmpty(limitCertificateFlag))
            {
                string InsurerNumber = _resultOfQualification.InsurerNumber;
                if (!string.IsNullOrEmpty(InsurerNumber) && CIUtil.IsNumberic(InsurerNumber) && InsurerNumber.Length == 8 && InsurerNumber.StartsWith("39"))
                {
                    string insuredPartialContributionRatio = _resultOfQualification.InsuredPartialContributionRatio;
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
                string elderlyRecipientCertificateInfo = _resultOfQualification.ElderlyRecipientCertificateInfo?.ElderlyRecipientContributionRatio;
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
    }
}
