namespace UseCase.Online.QualificationConfirmation
{
    public class KogakuKbnToLimitConsFlgConverter
    {
        public static string ConvertToClassification(int kogakuKbn, int age, string hokenSyaNo)
        {
            if (IsHoken39(hokenSyaNo) && kogakuKbn == 41)
            {
                return "01";
            }
            if (age >= 70)
            {
                switch (kogakuKbn)
                {
                    case 4:
                    case 5:
                        return "02";
                    case 26:
                    case 27:
                    case 28:
                    case 0:
                        return "01";
                }
            }
            else
            {
                switch (kogakuKbn)
                {
                    case 30:
                        return "02";
                    case 26:
                    case 27:
                    case 28:
                    case 29:
                        return "01";
                }
            }
            return string.Empty;
        }

        public static string ConvertToClassificationFlag(int kogakuKbn, int age, int sinDate, string hokenSyaNo)
        {
            if (IsHoken39(hokenSyaNo) && kogakuKbn == 41)
            {
                return "B09";
            }
            if (age >= 70)
            {
                switch (kogakuKbn)
                {
                    case 26:
                        return "B01";
                    case 27:
                        return "B02";
                    case 28:
                        return "B03";
                    case 0:
                        return sinDate > 20221001 ? "B10" : "B04";
                    case 4:
                        return "B05";
                    case 5:
                        return "B06";
                        //case 41:
                        //    return "B09";
                }
            }
            else
            {
                switch (kogakuKbn)
                {
                    case 26:
                        return "A01";
                    case 27:
                        return "A02";
                    case 28:
                        return "A03";
                    case 29:
                        return "A04";
                    case 30:
                        return "A05";
                }
            }
            return string.Empty;
        }

        public static int ClassificationFlagToKogakuKbn(string flag)
        {
            switch (flag)
            {
                // 26	現役Ⅲ
                case "26":
                    return 26;
                // 0
                case "0":
                    return 0;
                //41 一般Ⅱ
                case "41":
                    return 41;
            }

            switch (flag)
            {
                case "A01":
                case "B01":
                    return 26;
                case "A02":
                case "B02":
                    return 27;
                case "A03":
                case "B03":
                    return 28;
                case "A04":
                    return 29;
                case "A05":
                case "A06":
                    return 30;
                case "B04":
                case "B10":
                    return 0;
                case "B05":
                    return 4;
                case "B06":
                case "B07":
                case "B08":
                    return 5;
                case "B09":
                    return 41;
            }

            return 0;
        }

        public static int KogakuKbnDisplayToKogakuKbn(string flag)
        {
            switch (flag)
            {
                case "26 現役Ⅲ":
                    return 26;
                case "41 一般Ⅱ":
                    return 41;
            }
            return 0;
        }

        public static string KogakuKbnClassToDisplay(string kogakuKbnClass, int age)
        {
            if (kogakuKbnClass == "B09")
            {
                return "41 一般Ⅱ";
            }
            if (age >= 70)
            {
                switch (kogakuKbnClass)
                {
                    case "B01":
                        return "26 現役Ⅲ";
                    case "B02":
                        return "27 現役Ⅱ";
                    case "B03":
                        return "28 現役Ⅰ";
                    case "B05":
                        return "4 低所Ⅱ";
                    case "B06":
                    case "B07":
                    case "B08":
                        return "5 低所Ⅰ";
                    //case "B09":
                    //    return "41 一般Ⅱ";
                    case "B10":
                    case "B04":
                        return "0 一般";
                }
            }
            else
            {
                switch (kogakuKbnClass)
                {
                    case "A01":
                        return "26 区ア";
                    case "A02":
                        return "27 区イ";
                    case "A03":
                        return "28 区ウ";
                    case "A04":
                        return "29 区エ";
                    case "A05":
                    case "A06":
                        return "30 区オ";
                }
            }
            return string.Empty;
        }

        public static string KogakuKbnToDisplay(int kogakuKbn, int age)
        {
            if (kogakuKbn == 41)
            {
                return "41 一般Ⅱ";
            }
            if (age >= 70)
            {
                switch (kogakuKbn)
                {
                    case 26:
                        return "26 現役Ⅲ";
                    case 27:
                        return "27 現役Ⅱ";
                    case 28:
                        return "28 現役Ⅰ";
                    case 0:
                        return "0 一般";
                    case 4:
                        return "4 低所Ⅱ";
                    case 5:
                        return "5 低所Ⅰ";
                        //case 41:
                        //    return "41 一般Ⅱ";
                }
            }
            else
            {
                switch (kogakuKbn)
                {
                    case 26:
                        return "26 区ア";
                    case 27:
                        return "27 区イ";
                    case 28:
                        return "28 区ウ";
                    case 29:
                        return "29 区エ";
                    case 30:
                        return "30 区オ";
                }
            }
            return string.Empty;
        }

        public static bool IsHoken39(string hokenSyaNo)
        {
            if (string.IsNullOrEmpty(hokenSyaNo)) return false;
            return hokenSyaNo.Length == 8 && hokenSyaNo.StartsWith("39");
        }
    }
}
