using Helper.Common;
using System.Text.Json.Serialization;

namespace UseCase.MedicalExamination.GetHistory
{
    public class GroupOdrGHistoryItem
    {
        [JsonPropertyName("groupKouiCode")]
        public int GroupKouiCode
        {
            get
            {
                var firstTodayOdrInfModel = OdrInfs?.FirstOrDefault();
                if (firstTodayOdrInfModel != null)
                {
                    return firstTodayOdrInfModel.GroupOdrKouiKbn;
                }
                return 0;
            }
        }

        [JsonPropertyName("groupName")]
        public string GroupName
        {
            get
            {
                var firstTodayOdrInfModel = OdrInfs?.FirstOrDefault();
                if (firstTodayOdrInfModel != null)
                {
                    return OdrUtil.GetOdrGroupName(firstTodayOdrInfModel.OdrKouiKbn);
                }
                return "";
            }
        }

        [JsonPropertyName("hokenPid")]
        public int HokenPid { get; private set; }

        [JsonPropertyName("inOutKbn")]
        public int InOutKbn
        {
            get
            {
                var firstTodayOdrInfModel = OdrInfs?.FirstOrDefault();
                if (firstTodayOdrInfModel != null)
                {
                    return firstTodayOdrInfModel.InoutKbn;
                }
                return 0;
            }
        }

        [JsonPropertyName("inOutName")]
        public string InOutName
        {
            get
            {
                var firstTodayOdrInfModel = OdrInfs?.FirstOrDefault();
                if (firstTodayOdrInfModel != null)
                {
                    return OdrUtil.GetInOutName(firstTodayOdrInfModel.OdrKouiKbn, firstTodayOdrInfModel.InoutKbn);
                }
                return "";
            }
        }

        [JsonPropertyName("kouiCode")]
        public int KouiCode
        {
            get
            {
                var firstTodayOdrInfModel = OdrInfs?.FirstOrDefault();
                if (firstTodayOdrInfModel != null)
                {
                    return firstTodayOdrInfModel.OdrKouiKbn;
                }
                return 0;
            }
        }

        [JsonPropertyName("santeiKbn")]
        public int SanteiKbn
        {
            get
            {
                var firstTodayOdrInfModel = OdrInfs?.FirstOrDefault();
                if (firstTodayOdrInfModel != null)
                {
                    return firstTodayOdrInfModel.SanteiKbn;
                }
                return 0;
            }
        }

        [JsonPropertyName("santeiName")]
        public string SanteiName
        {
            get
            {
                if (SanteiKbn == 1)
                {
                    return "算定外";
                }
                else if (SanteiKbn == 2)
                {
                    return "自費算定";
                }
                return "";
            }
        }

        [JsonPropertyName("sikyuKbn")]
        public int SikyuKbn
        {
            get
            {
                var firstTodayOdrInfModel = OdrInfs?.FirstOrDefault();
                if (firstTodayOdrInfModel != null)
                {
                    return firstTodayOdrInfModel.SikyuKbn;
                }
                return 0;
            }
        }

        [JsonPropertyName("sikyuName")]
        public string SikyuName
        {
            get
            {
                if (IsDrug)
                {
                    return OdrUtil.GetSikyuName(SyohoSbt);
                }
                else if (IsKensa)
                {
                    return OdrUtil.GetSikyuKensa(SikyuKbn, TosekiKbn);
                }
                return "";
            }
        }

        [JsonPropertyName("isDrug")]
        public bool IsDrug
        {
            get => (KouiCode >= 20 && KouiCode <= 23) || KouiCode == 28 || KouiCode == 100 || KouiCode == 101;
        }

        [JsonPropertyName("isKensa")]
        public bool IsKensa
        {
            get => (KouiCode >= 60 && KouiCode < 70);
        }

        [JsonPropertyName("syohoSbt")]
        public int SyohoSbt
        {
            get
            {
                var firstTodayOdrInfModel = OdrInfs?.FirstOrDefault();
                if (firstTodayOdrInfModel != null)
                {
                    return firstTodayOdrInfModel.SyohoSbt;
                }
                return 0;
            }
        }

        [JsonPropertyName("tosekiKbn")]
        public int TosekiKbn
        {
            get
            {
                var firstTodayOdrInfModel = OdrInfs?.FirstOrDefault();
                if (firstTodayOdrInfModel != null)
                {
                    return firstTodayOdrInfModel.TosekiKbn;
                }
                return 0;
            }
        }

        [JsonPropertyName("sinkyuName")]
        public string SinkyuName { get; private set; }

        [JsonPropertyName("odrInfs")]
        public List<OdrInfHistoryItem> OdrInfs { get; private set; }

        public GroupOdrGHistoryItem(int hokenPid, string sinkyuName, List<OdrInfHistoryItem> odrInfs)
        {
            HokenPid = hokenPid;
            SinkyuName = sinkyuName;
            OdrInfs = odrInfs;
        }

        public GroupOdrGHistoryItem(List<OdrInfHistoryItem> odrInfs)
        {
            OdrInfs = odrInfs;
            SinkyuName = string.Empty;
        }
    }
}
