using Helper.Constants;
using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Constant;
using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Ika.Models
{
    public class OdrDtlTenModel
    {
        //public OdrInfDetail OdrInfDetail { get; } = null;
        public OdrInfDetailDataModel OdrInfDetail { get; } = null;
        //public TenMst TenMst { get; } = null;
        private TenMstModel _tenMst;
        private CmtKbnMst _cmtKbnMst;
        //public PtHokenPattern PtHokenPattern { get; } = null;
        //public OdrInf OdrInf { get; } = null;
        //public IpnKasanMst IpnKasanMst { get; } = null;
        //public RaiinInf RaiinInf { get; } = null;

        string _itemCd = "";
        string _odrItemCd = "";
        double _suryo = 0;
        string _receName = "";
        long _rpNo = 0;
        long _rpEdaNo = 0;
        int _rowNo = 0;
        string _santeiItemCd = "";
        string _tyuCd;

        int _hokenKbn;
        int _hokenPid;
        int _hokenId;
        int _hokenSbt;

        int _odrKouiKbn;
        int _santeiKbn;
        int _inoutKbn;
        int _syohoSbt;
        int _daysCnt;
        int _sortNo;

        int _kasan1;
        int _kasan2;

        double _minYakka;
        //string _kbn;
        //int _junSenpatu;

        string _sinStartTime;

        public OdrDtlTenModel(OdrInfDetail odrInfDetail, TenMstModel tenMst, CmtKbnMst cmtKbn,
            string receName,
            //PtHokenPattern ptHokenPattern,
            int hokenKbn,
            int hokenPid,
            int hokenId,
            int hokenSbt,
            //OdrInf odrInf, 
            int odrKouiKbn, int santeiKbn, int inoutKbn, int syohoSbt, int daysCnt, int sortNo,
            //IpnKasanMst ipnKasanMst, 
            int kasan1, int kasan2,
            //RaiinInf raiinInf
            string sinStartTime,
            double minYakka
            //string kbn,
            //int junSenpatu
            )

        {
            OdrInfDetail = new OdrInfDetailDataModel(odrInfDetail);
            SetArgData(tenMst, cmtKbn, receName, hokenKbn, hokenPid, hokenId, hokenSbt, odrKouiKbn, santeiKbn, inoutKbn, syohoSbt, daysCnt, sortNo, kasan1, kasan2, sinStartTime, minYakka);
        }

        public OdrDtlTenModel(OdrInfDetailDataModel odrInfDetail, TenMstModel tenMst, CmtKbnMst cmtKbn,
            string receName,
            //PtHokenPattern ptHokenPattern,
            int hokenKbn,
            int hokenPid,
            int hokenId,
            int hokenSbt,
            //OdrInf odrInf, 
            int odrKouiKbn, int santeiKbn, int inoutKbn, int syohoSbt, int daysCnt, int sortNo,
            //IpnKasanMst ipnKasanMst, 
            int kasan1, int kasan2,
            //RaiinInf raiinInf
            string sinStartTime,
            double minYakka
            //string kbn,
            //int junSenpatu
            )

        {
            OdrInfDetail = odrInfDetail;
            IsDummy = odrInfDetail.IsDummy;
            SetArgData(tenMst, cmtKbn, receName, hokenKbn, hokenPid, hokenId, hokenSbt, odrKouiKbn, santeiKbn, inoutKbn, syohoSbt, daysCnt, sortNo, kasan1, kasan2, sinStartTime, minYakka);
        }

        private void SetArgData(TenMstModel tenMst,
            CmtKbnMst cmtKbn,
            string receName,
            //PtHokenPattern ptHokenPattern,
            int hokenKbn,
            int hokenPid,
            int hokenId,
            int hokenSbt,
            //OdrInf odrInf, 
            int odrKouiKbn, int santeiKbn, int inoutKbn, int syohoSbt, int daysCnt, int sortNo,
            //IpnKasanMst ipnKasanMst, 
            int kasan1, int kasan2,
            //RaiinInf raiinInf
            string sinStartTime,
            double minYakka
            //string kbn,
            //int junSenpatu
            )
        {
            //TenMst = tenMst;
            _tenMst = null;
            if (tenMst != null)
            {
                _tenMst = tenMst;
            }
            _cmtKbnMst = cmtKbn;
            //PtHokenPattern = ptHokenPattern;
            _hokenKbn = hokenKbn;
            _hokenPid = hokenPid;
            _hokenId = hokenId;
            _hokenSbt = hokenSbt;
            //OdrInf = odrInf;
            _odrKouiKbn = odrKouiKbn;
            _santeiKbn = santeiKbn;
            _inoutKbn = inoutKbn;
            _syohoSbt = syohoSbt;
            _daysCnt = daysCnt;
            _sortNo = sortNo;

            _kasan1 = kasan1;
            _kasan2 = kasan2;

            _minYakka = minYakka;
            //_kbn = kbn;
            //_junSenpatu = junSenpatu;

            _sinStartTime = sinStartTime;

            //IpnKasanMst = ipnKasanMst;
            //RaiinInf = raiinInf;

            _rpNo = OdrInfDetail.RpNo;
            _rpEdaNo = OdrInfDetail.RpEdaNo;
            _rowNo = OdrInfDetail.RowNo;

            // ITEM_CD
            if (_tenMst != null && _tenMst.ItemCd != null && _tenMst.ItemCd != "")
            {
                _itemCd = _tenMst.ItemCd?.Trim();
            }
            else
            {
                _itemCd = OdrInfDetail.ItemCd?.Trim();
            }

            // ODR_ITEM_CD
            _odrItemCd = OdrInfDetail.ItemCd?.Trim();

            // SURYO
            _suryo = OdrInfDetail.Suryo;

            // RECE_NAME
            if (receName != "")
            {
                _receName = receName;
            }
            else
            {
                _receName = _tenMst?.ReceName ?? string.Empty;
                if (_receName == "")
                {
                    _receName = OdrInfDetail.ItemName;
                }
            }

            // SANTEI_ITEM_CD
            _santeiItemCd = _tenMst?.SanteiItemCd ?? string.Empty;

            _tyuCd = _tenMst?.TyuCd ?? string.Empty;
        }

        public TenMstModel TenMst
        {
            get { return _tenMst; }
            set { _tenMst = value; }
        }

        public CmtKbnMst CmtKbnMst
        {
            get { return _cmtKbnMst; }
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return OdrInfDetail.HpId; }
        }

        /// <summary>
        /// 患者ID
        ///       患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return OdrInfDetail.PtId; }
        }

        /// <summary>
        /// 診療日
        ///       yyyyMMdd
        /// </summary>
        public int SinDate
        {
            get { return OdrInfDetail.SinDate; }
        }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get { return OdrInfDetail.RaiinNo; }
        }

        /// <summary>
        /// 剤番号
        ///     ODR_INF.RP_NO
        /// </summary>
        public long RpNo
        {
            get { return _rpNo; }
            set { _rpNo = value; }
        }

        /// <summary>
        /// 剤枝番
        ///     ODR_INF.RP_EDA_NO
        /// </summary>
        public long RpEdaNo
        {
            get { return _rpEdaNo; }
            set { _rpEdaNo = value; }
        }

        /// <summary>
        /// 行番号
        /// </summary>
        public int RowNo
        {
            get { return _rowNo; }
            set { _rowNo = value; }
        }

        /// <summary>
        /// 行サブ番号
        /// </summary>
        public int RowSubNo { get; set; } = 0;

        /// <summary>
        /// 項目コード
        /// 算定用項目コードに変換したもの
        /// </summary>
        public string ItemCd
        {
            get { return _itemCd ?? ""; }
            set { _itemCd = value; }
        }

        /// <summary>
        /// オーダー時の項目コード
        /// </summary>
        public string OdrItemCd
        {
            get { return _odrItemCd ?? _itemCd ?? string.Empty; }
            set { _odrItemCd = value; }
        }

        /// <summary>
        /// 項目名称
        /// </summary>
        public string ItemName
        {
            get { return OdrInfDetail.ItemName ?? ""; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public double Suryo
        {
            get { return _suryo; }
            set { _suryo = value; }
        }

        /// <summary>
        /// 単位名称
        /// </summary>
        public string UnitName
        {
            get { return OdrInfDetail.UnitName ?? ""; }
        }

        /// <summary>
        /// 単位種別
        ///         0: 1,2以外
        ///         1: TEN_MST.単位
        ///         2: TEN_MST.数量換算単位
        /// </summary>
        public int UnitSBT
        {
            get { return OdrInfDetail.UnitSBT; }
        }

        /// <summary>
        /// 単位換算値
        ///          UNIT_SBT=0 -> TEN_MST.ODR_TERM_VAL
        ///          UNIT_SBT=0 -> TEN_MST.ODR_TERM_VAL
        /// </summary>
        public double TermVal
        {
            get { return OdrInfDetail.TermVal; }
        }

        /// <summary>
        /// 後発医薬品区分
        ///         当該医薬品が後発医薬品に該当するか否かを表す。
        ///             0: 後発医薬品のない先発医薬品
        ///             1: 先発医薬品がある後発医薬品である
        ///             2: 後発医薬品がある先発医薬品である
        ///             7: 先発医薬品のない後発医薬品である
        /// </summary>
        public int KohatuKbn
        {
            get { return OdrInfDetail.KohatuKbn; }
        }

        /// <summary>
        /// 処方せん記載区分
        ///             0: 指示なし（後発品のない先発品）
        ///             1: 変更不可
        ///             2: 後発品（他銘柄）への変更可 
        ///             3: 一般名処方
        /// </summary>
        public int SyohoKbn
        {
            get { return OdrInfDetail.SyohoKbn; }
        }

        /// <summary>
        /// 処方せん記載制限区分
        ///             0: 制限なし
        ///             1: 剤形不可
        ///             2: 含量規格不可
        ///             3: 含量規格・剤形不可
        /// </summary>
        public int SyohoLimitKbn
        {
            get { return OdrInfDetail.SyohoLimitKbn; }
        }

        /// <summary>
        /// レセ非表示区分
        ///          0: 表示
        ///          1: 非表示
        /// </summary>
        public int IsNodspRece
        {
            //get { return OdrInfDetail.IsNodspRece; }
            get
            {
                int ret = OdrInfDetail.IsNodspRece;

                if (OdrKouiKbn >= OdrKouiKbnConst.TouyakuMin &&
                    OdrKouiKbn <= OdrKouiKbnConst.TouyakuMax &&
                    InoutKbn == 1 &&
                    OdrInfDetail.ItemCd.StartsWith("8") && OdrInfDetail.ItemCd.Length == 9)
                {
                    // 院外処方コメントコードの場合は強制表示
                    ret = 0;
                }

                return ret;
            }
        }

        /// <summary>
        /// 一般名コード
        /// </summary>
        public string IpnCd
        {
            get { return OdrInfDetail.IpnCd ?? ""; }
        }

        /// <summary>
        /// 一般名
        /// </summary>
        public string IpnName
        {
            get { return OdrInfDetail.IpnName ?? ""; }
        }

        /// <summary>
        /// 分割調剤
        ///        7日単位の3分割の場合 "7+7+7"
        /// </summary>
        public string Bunkatu
        {
            get { return OdrInfDetail.Bunkatu ?? ""; }
        }

        /// <summary>
        /// コメントマスターの名称
        ///        ※当該項目がコメント項目の場合に使用
        /// </summary>
        public string CmtName
        {
            get { return OdrInfDetail.CmtName ?? ""; }
        }

        /// <summary>
        /// コメント文
        ///        コメントマスターの定型文に組み合わせる文字情報
        ///        ※当該項目がコメント項目の場合に使用
        /// </summary>
        public string CmtOpt
        {
            get { return OdrInfDetail.CmtOpt ?? ""; }
        }

        /// <summary>
        /// 検査項目コード
        ///     KENSA_MST.KENSA_ITEM_CD
        /// </summary>
        public string KensaItemCd
        {
            get { return TenMst?.KensaItemCd ?? ""; }
        }

        //-----------------------------------------------------
        //  ↓TenMst
        //-----------------------------------------------------

        /// <summary>
        /// 有効開始年月日
        ///     yyyymmdd
        /// </summary>
        public int StartDate
        {
            get { return TenMst?.StartDate ?? 0; }
        }

        /// <summary>
        /// 有効終了年月日
        ///     yyyymmdd
        /// </summary>
        public int EndDate
        {
            get { return TenMst?.EndDate ?? 99999999; }
        }

        /// <summary>
        /// マスター種別
        ///     S: 診療行為
        ///     Y: 医薬品
        ///     T: 特材
        ///     C: コメント
        ///     R: 労災
        ///     U: 労災特定器材
        ///     D: 労災コメントマスタ
        ///     
        /// ※Z特材で算定項目コード='7%'の場合、Tを返す
        /// 
        /// </summary>
        public string MasterSbt
        {
            get {
                string ret = TenMst?.MasterSbt ?? string.Empty;

                if(string.IsNullOrEmpty(ItemCd) == false && 
                    ItemCd.StartsWith("Z") == true && 
                    string.IsNullOrEmpty(SanteiItemCd) == false && 
                    SanteiItemCd.StartsWith("7") &&
                    string.IsNullOrEmpty(Z_MasterSbt) == false)
                {
                    ret = Z_MasterSbt;
                }
                return ret; 
            }
        }

        /// <summary>
        /// 診療行為区分
        /// </summary>
        public int SinKouiKbn
        {
            get { return TenMst?.SinKouiKbn ?? 0; }
        }

        /// <summary>
        /// 漢字名称
        /// </summary>
        public string Name
        {
            get { return TenMst?.Name ?? string.Empty; }
        }

        /// <summary>
        /// 請求用名称
        ///     計算後、請求用の名称
        ///     TEN_MSTから取得できない or NULLの場合、ODR_INF_DETAIL.ITEM_NAMEを返す
        /// </summary>
        public string ReceName
        {
            get { return _receName ?? ""; }
            set { _receName = value; }
        }

        /// <summary>
        /// 点数識別
        ///     1: 金額（整数部7桁、小数部2桁）
        ///     2: 都道府県購入価格
        ///     3: 点数（プラス）
        ///     4: 都道府県購入価格（点数）、金額（整数部のみ）
        ///     5: %加算
        ///     6: %減算
        ///     7: 減点診療行為
        ///     8: 点数（マイナス）
        ///     9: 乗算割合
        ///     10: 除算金額（金額を１０で除す。） ※ベントナイト用
        /// </summary>
        public int TenId
        {
            get {
                int ret = TenMst?.TenId ?? 0;

                if (string.IsNullOrEmpty(ItemCd) == false &&
                    ItemCd.StartsWith("Z") == true &&
                    string.IsNullOrEmpty(SanteiItemCd) == false &&
                    SanteiItemCd.StartsWith("7") &&
                    Z_TenId > 0)
                {
                    ret = Z_TenId;
                }
                return ret;
            }
        }

        /// <summary>
        /// 点数
        /// </summary>
        public double Ten
        {
            get { return TenMst?.Ten ?? 0; }
        }

        /// <summary>
        /// レセ単位コード
        /// </summary>
        public string ReceUnitCd
        {
            get { return TenMst?.ReceUnitCd ?? string.Empty; }
        }

        /// <summary>
        /// レセ単位名称
        /// </summary>
        public string ReceUnitName
        {
            get { return TenMst?.ReceUnitName ?? string.Empty; }
        }

        /// <summary>
        /// オーダー単位名称
        ///     オーダー時に使用する単位
        /// </summary>
        public string OdrUnitName
        {
            get { return TenMst?.OdrUnitName ?? string.Empty; }
        }

        /// <summary>
        /// 数量換算単位名称
        ///     薬剤情報提供書の全数量に換算した値を表示する場合の当該医薬品の換算単位名称を表す。
        /// </summary>
        public string CnvUnitName
        {
            get { return TenMst?.CnvUnitName ?? string.Empty; }
        }

        /// <summary>
        /// オーダー単位換算値
        ///     薬剤情報提供書の全数量に換算した値を表示する場合、当該医薬品の保険請求上の単位からオーダー単位へ換算するための値を表す。
        public double OdrTermVal
        {
            get { return TenMst?.OdrTermVal ?? 0; }
        }

        /// <summary>
        /// 数量換算単位換算値
        ///     薬剤情報提供書の全数量に換算した値を表示する場合、当該医薬品の保険請求上の単位から数量換算単位へ換算するための値を表す。
        /// </summary>
        public double CnvTermVal
        {
            get { return TenMst?.CnvTermVal ?? 0; }
        }

        ///// <summary>
        ///// 後期高齢者医療適用区分
        /////     0: 社保・後期高齢者ともに適用される診療行為
        /////     1: 社保のみに適用される診療行為
        /////     2: 後期高齢者のみに適用される診療行為
        ///// </summary>
        //public int KoukiKbn
        //{
        //    get { return TenMst?.KoukiKbn ?? 0; }
        //}

        /// <summary>
        /// 包括対象検査
        ///     0: 1～12以外の診療行為 
        ///     1: 血液化学検査の包括項目 
        ///     2: 内分泌学的検査の包括項目 
        ///     3: 肝炎ウイルス関連検査の包括項目 
        ///     5: 腫瘍マーカーの包括項目 
        ///     6: 出血・凝固検査の包括項目 
        ///     7: 自己抗体検査の包括項目 
        ///     8: 内分泌負荷試験の包括項目 
        ///     9: 感染症免疫学的検査のうち、ウイルス抗体価（定性・半定量・定量） 
        ///     10: 感染症免疫学的検査のうち、グロブリンクラス別ウイルス抗体価 
        ///     11:血漿蛋白免疫学的検査のうち、特異的ＩｇＥ半定量・定量及びアレルゲン刺激性遊離ヒスタミン（ＨＲＴ） 
        ///     12: 悪性腫瘍遺伝子検査の包括項目 2020/04～廃止
        ///     13: 悪性腫瘍遺伝子検査（処理が容易なもの） 2020/04～追加
        ///     14: 悪性腫瘍遺伝子検査（処理が複雑なもの） 2020/04～追加
        ///     15: 悪性腫瘍遺伝子検査（血液・血漿）（ＲＯＳ１融合遺伝子検査、ＡＬＫ融合遺伝子検査、ＥＧＦＲ遺伝子検査（血漿））2022/04～追加
        ///     16: 悪性腫瘍遺伝子検査（血液・血漿）（ＭＥＴｅｘ14遺伝子検査、ＮＴＲＫ融合遺伝子検査）2022/04～追加
        /// </summary>
        public int HokatuKensa
        {
            get { return TenMst?.HokatuKensa ?? 0; }
        }

        ///// <summary>
        ///// 傷病名関連区分
        /////     0: 3～9以外の診療行為 
        /////     3: 皮膚科特定疾患指導管理料（Ⅰ） 
        /////     4: 皮膚科特定疾患指導管理料（Ⅱ） 
        /////     5: 特定疾患療養管理料、特定疾患処方管理加算１（処方料）、特定疾患処方管理加算１（処方箋料）、特定疾患処方管理加算２（処方料）、特定疾患処方管理加算２（処方箋料） 
        /////     7: てんかん指導料 
        /////     9: 難病外来指導管理料
        ///// </summary>
        //public int ByomeiKbn
        //{
        //    get { return TenMst?.ByomeiKbn ?? 0; }
        //}

        ///// <summary>
        ///// 医学管理料
        /////     2以上の医学管理等を行った場合に、主たる医学管理等の所定点数を算定する背反関係がある診療行為に限り、コードを設定する。
        ///// </summary>
        //public int Igakukanri
        //{
        //    get { return TenMst?.Igakukanri ?? 0; }
        //}

        /// <summary>
        /// 実日数カウント
        ///     0: 実日数に含めない
        ///     1: 実日数に含める
        /// </summary>
        public int JitudayCount
        {
            get { return TenMst?.JitudayCount ?? 0; }
        }

        ///// <summary>
        ///// 実日数
        /////     0: 1～4以外の診療行為 
        /////     1: 算定回数が診療実日数以下の診療行為 
        /////     2: 初診料、再診料、外来診療料等 
        /////     3: 入院基本料、特定入院料 
        /////     4: 外泊
        ///// </summary>
        //public int Jituday
        //{
        //    get { return TenMst?.Jituday ?? 0; }
        //}

        ///// <summary>
        ///// 日数・回数
        /////     実日数=0, 日数回数=0 - 算定回数と実日数の確認を要しない
        /////     実日数=1, 日数回数=0 - 算定回数が実日数以下である確認を要する
        /////     実日数 = 2, 日数回数 = 1 - 初診料
        /////     実日数=2, 日数回数=2 - 再診料、外来診療料自体、又は再診料、外来診療料が含まれる診療行為
        /////     実日数 = 3, 日数回数 = 3 - 入院基本料、特定入院料
        /////     実日数 = 4, 日数回数 = 0 - 外泊
        ///// </summary>
        //public int DayCount
        //{
        //    get { return TenMst?.DayCount ?? 0; }
        //}

        /// <summary>
        /// 医薬品関連区分
        ///     医薬品の種類を算定要件とする診療行為であるか否かを表す。
        ///     0: 1～4以外の診療行為 
        ///     1: 麻薬加算、毒薬加算、覚醒剤加算、向精神薬加算、麻薬注射加算 
        ///     3: 神経ブロック（神経破壊剤使用） 
        ///     4: 生物学的製剤加算
        /// </summary>
        public int DrugKanrenKbn
        {
            get { return TenMst?.DrugKanrenKbn ?? 0; }
        }

        /// <summary>
        /// きざみ値計算識別
        ///     0: きざみ値により算定しない診療行為（項番１２「新又は現点数」により算定する。） 
        ///     1: きざみ値により算定する診療行為
        /// </summary>
        public int KizamiId
        {
            get { return TenMst?.KizamiId ?? 0; }
        }

        /// <summary>
        /// きざみ下限値
        ///     きざみ値により算定する診療行為において「数量データ」の下限値を表す。
        ///     下限値の制限がない場合は「0」である。
        /// </summary>
        public int KizamiMin
        {
            get { return TenMst?.KizamiMin ?? 0; }
        }

        /// <summary>
        /// きざみ上限値
        ///     きざみ値により算定する診療行為において「数量データ」の上限値を表す。
        ///     上限値の制限がない場合は「99999999」である。
        /// </summary>
        public int KizamiMax
        {
            get { return TenMst?.KizamiMax ?? 0; }
        }

        /// <summary>
        /// きざみ値
        ///     きざみ値により算定する診療行為において点数のきざみ単位を表す。
        /// </summary>
        public int KizamiVal
        {
            get { return TenMst?.KizamiVal ?? 0; }
        }

        /// <summary>
        /// きざみ点数
        ///     きざみ値により算定する診療行為においてきざみ点数を表す。
        /// </summary>
        public double KizamiTen
        {
            get { return TenMst?.KizamiTen ?? 0; }
        }

        /// <summary>
        /// きざみ値上下限エラー処理
        ///     当該診療行為に係る「数量データ」が「下限値－きざみ値」以下又は「上限値」を超えた場合の対処方法を表す。
        ///     上下限エラー処理は「0」～「3」の4つの値を持ち、「下限値－きざみ値」以下の場合の条件、
        ///     及び「上限値」を超えた場合の条件を両方共に満たす値を設定する。
        /// </summary>
        public int KizamiErr
        {
            get { return TenMst?.KizamiErr ?? 0; }
        }

        /// <summary>
        /// 上限回数
        ///     0: 上限未設定
        /// </summary>
        public int MaxCount
        {
            get { return TenMst?.MaxCount ?? 0; }
        }

        ///// <summary>
        ///// 上限回数エラー処理
        /////     当該診療行為の算定可能回数が上限回数を超えた場合の処理方法を表す。
        /////     0: 上限回数を確認する。
        /////     1: 上限回数にて算定する。
        ///// </summary>
        //public int MaxCountErr
        //{
        //    get { return TenMst?.MaxCountErr ?? 0; }
        //}

        /// <summary>
        /// 注加算コード
        ///     注加算が算定可能な診療行為（基本項目、合成項目及び準用項目）と注加算を関連付ける任意の同一番号を設定する。
        ///    「告示等識別区分（１）」に「７：加算項目」を設定している診療行為のうち、
        ///     注加算コードを設定せずに専用の項目を設定して算定可否を判定する診療行為は「別紙７－８」のとおりである。 
        /// </summary>
        public string TyuCd
        {
            //get { return TenMst?.TyuCd ?? string.Empty; }
            get { return _tyuCd; }
            set { _tyuCd = value; }
        }

        /// <summary>
        /// 注加算通番
        ///     １つの診療行為に対して同時に算定が可能な注加算に、異なる番号を設定する。 
        ///     注加算が算定可能な診療行為（基本項目、合成項目及び準用項目）に「0」を、
        ///     注加算である診療行為に「1」から「9」及び「A」から「Z」（昇順、アルファベット順）を設定する。 
        ///     注加算コードと注加算通番の関連は「別紙７－９」のとおりである。 
        /// </summary>
        public string TyuSeq
        {
            get { return TenMst?.TyuSeq ?? string.Empty; }
        }

        /// <summary>
        /// 通則年齢
        ///     当該診療行為が年齢の通則加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 通則年齢加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 通則年齢加算自体
        /// </summary>
        public int TusokuAge
        {
            get { return TenMst?.TusokuAge ?? 0; }
        }

        /// <summary>
        /// 上下限年齢下限年齢
        ///     当該診療行為が算定可能な年齢の下限値を表す。
        ///     算定可能な年齢 ≧ 下限年齢
        ///     下限年齢に制限のない場合は「00」である。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA：生後２８日
        ///     B3：３歳に達した日の翌月の１日
        ///     B6：６歳に達した日の翌月の１日
        ///     BF：１５歳に達した日の翌月の１日
        ///     BK：２０歳に達した日の翌月の１日
        ///     MG：未就学
        /// </summary>
        public string MinAge
        {
            get { return TenMst?.MinAge ?? string.Empty; }
        }

        /// <summary>
        /// 上下限年齢上限年齢
        ///     当該診療行為が算定可能な年齢の「上限値＋１」を表す。
        ///     算定可能な年齢 ＜ 上限年齢
        ///     上限年齢に制限のない場合は「00」である。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA：生後２８日
        ///     B3：３歳に達した日の翌月の１日
        ///     B6：６歳に達した日の翌月の１日
        ///     BF：１５歳に達した日の翌月の１日
        ///     BK：２０歳に達した日の翌月の１日
        ///     MG：未就学
        /// </summary>
        public string MaxAge
        {
            get { return TenMst?.MaxAge ?? string.Empty; }
        }

        /// <summary>
        /// 時間加算区分
        ///     当該診療行為が時間加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1, 3以外の診療行為
        ///     　1: 時間加算が算定可能な診療行為（含む合成項目）
        ///     　3: 初診料の休日加算に係る診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 下記以外の診療行為
        ///     　1: 時間外加算自体
        ///     　2: 休日加算自体
        ///     　3: 初診料の休日加算自体
        ///     　4: 深夜加算自体
        ///     　5: 時間外特例加算自体
        ///     　6: 夜間・早朝加算自体
        ///     　7: 夜間加算自体
        ///     　8: 時間外、深夜、時間外特例加算（手術又は、1000 点以上の処置）（注加算又は通則加算）自体
        ///     　9: 休日加算（手術又は、1000 点以上の処置）（注加算又は通則加算）自体
        /// </summary>
        public int TimeKasanKbn
        {
            get { return TenMst?.TimeKasanKbn ?? 0; }
        }

        ///// <summary>
        ///// 基準不適合逓減区分
        /////     当該診療行為が施設基準不適合の場合点数を逓減して算定できる診療行為であるか否かを表す。
        /////     　0: 点数逓減して算定できる診療行為以外
        /////     　1: 逓減コード自体
        /////     　2: 点数逓減して算定できる診療行為
        /////     （削）3: 年齢が１歳未満のとき、点数逓減して算定できる診療行為
        ///// </summary>
        //public int FutekiKbn
        //{
        //    get { return TenMst?.FutekiKbn ?? 0; }
        //}

        ///// <summary>
        ///// 基準不適合逓減対象施設区分
        /////     当該診療行為が施設基準不適合の場合点数を逓減して算定できる診療行為について設定した施設基準コードを表す。
        /////     基準不適合逓減対象施設区分（施設基準コード）については「別紙５」を参照。
        ///// </summary>
        //public int FutekiSisetuKbn
        //{
        //    get { return TenMst?.FutekiSisetuKbn ?? 0; }
        //}

        /// <summary>
        /// 処置乳幼児加算区分
        ///     当該診療行為が処置乳幼児加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １～５以外の診療行為
        ///     　1: ３歳未満乳幼児加算（処置）（１１０点）が算定できる診療行為
        ///     　2: ３歳未満乳幼児加算（処置）（５５点）が算定できる診療行為
        ///     　3: ６歳未満乳幼児加算（処置）（１１０点）が算定できる診療行為
        ///     　4: ６歳未満乳幼児加算（処置）（８３点）が算定できる診療行為
        ///     　5: ６歳未満乳幼児加算（処置）（５５点）が算定できる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １～５以外の診療行為
        ///     　1: ３歳未満乳幼児加算（処置）（１１０点）自体
        ///     　2: ３歳未満乳幼児加算（処置）（５５点）自体
        ///     　3: ６歳未満乳幼児加算（処置）（１１０点）自体
        ///     　4: ６歳未満乳幼児加算（処置）（８３点）自体
        ///     　5: ６歳未満乳幼児加算（処置）（５５点）自体
        /// </summary>
        public int SyotiNyuyojiKbn
        {
            get { return TenMst?.SyotiNyuyojiKbn ?? 0; }
        }

        /// <summary>
        /// 極低出生体重児加算区分
        ///     当該診療行為が極低出生体重児加算（手術）（４００％）又は新生児加算（手術）（３００％）が算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 極低出生体重児加算（手術）（４００％）、新生児加算（手術）（３００％）が算定できる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 極低出生体重児加算（手術）（４００％）、新生児加算（手術）（３００％）自体
        /// </summary>
        public int LowWeightKbn
        {
            get { return TenMst?.LowWeightKbn ?? 0; }
        }

        /// <summary>
        /// 検査等実施判断区分
        ///     当該診療行為が検査等の実施料又は判断料に関するものであるか否かを表す。
        ///     　0: 1, 2以外の診療行為
        ///     　1: 検体検査実施料、生体検査実施料、核医学撮影料、コンピューター断層撮影料、病理標本作製料に係る診療行為
        ///     　2: 検体検査判断料、生体検査判断料、核医学診断料、コンピューター断層診断料、病理診断料、病理判断料に係る診療行為
        /// </summary>
        public int HandanKbn
        {
            get { return TenMst?.HandanKbn ?? 0; }
        }

        /// <summary>
        /// 検査等実施判断グループ区分
        ///     当該診療行為が検査等の場合、判断料・診断料又は判断料･診断料を算定できるグループ区分を表す。
        ///     　0: 1～42以外の診療行為
        ///     (検体検査)
        ///     　　1: 尿・糞便等検査　2: 血液学的検査　3: 生化学的検査（Ⅰ）　4: 生化学的検査（Ⅱ）　5: 免疫学的検査
        ///     (細菌検査)
        ///     　　6: 微生物学的検査
        ///     (検体検査)
        ///     　　8: 基本的検体検査
        ///     (生理検査)
        ///     　　11: 呼吸機能検査　13: 脳波検査　14: 神経・筋検査　15: ラジオアイソトープ検査 16: 眼科学的検査
        ///     (その他検査)
        ///     　　31: 核医学診断（Ｅ１０１－２～Ｅ１０１－５）
        ///     　　32: 核医学診断（それ以外） 33: コンピューター断層診断
        ///     (病理検査)
        ///     　　40: 病理診断 ※
        ///     　　41: 病理診断（組織診断）
        ///     　　42: 病理診断（細胞診断）
        ///     　　※40: 病理診断は41: 病理診断（組織診断）、42: 病理診断（細胞診断）を含む。
        /// </summary>
        public int HandanGrpKbn
        {
            get { return TenMst?.HandanGrpKbn ?? 0; }
        }

        /// <summary>
        /// 逓減対象区分
        ///     当該診療行為が算定回数による逓減計算の対象となるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 逓減計算の対象となる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 逓減コード自体
        /// </summary>
        public int TeigenKbn
        {
            get { return TenMst?.TeigenKbn ?? 0; }
        }

        ///// <summary>
        ///// 脊髄誘発電位測定等加算区分
        /////     当該診療行為が脊髄誘発電位測定等加算を算定できるものであるか否かを表す。
        /////     ＜基本項目、合成項目、準用項目＞
        /////     　0: 1, 2以外の診療行為
        /////     　1: 脊髄誘発電位測定等加算（１ 脳、脊椎、脊髄又は大動脈瘤の手術に用いた場合）が算定可能な診療行為
        /////     　2: 脊髄誘発電位測定等加算（２ 甲状腺又は副甲状腺の手術に用いた場合）が算定可能な診療行為
        /////     ＜加算項目、通則加算項目＞
        /////     　0: 1, 2以外の診療行為
        /////     　1: 脊髄誘発電位測定等加算（１ 脳、脊椎、脊髄又は大動脈瘤の手術に用いた場合）自体
        /////     　2: 脊髄誘発電位測定等加算（２ 甲状腺又は副甲状腺の手術に用いた場合）自体
        ///// </summary>
        //public int SekituiKbn
        //{
        //    get { return TenMst?.SekituiKbn ?? 0; }
        //}

        ///// <summary>
        ///// 頸部郭清術併施加算区分
        /////     当該診療行為が頸部郭清術併施加算を算定できるものであるか否かを表す。
        /////     ＜基本項目、合成項目、準用項目＞
        /////     　0: 1以外の診療行為
        /////     　1: 頸部郭清術併施加算が算定可能な診療行為
        /////     ＜加算項目、通則加算項目＞
        /////     　0: 頸部郭清術併施加算が算定できない診療行為
        /////     　1: 頸部郭清術併施加算自体
        ///// </summary>
        //public int KeibuKbn
        //{
        //    get { return TenMst?.KeibuKbn ?? 0; }
        //}

        ///// <summary>
        ///// 自動縫合器加算区分
        /////     当該診療行為が自動縫合器加算を算定できるものであるか否かを表す。
        /////     ＜基本項目、合成項目、準用項目＞
        /////     　0: １以外の診療行為
        /////     　1: 自動縫合器加算（2500点）が算定可能な診療行為
        /////     ＜加算項目、通則加算項目＞
        /////     　0: １以外の診療行為
        /////     　1: 自動縫合器加算（2500点）自体
        ///// </summary>
        //public int AutoHougouKbn
        //{
        //    get { return TenMst?.AutoHougouKbn ?? 0; }
        //}

        /// <summary>
        /// 外来管理加算区分
        ///     当該診療行為が外来管理加算を算定できないものであるか否かを表す。
        ///     　0: １，２以外の診療行為
        ///     　1: 算定した場合に外来管理加算が算定できない診療行為
        ///     　2: 外来管理加算自体
        /// </summary>
        public int GairaiKanriKbn
        {
            get { return TenMst?.GairaiKanriKbn ?? 0; }
        }

        /// <summary>
        /// 通則加算所定点数対象区分
        ///     通則加算を行う場合において、所定点数として取扱うか否かを表す。
        ///     　0: 所定点数として取扱う診療行為及び通則加算
        ///     　1: 所定点数として取扱わない基本診療行為
        /// </summary>
        public int TusokuTargetKbn
        {
            get { return TenMst?.TusokuTargetKbn ?? 0; }
        }

        /// <summary>
        /// 包括逓減区分
        ///     逓減対象検査等のグループ区分を表す。
        /// </summary>
        public int HokatuKbn
        {
            get { return TenMst?.HokatuKbn ?? 0; }
        }

        /// <summary>
        /// 超音波内視鏡加算区分
        ///     当該診療行為が超音波内視鏡加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 超音波内視鏡加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 超音波内視鏡加算自体
        /// </summary>
        public int TyoonpaNaisiKbn
        {
            get { return TenMst?.TyoonpaNaisiKbn ?? 0; }
        }

        ///// <summary>
        ///// 自動吻合器加算区分
        /////     通則加算を行う場合において、所定点数として取扱うか否かを表す。
        /////     　0: 所定点数として取扱う診療行為及び通則加算
        /////     　1: 所定点数として取扱わない基本診療行為
        ///// </summary>
        //public int AutoFungoKbn
        //{
        //    get { return TenMst?.AutoFungoKbn ?? 0; }
        //}

        ///// <summary>
        ///// 超音波凝固切開装置等加算区分
        /////     当該診療行為が超音波凝固切開装置等加算を算定できるものであるか否かを表す。
        /////     ＜基本項目、合成項目、準用項目＞
        /////     　0: 1以外の診療行為
        /////     　1: 超音波凝固切開装置等加算が算定可能な診療行為
        /////     ＜加算項目、通則加算項目＞
        /////     　0: 1以外の診療行為
        /////     　1: 超音波凝固切開装置等加算自体
        ///// </summary>
        //public int TyoonpaGyokoKbn
        //{
        //    get { return TenMst?.TyoonpaGyokoKbn ?? 0; }
        //}

        ///// <summary>
        ///// 画像等手術支援加算
        /////     当該診療行為が画像等手術支援加算を算定できるものであるか否かを表す。
        /////     ＜基本項目、合成項目、準用項目＞
        /////     　0: 1～5以外の診療行為
        /////     　1: ナビゲーションによる支援加算（２０００点）が算定できる診療行為
        /////     　2: 実物大臓器立体モデルによる支援加算（２０００点）が算定できる診療行為
        /////     　3: ナビゲーション又は実物大臓器立体モデルによる支援加算（共に２０００点）が算定できる診療行為
        /////     　4: 患者適合型手術支援ガイドによる支援加算（２０００点）が算定できる診療行為
        /////     　5: ナビゲーション又は患者適合型手術支援ガイドによる支援加算（共に２０００点）が算定できる診療行為
        /////     ＜加算項目、通則加算項目＞
        /////     　0: 1,2,4以外の診療行為
        /////     　1: ナビゲーションによる支援加算自体
        /////     　2: 実物大臓器立体モデルによる支援加算自体
        /////     　4: 患者適合型手術支援ガイドによる支援加算自体
        ///// </summary>
        //public int GazoKasan
        //{
        //    get { return TenMst?.GazoKasan ?? 0; }
        //}

        ///// <summary>
        ///// 医療観察法対象区分
        /////     当該診療行為が医療観察法において算定可能であるか否かを表す。
        /////     　0: 1～4以外の診療行為
        /////     　1: 入院のみに出来高部分で算定可能な診療行為
        /////     　2: 外来（通院）のみに出来高部分で算定可能な診療行為
        /////     　3: 入院、外来（通院）共に出来高部分で算定可能な診療行為
        /////     　4: 医療観察法専用の診療行為
        ///// </summary>
        //public int KansatuKbn
        //{
        //    get { return TenMst?.KansatuKbn ?? 0; }
        //}

        /// <summary>
        /// 麻酔識別区分
        ///     当該診療行為がマスク又は気管内挿管による閉鎖循環式全身麻酔であるか否かを表す。
        ///     　0: １～９以外の診療行為
        ///     　1: マスク又は気管内挿管による閉鎖循環式全身麻酔１
        ///     　2: マスク又は気管内挿管による閉鎖循環式全身麻酔２
        ///     　3: マスク又は気管内挿管による閉鎖循環式全身麻酔３
        ///     　4: マスク又は気管内挿管による閉鎖循環式全身麻酔４
        ///     　5: マスク又は気管内挿管による閉鎖循環式全身麻酔５
        ///     　8: マスク又は気管内挿管による閉鎖循環式全身麻酔の加算（硬膜外麻酔併施加算以外）
        ///     　9: 硬膜外麻酔併施加算
        /// </summary>
        public int MasuiKbn
        {
            get { return TenMst?.MasuiKbn ?? 0; }
        }

        ///// <summary>
        ///// 副鼻腔手術用内視鏡加算
        /////     当該診療行為が副鼻腔手術用内視鏡加算を算定できるものであるか否かを表す。
        /////     ＜基本項目、合成項目、準用項目＞
        /////     　0: 1以外の診療行為
        /////     　1: 副鼻腔手術用内視鏡加算が算定可能な診療行為
        /////     ＜加算項目、通則加算項目＞
        /////     　0: 1以外の診療行為
        /////     　1: 副鼻腔手術用内視鏡加算自体
        ///// </summary>
        //public int FukubikuNaisiKasan
        //{
        //    get { return TenMst?.FukubikuNaisiKasan ?? 0; }
        //}

        ///// <summary>
        ///// 副鼻腔手術用骨軟部組織切除機器加算
        /////     当該診療行為が副鼻腔手術用骨軟部組織切除機器加算を算定できるものであるか否かを表す。
        /////     ＜基本項目、合成項目、準用項目＞
        /////     　0: 1以外の診療行為
        /////     　1: 副鼻腔手術用骨軟部組織切除機器加算が算定可能な診療行為
        /////     ＜加算項目、通則加算項目＞
        /////     　0: 1以外の診療行為
        /////     　1: 副鼻腔手術用骨軟部組織切除機器加算自体
        ///// </summary>
        //public int FukubikuKotunanKasan
        //{
        //    get { return TenMst?.FukubikuKotunanKasan ?? 0; }
        //}

        ///// <summary>
        ///// 長時間麻酔管理加算
        /////     当該診療行為が長時間麻酔管理加算を算定できるものであるか否かを表す。
        /////     ＜基本項目、合成項目、準用項目＞
        /////     　0: 1,2以外の診療行為
        /////     　1: 長時間麻酔管理加算が算定可能な診療行為
        /////     　2: L008に掲げるマスク又は気管内挿管による閉鎖循環式全身麻酔の実施時間が８時間を超え、長時間麻酔管理加算を算定する場合に実施している必要がある手術
        /////     ＜加算項目、通則加算項目＞
        /////     　0: 1以外の診療行為
        /////     　1: 長時間麻酔管理加算自体
        ///// </summary>
        //public int MasuiKasan
        //{
        //    get { return TenMst?.MasuiKasan ?? 0; }
        //}

        ///// <summary>
        ///// 非侵襲的血行動態モニタリング加算
        /////     当該診療行為が非侵襲的血行動態モニタリング加算を算定できるものであるか否かを表す。
        /////     ＜基本項目、合成項目、準用項目＞
        /////     　0: １、２以外の診療行為
        /////     　1: 非侵襲的血行動態モニタリング加算が算定可能な診療行為
        /////     　2: 非侵襲的血行動態モニタリング加算を算定する場合に実施している必要がある手術
        /////     ＜加算項目、通則加算項目＞
        /////     　0: １以外の診療行為
        /////     　1: 非侵襲的血行動態モニタリング加算自体
        ///// </summary>
        //public int MoniterKasan
        //{
        //    get { return TenMst?.MoniterKasan ?? 0; }
        //}

        ///// <summary>
        ///// 凍結保存同種組織加算
        /////     当該診療行為が凍結保存同種組織加算を算定できるものであるか否かを表す。
        /////     ＜基本項目、合成項目、準用項目＞
        /////     　0: １以外の診療行為
        /////     　1: 凍結保存同種組織加算が算定可能な診療行為
        /////     ＜加算項目、通則加算項目＞
        /////     　0: 1以外の診療行為
        /////     　1: 凍結保存同種組織加算自体
        ///// </summary>
        //public int ToketuKasan
        //{
        //    get { return TenMst?.ToketuKasan ?? 0; }
        //}

        ///// <summary>
        ///// 点数表区分番号
        /////     医科点数表の「第２章 特掲診療料」「第１０部 手術」に規定する診療行為（通則及び注に掲げる加算等を除く。）の区分番号及び項番等を表す。
        ///// </summary>
        //public string TenKbnNo
        //{
        //    get { return TenMst?.TenKbnNo ?? string.Empty; }
        //}

        ///// <summary>
        ///// 短期滞在手術
        /////     当該診療行為が短期滞在手術等基本料を算定できるものであるか否かを表す。
        /////     　０：１～４以外の診療行為
        /////     　１：短期滞在手術等基本料１
        /////     　２：短期滞在手術等基本料２
        /////     　３：短期滞在手術等基本料１が算定可能な診療行為（手術）
        /////     　４：短期滞在手術等基本料２が算定可能な診療行為（手術）
        ///// </summary>
        //public int ShortstayOpe
        //{
        //    get { return TenMst?.ShortstayOpe ?? 0; }
        //}

        /// <summary>
        /// 部位区分
        ///     画像診断撮影部位マスターにおいて撮影部位を表す。
        ///     　0: 部位以外
        ///     　1: 頭部
        ///     　2: 躯幹
        ///     　3: 四肢
        ///     　5: 胸部
        ///     　6: 腹部
        ///     　7: 脊椎
        ///     　8: 消化管
        ///      10: 指
        ///      99: その他部位（撮影部位マスターでない場合も含む。）
        /// </summary>
        public int BuiKbn
        {
            get { return TenMst?.BuiKbn ?? 0; }
        }

        ///// <summary>
        ///// 施設基準コード１
        /////     ＜診療行為＞
        /////        当該診療行為が施設基準に関するものであるか否かを表す。
        /////     　「施設基準コード１」から使用し最大１０項目（「施設基準コード１０」）まで使用可能である。
        /////     　施設基準コードについては「別紙５」を参照。
        /////     ＜医薬品＞
        /////     　当該医薬品について薬価基準の規格単位数を表す。
        /////     　ただし、規格単位数が１の場合は省略し０を収容する。
        ///// </summary>
        //public int Sisetucd1
        //{
        //    get { return TenMst?.Sisetucd1 ?? 0; }
        //}

        ///// <summary>
        ///// 施設基準コード２
        /////     ＜診療行為＞
        /////     施設基準コード１を参照。
        /////     ＜医薬品＞
        /////     　当該医薬品が湿布薬で単位が「ｇ」の場合は膏体量を収容する。
        /////             /// </summary>
        //public int Sisetucd2
        //{
        //    get { return TenMst?.Sisetucd2 ?? 0; }
        //}

        ///// <summary>
        ///// 施設基準コード３
        ///// </summary>
        //public int Sisetucd3
        //{
        //    get { return TenMst?.Sisetucd3 ?? 0; }
        //}

        ///// <summary>
        ///// 施設基準コード４
        ///// </summary>
        //public int Sisetucd4
        //{
        //    get { return TenMst?.Sisetucd4 ?? 0; }
        //}

        ///// <summary>
        ///// 施設基準コード５
        ///// </summary>
        //public int Sisetucd5
        //{
        //    get { return TenMst?.Sisetucd5 ?? 0; }
        //}

        ///// <summary>
        ///// 施設基準コード６
        ///// </summary>
        //public int Sisetucd6
        //{
        //    get { return TenMst?.Sisetucd6 ?? 0; }
        //}

        ///// <summary>
        ///// 施設基準コード７
        ///// </summary>
        //public int Sisetucd7
        //{
        //    get { return TenMst?.Sisetucd7 ?? 0; }
        //}

        ///// <summary>
        ///// 施設基準コード８
        ///// </summary>
        //public int Sisetucd8
        //{
        //    get { return TenMst?.Sisetucd8 ?? 0; }
        //}

        ///// <summary>
        ///// 施設基準コード９
        ///// </summary>
        //public int Sisetucd9
        //{
        //    get { return TenMst?.Sisetucd9 ?? 0; }
        //}

        ///// <summary>
        ///// 施設基準コード１０
        ///// </summary>
        //public int Sisetucd10
        //{
        //    get { return TenMst?.Sisetucd10 ?? 0; }
        //}

        /// <summary>
        /// 年齢加算下限年齢１
        ///     当該診療行為に算定可能な年齢注加算の診療行為コードを表し、最大４つの年齢範囲まで記録する。
        ///     未使用部分には、下限年齢、上限年齢及び注加算診療行為コードに「ゼロ」を記録する。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA: 生後28日
        ///     B3: 3歳に達した日の翌月の１日
        ///     BF: 15歳に達した日の翌月の１日
        ///     BK: 20歳に達した日の翌月の１日
        ///     年齢加算下限年齢：
        ///     当該診療行為に注加算の算定が可能な場合、記録された注加算診療行為コードの下限年齢を表す。
        ///     年齢加算上限年齢：
        ///     　当該診療行為に注加算の算定が可能な場合、記録された注加算診療行為コードの上限年齢を表す。
        ///     注加算診療行為コード：
        ///     　年齢注加算の診療行為コードを表す。
        /// </summary>
        public string AgekasanMin1
        {
            get { return TenMst?.AgekasanMin1 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算上限年齢１
        ///     年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMax1
        {
            get { return TenMst?.AgekasanMax1 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算注加算診療行為コード１
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanCd1
        {
            get { return TenMst?.AgekasanCd1 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算下限年齢２
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMin2
        {
            get { return TenMst?.AgekasanMin2 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算上限年齢２
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMax2
        {
            get { return TenMst?.AgekasanMax2 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算注加算診療行為コード２
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanCd2
        {
            get { return TenMst?.AgekasanCd2 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算下限年齢３
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMin3
        {
            get { return TenMst?.AgekasanMin3 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算上限年齢３
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMax3
        {
            get { return TenMst?.AgekasanMax3 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算注加算診療行為コード３
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanCd3
        {
            get { return TenMst?.AgekasanCd3 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算下限年齢４
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMin4
        {
            get { return TenMst?.AgekasanMin4 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算上限年齢４
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMax4
        {
            get { return TenMst?.AgekasanMax4 ?? string.Empty; }
        }

        /// <summary>
        /// 年齢加算注加算診療行為コード４
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanCd4
        {
            get { return TenMst?.AgekasanCd4 ?? string.Empty; }
        }

        ///// <summary>
        ///// 検体検査コメント
        /////     当該診療行為が、検体検査の検体コメントであるか否かを表す。
        /////     　0: 検体コメント以外
        /////     　1: 検体コメント
        ///// </summary>
        public int KensaCmt
        {
            get { return TenMst?.KensaCmt ?? 0; }
        }

        /// <summary>
        /// 麻毒区分
        ///     当該医薬品が麻薬、毒薬、覚せい剤原料又は向精神薬であるか否かを表す。
        ///     　0: 麻薬、毒薬、覚せい剤原料又は向精神薬以外
        ///     　1: 麻薬
        ///     　2: 毒薬
        ///     　3: 覚せい剤原料
        ///     　5: 向精神薬
        /// </summary>
        public int MadokuKbn
        {
            get { return TenMst?.MadokuKbn ?? 0; }
        }

        /// <summary>
        /// 神経破壊剤区分
        ///     当該医薬品が神経破壊剤であるか否かを表す。
        ///     　0: 神経破壊剤以外
        ///     　1: 神経破壊剤
        /// </summary>
        public int SinkeiKbn
        {
            get { return TenMst?.SinkeiKbn ?? 0; }
        }

        /// <summary>
        /// 生物学的製剤区分
        ///     当該医薬品が生物学的製剤加算対象品目であるか否かを表す。
        ///     　0: 生物学的製剤加算対象品目以外
        ///     　1: 生物学的製剤加算対象品目
        /// </summary>
        public int SeibutuKbn
        {
            get { return TenMst?.SeibutuKbn ?? 0; }
        }

        /// <summary>
        /// 造影剤区分
        ///     当該医薬品が造影剤又は造影補助剤であるか否かを表す。
        ///     　0: 造影剤、造影補助剤以外
        ///     　1: 造影剤
        ///     　2: 造影補助剤
        /// </summary>
        public int ZoueiKbn
        {
            get { return TenMst?.ZoueiKbn ?? 0; }
        }

        /// <summary>
        /// 薬剤区分
        ///     当該医薬品の薬剤区分を表す。
        ///       0: 薬剤以外
        ///     　1: 内用薬
        ///     　3: その他
        ///     　4: 注射薬
        ///     　6: 外用薬
        ///     　8: 歯科用薬剤
        ///     （削）9: 歯科特定薬剤
        ///     ※レセプト電算マスターの項目「剤型」を収容する。
        /// </summary>
        public int DrugKbn
        {
            get { return TenMst?.DrugKbn ?? 0; }
        }

        public bool IsDrug
        {
            get
            {
                bool ret = false;

                if (DrugKbn > 0 ||
                    (string.IsNullOrEmpty(ItemCd) == false && ItemCd.Length == 9 && ItemCd.StartsWith("6")))
                {
                    ret = true;
                }

                return ret;
            }
        }

        /// <summary>
        /// 剤型区分
        ///     当該医薬品の剤型区分を表す。
        ///     　0: 下記以外
        ///     　1: 散剤
        ///     　2: 顆粒剤（細粒剤）
        ///     　3: 液剤
        ///     ※レセプト電算マスターの項目「剤型」とは異なる。
        /// </summary>
        public int ZaiKbn
        {
            get { return TenMst?.ZaiKbn ?? 0; }
        }

        /// <summary>
        /// 注射容量
        ///     当該医薬品が注射薬の場合、その容量（単位はｍＬ）を表す。
        /// </summary>
        public int Capacity
        {
            get { return TenMst?.Capacity ?? 0; }
        }

        /// <summary>
        /// 特定器材年齢加算区分
        ///     当該特定器材が年齢加算に関係があるか否かを表す。
        ///     　0: 年齢加算に関係のない特定器材
        ///     　1: 年齢加算又は年齢加算が算定可能な特定器材
        ///     　　　＊胸部又は腹部単純撮影の乳幼児加算、及びフィルム料
        /// </summary>
        public int TokuzaiAgeKbn
        {
            get { return TenMst?.TokuzaiAgeKbn ?? 0; }
        }

        /// <summary>
        /// 酸素等区分
        ///     当該特定器材が酸素又は窒素に関するものであるか否かを表す。
        ///     　0: 酸素、窒素、酸素補正率及び高気圧酸素加算以外
        ///     　1: 酸素補正率及び高気圧酸素加算
        ///     　2: 定置式液化酸素貯槽（ＣＥ）
        ///     　3: 可搬式液化酸素容器（ＬＧＣ）
        ///     　4: 大型ボンベ
        ///     　5: 小型ボンベ
        ///     　9: 窒素
        /// </summary>
        public int SansoKbn
        {
            get { return TenMst?.SansoKbn ?? 0; }
        }

        /// <summary>
        /// 特定器材種別１
        ///     当該特定器材の点数算定方法の種別を表す。
        ///     　0:  ↑購入価格｜
        ///     　　　｜－－－－｜
        ///     　　　｜　１０円↓により算定する特定器材
        ///     　2:  ↑↑購入価格↓｜
        ///     　　　｜－－－－－－｜
        ///     　　　｜　　１０円　↓により算定する特定器材
        ///     （酸素、窒素）
        ///     　3:  ↑購入価格｜
        ///     　　　｜－－－－｜
        ///     　　　｜　５０円↓により算定する特定器材（高線量率イリジウム）
        ///     　4:  ↑購入価格　｜
        ///     　　　｜－－－－－｜
        ///     　　　｜１０００円↓により算定する特定器材（コバルト）
        ///     　　　↑↓：四捨五入
        /// </summary>
        public int TokuzaiSbt
        {
            get { return TenMst?.TokuzaiSbt ?? 0; }
        }

        /// <summary>
        /// 上限価格
        ///     当該特定器材の金額に酸素の上限価格の設定がされていることを表す。
        ///     　0: 下記以外
        ///     　1: 上限価格の設定がされている場合
        /// </summary>
        public int MaxPrice
        {
            get { return TenMst?.MaxPrice ?? 0; }
        }

        /// <summary>
        /// 上限点数
        ///     当該特定器材（眼底カメラ検査用インスタントフィルム）が算定可能な上限点数を表す。上限点数の設定されない場合は「０」である。
        /// </summary>
        public int MaxTen
        {
            get { return TenMst?.MaxTen ?? 0; }
        }

        /// <summary>
        /// 点数欄集計先識別（外来）
        ///     当該診療行為の入院外レセプトにおける点数欄への集計先を表す。
        ///     点数欄集計先識別については「別紙９」を参照。
        ///     入院外レセプトで使用不可の診療行為は「０」である。
        /// </summary>
        public string SyukeiSaki
        {
            get { return TenMst?.SyukeiSaki ?? ""; }
        }

        /// <summary>
        /// コード表用区分－区分
        ///     当該診療行為について医科点数表の章、部、区分番号及び項番を記録する。
        ///             区分（アルファベット部）：
        ///     　点数表の区分番号のアルファベット部を記録する。
        ///     　なお、介護老人保健施設入所者に係る診療料、医療観察法、入院時食事療養、入院時生活療養及び標準負担額については
        ///     　「－」（ハイホン）を、点数表に区分設定がないものは「＊」を記録する。
        ///     章：
        ///     部：
        ///     区分番号：
        ///     枝番：
        ///     項番：
        /// </summary>
        public string CdKbn
        {
            get { return TenMst?.CdKbn ?? string.Empty; }
        }

        /// <summary>
        /// コード表用区分－章
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdSyo
        {
            get { return TenMst?.CdSyo ?? 0; }
        }

        /// <summary>
        /// コード表用区分－部
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdBu
        {
            get { return TenMst?.CdBu ?? 0; }
        }

        /// <summary>
        /// コード表用区分－区分番号
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdKbnno
        {
            get { return TenMst?.CdKbnno ?? 0; }
        }

        /// <summary>
        /// コード表用区分－区分番号－枝番
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdEdano
        {
            get { return TenMst?.CdEdano ?? 0; }
        }

        /// <summary>
        /// コード表用区分－項番
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdKouno
        {
            get { return TenMst?.CdKouno ?? 0; }
        }

        ///// <summary>
        ///// 告知・通知関連番号－区分
        /////     当該診療行為が準用項目の場合、準用元の医科点数表の章、部、区分番号及び項番を記録する。
        /////             区分（アルファベット部）：
        /////     　点数表の区分番号のアルファベット部を記録する。準用項目以外は未使用。
        /////     章：
        /////     部：
        /////     区分番号：
        /////     枝番：
        /////     項番：
        ///// </summary>
        //public string KokujiKbn
        //{
        //    get { return TenMst?.KokujiKbn ?? string.Empty; }
        //}

        ///// <summary>
        ///// 告知・通知関連番号－章
        /////     告知・通知関連番号－区分を参照。
        ///// </summary>
        //public int KokujiSyo
        //{
        //    get { return TenMst?.KokujiSyo ?? 0; }
        //}

        ///// <summary>
        ///// 告知・通知関連番号－部
        /////     告知・通知関連番号－区分を参照。
        ///// </summary>
        //public int KokujiBu
        //{
        //    get { return TenMst?.KokujiBu ?? 0; }
        //}

        ///// <summary>
        ///// 告知・通知関連番号－区分番号
        /////     告知・通知関連番号－区分を参照。
        ///// </summary>
        //public int KokujiKbnNo
        //{
        //    get { return TenMst?.KokujiKbnNo ?? 0; }
        //}

        ///// <summary>
        ///// 告知・通知関連番号－区分番号－枝番
        /////     告知・通知関連番号－区分を参照。
        ///// </summary>
        //public int KokujiEdaNo
        //{
        //    get { return TenMst?.KokujiEdaNo ?? 0; }
        //}

        ///// <summary>
        ///// 告知・通知関連番号－項番
        /////     告知・通知関連番号－区分を参照。
        ///// </summary>
        //public int KokujiKouNo
        //{
        //    get { return TenMst?.KokujiKouNo ?? 0; }
        //}

        /// <summary>
        /// 告示等識別区分（１）
        ///     当該診療行為についてコンピューター運用上の取扱い（磁気媒体に記録する際の取扱い）を表す。
        ///     　1: 基本項目（告示）　※基本項目
        ///     　3: 合成項目　　　　　※基本項目
        ///     　5: 準用項目（通知）　※基本項目
        ///     　7: 加算項目　　　　　※加算項目
        ///     　9: 通則加算項目　　　※加算項目
        ///       0: 診療行為以外（薬剤、特材等）
        ///       A: 入院基本料労災乗数項目又は四肢加算（手術）項目
        /// </summary>
        public string Kokuji1
        {
            get { return TenMst?.Kokuji1 ?? string.Empty; }
        }

        /// <summary>
        /// 告示等識別区分（２）
        ///     当該診療行為について点数表上の取扱いを表す。
        ///     　1: 基本項目（告示）
        ///     　3: 合成項目
        ///     （削）5: 準用項目（通知）
        ///     　7: 加算項目（告示）
        ///     （削）9: 通則加算項目
        ///       0: 診療行為以外（薬剤、特材等）
        /// </summary>
        public string Kokuji2
        {
            get { return TenMst?.Kokuji2 ?? string.Empty; }
        }

        /// <summary>
        /// 公表順序番号
        ///     コード表用番号による順序番号を記録する。
        /// </summary>
        public int KohyoJun
        {
            get { return TenMst?.KohyoJun ?? 0; }
        }

        /// <summary>
        /// 個別医薬品コード
        ///     薬価基準収載医薬品コードと同様に英数12桁のコードですが、統一名収載品目の個々の商品に対して別々のコードが付与されます。
        ///     銘柄別収載品目（商品名で官報に収載されるもの）については、薬価基準収載医薬品コードと同じコードです。
        /// </summary>
        public string YjCd
        {
            get { return TenMst?.YjCd ?? string.Empty; }
        }

        /// <summary>
        /// 薬価基準収載医薬品コード
        ///     当該医薬品に係る薬価基準収載医薬品コードを表す。
        /// </summary>
        public string YakkaCd
        {
            get { return TenMst?.YakkaCd ?? string.Empty; }
        }

        ///// <summary>
        ///// 収載方式等識別
        /////     当該医薬品の薬価基準収載方式の分類を表す。
        /////     　0: 1～8以外の医薬品
        /////     　1: 日本薬局方収載医薬品（局方品）
        /////     　2: 局方品で生物学的製剤基準医薬品
        /////     　3: 局方品で生薬
        /////     　6: 生物学的製剤基準医薬品
        /////     　7: 生薬
        /////     　8: 1～7以外の統一名収載品
        ///// </summary>
        //public int SyusaiSbt
        //{
        //    get { return TenMst?.SyusaiSbt ?? 0; }
        //}

        /// <summary>
        /// 労災区分
        ///     当該診療行為が労災保険で算定可能かを表す。
        ///     　0: 健保・労災において算定可能
        ///     　1: 労災のみ算定可能
        ///     　2: 健保のみ算定可能
        /// </summary>
        public int RousaiKbn
        {
            get { return TenMst?.RousaiKbn ?? 0; }
        }

        /// <summary>
        /// 四肢加算区分（労災）
        ///     当該診療行為の四肢に対する特例の取扱い（1.5倍・2.0倍）を表す。
        ///     　0: 1～5以外の診療行為
        ///     　1: 1.5倍又は2.0倍の対象
        ///     　2: 1.5倍のみ対象
        ///     　3: 2.0倍のみ対象
        ///     　4: 1.5倍の加算自体
        ///     　5: 2.0倍の加算自体
        /// </summary>
        public int SisiKbn
        {
            get { return TenMst?.SisiKbn ?? 0; }
        }

        /// <summary>
        /// フィルム撮影回数
        ///     フィルム1枚あたりの撮影回数
        ///     0: フィルム以外
        /// </summary>
        public int ShotCnt
        {
            get { return TenMst?.ShotCnt ?? 0; }
        }

        /// <summary>
        /// 紙レセ非表示区分
        ///     0: 表示
        ///     1: 非表示（摘要欄に表示しない、点数欄には表示する）
        /// </summary>
        public int IsNodspPaperRece
        {
            get { return TenMst?.IsNodspPaperRece ?? 0; }
        }

        /// <summary>
        /// 領収証非表示区分
        ///     0: 表示
        ///     1: 非表示
        /// </summary>
        public int IsNodspRyosyu
        {
            get { return TenMst?.IsNodspRyosyu ?? 0; }
        }

        /// <summary>
        /// カルテ非表示区分
        ///     0: 表示
        ///     1: 非表示
        /// </summary>
        public int IsNodspKarte
        {
            get { return TenMst?.IsNodspKarte ?? 0; }
        }

        /// <summary>
        /// 自費種別コード
        ///     0: 自費項目以外
        ///     >0: 自費項目
        ///     JIHI_SBT_MST.自費種別
        /// </summary>
        public int JihiSbt
        {
            get { return TenMst?.JihiSbt ?? 0; }
        }

        /// <summary>
        /// 課税区分
        ///     0: 非課税
        ///     1: 外税
        ///     2: 内税
        /// </summary>
        public int KazeiKbn
        {
            get { return TenMst?.KazeiKbn ?? 0; }
        }

        /// <summary>
        /// 用法区分
        ///     0: 用法以外
        ///     1: 基本用法
        ///     2: 補助用法
        /// </summary>
        public int YohoKbn
        {
            get { return TenMst?.YohoKbn ?? 0; }
        }

        /// <summary>
        /// 用法区分
        ///     0: 用法以外
        ///     1: 基本用法
        ///     2: 補助用法
        /// </summary>
        public int OdrYohoKbn
        {
            get { return OdrInfDetail.YohoKbn; }
        }

        /// <summary>
        /// 一般名コード
        ///     YJ_CDの頭9桁（例外あり）
        /// </summary>
        public string IpnNameCd
        {
            get { return TenMst?.IpnNameCd ?? string.Empty; }
        }

        /// <summary>
        /// 数量端数切り上げ区分
        ///     1: 端数切り上げ
        /// </summary>
        public int SuryoRoundupKbn
        {
            get { return TenMst?.SuryoRoundupKbn ?? 0; }
        }

        /// <summary>
        /// 向精神薬区分
        ///     0: 向精神薬以外
        ///     1:抗不安薬
        ///     2:睡眠薬
        ///     3:抗うつ薬
        ///     4:抗精神病薬
        /// </summary>
        public int KouseisinKbn
        {
            get { return TenMst?.KouseisinKbn ?? 0; }
        }

        /// <summary>
        /// 注射薬剤種別
        ///     ※1～5は、人工腎臓に包括される
        ///     0:1～5以外
        ///     1:透析液
        ///     2:血液凝固阻止剤
        ///     3:エリスロポエチン
        ///     4:ダルベポエチン
        ///     5:生理食塩水"										
        /// </summary>
        public int ChusyaDrugSbt
        {
            get { return TenMst?.ChusyaDrugSbt ?? 0; }
        }

        /// <summary>
        /// 検査1来院複数回算定
        ///     0: オーダー用検査項目は複数回算定不可、医事検査項目は複数回算定可
        ///     1: 複数回算定可
        ///     2: 複数回算定不可
        ///     ※検体検査(SIN_KOUI_KBN= 61)の項目に有効な設定
        ///     ※単位あり項目については、当該区分の設定に関わらず、複数回算定可
        /// </summary>
        public int KensaFukusuSantei
        {
            get { return TenMst?.KensaFukusuSantei ?? 0; }
        }

        /// <summary>
        /// 算定診療行為コード
        ///     算定時の診療行為コード（自己結合でデータ取得）
        /// </summary>
        public string SanteiItemCd
        {
            get { return _santeiItemCd ?? ""; }
            set { _santeiItemCd = value; }
        }

        /// <summary>
        /// 算定外区分
        ///     0: 算定する
        ///     1: 算定外
        /// </summary>
        public int SanteigaiKbn
        {
            get { return TenMst?.SanteigaiKbn ?? 0; }
        }

        /// <summary>
        /// 採血料区分
        ///     当該診療行為を算定した場合に血液採取料を自動発生させるか否かを表す。
        ///     　0: 下記以外
        ///     　1: 採血料を算定する
        /// </summary>
        public int SaiketuKbn
        {
            get { return TenMst?.SaiketuKbn ?? 0; }
        }

        /// <summary>
        /// コメント区分
        ///     計算時、コメントを自動で付与する場合、付与するコメントの種類を設定
        /// 0: 自動付与なし
        /// 1: 実施日
        /// 2: 前回日（840000087 :前回実施 月日）
        /// 3: 初回日（840000085 :初回実施 月日）
        /// 4: 前回日 or 初回日
        /// 5: 初回算定日（840000085 :初回算定 月日）
        /// 6: 実施日（列挙）
        /// 7: 実施日（列挙：前月末・翌月頭含む）
        /// 8: 実施日（列挙：項目名あり）
        /// 9: 実施日数（840000096 :実施日数 日）
        /// 10: 前回日 or 初回日（項目名あり）
        /// </summary>
        public int CmtKbn
        {
            //get { return TenMst?.CmtKbn ?? 0; }
            get { return _cmtKbnMst?.CmtKbn ?? 0; }
        }

        /// <summary>
        /// カラム位置１
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol1
        {
            get { return TenMst?.CmtCol1 ?? 0; }
        }

        /// <summary>
        /// 桁数１
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta1
        {
            get { return TenMst?.CmtColKeta1 ?? 0; }
        }

        /// <summary>
        /// カラム位置２
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol2
        {
            get { return TenMst?.CmtCol2 ?? 0; }
        }

        /// <summary>
        /// 桁数２
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta2
        {
            get { return TenMst?.CmtColKeta2 ?? 0; }
        }

        /// <summary>
        /// カラム位置３
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol3
        {
            get { return TenMst?.CmtCol3 ?? 0; }
        }

        /// <summary>
        /// 桁数３
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta3
        {
            get { return TenMst?.CmtColKeta3 ?? 0; }
        }

        /// <summary>
        /// カラム位置４
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol4
        {
            get { return TenMst?.CmtCol4 ?? 0; }
        }

        /// <summary>
        /// 桁数４
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta4
        {
            get { return TenMst?.CmtColKeta4 ?? 0; }
        }

        /// <summary>
        /// 選択式コメント識別
        ///     選択式コメントであるか否かを表す。  
        ///     0: 選択式以外のコメント 
        ///     1: 選択式コメント
        /// </summary>
        public int SelectCmtId
        {
            get { return TenMst?.SelectCmtId ?? 0; }
        }
        /// <summary>
        /// 外来感染症対策向上加算等
        /// 
        /// 外来感染症対策向上加算等を算定可能な診療行為であるか否かを表す。
        /// ＜基本項目、合成項目、準用項目＞
        /// 0：1,2以外の診療行為
        /// 1：外来感染症対策向上加算等（医学管理等）を算定可能な診療行為
        /// 2：外来感染症対策向上加算等（在宅医療）を算定可能な診療行為
        /// 
        /// ＜加算項目、通則加算項目＞
        /// 0：1～6以外の診療行為
        /// 1：外来感染症対策向上加算（医学管理等）自体
        /// 2：連携強化加算（医学管理等）自体
        /// 3：サーベイランス強化加算（医学管理等）自体
        /// 4：外来感染症対策向上加算（在宅医療）自体
        /// 5：連携強化加算（在宅医療）自体
        /// 6：サーベイランス強化加算（在宅医療）自体
        /// </summary>
        public int GairaiKansen
        {
            get { return TenMst?.GairaiKansen ?? 0; }
        }
        /// <summary>
        /// 耳鼻咽喉科乳幼児処置加算
        /// 
        /// 耳鼻咽喉科乳幼児処置加算を算定可能な診療行為であるか否かを表す。
        /// 
        /// 0：耳鼻咽喉科乳幼児加算を算定できない診療行為
        /// 1：耳鼻咽喉科乳幼児処置加算を算定可能な診療行為
        /// 2：耳鼻咽喉科乳幼児処置加算自体
        /// </summary>
        public int JibiAgeKasan
        {
            get { return TenMst?.JibiAgeKasan ?? 0; }
        }
        /// <summary>
        /// コメント種別
        /// コメントマスターの場合、コメントの種類を表す。
        /// 0:下記以外
        /// 1:初回日
        /// 2:前回日
        /// 3:実施日
        /// 4:手術日
        /// 5:発症日
        /// 6:治療開始日
        /// 7:発症日または治療開始日
        /// 8:急性憎悪
        /// 9:初回診断
        /// 10:診療時間
        /// 11:疾患名
        /// 20:撮影部位
        /// 21:撮影部位（胸部）
        /// 22:撮影部位（腹部）
        /// </summary>
        public int CmtSbt
        {
            get { return TenMst?.CmtSbt ?? 0; }
        }
        /// <summary>
        /// AgeKasanMinに配列でアクセスする
        /// </summary>
        /// <param name="index">取得したいAgeKasanMinのIndex(1～4)</param>
        /// <returns>Indexで指定したAgeKasanMin</returns>
        public string AgekasanMin(int index)
        {
            switch (index)
            {
                case 1:
                    return AgekasanMin1;
                case 2:
                    return AgekasanMin2;
                case 3:
                    return AgekasanMin3;
                case 4:
                    return AgekasanMin4;
            }
            return "";
        }

        /// <summary>
        /// AgeKasanMaxに配列でアクセスする
        /// </summary>
        /// <param name="index">取得したいAgeKasanMaxのIndex(1～4)</param>
        /// <returns>Indexで指定したAgeKasanMax</returns>
        public string AgekasanMax(int index)
        {
            switch (index)
            {
                case 1:
                    return AgekasanMax1;
                case 2:
                    return AgekasanMax2;
                case 3:
                    return AgekasanMax3;
                case 4:
                    return AgekasanMax4;
            }
            return "";
        }

        /// <summary>
        /// AgeKasanCdに配列でアクセスする
        /// </summary>
        /// <param name="index">取得したいAgeKasanCdのIndex(1～4)</param>
        /// <returns>Indexで指定したAgeKasanCd</returns>
        public string AgekasanCd(int index)
        {
            switch (index)
            {
                case 1:
                    return AgekasanCd1;
                case 2:
                    return AgekasanCd2;
                case 3:
                    return AgekasanCd3;
                case 4:
                    return AgekasanCd4;
            }
            return "";
        }

        /// <summary>
        /// 基本項目(Kokuji1 in (1,3,5) または、MasterSbt in ('S', 'R') and Kokuji1=0)の場合、tureを返す
        /// </summary>
        public bool IsKihonKoumoku
        {
            get { return (new string[] { "1", "3", "5" }.Contains(Kokuji1)) || (new string[] { "S", "R" }.Contains(MasterSbt) && Kokuji1 == "0"); }
        }
        /// <summary>
        /// 基本項目(Kokuji2 in (1,3,5) または、MasterSbt in ('S', 'R') and Kokuji1=0)の場合、tureを返す
        /// </summary>
        public bool IsKihonKoumoku2
        {
            get { return (new string[] { "1", "3", "5" }.Contains(Kokuji2)) || (new string[] { "S", "R" }.Contains(MasterSbt) && Kokuji1 == "0"); }
        }
        /// <summary>
        /// フィルム項目の場合、trueを返す
        /// </summary>
        public bool IsFilm
        {
            get { return (SinKouiKbn == 77 || ItemCd == ItemCdConst.GazoDensibaitaiHozon); }
        }
        public bool IsOnlyFilm
        {
            get { return (SinKouiKbn == 77); }
        }

        /// <summary>
        /// 自費項目の場合、trueを返す
        /// </summary>
        public bool IsJihi
        {
            get { return (TenMst != null && TenMst.JihiSbt > 0); }
        }

        /// <summary>
        /// コメントの場合、trueを返す
        /// </summary>
        public bool IsComment
        {
            get
            {
                return ItemCd == "" ||
                       (ItemCd.StartsWith("8") && ItemCd.Length == 9) ||
                       (TenMst != null && (TenMst.MasterSbt == "C" || TenMst.MasterSbt == "D"));
            }
        }

        /// <summary>
        /// コメントの場合、trueを返す
        /// </summary>
        public bool IsSelectComment
        {
            get
            {
                return (ItemCd.StartsWith("8") && ItemCd.Length == 9);
            }
        }
        /// <summary>
        /// 部位コメントの場合、trueを返す
        /// </summary>
        public bool IsBuiComment
        {
            get
            {
                return (TenMst != null ? new int[] { 20, 21, 22 }.Contains(TenMst.CmtSbt) : false);
            }
        }
        /// <summary>
        /// 部位コメント以外のコメントの場合、trueを返す
        /// </summary>
        public bool IsCommentExcludeBuiComment
        {
            get
            {
                return (IsComment && IsBuiComment == false);
            }
        }

        /// <summary>
        /// 診療行為(種別=S or R)またはコメントの場合trueを返す
        /// </summary>
        /// <param name="commentSkip">true: コメントの場合はスキップ(falseを返す)</param>
        /// <returns></returns>
        public bool IsSorCommentItem(bool commentSkip)
        {
            return (IsSItem || IsBui ||
                (commentSkip == false && IsComment) ||
                (!IsComment && !IsYoho && !IsYItem && !IsTItem));
        }

        public bool IsBui
        {
            get
            {
                bool ret = false;

                if (TenMst != null && TenMst.BuiKbn > 0)
                {
                    ret = true;
                }

                return ret;
            }
        }

        /// <summary>
        /// 薬剤(種別=Y)またはコメントの場合trueを返す
        /// </summary>
        /// <param name="commentSkip">true: コメントの場合はスキップ(falseを返す)</param>
        /// <returns></returns>
        public bool IsYorCommentItem(bool commentSkip)
        {
            return (IsYItem ||
                    (commentSkip == false && IsComment));

        }

        /// <summary>
        /// 用法の場合、trueを返す
        /// </summary>
        /// <returns></returns>
        public bool IsYoho
        {
            get
            {
                return (YohoKbn > 0) || (ItemCd.StartsWith("Y"));
            }
        }

        /// <summary>
        /// 特材(種別=T or U)またはコメントの場合trueを返す
        /// </summary>
        /// <param name="commentSkip">true: コメントの場合はスキップ(falseを返す)</param>
        /// <returns></returns>
        public bool IsTorCommentItem(bool commentSkip)
        {
            return (IsTItem ||
                    (commentSkip == false && IsComment));
        }

        /// <summary>
        /// 診療行為(種別=S or R)の場合trueを返す
        /// </summary>
        public bool IsSItem
        {
            get { return (MasterSbt == "S" || MasterSbt == "R"); }
        }

        /// <summary>
        /// 薬剤(種別=Y)の場合trueを返す
        /// </summary>
        public bool IsYItem
        {
            get { return (MasterSbt == "Y"); }
        }

        /// <summary>
        /// 特材(種別=T or U)の場合trueを返す
        /// </summary>
        public bool IsTItem
        {
            get { return (MasterSbt == "T" || MasterSbt == "U"); }
        }

        /// <summary>
        /// 診断料項目（きざみ）の場合、trueを返す
        /// </summary>
        public bool IsSindanKizami
        {
            get { return CdKbn == "E" && CdKbnno == 1 && KizamiId == 1; }
        }
        /// <summary>
        /// 診断料項目（きざみ以外）の場合、trueを返す
        /// </summary>
        public bool IsSindan
        {
            get { return CdKbn == "E" && CdKbnno == 1 && KizamiId != 1; }
        }
        /// <summary>
        /// 撮影料項目（きざみ）の場合、trueを返す
        /// </summary>
        public bool IsSatueiKizami
        {
            get { return CdKbn == "E" && CdKbnno == 2 && KizamiId == 1; }
        }
        /// <summary>
        /// 撮影料項目（きざみ以外）の場合、trueを返す
        /// </summary>
        public bool IsSatuei
        {
            get { return CdKbn == "E" && CdKbnno == 2 && KizamiId != 1; }
        }
        //-------------------------------------------------------------------------

        /// <summary>
        /// 保険区分
        ///  0:自費
        ///  1:社保
        ///  2:国保
        ///  11:労災(短期給付)
        ///  12:労災(傷病年金)
        ///  13:アフターケア
        ///  14:自賠責
        /// </summary>
        public int HokenKbn
        {
            get { return _hokenKbn; }
        }

        /// <summary>
        /// 保険種別
        ///  0:健保
        ///  1:労災
        ///  2:アフターケア
        ///  3:自賠責
        ///  4:自費
        /// </summary>
        public int HokenSyu
        {
            get
            {
                //if (new List<int> { 1, 2 }.Contains(PtHokenPattern.HokenKbn))
                if (new List<int> { 1, 2 }.Contains(_hokenKbn))
                {
                    return Domain.Constant.HokenSyu.Kenpo;
                }
                //else if (new List<int> { 11, 12 }.Contains(PtHokenPattern.HokenKbn))
                else if (new List<int> { 11, 12 }.Contains(_hokenKbn))
                {
                    return Domain.Constant.HokenSyu.Rosai;
                }
                //else if (new List<int> { 13 }.Contains(PtHokenPattern.HokenKbn))
                else if (new List<int> { 13 }.Contains(_hokenKbn))
                {
                    return Domain.Constant.HokenSyu.After;
                }
                //else if (new List<int> { 14 }.Contains(PtHokenPattern.HokenKbn))
                else if (new List<int> { 14 }.Contains(_hokenKbn))
                {
                    return Domain.Constant.HokenSyu.Jibai;
                }
                else
                {
                    return Domain.Constant.HokenSyu.Jihi;
                }
            }
        }

        public int HokenPid
        {
            //get { return PtHokenPattern.HokenPid; }
            get { return _hokenPid; }
        }

        public int HokenId
        {
            get { return _hokenId; }
        }

        public int HokenSbt
        {
            get { return _hokenSbt; }
        }
        //-------------------------------------------------------------------------
        /// <summary>
        /// オーダー行為区分
        /// </summary>
        public int OdrKouiKbn
        {
            //get { return OdrInf.OdrKouiKbn; }
            get { return _odrKouiKbn; }
            set { _odrKouiKbn = value; }
        }

        /// <summary>
        /// 算定区分
        /// 1: 算定外 2: 自費算定
        /// </summary>
        public int SanteiKbn
        {
            //get { return OdrInf.SanteiKbn; }
            get { return _santeiKbn; }
        }

        /// <summary>
        /// 院内院外区分
        ///     0: 院内
        ///     1: 院外
        /// </summary>
        public int InoutKbn
        {
            //get { return OdrInf.InoutKbn; }
            get { return _inoutKbn; }
        }

        /// <summary>
        /// 処方種別
        ///     0: 日数判断
        ///     1: 臨時
        ///     2: 常態
        /// </summary>
        public int SyohoSbt
        {
            //get { return OdrInf.SyohoSbt; }
            get { return _syohoSbt; }
        }

        /// <summary>
        /// 日数回数
        ///     処方日数
        /// </summary>
        public int DaysCnt
        {
            //get { return OdrInf.DaysCnt; }
            get { return _daysCnt; }
        }

        /// <summary>
        /// オーダーの並び順
        /// </summary>
        public int SortNo
        {
            get { return _sortNo; }
            set { _sortNo = value; }
        }

        /// <summary>
        /// 加算１
        /// </summary>
        public int Kasan1
        {
            //get { return IpnKasanMst?.Kasan1 ?? 0; }            
            get { return _kasan1; }
        }
        /// <summary>
        /// 加算２
        /// </summary>
        public int Kasan2
        {
            //get { return IpnKasanMst?.Kasan2 ?? 0; }            
            get { return _kasan2; }
        }

        /// <summary>
        /// 最低薬価
        /// </summary>
        public double MinYakka
        {
            get { return _minYakka; }
        }

        ///// <summary>
        ///// 一般名処方区分
        ///// 　1: 後発品のない先発品
        ///// 　2: 後発品のある先発品
        ///// 　★:後発品のある先発品のうち、加算対象外の薬剤
        ///// 　3: 後発品
        ///// 　☆:後発品のうち、加算対象外の薬剤
        ///// 　NULL:その他
        ///// </summary>
        //public string Kbn
        //{
        //    get { return _kbn; }
        //}

        ///// <summary>
        ///// 準先発品等区分
        /////   1: 一般名処方加算の対象となる準先発品等
        ///// </summary>
        //public int JunSenpatu
        //{
        //    get { return _junSenpatu; }
        //}

        /// <summary>
        /// 診療開始時間
        /// </summary>
        public string SinStartTime
        {
            //get { return RaiinInf.SinStartTime; }
            get { return _sinStartTime; }
        }

        /// <summary>
        /// 酸素項目かどうか
        ///     true - 酸素項目
        /// </summary>
        public bool IsSanso
        {
            get
            {
                bool ret = false;

                if (TenMst != null)
                {
                    ret = TenMst.SansoKbn >= 2 && TenMst.SansoKbn <= 5;
                }
                return ret;
            }
        }

        /// <summary>
        /// 数量をレセ単位に換算した値
        /// </summary>
        public double CalcedSuryo
        {
            get
            {
                double ret = 0;

                ret = (Suryo != 0 ? Suryo : 1);

                if (string.IsNullOrEmpty(OdrUnitName) && IsKihonKoumoku)
                {
                    // 基本項目で単位がない場合は1固定
                    ret = 1;
                }
                else
                {
                    if (TermVal > 0)
                    {
                        ret = ret * TermVal;
                        //Math.Round(ret * TermVal, 3, MidpointRounding.AwayFromZero);
                    }

                    ret = Math.Round(ret, 3, MidpointRounding.AwayFromZero);

                    if (TenMst != null)
                    {
                        switch (TenMst.SuryoRoundupKbn)
                        {
                            case 1:
                                // 注射のみ
                                if (OdrKouiKbn >= OdrKouiKbnConst.ChusyaMin && OdrKouiKbn <= OdrKouiKbnConst.ChusyaMax)
                                {
                                    ret = Math.Ceiling(ret);
                                }
                                break;
                            case 2:
                                // すべて
                                ret = Math.Ceiling(ret);
                                break;
                            case 3:
                                // 注射以外
                                if (OdrKouiKbn < OdrKouiKbnConst.ChusyaMin || OdrKouiKbn > OdrKouiKbnConst.ChusyaMax)
                                {
                                    ret = Math.Ceiling(ret);
                                }
                                break;
                        }
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// false:点単位の項目 true:円単位の項目
        /// </summary>
        public bool IsEnKoumoku
        {
            get
            {
                bool ret = false;

                if (MasterSbt != "Y" && MasterSbt != "T" && (ItemCd.Length == 9 || ItemCd.StartsWith("R")) && BuiKbn == 0)
                {
                    if (new int[] { 1, 2, 4, 10, 11 }.Contains(TenId))
                    {
                        ret = true;
                    }
                }

                return ret;
            }
        }

        public bool IsRosaiEnKoumoku
        {
            get
            {
                bool ret = false;

                if (new int[] { 99 }.Contains(TenId))
                {
                    ret = true;
                }

                return ret;
            }
        }

        ///// <summary>
        ///// 一般名処方加算対象区分
        ///// （区分が2 or 3、または、準先発品区分が1
        /////   true: 一般名処方加算の対象
        ///// </summary>
        //public bool CanIpnSyoho
        //{
        //    get
        //    {
        //        //return new string[] { "2", "3" }.Contains(_kbn) || _junSenpatu == 1;
        //        return _canIpnSyoho;
        //    }
        //}

        /// <summary>
        /// リハビリ項目の場合、trueを返す
        /// </summary>
        public bool IsRihabiri
        {
            get
            {
                bool ret = false;
                if (ItemCd.StartsWith("Z") == false && CdKbn == "H" &&
                    (Kokuji1 == "1" || Kokuji1 == "3") &&
                    (
                        (CdKbnno == 0 && CdEdano == 0) ||
                        (CdKbnno == 1 && CdEdano == 0) ||
                        (CdKbnno == 1 && CdEdano == 2) ||
                        (CdKbnno == 2 && CdEdano == 0) ||
                        (CdKbnno == 3 && CdEdano == 0)))
                {
                    ret = true;
                }
                return ret;
            }
        }

        /// <summary>
        /// 在医総・施医総の項目かどうかを判定する
        /// </summary>
        public bool IsZaiiso
        {
            get
            {
                bool ret = false;

                if (CdKbn == "C" && CdKbnno == 2 && Kokuji2 == "1")
                {
                    ret = true;
                }

                return ret;
            }
        }
        /// <summary>
        /// 在医総管（在支診等以外）（１００分の８０）減算
        /// </summary>
        public bool IsZaiisoFutakiTarget
        {
            get
            {
                bool ret = false;

                if (TyuCd == "1420")
                {
                    ret = true;
                }

                return ret;
            }
        }
        /// <summary>
        /// 人工腎臓の項目かどうかを判定する
        /// </summary>
        public bool IsJinkoJinzo
        {
            get
            {
                bool ret = false;

                if (CdKbn == "J" && CdKbnno == 38 && new int[] { 1, 2, 3 }.Contains(CdKouno) && Kokuji1 == "1" && Kokuji2 == "1")
                {
                    ret = true;
                }

                return ret;
            }
        }
        /// <summary>
        /// 訪問診療料かどうか
        /// </summary>
        public bool IsHoumonSinryo
        {
            get
            {
                bool ret = false;
                if (Kokuji2 == "1" && CdKbn == "C")
                {
                    if ((CdKbnno == 1 && CdEdano == 0 && CdKouno == 1) ||
                        (CdKbnno == 1 && CdEdano == 0 && CdKouno == 2) ||
                        (CdKbnno == 1 && CdEdano == 2 && CdKouno == 0))
                    {
                        ret = true;
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// 訪問看護指導料かどうか
        /// </summary>
        public bool IsHoumonKango
        {
            get
            {
                bool ret = false;
                if (Kokuji2 == "1" && CdKbn == "C")
                {
                    if (CdKbnno == 5 && CdEdano == 0)
                    {
                        ret = true;
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 同一建物居住者訪問看護・指導料かどうか
        /// </summary>
        public bool IsDoituHoumonKango
        {
            get
            {
                bool ret = false;
                if (Kokuji2 == "1" && CdKbn == "C")
                {
                    if (CdKbnno == 5 && CdEdano == 1)
                    {
                        ret = true;
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 在宅患者訪問リハビリテーション指導管理料
        /// </summary>
        public bool IsHoumonRiha
        {
            get
            {
                bool ret = false;
                if (Kokuji2 == "1" && CdKbn == "C")
                {
                    if (CdKbnno == 6 && CdEdano == 0)
                    {
                        ret = true;
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 在宅患者訪問薬剤管理指導料
        /// </summary>
        public bool IsHoumonYakuzai
        {
            get
            {
                bool ret = false;
                if (Kokuji2 == "1" && CdKbn == "C")
                {
                    if (CdKbnno == 8 && CdEdano == 0)
                    {
                        ret = true;
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 在宅患者訪問栄養食事指導料
        /// </summary>
        public bool IsHoumonEiyo
        {
            get
            {
                bool ret = false;
                if (Kokuji2 == "1" && CdKbn == "C")
                {
                    if (CdKbnno == 9 && CdEdano == 0)
                    {
                        ret = true;
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 精訪看
        /// </summary>
        public bool IsSeiHouKan
        {
            get
            {
                bool ret = false;
                if (Kokuji2 == "1" && CdKbn == "I")
                {
                    if (CdKbnno == 12 && CdEdano == 0)
                    {
                        ret = true;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 医科外来等感染症対策実施加算（在宅医療）の対象項目かどうか
        /// </summary>
        public bool IsKansenTaisakuTargetIgaku
        {
            get
            {
                return new List<string>
                    {
                        // 外来リハビリテーション診療料１
                        ItemCdConst.IgakuGairaiRiha1,
                        // 外来リハビリテーション診療料２
                        ItemCdConst.IgakuGairaiRiha2,
                        // 外来放射線照射診療料
                        ItemCdConst.IgakuGairaiHosya,
                        // 救急救命管理料
                        ItemCdConst.IgakuKyukyuKyumei,
                        // 退院後訪問指導料
                        ItemCdConst.IgakuKTaiinHoumon,
                    }.Contains(ItemCd);
            }
        }
        /// <summary>
        /// 医科外来等感染症対策実施加算（在宅医療）の対象項目かどうか
        /// </summary>
        public bool IsKansenTaisakuTargetZaitaku
        {
            get
            {
                return (IsHoumonSinryo ||
                        IsHoumonKango ||
                        IsDoituHoumonKango ||
                        IsHoumonRiha ||
                        IsHoumonYakuzai ||
                        IsHoumonEiyo ||
                        new string[] {
                            ItemCdConst.ZaiHoumonTenteki,
                            ItemCdConst.ZaiConference

                        }.Contains(ItemCd));
            }
        }
        /// <summary>
        /// 医科外来等感染症対策実施加算（精神科訪問看護・指導料）の対象項目かどうか
        /// </summary>
        public bool IsKansenTaisakuTargetSeisin
        {
            get
            {
                return IsSeiHouKan;
            }
        }

        /// <summary>
        /// 外来感染対策向上加算等（医学管理）の対象項目かどうか
        /// </summary>
        public bool IsKansenKojoTargetIgaku
        {
            get
            {
                bool ret = false;
                if (new List<string> { "1", "3", "5"}.Contains(Kokuji2))
                {
                    //基本項目
                    if (GairaiKansen ==  1)
                    {
                        //医学管理の加算対象
                        ret = true;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 外来感染対策向上加算等（在宅）の対象項目かどうか
        /// </summary>
        public bool IsKansenKojoTargetZaitaku
        {
            get
            {
                bool ret = false;
                if (new List<string> { "1", "3", "5" }.Contains(Kokuji2))
                {
                    //基本項目
                    if (GairaiKansen == 2)
                    {
                        //在宅の加算対象
                        ret = true;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 外来感染対策向上加算（精神科訪問看護・指導料）の対象項目かどうか
        /// </summary>
        public bool IsKansenKojoTargetSeisin
        {
            get
            {
                return IsSeiHouKan;
            }
        }

        /// <summary>
        /// 算定漏れチェック用項目の場合、true
        /// </summary>
        public bool IsDummy { get; set; } = false;

        /// <summary>
        /// Z特材用 マスター種別
        /// </summary>
        public string Z_MasterSbt { get; set; } = string.Empty;
        /// <summary>
        /// Z特材用 点数識別
        /// </summary>
        public int Z_TenId { get; set; } = 0;
    }
}
