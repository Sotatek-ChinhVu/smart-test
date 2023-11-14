using Domain.Converter;
using Domain.Models.Online;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Domain.Models.HokenMst
{
    public class HokenConfirmationModel
    {
        private readonly OnlineKogakuHelper? OnlineKogakuHelper;
        public HokenConfirmationModel(string name, string value, string compareValue, int age = -1, int sinDate = 0, OnlineKogakuHelper? onlineKogakuHelper = null)
        {
            AttributeName = name;
            Age = age;
            SinDate = sinDate;
            OnlineKogakuHelper = onlineKogakuHelper;
            CurrentValue = value.AsString();

            if (AttributeName == HokenConfOnlQuaConst.KOGAKU_KBN && OnlineKogakuHelper != null)
            {
                XmlValue = OnlineKogakuHelper.GetXmlValue().AsString();
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
                IsReflect = CIUtil.ToHalfsize(CurrentValueDisplay.AsString()).Replace("/", "").Trim()
                            .Equals(CIUtil.ToHalfsize(XmlValueDisplay.AsString()).Replace("/", ""));
            }
            else if (AttributeName == HokenConfOnlQuaConst.KOGAKU_KBN && OnlineKogakuHelper != null)
            {
                IsReflect = OnlineKogakuHelper.IsReflect(CurrentValue.AsInteger(), CurrentValueDisplay);
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

        public string XmlValue { get; set; }

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
                        if (Age >= 0 && OnlineKogakuHelper != null)
                        {
                            returnValue = OnlineKogakuHelper.GetXmlDisplay(CurrentValueDisplay);
                        }
                        break;
                    default:
                        if (!string.IsNullOrEmpty(XmlValue))
                        {
                            returnValue = XmlValue;
                        }
                        break;
                }

                if (string.IsNullOrEmpty(returnValue) && AttributeName != HokenConfOnlQuaConst.KIGO
                        && AttributeName != HokenConfOnlQuaConst.CREDENTIAL)
                {
                    returnValue = "(情報なし)";
                }

                return returnValue;
            }
        }

        public bool CanCheck { get; private set; }

        public bool IsReflect { get; private set; }

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

        public int Age { get; set; }
    }
}
