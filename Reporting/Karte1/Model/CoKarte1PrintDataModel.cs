using Domain.Constant;

namespace Reporting.Karte1.Model
{
    class CoKarte1PrintDataModel
    {
        public string Byomei { get; set; }
        public int StartDate { get; set; }
        public int TenkiKbn { get; set; }
        public int TenkiDate { get; set; }

        public string Tenki
        {
            get
            {
                string ret = "";

                switch (TenkiKbn)
                {
                    case TenkiKbnConst.Cured:
                        ret = "治ゆ";
                        break;
                    case TenkiKbnConst.Dead:
                        ret = "死亡";
                        break;
                    case TenkiKbnConst.Canceled:
                        ret = "中止";
                        break;
                    case TenkiKbnConst.Other:
                        ret = "その他";
                        break;
                }

                return ret;
            }
        }
    }
}
