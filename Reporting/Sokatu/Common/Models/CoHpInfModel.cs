using Entity.Tenant;
using Helper.Common;

namespace Reporting.Sokatu.Common.Models
{
    public class CoHpInfModel
    {
        public HpInf HpInf { get; private set; }

        public CoHpInfModel(HpInf hpInf)
        {
            HpInf = hpInf;
        }

        public string HpCd
        {
            get => HpInf == null ? "" : HpInf.HpCd.PadLeft(7, '0');
        }

        public string ReceHpCd
        {
            get
            {
                string wrkCd = HpInf == null ? "" : HpInf.HpCd.PadLeft(7, '0');

                if (new int[] { 2, 12, 13, 17, 18, 25, 32, 35, 41, 43, 44, 46 }.Contains(HpInf.PrefNo))
                {
                    //xx,xxxx,x タイプ
                    return string.Format("{0},{1},{2}", wrkCd.Substring(0, 2), wrkCd.Substring(2, 4), wrkCd.Substring(6, 1));
                }
                else if (new int[] { 3, 22, 34 }.Contains(HpInf.PrefNo))
                {
                    //フォーマットなし
                    return wrkCd;
                }
                else if (new int[] { 27 }.Contains(HpInf.PrefNo))
                {
                    //xx-x,xxx,x タイプ
                    return string.Format("{0}-{1},{2},{3}", wrkCd.Substring(0, 2), wrkCd.Substring(2, 1), wrkCd.Substring(3, 3), wrkCd.Substring(6, 1));
                }
                else
                {
                    //xxx,xxx,x タイプ
                    return string.Format("{0},{1},{2}", wrkCd.Substring(0, 3), wrkCd.Substring(3, 3), wrkCd.Substring(6, 1));
                }
            }
        }

        public string RousaiHpCd
        {
            get => HpInf.RousaiHpCd == null ? string.Empty : HpInf.RousaiHpCd.PadLeft(7, '0');
        }

        public string ReceHpName
        {
            get => HpInf.ReceHpName;
        }

        public string KaisetuName
        {
            get => HpInf.KaisetuName;
        }

        public int PrefNo
        {
            get => HpInf.PrefNo;
        }

        public string PostCd
        {
            get
            {
                string ret = HpInf.PostCd ?? "";

                ret = ret.Replace("-", "");

                return ret;
            }
        }
        public string PostCdDsp
        {
            get { return CIUtil.GetDspPostCd(PostCd); }
        }

        public string Address1
        {
            get => HpInf.Address1;
        }

        public string Address2
        {
            get => HpInf.Address2;
        }

        public string Tel
        {
            get => HpInf.Tel;
        }
    }
}
