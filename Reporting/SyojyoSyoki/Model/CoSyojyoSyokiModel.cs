using Entity.Tenant;

namespace Reporting.SyojyoSyoki.Model
{
    public class CoSyojyoSyokiModel
    {
        public HpInf HpInf { get; private set; }
        public List<CoSyoukiInfModel> SyoukiInfs { get; private set; }
        public ReceInf ReceInf { get; private set; }
        public PtInf PtInf { get; private set; }
        public PtHokenInf PtHokenInf { get; private set; }
        public PtKohi PtKohi1 { get; private set; }
        public PtKohi PtKohi2 { get; private set; }
        public PtKohi PtKohi3 { get; private set; }
        public PtKohi PtKohi4 { get; private set; }

        public CoSyojyoSyokiModel(HpInf hpInf, List<CoSyoukiInfModel> syoukiInfs, ReceInf receInf, PtInf ptInf, PtHokenInf ptHokenInf, PtKohi ptKohi1, PtKohi ptKohi2, PtKohi ptKohi3, PtKohi ptKohi4)
        {
            HpInf = hpInf;
            SyoukiInfs = syoukiInfs;
            ReceInf = receInf;
            PtInf = ptInf;
            PtHokenInf = ptHokenInf;
            PtKohi1 = ptKohi1;
            PtKohi2 = ptKohi2;
            PtKohi3 = ptKohi3;
            PtKohi4 = ptKohi4;
        }

        public CoSyojyoSyokiModel()
        {
            HpInf = new();
            SyoukiInfs = new();
            ReceInf = new();
            PtInf = new();
            PtHokenInf = new();
            PtKohi1 = new();
            PtKohi2 = new();
            PtKohi3 = new();
            PtKohi4 = new();
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        public long PtNum
        {
            get => PtInf == null ? 0 : PtInf.PtNum;
        }
        public long PtId
        {
            get => PtInf == null ? 0 : PtInf.PtId;
        }
        public string PtName
        {
            get => PtInf == null ? string.Empty : PtInf.Name ?? string.Empty;
        }
        public int Birthday
        {
            get => PtInf == null ? 0 : PtInf.Birthday;
        }
        /// <summary>
        /// 診療年月
        /// </summary>
        public int SinYm
        {
            get => ReceInf == null ? 0 : ReceInf.SinYm;
        }
        /// <summary>
        /// 県番号
        /// </summary>
        public int PrefNo
        {
            get => HpInf == null ? 0 : HpInf.PrefNo;
        }
        /// <summary>
        /// 医療機関コード
        /// </summary>
        public string HpCd
        {
            get => HpInf == null ? string.Empty : HpInf.HpCd ?? string.Empty;
        }
        /// <summary>
        /// 医療機関名称
        /// </summary>
        public string HpName
        {
            get => HpInf == null ? string.Empty : HpInf.HpName ?? string.Empty;
        }
        /// <summary>
        /// レセ種別
        /// </summary>
        public string ReceiptSbt
        {
            get => ReceInf == null ? string.Empty : ReceInf.ReceSbt ?? string.Empty;
        }
        /// <summary>
        /// 保険区分
        /// </summary>
        public int HokenKbn
        {
            get => ReceInf == null ? 0 : ReceInf.HokenKbn;
        }
        /// <summary>
        /// 保険ID
        /// </summary>
        public int HokenId
        {
            get => ReceInf == null ? 0 : ReceInf.HokenId;
        }

        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get => PtHokenInf == null ? string.Empty : PtHokenInf.HokensyaNo ?? string.Empty;
        }
        /// <summary>
        /// 記号
        /// </summary>
        public string Kigo
        {
            get => PtHokenInf == null ? string.Empty : PtHokenInf.Kigo ?? string.Empty;
        }
        /// <summary>
        /// 番号
        /// </summary>
        public string Bango
        {
            get => PtHokenInf == null ? string.Empty : PtHokenInf.Bango ?? string.Empty;
        }
        /// <summary>
        /// 枝番
        /// </summary>
        public string EdaNo
        {
            get => PtHokenInf == null ? string.Empty : PtHokenInf.EdaNo ?? string.Empty;
        }
        /// <summary>
        /// 負担者番号
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string KohiFutansyaNo(int index)
        {
            string ret = string.Empty;

            switch (index)
            {
                case 1:
                    ret = PtKohi1.FutansyaNo ?? string.Empty;
                    break;
                case 2:
                    ret = PtKohi2.FutansyaNo ?? string.Empty;
                    break;
                case 3:
                    ret = PtKohi3.FutansyaNo ?? string.Empty;
                    break;
                case 4:
                    ret = PtKohi4.FutansyaNo ?? string.Empty;
                    break;
            }
            return ret;
        }
        /// <summary>
        /// 受給者番号
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string KohiJyukyusyaNo(int index)
        {
            string ret = string.Empty;

            switch (index)
            {
                case 1:
                    ret = PtKohi1.JyukyusyaNo ?? string.Empty;
                    break;
                case 2:
                    ret = PtKohi2.JyukyusyaNo ?? string.Empty;
                    break;
                case 3:
                    ret = PtKohi3.JyukyusyaNo ?? string.Empty;
                    break;
                case 4:
                    ret = PtKohi4.JyukyusyaNo ?? string.Empty;
                    break;
            }
            return ret;
        }

        public int KohiReceKisai(int index)
        {
            int ret = 0;

            if (ReceInf != null)
            {
                switch (index)
                {
                    case 1:
                        ret = ReceInf.Kohi1ReceKisai;
                        break;
                    case 2:
                        ret = ReceInf.Kohi2ReceKisai;
                        break;
                    case 3:
                        ret = ReceInf.Kohi3ReceKisai;
                        break;
                    case 4:
                        ret = ReceInf.Kohi4ReceKisai;
                        break;
                }
            }

            return ret;
        }
    }
}
