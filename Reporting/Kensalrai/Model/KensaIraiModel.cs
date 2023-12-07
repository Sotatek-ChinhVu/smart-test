using Entity.Tenant;
using Helper.Common;

namespace Reporting.Kensalrai.Model
{
    public class KensaIraiModel
    {
        public PtInf PtInf { get; }
        public RaiinInf RaiinInf { get; }

        public KensaIraiModel
            (long iraiCd, int tosekiKbn, int sikyuKbn,
            PtInf ptInf, RaiinInf raiinInf, List<KensaIraiDetailModel> details)
        {
            PtInf = ptInf;
            RaiinInf = raiinInf;
            SinDate = raiinInf.SinDate;
            RaiinNo = raiinInf.RaiinNo;
            IraiCd = iraiCd;
            PtId = ptInf.PtId;
            PtNum = ptInf.PtNum;
            Name = ptInf.Name ?? string.Empty;
            KanaName = ptInf.KanaName ?? string.Empty;
            Sex = ptInf.Sex;
            Birthday = ptInf.Birthday;
            KaId = raiinInf.KaId;
            TosekiKbn = tosekiKbn;
            SikyuKbn = sikyuKbn;

            Details = details;
            if (Details != null)
            {
                Details = Details.FindAll(p => p.IsSelected);
            }
        }

        public KensaIraiModel(int sinDate, long raiinNo, long iraiCd, long ptId, long ptNum, string name, string kanaName, int sex, int birthday, int tosekiKbn, int sikyuKbn, int kaId, double weight, double height, string tantoName, string tantoKanaName, string kaSName, string updateTime, List<KensaIraiDetailModel> details)
        {
            SinDate = sinDate;
            RaiinNo = raiinNo;
            IraiCd = iraiCd;
            PtId = ptId;
            PtNum = ptNum;
            Name = name;
            KanaName = kanaName;
            Sex = sex;
            Birthday = birthday;
            KaId = kaId;
            TosekiKbn = tosekiKbn;
            SikyuKbn = sikyuKbn;
            Weight = weight;
            Height = height;
            Details = details;
            TantoName = tantoName;
            TantoKanaName = tantoKanaName;
            KaSName = kaSName;
            UpdateTime = updateTime;
            if (Details != null)
            {
                Details = Details.FindAll(p => p.IsSelected);
            }
        }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public long IraiCd { get; set; } = 0;

        public long PtId { get; private set; }

        public long PtNum { get; private set; }

        public string Name { get; private set; }

        public string KanaName { get; private set; }

        public int Sex { get; private set; }

        public string KaSName { get; private set; }

        public string GetSexStr(string men, string female)
        {
            string ret = string.Empty;
            switch (Sex)
            {
                case 1:
                    ret = men;
                    break;
                case 2:
                    ret = female;
                    break;
            }

            return ret;
        }

        public int Birthday { get; private set; }
        /// <summary>
        /// 年齢
        /// </summary>
        public int Age
        {
            get { return CIUtil.SDateToAge(Birthday, SinDate); }
        }
        public double Height { get; set; } = 0;
        public double Weight { get; set; } = 0;
        public int TosekiKbn { get; set; } = 0;
        public string TosekiStr
        {
            get
            {
                string ret = string.Empty;

                switch (TosekiKbn)
                {
                    case 1:
                        ret = "前";
                        break;
                    case 2:
                        ret = "後";
                        break;
                }

                return ret;
            }
        }
        public int SikyuKbn { get; set; } = 0;
        public string SikyuStr
        {
            get
            {
                string ret = string.Empty;

                if (SikyuKbn == 1)
                {
                    ret = "急";
                }

                return ret;
            }
        }
        public string KaName { get; set; } = string.Empty;
        public string KaCodeName { get; set; } = string.Empty;

        public int KaId { get; private set; }

        public string TantoName { get; set; } = string.Empty;
        public string TantoKanaName { get; set; } = string.Empty;
        public string DrName { get; set; } = string.Empty;

        public List<KensaIraiDetailModel> Details { get; set; }

        public int DetailCount
        {
            get
            {
                int ret = 0;
                if (Details != null)
                {
                    ret = Details.Count;
                }
                return ret;
            }
        }

        public string UpdateTime { get; set; } = string.Empty;
    }
}
