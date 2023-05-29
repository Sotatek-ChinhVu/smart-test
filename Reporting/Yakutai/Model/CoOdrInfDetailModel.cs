﻿using Entity.Tenant;
using Reporting.CommonMasters.Config;

namespace Reporting.Yakutai.Model
{
    public class CoOdrInfDetailModel
    {
        public  ISystemConfig _systemConfig;
        public OdrInfDetail OdrInfDetail { get; } = null;
        public OdrInf OdrInf { get; } = null;
        public TenMst TenMst { get; } = null;
        public YohoInfMst YohoInfMst { get; } = null;

        public CoOdrInfDetailModel(OdrInfDetail odrInfDetail, OdrInf odrInf, TenMst tenMst, YohoInfMst yohoInfMst, ISystemConfig systemConfig)
        {
            OdrInfDetail = odrInfDetail;
            OdrInf = odrInf;
            TenMst = tenMst;

            FukuyoRise = (TenMst != null ? TenMst.FukuyoRise : 0);
            FukuyoMorning = (TenMst != null ? TenMst.FukuyoMorning : 0);
            FukuyoDaytime = (TenMst != null ? TenMst.FukuyoDaytime : 0);
            FukuyoNight = (TenMst != null ? TenMst.FukuyoNight : 0);
            FukuyoSleep = (TenMst != null ? TenMst.FukuyoSleep : 0);

            YohoInfMst = yohoInfMst;
            _systemConfig = systemConfig;
        }

        /// <summary>
        /// オーダー情報詳細
        /// </summary>
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
            get { return OdrInfDetail.RpNo; }
        }

        /// <summary>
        /// 剤枝番
        ///     ODR_INF.RP_EDA_NO
        /// </summary>
        public long RpEdaNo
        {
            get { return OdrInfDetail.RpEdaNo; }
        }

        /// <summary>
        /// 行番号
        /// </summary>
        public int RowNo
        {
            get { return OdrInfDetail.RowNo; }
        }

        /// <summary>
        /// 診療行為区分
        /// </summary>
        public int SinKouiKbn
        {
            get { return OdrInfDetail.SinKouiKbn; }
        }

        /// <summary>
        /// 項目コード
        /// </summary>
        public string ItemCd
        {
            get { return OdrInfDetail.ItemCd ?? ""; }
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
            get { return OdrInfDetail.Suryo; }
        }
        /// <summary>
        /// 数量（換算単位を使用する設定で換算単位の設定がある薬剤の場合は換算した数量を返す）
        /// </summary>
        public double SuryoDsp
        {
            get
            {
                if (IsCnvSuryo)
                {
                    return CnvSuryo;
                }
                else
                {
                    return Suryo;
                }
            }
        }
        /// <summary>
        /// 単位名称
        /// </summary>
        public string UnitName
        {
            get { return OdrInfDetail.UnitName ?? ""; }
        }
        /// <summary>
        /// 単位名称（換算単位を使用する設定で換算単位の設定がある場合は換算単位を返す）
        /// </summary>
        public string UnitNameDsp
        {
            get
            {
                string ret = UnitName ?? "";

                if (IsCnvSuryo)
                {
                    ret = CnvUnitName;
                }

                return ret;
            }
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
        /// 薬剤区分
        ///        当該医薬品の薬剤区分を表す。
        ///             0: 薬剤以外
        ///             1: 内用薬
        ///             3: その他
        ///             4: 注射薬
        ///             6: 外用薬
        ///             8: 歯科用薬剤
        /// </summary>
        public int DrugKbn
        {
            get { return OdrInfDetail.DrugKbn; }
        }

        /// <summary>
        /// 用法区分
        ///          0: 用法以外
        ///          1: 基本用法
        ///          2: 補助用法
        /// </summary>
        public int YohoKbn
        {
            get { return OdrInfDetail.YohoKbn; }
        }

        /// <summary>
        /// 告示等識別区分（１）
        ///        当該診療行為についてコンピューター運用上の取扱い（磁気媒体に記録する際の取扱い）を表す。
        ///          1: 基本項目（告示）　※基本項目
        ///          3: 合成項目　　　　　※基本項目
        ///          5: 準用項目（通知）　※基本項目
        ///          7: 加算項目　　　　　※加算項目
        ///          9: 通則加算項目　　　※加算項目
        ///          0: 診療行為以外（薬剤、特材等）
        /// </summary>
        public string Kokuji1
        {
            get { return OdrInfDetail.Kokuji1; }
        }

        /// <summary>
        /// 告示等識別区分（２）
        ///        当該診療行為について点数表上の取扱いを表す。
        ///           1: 基本項目（告示）
        ///           3: 合成項目
        ///     （削）5: 準用項目（通知）
        ///          7: 加算項目（告示）
        ///       削）9: 通則加算項目
        ///           0: 診療行為以外（薬剤、特材等）
        /// </summary>
        public string Kokiji2
        {
            get { return OdrInfDetail.Kokiji2; }
        }

        /// <summary>
        /// レセ非表示区分
        ///          0: 表示
        ///          1: 非表示
        /// </summary>
        public int IsNodspRece
        {
            get { return OdrInfDetail.IsNodspRece; }
        }

        /// <summary>
        /// 一般名コード
        /// </summary>
        public string IpnCd
        {
            get { return OdrInfDetail.IpnCd; }
        }

        /// <summary>
        /// 一般名
        /// </summary>
        public string IpnName
        {
            get { return OdrInfDetail.IpnName; }
        }

        /// <summary>
        /// 実施区分
        ///          0: 未実施
        ///          1: 実施
        /// </summary>
        public int JissiKbn
        {
            get { return OdrInfDetail.JissiKbn; }
        }

        /// <summary>
        /// 実施日時
        /// </summary>
        public DateTime? JissiDate
        {
            get { return OdrInfDetail.JissiDate; }
        }

        /// <summary>
        /// 実施者
        /// </summary>
        public int JissiId
        {
            get { return OdrInfDetail.JissiId; }
        }

        /// <summary>
        /// 実施端末
        /// </summary>
        public string JissiMachine
        {
            get { return OdrInfDetail.JissiMachine; }
        }

        /// <summary>
        /// 検査依頼コード
        /// </summary>
        public string ReqCd
        {
            get { return OdrInfDetail.ReqCd; }
        }

        /// <summary>
        /// 分割調剤
        ///        7日単位の3分割の場合 "7+7+7"
        /// </summary>
        public string Bunkatu
        {
            get { return OdrInfDetail.Bunkatu; }
        }

        /// <summary>
        /// コメントマスターの名称
        ///        ※当該項目がコメント項目の場合に使用
        /// </summary>
        public string CmtName
        {
            get { return OdrInfDetail.CmtName; }
        }

        /// <summary>
        /// コメント文
        ///        コメントマスターの定型文に組み合わせる文字情報
        ///        ※当該項目がコメント項目の場合に使用
        /// </summary>
        public string CmtOpt
        {
            get { return OdrInfDetail.CmtOpt; }
        }

        /// <summary>
        /// 文字色
        /// </summary>
        public string FontColor
        {
            get { return OdrInfDetail.FontColor; }
        }
        /// <summary>
        /// 服用時設定-起床時
        ///     0: 服用しない
        ///     >1: 服用する
        /// </summary>
        public int FukuyoRise { get; set; } = 0;
        /// <summary>
        /// 服用時設定-朝
        ///     0: 服用しない
        ///     >1: 服用する
        /// </summary>
        public int FukuyoMorning { get; set; } = 0;
        /// <summary>
        /// 服用時設定-昼
        ///     0: 服用しない
        ///     >1: 服用する
        /// </summary>
        public int FukuyoDaytime { get; set; } = 0;
        /// <summary>
        /// 服用時設定-夜
        ///     0: 服用しない
        ///     >1: 服用する
        /// </summary>
        public int FukuyoNight { get; set; } = 0;
        /// <summary>
        /// 服用時設定-眠前
        ///     0: 服用しない
        ///     >1: 服用する
        /// </summary>
        public int FukuyoSleep { get; set; } = 0;
        /// <summary>
        /// 薬袋表示
        ///     0: 薬袋に表示する
        ///     1: 薬袋に表示しない
        /// </summary>
        public int IsNodspYakutai
        {
            get { return (TenMst != null ? TenMst.IsNodspYakutai : 0); }
        }
        /// <summary>
        /// 剤形ポイント
        /// 薬袋フォーム切替条件判定の際、数量を換算するのに使用
        /// </summary>
        public double ZaikeiPoint
        {
            get { return (TenMst != null ? TenMst.ZaikeiPoint : 0); }
        }
        /// <summary>
        /// オーダー数量を換算単位に変換した数量
        /// ただし、換算単位の設定がない場合は、オーダー数量
        /// </summary>
        public double CnvSuryo
        {
            get
            {
                double ret = Suryo;

                if (TenMst != null)
                {
                    if (string.IsNullOrEmpty(TenMst.CnvUnitName) == false && UnitName == TenMst.OdrUnitName)
                    {
                        ret =
                            Suryo *
                            (TenMst.OdrTermVal > 0 ? TenMst.OdrTermVal : 1) /
                            (TenMst.CnvTermVal > 0 ? TenMst.CnvTermVal : 1);
                    }
                }

                return ret;
            }
        }
        /// <summary>
        /// 数量換算する場合、trueを返す
        /// </summary>
        /// <returns></returns>
        private bool IsCnvSuryo
        {
            get
            {
                bool ret = false;

                if (_systemConfig.YakutaiTaniDsp() == 1&&
                    TenMst != null &&
                    TenMst.YohoKbn == 0 &&
                    string.IsNullOrEmpty(TenMst.CnvUnitName) == false &&
                    TenMst.CnvTermVal > 0 &&
                    UnitName == TenMst.OdrUnitName)
                {
                    ret = true;
                }

                return ret;
            }
        }
        /// <summary>
        /// 数量換算単位名称
        ///     薬剤情報提供書の全数量に換算した値を表示する場合の当該医薬品の換算単位名称を表す。
        /// </summary>
        public string CnvUnitName
        {
            get { return (TenMst != null ? (TenMst.CnvUnitName ?? "") : ""); }
        }
        /// <summary>
        /// 服用量(数量 * 剤型ポイント)　薬袋用紙の判別に使用する
        public double Fukuyoryo
        {
            get
            {
                double ret = SuryoDsp;

                if (ZaikeiPoint > 0)
                {
                    ret = ret * ZaikeiPoint;
                }

                return ret;
            }
        }

        public string YohoSuffix
        {
            get { return YohoInfMst != null ? YohoInfMst.YohoSuffix : ""; }
        }
        public bool Delete { get; set; } = false;
        public bool IsIppoYoho { get; set; } = false;
    }
}
