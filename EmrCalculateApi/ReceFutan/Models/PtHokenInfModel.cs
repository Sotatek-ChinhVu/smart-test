using Entity.Tenant;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class PtHokenInfModel
    {
        public PtHokenInf PtHokenInf { get; }
        public HokenMst HokenMst { get; }

        public PtHokenInfModel(PtHokenInf ptHokenInf, HokenMst hokenMst, int sinkei, int tenki)
        {
            PtHokenInf = ptHokenInf;
            HokenMst = hokenMst;
            Sinkei = sinkei;
            Tenki = tenki;
        }

        /// <summary>
        /// 患者保険情報
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        //public int HpId
        //{
        //    get { return PtHokenInf.HpId; }
        //}

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return PtHokenInf.PtId; }
        }

        /// <summary>
        /// 保険ID
        ///  患者別に保険情報を識別するための固有の番号
        /// </summary>
        public int HokenId
        {
            get { return PtHokenInf.HokenId; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        //public long SeqNo
        //{
        //    get { return PtHokenInf.SeqNo; }
        //}

        /// <summary>
        /// 保険番号
        ///  保険マスタに登録された保険番号
        /// </summary>
        public int HokenNo
        {
            get { return PtHokenInf.HokenNo; }
        }

        /// <summary>
        /// 保険番号枝番
        ///  保険マスタに登録された保険番号枝番
        /// </summary>
        public int HokenEdaNo
        {
            get { return PtHokenInf.HokenEdaNo; }
        }

        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get { return PtHokenInf.HokensyaNo ?? string.Empty; }
        }

        /// <summary>
        /// 記号
        /// </summary>
        public string Kigo
        {
            get { return PtHokenInf.Kigo ?? string.Empty; }
        }

        /// <summary>
        /// 番号
        /// </summary>
        public string Bango
        {
            get { return PtHokenInf.Bango ?? string.Empty; }
        }

        public string EdaNo
        {
            get => PtId == 0 ? string.Empty : (PtHokenInf.EdaNo ?? string.Empty);
        }

        /// <summary>
        /// 高額療養費処理区分
        ///  1:高額委任払い 
        ///  2:適用区分一般
        /// </summary>
        public int KogakuType
        {
            get { return PtHokenInf.KogakuType; }
        }

        /// <summary>
        /// 職務上区分
        ///  1:職務上
        ///  2:下船後３月以内 
        ///  3:通勤災害
        /// </summary>
        public int SyokumuKbn
        {
            get { return PtHokenInf.SyokumuKbn; }
        }

        /// <summary>
        /// 特記事項１
        /// </summary>
        public string Tokki1
        {
            get { return PtHokenInf.Tokki1 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項２
        /// </summary>
        public string Tokki2
        {
            get { return PtHokenInf.Tokki2 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項３
        /// </summary>
        public string Tokki3
        {
            get { return PtHokenInf.Tokki3 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項４
        /// </summary>
        public string Tokki4
        {
            get { return PtHokenInf.Tokki4 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項５
        /// </summary>
        public string Tokki5
        {
            get { return PtHokenInf.Tokki5 ?? string.Empty; }
        }

        /// <summary>
        /// 労災交付番号
        ///  短期給付: 労働保険番号
        ///  傷病年金: 年金証書番号
        ///  アフターケア: 健康管理手帳番号
        /// </summary>
        public string RousaiKofuNo
        {
            get { return PtHokenInf.RousaiKofuNo ?? string.Empty; }
        }

        /// <summary>
        /// 労災災害区分
        ///  1:業務中の災害 
        ///  2:通勤途上の災害
        /// </summary>
        public int RousaiSaigaiKbn
        {
            get { return PtHokenInf.RousaiSaigaiKbn; }
        }

        /// <summary>
        /// 労災事業所名
        /// </summary>
        public string RousaiJigyosyoName
        {
            get { return PtHokenInf.RousaiJigyosyoName ?? string.Empty; }
        }

        /// <summary>
        /// 労災都道府県名
        /// </summary>
        public string RousaiPrefName
        {
            get { return PtHokenInf.RousaiPrefName ?? string.Empty; }
        }

        /// <summary>
        /// 労災所在地郡市区名
        /// </summary>
        public string RousaiCityName
        {
            get { return PtHokenInf.RousaiCityName ?? string.Empty; }
        }

        /// <summary>
        /// 労災傷病年月日
        ///  yyyymmdd 
        /// </summary>
        public int RousaiSyobyoDate
        {
            get { return PtHokenInf.RousaiSyobyoDate; }
        }

        /// <summary>
        /// 労災傷病コード
        /// </summary>
        public string RousaiSyobyoCd
        {
            get { return PtHokenInf.RousaiSyobyoCd ?? string.Empty; }
        }

        /// <summary>
        /// 労災労働局コード
        /// </summary>
        public string RousaiRoudouCd
        {
            get { return PtHokenInf.RousaiRoudouCd ?? string.Empty; }
        }

        /// <summary>
        /// 労災監督署コード
        /// </summary>
        public string RousaiKantokuCd
        {
            get { return PtHokenInf.RousaiKantokuCd ?? string.Empty; }
        }

        /// <summary>
        /// 労災レセ請求回数
        /// </summary>
        public int RousaiReceCount
        {
            get { return PtHokenInf.RousaiReceCount; }
        }

        /// <summary>
        /// 自賠保険会社名
        /// </summary>
        public string JibaiHokenName
        {
            get { return PtHokenInf.JibaiHokenName ?? string.Empty; }
        }

        /// <summary>
        /// 自賠受傷日
        ///  yyyymmdd 
        /// </summary>
        public int JibaiJyusyouDate
        {
            get { return PtHokenInf.JibaiJyusyouDate; }
        }

        /// <summary>
        /// 点数単価
        ///  点数1点あたりの単価を円で表す
        /// </summary>
        public int EnTen
        {
            get { return HokenMst.EnTen; }
        }

        /// <summary>
        /// レセプト記載   
        ///  0:記載あり
        ///  1:上限未満記載なし
        ///  2:上限以下記載なし
        ///  3:記載なし
        /// </summary>
        public int ReceKisai
        {
            get { return HokenMst.ReceKisai; }
        }


        /// <summary>
        /// 特記事項の取得
        /// </summary>
        /// <param name="seqNo">連番</param>
        /// <returns></returns>
        public string GetTokki(int seqNo)
        {
            switch (seqNo)
            {
                case 1: return Tokki1;
                case 2: return Tokki2;
                case 3: return Tokki3;
                case 4: return Tokki4;
                case 5: return Tokki5;
                default: return  string.Empty;
            }
        }

        /// <summary>
        /// 労災新継再別
        /// </summary>
        public int Sinkei { get; private set; }

        /// <summary>
        /// 労災転帰事由
        /// </summary>
        public int Tenki { get; private set; }
    }

}
