using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MedicalExamination.GetHistory
{
    public class GroupOdrGHistoryItem
    {
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
        public int HokenPid { get; private set; }

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

        public bool IsDrug
        {
            get => (KouiCode >= 20 && KouiCode <= 23) || KouiCode == 28 || KouiCode == 100 || KouiCode == 101;
        }

        public bool IsKensa
        {
            get => (KouiCode >= 60 && KouiCode < 70);
        }

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

        public string SinkyuName { get; private set; }
        public List<OdrInfHistoryItem> OdrInfs { get; private set; }

        public GroupOdrGHistoryItem(int hokenPid, string sinkyuName, List<OdrInfHistoryItem> odrInfs)
        {
            HokenPid = hokenPid;
            SinkyuName = sinkyuName;
            OdrInfs = odrInfs;
        }
    }
}
