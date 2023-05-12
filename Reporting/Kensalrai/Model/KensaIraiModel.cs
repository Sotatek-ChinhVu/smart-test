using Entity.Tenant;
using Helper.Common;

namespace Reporting.Kensalrai.Model
{
    public class KensaIraiModel
    {
        public PtInf PtInf { get; } = null;
        public RaiinInf RaiinInf { get; } = null;

        public KensaIraiModel
            (long iraiCd, int tosekiKbn, int sikyuKbn,
            PtInf ptInf, RaiinInf raiinInf, List<KensaIraiDetailModel> details)
        {
            IraiCd = iraiCd;
            TosekiKbn = tosekiKbn;
            SikyuKbn = sikyuKbn;
            PtInf = ptInf;
            RaiinInf = raiinInf;

            Details = details;
            if (Details != null)
            {
                Details = Details.FindAll(p => p.IsSelected == true);
            }
        }

        public int SinDate
        {
            get { return RaiinInf.SinDate; }
        }
        public long RaiinNo
        {
            get { return RaiinInf.RaiinNo; }
        }
        public long IraiCd { get; set; } = 0;
        public long PtId
        {
            get { return PtInf.PtId; }
        }
        public long PtNum
        {
            get { return PtInf.PtNum; }
        }
        public string Name
        {
            get { return PtInf.Name; }
        }
        public string KanaName
        {
            get { return PtInf.KanaName; }
        }
        public int Sex
        {
            get { return PtInf.Sex; }
        }
        public string GetSexStr(string men, string female)
        {
            string ret = "";
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

        public int Birthday
        {
            get { return PtInf.Birthday; }
        }
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
                string ret = "";

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
                string ret = "";

                if (SikyuKbn == 1)
                {
                    ret = "急";
                }

                return ret;
            }
        }
        public string KaName { get; set; } = "";
        public string KaCodeName { get; set; } = "";
        public int KaId
        {
            get { return RaiinInf.KaId; }
        }
        public string TantoName { get; set; } = "";
        public string TantoKanaName { get; set; } = "";
        public string DrName { get; set; } = "";

        public List<KensaIraiDetailModel> Details
        {
            get ;
            set ;
        }

        public int DetailCount
        {
            get
            {
                int ret = 0;
                if (Details != null)
                {
                    ret = Details.Count();
                }
                return ret;
            }
        }

        public string UpdateTime { get; set; } = "";
    }
}
