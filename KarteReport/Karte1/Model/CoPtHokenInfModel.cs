using Entity.Tenant;

namespace KarteReport.Karte1.Model
{
    public class CoPtHokenInfModel
    {
        public PtHokenInf PtHokenInf { get; } = null;
        public PtKohi PtKohi1 { get; } = null;
        public PtKohi PtKohi2 { get; } = null;
        public PtKohi PtKohi3 { get; } = null;
        public PtKohi PtKohi4 { get; } = null;


        public HokenMst HokenMst { get; } = null;
        public HokenMst KohiMst1 { get; } = null;
        public HokenMst KohiMst2 { get; } = null;
        public HokenMst KohiMst3 { get; } = null;
        public HokenMst KohiMst4 { get; } = null;
        public List<CoPtKohiModel> PtKohis { get; private set; } = null;
        public CoPtHokenInfModel(PtHokenInf ptHokenInf, HokenMst hokenMst,
            PtKohi ptKohi1, HokenMst kohiMst1,
            PtKohi ptKohi2, HokenMst kohiMst2,
            PtKohi ptKohi3, HokenMst kohiMst3,
            PtKohi ptKohi4, HokenMst kohiMst4)
        {
            void _addPtKohi(PtKohi appendPtKohi, HokenMst appendHokenMst)
            {
                if (appendPtKohi != null)
                {
                    if (PtKohis == null)
                    {
                        PtKohis = new List<CoPtKohiModel>();
                    }
                    PtKohis.Add(new CoPtKohiModel(appendPtKohi, appendHokenMst));
                }
            }

            PtHokenInf = ptHokenInf;
            HokenMst = hokenMst;

            if (kohiMst1 != null)
            {
                if (kohiMst1.HokenSbtKbn == 2)
                {
                    // マル長の場合、ずらしておく
                    PtKohi1 = ptKohi2;
                    KohiMst1 = kohiMst2;

                    PtKohi2 = ptKohi3;
                    KohiMst2 = kohiMst3;

                    PtKohi3 = ptKohi4;
                    KohiMst3 = kohiMst4;

                }
                else
                {
                    PtKohi1 = ptKohi1;
                    KohiMst1 = kohiMst1;

                    PtKohi2 = ptKohi2;
                    KohiMst2 = kohiMst2;

                    PtKohi3 = ptKohi3;
                    KohiMst3 = kohiMst3;

                    PtKohi4 = ptKohi4;
                    KohiMst4 = kohiMst4;
                }

                _addPtKohi(PtKohi1, kohiMst1);
                _addPtKohi(PtKohi2, kohiMst2);
                _addPtKohi(PtKohi3, kohiMst3);
                _addPtKohi(PtKohi4, kohiMst4);
            }

        }

        /// <summary>
        /// 患者保険情報
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return PtHokenInf.HpId; }
        }

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
        public long SeqNo
        {
            get { return PtHokenInf.SeqNo; }
        }

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
            get { return PtHokenInf.HokensyaNo; }
        }

        /// <summary>
        /// 記号
        /// </summary>
        public string Kigo
        {
            get { return PtHokenInf.Kigo; }
        }

        /// <summary>
        /// 番号
        /// </summary>
        public string Bango
        {
            get { return PtHokenInf.Bango; }
        }
        /// <summary>
        /// 枝番
        /// </summary>
        public string EdaNo
        {
            get
            {
                string ret = "";
                if (string.IsNullOrEmpty(PtHokenInf.EdaNo) == false)
                {
                    ret = PtHokenInf.EdaNo.PadLeft(2, '0');
                }
                return ret;
            }
        }
        /// <summary>
        /// 記号番号
        /// 労災の場合、交付番号
        /// 自賠の場合、保険会社名
        /// </summary>
        public string KigoBango
        {
            get
            {
                string ret = "";

                if (new int[] { 11, 12, 13 }.Contains(HokenKbn))
                {
                    // 労災
                    ret = RousaiKofuNo;
                }
                else if (HokenKbn == 14)
                {
                    // 自賠
                    ret = JibaiHokenName;
                }
                else
                {
                    ret = Kigo + "・" + Bango;
                    if (string.IsNullOrEmpty(EdaNo) == false)
                    {
                        ret = ret + "(" + EdaNo + ")";
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 本人家族区分
        ///  1:本人
        ///  2:家族
        /// </summary>
        public int HonkeKbn
        {
            get { return PtHokenInf.HonkeKbn; }
        }
        public string Honke
        {
            get
            {
                string ret = "本人";

                if (HonkeKbn == 2)
                {
                    ret = "家族";
                }

                return ret;
            }
        }
        /// <summary>
        /// 保険区分
        ///  0:自費 
        ///  1:社保 
        ///  2:国保
        ///  
        ///  11:労災(短期給付)
        ///  12:労災(傷病年金)
        ///  13:アフターケア
        ///  14:自賠責
        /// </summary>
        public int HokenKbn
        {
            get { return PtHokenInf.HokenKbn; }
        }

        /// <summary>
        /// 保険者番号の桁数
        ///  0桁: [0]自費保険
        ///  4桁: [01]社保
        ///  6桁: [100]国保
        ///  8桁: 保険者番号の前2桁
        ///   [67]退職国保
        ///   [39]後期高齢
        ///   [01]協会健保、[02]船員、[03,04]日雇、
        ///   [06]組合健保、[07]自衛官、
        ///   [31..34]共済組合
        ///    [31]国家公務員共済組合
        ///    [32]地方公務員等共済組合
        ///    [33]警察共済組合
        ///    [34]公立学校共済組合
        ///　    日本私立学校振興・共済事業団
        ///   [63,72..75]特定共済組合
        /// </summary>
        public string Houbetu
        {
            get { return PtHokenInf.Houbetu; }
        }

        /// <summary>
        /// 被保険者名
        /// </summary>
        public string HokensyaName
        {
            get { return PtHokenInf.HokensyaName; }
        }

        /// <summary>
        /// 被保険者郵便番号
        /// </summary>
        public string HokensyaPost
        {
            get { return PtHokenInf.HokensyaPost; }
        }

        /// <summary>
        /// 被保険者住所
        /// </summary>
        public string HokensyaAddress
        {
            get { return PtHokenInf.HokensyaAddress; }
        }

        /// <summary>
        /// 被保険者電話番号
        /// </summary>
        public string HokensyaTel
        {
            get { return PtHokenInf.HokensyaTel; }
        }

        /// <summary>
        /// 継続区分
        ///  1:任意継続
        /// </summary>
        public int KeizokuKbn
        {
            get { return PtHokenInf.KeizokuKbn; }
        }

        /// <summary>
        /// 資格取得日
        ///  yyyymmdd 
        /// </summary>
        public int SikakuDate
        {
            get { return PtHokenInf.SikakuDate; }
        }

        /// <summary>
        /// 交付日
        ///  yyyymmdd 
        /// </summary>
        public int KofuDate
        {
            get { return PtHokenInf.KofuDate; }
        }

        /// <summary>
        /// 適用開始日
        ///  yyyymmdd 
        /// </summary>
        public int StartDate
        {
            get { return PtHokenInf.StartDate; }
        }

        /// <summary>
        /// 適用終了日
        ///  yyyymmdd 
        /// </summary>
        public int EndDate
        {
            get { return PtHokenInf.EndDate; }
        }
        /// <summary>
        /// 保険有効期限
        /// </summary>
        public int HokenEndDate
        {
            get
            {
                int ret = 0;

                if (new int[] { 11, 12, 13, 14 }.Contains(HokenKbn))
                {
                    // 労災・自賠
                    ret = RyoyoEndDate;
                }
                else
                {
                    ret = EndDate;
                }

                return ret;
            }
        }
        /// <summary>
        /// 負担率
        /// </summary>
        public int Rate
        {
            get { return HokenMst == null ? PtHokenInf.Rate : HokenMst.FutanRate; }
        }

        /// <summary>
        /// 一部負担限度額
        ///  ※未使用
        /// </summary>
        public int Gendogaku
        {
            get { return PtHokenInf.Gendogaku; }
        }

        /// <summary>
        /// 高額療養費区分
        ///  70歳以上
        ///   0:一般
        ///   3:上位(～2018/07)
        ///   4:低所Ⅱ
        ///   5:低所Ⅰ
        ///   6:特定収入(～2008/12)
        ///   26:現役Ⅲ
        ///   27:現役Ⅱ
        ///   28:現役Ⅰ
        ///  70歳未満
        ///   0:限度額認定証なし
        ///   17:上位[A] (～2014/12)
        ///   18:一般[B] (～2014/12)
        ///   19:低所[C] (～2014/12)
        ///   26:区ア／標準報酬月額83万円以上
        ///   27:区イ／標準報酬月額53..79万円
        ///   28:区ウ／標準報酬月額28..50万円
        ///   29:区エ／標準報酬月額26万円以下
        ///   30:区オ／低所得者
        /// </summary>
        public int KogakuKbn
        {
            get { return PtHokenInf.KogakuKbn; }
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
        /// 限度額特例対象年月１
        ///  yyyymm
        /// </summary>
        public int TokureiYm1
        {
            get { return PtHokenInf.TokureiYm1; }
        }

        /// <summary>
        /// 限度額特例対象年月２
        ///  yyyymm
        /// </summary>
        public int TokureiYm2
        {
            get { return PtHokenInf.TokureiYm2; }
        }

        /// <summary>
        /// 多数回該当適用開始年月
        ///  yyyymm
        /// </summary>
        public int TasukaiYm
        {
            get { return PtHokenInf.TasukaiYm; }
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
        /// 国保減免区分
        ///  1:減額 
        ///  2:免除 
        ///  3:支払猶予
        /// </summary>
        public int GenmenKbn
        {
            get { return PtHokenInf.GenmenKbn; }
        }

        /// <summary>
        /// 国保減免割合
        ///  ※不要？
        /// </summary>
        public int GenmenRate
        {
            get { return PtHokenInf.GenmenRate; }
        }

        /// <summary>
        /// 国保減免金額
        ///  ※不要？
        /// </summary>
        public int GenmenGaku
        {
            get { return PtHokenInf.GenmenGaku; }
        }

        /// <summary>
        /// 特記事項１
        /// </summary>
        public string Tokki1
        {
            get { return PtHokenInf.Tokki1; }
        }

        /// <summary>
        /// 特記事項２
        /// </summary>
        public string Tokki2
        {
            get { return PtHokenInf.Tokki2; }
        }

        /// <summary>
        /// 特記事項３
        /// </summary>
        public string Tokki3
        {
            get { return PtHokenInf.Tokki3; }
        }

        /// <summary>
        /// 特記事項４
        /// </summary>
        public string Tokki4
        {
            get { return PtHokenInf.Tokki4; }
        }

        /// <summary>
        /// 特記事項５
        /// </summary>
        public string Tokki5
        {
            get { return PtHokenInf.Tokki5; }
        }

        /// <summary>
        /// 労災交付番号
        ///  短期給付: 労働保険番号
        ///  傷病年金: 年金証書番号
        ///  アフターケア: 健康管理手帳番号
        /// </summary>
        public string RousaiKofuNo
        {
            get { return PtHokenInf.RousaiKofuNo; }
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
            get { return PtHokenInf.RousaiJigyosyoName; }
        }

        /// <summary>
        /// 労災都道府県名
        /// </summary>
        public string RousaiPrefName
        {
            get { return PtHokenInf.RousaiPrefName; }
        }

        /// <summary>
        /// 労災所在地郡市区名
        /// </summary>
        public string RousaiCityName
        {
            get { return PtHokenInf.RousaiCityName; }
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
            get { return PtHokenInf.RousaiSyobyoCd; }
        }

        /// <summary>
        /// 労災労働局コード
        /// </summary>
        public string RousaiRoudouCd
        {
            get { return PtHokenInf.RousaiRoudouCd; }
        }

        /// <summary>
        /// 労災監督署コード
        /// </summary>
        public string RousaiKantokuCd
        {
            get { return PtHokenInf.RousaiKantokuCd; }
        }

        /// <summary>
        /// 労災レセ請求回数
        /// </summary>
        public int RousaiReceCount
        {
            get { return PtHokenInf.RousaiReceCount; }
        }

        /// <summary>
        /// 労災療養開始日
        /// 
        /// </summary>
        public int RyoyoStartDate
        {
            get { return PtHokenInf.RyoyoStartDate; }
        }

        /// <summary>
        /// 労災療養終了日
        /// 
        /// </summary>
        public int RyoyoEndDate
        {
            get { return PtHokenInf.RyoyoEndDate; }
        }

        /// <summary>
        /// 自賠保険会社名
        /// </summary>
        public string JibaiHokenName
        {
            get { return PtHokenInf.JibaiHokenName; }
        }

        /// <summary>
        /// 自賠保険担当者
        /// </summary>
        public string JibaiHokenTanto
        {
            get { return PtHokenInf.JibaiHokenTanto; }
        }

        /// <summary>
        /// 自賠保険連絡先
        /// </summary>
        public string JibaiHokenTel
        {
            get { return PtHokenInf.JibaiHokenTel; }
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
        /// 削除区分
        ///  1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return PtHokenInf.IsDeleted; }
        }
        /// <summary>
        /// 保険種別区分
        ///		0:保険なし 
        ///		1:主保険   
        ///		2:マル長   
        ///		3:労災  
        ///		4:自賠
        ///		5:生活保護 
        ///		6:分点公費
        ///		7:一般公費  
        ///		8:自費
        /// </summary>
        public int HokenSbtKbn
        {
            get => HokenMst == null ? 0 : HokenMst.HokenSbtKbn;
        }
        /// <summary>
        /// 保険種別
        /// </summary>
        public string HokenSbt
        {
            get
            {
                string ret = "";

                if (HokenSbtKbn == 8)
                {
                    // 自費レセ
                    ret = "自レ";
                }
                else if (new int[] { 11, 12, 13 }.Contains(PtHokenInf.HokenKbn))
                {
                    // 労災
                    ret = "労災";
                }
                else if (new int[] { 14 }.Contains(PtHokenInf.HokenKbn))
                {
                    // 自賠
                    ret = "自賠責";
                }
                else if (new int[] { 1, 2 }.Contains(PtHokenInf.HokenKbn))
                {
                    if (PtHokenInf.HokenKbn == 1)
                    {
                        // 社保
                        if (HokenSbtKbn == 0)
                        {
                            ret = "公費";
                        }
                        else
                        {
                            ret = "社保" + Honke;
                        }
                    }
                    else if (PtHokenInf.HokenKbn == 2)
                    {
                        // 国保
                        if (Houbetu == "39")
                        {
                            ret = "後期本人";
                        }
                        else if (Houbetu == "67")
                        {
                            ret = "退職" + Honke;
                        }
                        else
                        {
                            ret = "国保" + Honke;
                        }
                    }
                }
                else if (PtHokenInf.HokenKbn == 0)
                {
                    ret = "自費";
                }

                return ret;
            }
        }

        /// <summary>
        /// 公費ID
        /// </summary>
        /// <param name="index">取得したい公費のID</param>
        /// <returns></returns>
        public int KohiId(int index)
        {
            int ret = 0;

            switch (index)
            {
                case 1:
                    ret = PtKohi1?.HokenId ?? 0;
                    break;
                case 2:
                    ret = PtKohi2?.HokenId ?? 0;
                    break;
                case 3:
                    ret = PtKohi3?.HokenId ?? 0;
                    break;
                case 4:
                    ret = PtKohi4?.HokenId ?? 0;
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 公費負担者番号
        /// </summary>
        /// <param name="index">取得したい公費のID</param>
        /// <returns></returns>
        public string KohiFutansyaNo(int index)
        {
            string ret = "";

            switch (index)
            {
                case 1:
                    ret = PtKohi1?.FutansyaNo ?? "";
                    break;
                case 2:
                    ret = PtKohi2?.FutansyaNo ?? "";
                    break;
                case 3:
                    ret = PtKohi3?.FutansyaNo ?? "";
                    break;
                case 4:
                    ret = PtKohi4?.FutansyaNo ?? "";
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 公費受給者番号
        /// </summary>
        /// <param name="index">取得したい公費のID</param>
        /// <returns></returns>
        public string KohiJyukyusyaNo(int index)
        {
            string ret = "";

            switch (index)
            {
                case 1:
                    ret = PtKohi1?.JyukyusyaNo ?? "";
                    break;
                case 2:
                    ret = PtKohi2?.JyukyusyaNo ?? "";
                    break;
                case 3:
                    ret = PtKohi3?.JyukyusyaNo ?? "";
                    break;
                case 4:
                    ret = PtKohi4?.JyukyusyaNo ?? "";
                    break;
            }

            return ret;
        }
        /// <summary>
        /// 公費有効期限
        /// </summary>
        /// <param name="index">取得したい公費のID</param>
        /// <returns></returns>
        public int KohiStartDate(int index)
        {
            int ret = 0;

            switch (index)
            {
                case 1:
                    ret = PtKohi1?.StartDate ?? 0;
                    break;
                case 2:
                    ret = PtKohi2?.StartDate ?? 0;
                    break;
                case 3:
                    ret = PtKohi3?.StartDate ?? 0;
                    break;
                case 4:
                    ret = PtKohi4?.StartDate ?? 0;
                    break;
            }

            return ret;
        }
        /// <summary>
        /// 公費有効期限
        /// </summary>
        /// <param name="index">取得したい公費のID</param>
        /// <returns></returns>
        public int KohiEndDate(int index)
        {
            int ret = 0;

            switch (index)
            {
                case 1:
                    ret = PtKohi1?.EndDate ?? 0;
                    break;
                case 2:
                    ret = PtKohi2?.EndDate ?? 0;
                    break;
                case 3:
                    ret = PtKohi3?.EndDate ?? 0;
                    break;
                case 4:
                    ret = PtKohi4?.EndDate ?? 0;
                    break;
            }

            return ret;
        }
        /// <summary>
        /// 公費資格取得日
        /// </summary>
        /// <param name="index">取得したい公費のID</param>
        /// <returns></returns>
        public int KohiSikakuDate(int index)
        {
            int ret = 0;

            switch (index)
            {
                case 1:
                    ret = PtKohi1?.SikakuDate ?? 0;
                    break;
                case 2:
                    ret = PtKohi2?.SikakuDate ?? 0;
                    break;
                case 3:
                    ret = PtKohi3?.SikakuDate ?? 0;
                    break;
                case 4:
                    ret = PtKohi4?.SikakuDate ?? 0;
                    break;
            }

            return ret;
        }
        /// <summary>
        /// 公費交付日
        /// </summary>
        /// <param name="index">取得したい公費のID</param>
        /// <returns></returns>
        public int KohiKofuDate(int index)
        {
            int ret = 0;

            switch (index)
            {
                case 1:
                    ret = PtKohi1?.KofuDate ?? 0;
                    break;
                case 2:
                    ret = PtKohi2?.KofuDate ?? 0;
                    break;
                case 3:
                    ret = PtKohi3?.KofuDate ?? 0;
                    break;
                case 4:
                    ret = PtKohi4?.KofuDate ?? 0;
                    break;
            }

            return ret;
        }
    }
}
