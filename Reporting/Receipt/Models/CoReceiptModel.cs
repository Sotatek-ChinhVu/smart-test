using Entity.Tenant;
using Helper.Common;
using Helper.Constants;

namespace Reporting.Receipt.Models;

public class CoReceiptModel
{
    private ReceInfModel _receInfModel;
    private PtInfModel _ptInfModel;
    private HokenDataModel _hokenDataModel;
    private List<KohiDataModel> _kohiDataModels;
    private List<KohiDataModel> _kohiDataModelsAll;
    private List<SyobyoDataModel> _syobyoDataModels;
    private List<SinMeiDataModel> _sinMeiDataModels;
    private PtKyuseiModel _ptKyuseiModel;
    private RousaiReceiptModel _rousaiReceiptModel;
    private SyobyoKeikaModel _syobyoKeikaModel;
    private HpInfModel _hpInfModel;

    private List<int> _tuuinDays;

    private CoReceiptTensuModel _coReceiptTensuModel;

    private int _receiptNo;
    private int _futanKbn;

    private int _recordYm;
    private int _sinDate;
    private int _kensaDate;
    private int _zenkaiKensaDate;

    private int _afterSyokei;
    private int _afterSyokeiGaku_I;
    private int _afterSyokeiGaku_RO;

    /// <summary>
    /// レセプト情報
    /// </summary>
    /// <param name="receInf">レセプト情報</param>
    /// <param name="ptInfModel">患者基本情報</param>
    /// <param name="hokenDataModel">保険情報</param>
    /// <param name="kohiDataModels">公費情報</param>
    /// <param name="syobyoDataModels">傷病情報</param>
    /// <param name="sinMeiDataModels">診療明細情報</param>
    /// <param name="syojyoSyokiModels">症状詳記</param>
    /// <param name="ptKyuseiModel">旧姓情報</param>
    /// <param name="rousaiReceiptModel">労災レセプト情報（労災のみ）</param>
    /// <param name="syobyoKeikaModel">傷病の経過（労災のみ）</param>
    /// <param name="recedenRirekiInfModel">返戻履歴情報</param>
    public CoReceiptModel(
        ReceInfModel receInf, PtInfModel ptInfModel, HokenDataModel hokenDataModel, List<KohiDataModel> kohiDataModels, List<KohiDataModel> kohiDataModelsAll,
        List<SyobyoDataModel> syobyoDataModels, List<SinMeiDataModel> sinMeiDataModels,
        PtKyuseiModel ptKyuseiModel, RousaiReceiptModel rousaiReceiptModel, SyobyoKeikaModel syobyoKeikaModel, HpInfModel hpInfModel, CoReceiptTensuModel coReceitpTensuModel)
    {
        _receInfModel = receInf;
        _ptInfModel = ptInfModel;
        _hokenDataModel = hokenDataModel;
        _kohiDataModels = kohiDataModels;
        _kohiDataModelsAll = kohiDataModelsAll;
        _syobyoDataModels = syobyoDataModels;
        _sinMeiDataModels = sinMeiDataModels;
        _ptKyuseiModel = ptKyuseiModel;
        _rousaiReceiptModel = rousaiReceiptModel;
        _syobyoKeikaModel = syobyoKeikaModel;
        _hpInfModel = hpInfModel;
        _coReceiptTensuModel = coReceitpTensuModel;
    }

    /// <summary>
    /// レセプト番号
    /// </summary>
    public int ReceiptNo
    {
        get { return _receiptNo; }
        set { _receiptNo = value; }
    }
    /// <summary>
    /// レセプト種別
    /// </summary>
    public string ReceiptSbt
    {
        get { return _receInfModel.ReceSbt ?? string.Empty; }
    }
    public int GetReceiptSbt(int index)
    {
        int ret = 0;

        if (index > 0 && ReceiptSbt.Length >= index)
        {
            ret = CIUtil.StrToIntDef(ReceiptSbt.Substring(index - 1, 1), 0);
        }

        return ret;
    }
    /// <summary>
    /// 診療年月
    /// </summary>
    public int SinYm
    {
        get { return _receInfModel.SinYm; }
    }
    /// <summary>
    /// 請求年月
    /// </summary>
    public int SeikyuYm
    {
        get { return _receInfModel.SeikyuYm; }
    }
    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum
    {
        get { return _ptInfModel.PtInf.PtNum; }
    }

    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId
    {
        get { return _receInfModel.PtId; }
    }
    public long HokenId
    {
        get { return _receInfModel.HokenId; }
    }
    public long HokenId2
    {
        get { return _receInfModel.HokenId2; }
    }
    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName
    {
        get
        {
            string ret = _ptInfModel.Name ?? string.Empty;
            if (_ptKyuseiModel != null)
            {
                ret = _ptKyuseiModel.Name;
            }

            return ret.Replace(" ", "　");
        }
    }
    /// <summary>
    /// 性別
    /// </summary>
    public int Sex
    {
        get { return _ptInfModel.Sex; }
    }
    /// <summary>
    /// 生年月日
    /// </summary>
    public int BirthDay
    {
        get { return _ptInfModel.Birthday; }
    }
    /// <summary>
    /// 給付割合
    /// </summary>
    public int KyufuRate
    {
        get { return 100 - _receInfModel.HokenRate; }
    }
    /// <summary>
    /// 負担区分
    /// </summary>
    public int FutanKbn
    {
        get { return _futanKbn; }
        set { _futanKbn = value; }
    }
    /// <summary>
    /// 特記事項
    /// </summary>
    public string Tokki
    {
        get { return _receInfModel.Tokki ?? string.Empty; }
    }
    public List<string> TokkiJiko
    {
        get
        {
            List<string> ret = new List<string>();
            int index = 0;

            for (int i = 1; i <= 5; i++)
            {
                string tokki = Tokkijiko(i);
                if (tokki != "")
                {
                    ret.Add(tokki);
                    index++;
                }
            }

            // 残りを埋めて、絶対に要素数が5になるようにする
            for (int i = index; i <= 4; i++)
            {
                ret.Add("");
            }
            return ret;
        }
    }

    public string Tokkijiko(int index)
    {
        string ret = "";

        switch (index)
        {
            case 1:
                ret = _receInfModel.Tokki1 ?? "";
                break;
            case 2:
                ret = _receInfModel.Tokki2 ?? "";
                break;
            case 3:
                ret = _receInfModel.Tokki3 ?? "";
                break;
            case 4:
                ret = _receInfModel.Tokki4 ?? "";
                break;
            case 5:
                ret = _receInfModel.Tokki5 ?? "";
                break;
        }

        return ret;
    }

    /// <summary>
    /// 一部負担金・食事療養費・生活療養費標準負担額区分
    /// </summary>
    public int IchibuKbn
    {
        get
        {
            int ret = 0;

            if (_receInfModel.KogakuOverKbn == 1)
            {
                switch (_receInfModel.KogakuKbn)
                {
                    case 4:
                        ret = 1;
                        break;
                    case 5:
                        ret = 3;
                        break;
                }
            }

            return ret;
        }
    }
    /// <summary>
    /// カルテID
    /// </summary>
    public long KarteId
    {
        get { return _receInfModel.PtId; }
    }
    /// <summary>
    /// 患者カタカナ氏名
    /// </summary>
    public string PtKanaName
    {
        get
        {
            string ret = _ptInfModel.KanaName ?? string.Empty;

            if (_ptKyuseiModel != null)
            {
                ret = _ptKyuseiModel.KanaName;
            }
            return ret;
        }
    }

    /// <summary>
    /// 患者の状態
    /// </summary>
    public string PtStatus
    {
        get { return _receInfModel.PtStatus ?? string.Empty; }
    }

    /// <summary>
    /// 傷病の経過
    /// </summary>
    public string SyobyoKeika
    {
        get
        {
            string ret = "";
            if (_syobyoKeikaModel != null)
            {
                ret = _syobyoKeikaModel.Keika;
            }

            return ret;
        }
    }

    /// <summary>
    /// 労働局コード
    /// </summary>
    public string RoudouCd
    {
        get
        {
            return _receInfModel.RoudoukyokuCd;
        }
    }
    /// <summary>
    /// 監督署コード
    /// </summary>
    public string KantokuCd
    {
        get
        {
            return _receInfModel.KantokusyoCd;
        }
    }
    /// <summary>
    /// 円点レート
    /// </summary>
    public int EnTen
    {
        get
        {
            return _receInfModel.EnTen;
        }
    }

    public int ReceKisai(int index)
    {
        int ret = 0;

        switch (index)
        {
            case 1:
                ret = _receInfModel.Kohi1ReceKisai;
                break;
            case 2:
                ret = _receInfModel.Kohi2ReceKisai;
                break;
            case 3:
                ret = _receInfModel.Kohi3ReceKisai;
                break;
            case 4:
                ret = _receInfModel.Kohi4ReceKisai;
                break;
        }

        return ret;
    }

    public int KohiCount
    {
        get { return _kohiDataModels.Count(); }
    }
    /// <summary>
    /// KOHI_DATAから法別番号を取得する
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string KohiHoubetu(int index)
    {
        string ret = "";
        if (index >= 1 && index <= _kohiDataModels.Count)
        {
            ret = _kohiDataModels[index - 1].Houbetu;
        }

        return ret;
    }
    public string KohiHoubetuAll(int index)
    {
        string ret = "";
        if (index >= 1 && index <= _kohiDataModelsAll.Count)
        {
            ret = _kohiDataModelsAll[index - 1].Houbetu;
        }

        return ret;
    }
    /// <summary>
    /// RECE_INFの法別番号を取得する
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string KohiHoubetuReceInf(int index)
    {
        string ret = "";

        switch (index)
        {
            case 1:
                ret = _receInfModel.Kohi1Houbetu;
                break;
            case 2:
                ret = _receInfModel.Kohi2Houbetu;
                break;
            case 3:
                ret = _receInfModel.Kohi3Houbetu;
                break;
            case 4:
                ret = _receInfModel.Kohi4Houbetu;
                break;
        }

        return ret;
    }

    public ReceStatus ReceStatus
    {
        get { return _receInfModel.ReceStatus; }
    }

    /// <summary>
    /// 出力フラグ
    /// </summary>
    public int Output
    {
        get { return _receInfModel.Output; }
        set { _receInfModel.Output = value; }
    }
    public DateTime ReceStatusCreateDate
    {
        get { return _receInfModel.ReceStatusCreateDate; }
        set { _receInfModel.ReceStatusCreateDate = value; }
    }
    public int ReceStatusCreateId
    {
        get { return _receInfModel.ReceStatusCreateId; }
        set { _receInfModel.ReceStatusCreateId = value; }
    }
    public string ReceStatusCreateMachine
    {
        get { return _receInfModel.ReceStatusCreateMachine; }
        set { _receInfModel.ReceStatusCreateMachine = value; }
    }
    public DateTime ReceStatusUpdateDate
    {
        get { return _receInfModel.ReceStatusUpdateDate; }
        set { _receInfModel.ReceStatusUpdateDate = value; }
    }
    public int ReceStatusUpdateId
    {
        get { return _receInfModel.ReceStatusUpdateId; }
        set { _receInfModel.ReceStatusUpdateId = value; }
    }
    public string ReceStatusUpdateMachine
    {
        get { return _receInfModel.ReceStatusUpdateMachine; }
        set { _receInfModel.ReceStatusUpdateMachine = value; }
    }

    public bool ReceStatusAddNew
    {
        get
        {
            return _receInfModel.ReceStatusAddNew;
        }
    }

    /// <summary>
    /// 保険区分
    ///     0:自費
    ///     1:社保
    ///     2:国保
    ///     11:労災(短期給付)
    ///     12:労災(傷病年金)
    ///     13:アフターケア
    ///     14:自賠責
    /// </summary>
    public int HokenKbn
    {
        get { return _receInfModel.HokenKbn; }
    }
    /// <summary>
    /// 都道府県番号
    /// </summary>
    public int PrefNo
    {
        get { return _hpInfModel.PrefNo; }
    }
    /// <summary>
    /// 医療機関コード
    /// </summary>
    public string HpCd
    {
        get { return _hpInfModel.HpCd; }
    }
    /// <summary>
    /// 労災医療機関コード
    /// </summary>
    public string RousaiHpCd
    {
        get { return _hpInfModel.RousaiHpCd; }
    }
    /// <summary>
    /// 医療機関住所
    /// </summary>
    public string HpAddress
    {
        get { return _hpInfModel.Address1 + _hpInfModel.Address2; }
    }
    /// <summary>
    /// 医療機関名称
    /// </summary>
    public string HpName
    {
        get { return _hpInfModel.HpName; }
    }
    /// <summary>
    /// レセ医療機関名   
    /// </summary>
    public string ReceHpName
    {
        get { return _hpInfModel.ReceHpName; }
    }
    /// <summary>
    /// 医療機関電話番号
    /// </summary>
    public string HpTel
    {
        get { return _hpInfModel.Tel; }
    }
    /// <summary>
    /// 保険者番号
    /// </summary>
    public string HokensyaNo
    {
        get { return _hokenDataModel == null ? "" : _hokenDataModel.HokensyaNo; }
    }
    /// <summary>
    /// 記号
    /// </summary>
    public string Kigo
    {
        get { return _hokenDataModel == null ? "" : _hokenDataModel.Kigo; }
    }
    /// <summary>
    /// 番号
    /// </summary>
    public string Bango
    {
        get { return _hokenDataModel == null ? "" : _hokenDataModel.Bango; }
    }
    /// <summary>
    /// 枝番
    /// </summary>
    public string EdaNo
    {
        get { return _hokenDataModel == null ? "" : _hokenDataModel.EdaNo; }
    }
    /// <summary>
    /// 保険負担率
    /// </summary>
    public int HokenRate
    {
        get { return _receInfModel.HokenRate; }
    }
    /// <summary>
    /// 公費負担者番号
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string KohiFutansyaNo(int index)
    {
        string ret = "";

        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].FutansyaNo;
        }

        return ret;
    }

    /// <summary>
    /// 公費受給者番号
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string KohiJyukyusyaNo(int index)
    {
        string ret = "";

        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].JyukyusyaNo;
        }

        return ret;
    }
    /// <summary>
    /// 公費特殊番号
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string KohiTokusyuNo(int index)
    {
        string ret = "";

        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].TokusyuNo;
        }

        return ret;
    }
    /// <summary>
    /// 公費負担率
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int KohiRate(int index)
    {
        int ret = 0;

        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].Rate;
        }

        return ret;
    }
    /// <summary>
    /// 職務上の事由
    ///     1:職務上 
    ///     2:下船後３月以内 
    ///     3:通勤災害
    /// </summary>
    public int SyokumuKbn
    {
        get { return _hokenDataModel == null ? 0 : _hokenDataModel.SyokumuKbn; }
    }

    /// <summary>
    /// 減免区分
    ///     1:減額 
    ///     2:免除 
    ///     3:支払猶予 
    ///     4:自立支援減免
    /// </summary>
    public int GenmenKbn
    {
        get { return _receInfModel.GenmenKbn; }
    }

    /// <summary>
    /// 減免額
    /// </summary>
    public int GenmenGaku
    {
        get { return _hokenDataModel == null ? 0 : _hokenDataModel.GenmenGaku; }
    }

    /// <summary>
    /// 減免率
    /// </summary>
    public int GenmenRate
    {
        get { return _hokenDataModel == null ? 0 : _hokenDataModel.GenmenRate; }
    }
    /// <summary>
    /// 保険実日数
    /// </summary>
    public int? HokenNissu
    {
        get { return _receInfModel.HokenNissu; }
    }
    /// <summary>
    /// 公費実日数
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int? KohiNissu(int index)
    {
        int? ret = null;

        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].JituNissu;
        }

        //switch (index)
        //{
        //    case 1:
        //        ret = _receInfModel.Kohi1Nissu;
        //        break;
        //    case 2:
        //        ret = _receInfModel.Kohi2Nissu;
        //        break;
        //    case 3:
        //        ret = _receInfModel.Kohi3Nissu;
        //        break;
        //    case 4:
        //        ret = _receInfModel.Kohi4Nissu;
        //        break;
        //}
        return ret;
    }
    /// <summary>
    /// 総点数
    /// </summary>
    public int Tensu
    {
        get => _receInfModel.Tensu;
    }
    /// <summary>
    /// 保険点数
    /// </summary>
    public int? HokenReceTensu
    {
        get { return _receInfModel.HokenReceTensu; }
    }
    /// <summary>
    /// 公費レセ点数
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int? KohiReceTensu(int index)
    {
        int? ret = null;

        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].ReceTen;
        }

        //switch (index)
        //{
        //    case 1:
        //        ret = _receInfModel.Kohi1ReceTensu;
        //        break;
        //    case 2:
        //        ret = _receInfModel.Kohi2ReceTensu;
        //        break;
        //    case 3:
        //        ret = _receInfModel.Kohi3ReceTensu;
        //        break;
        //    case 4:
        //        ret = _receInfModel.Kohi4ReceTensu;
        //        break;
        //}
        return ret;
    }

    /// <summary>
    /// 公費点数
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int? KohiTensu(int index)
    {
        int? ret = null;

        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].Tensu;
        }

        return ret;
    }
    /// <summary>
    /// 公費点数
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int? KohiTensuReceInf(int index)
    {
        int? ret = null;

        switch (index)
        {
            case 1:
                ret = _receInfModel.Kohi1Tensu;
                break;
            case 2:
                ret = _receInfModel.Kohi2Tensu;
                break;
            case 3:
                ret = _receInfModel.Kohi3Tensu;
                break;
            case 4:
                ret = _receInfModel.Kohi4Tensu;
                break;
        }
        return ret;
    }
    /// <summary>
    /// 保険一部負担
    /// </summary>
    public int? HokenReceFutan
    {
        get { return _receInfModel.HokenReceFutan; }
    }
    /// <summary>
    /// 公費一部負担
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int? KohiReceFutan(int index)
    {
        int? ret = null;
        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].ReceFutan;
        }
        //switch (index)
        //{
        //    case 1:
        //        ret = _receInfModel.Kohi1ReceFutan;
        //        break;
        //    case 2:
        //        ret = _receInfModel.Kohi2ReceFutan;
        //        break;
        //    case 3:
        //        ret = _receInfModel.Kohi3ReceFutan;
        //        break;
        //    case 4:
        //        ret = _receInfModel.Kohi4ReceFutan;
        //        break;
        //}
        return ret;
    }
    public int? KohiReceFutanReceInf(int index)
    {
        int? ret = null;
        switch (index)
        {
            case 1:
                ret = _receInfModel.Kohi1ReceFutan;
                break;
            case 2:
                ret = _receInfModel.Kohi2ReceFutan;
                break;
            case 3:
                ret = _receInfModel.Kohi3ReceFutan;
                break;
            case 4:
                ret = _receInfModel.Kohi4ReceFutan;
                break;
        }
        return ret;
    }
    /// <summary>
    /// 公費給付額
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int? KohiKyufu(int index)
    {
        int? ret = null;
        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].ReceKyufu;
        }
        //switch (index)
        //{
        //    case 1:
        //        ret = _receInfModel.Kohi1ReceKyufu;
        //        break;
        //    case 2:
        //        ret = _receInfModel.Kohi2ReceKyufu;
        //        break;
        //    case 3:
        //        ret = _receInfModel.Kohi3ReceKyufu;
        //        break;
        //    case 4:
        //        ret = _receInfModel.Kohi4ReceKyufu;
        //        break;
        //}
        return ret;
    }
    /// <summary>
    /// 本人家族区分
    ///     0:本人
    ///     1:家族
    /// </summary>
    public int HonkeKbn
    {
        get { return _hokenDataModel == null ? 0 : _hokenDataModel.HonkeKbn; }
    }
    /// <summary>
    /// 点数欄点数
    /// </summary>
    /// <param name="syukeiSaki">集計先</param>
    /// <returns></returns>
    public double TenColTensu(string syukeiSaki)
    {
        double ret = _coReceiptTensuModel.Tensu(syukeiSaki);

        return ret;
    }
    public double TenColTensu(List<string> syukeiSakis)
    {
        double ret = _coReceiptTensuModel.Tensu(syukeiSakis);

        return ret;
    }
    public double TenColTensuSum(List<string> syukeiSakis)
    {
        double ret = _coReceiptTensuModel.TensuSum(syukeiSakis);

        return ret;
    }
    /// <summary>
    /// 点数欄回数
    /// </summary>
    /// <param name="syukeiSaki">集計先</param>
    /// <returns></returns>
    public int TenColCount(string syukeiSaki, bool onlySI)
    {
        int ret = _coReceiptTensuModel.TenColCount(syukeiSaki, onlySI);

        return ret;
    }
    public int TenColCount(List<string> syukeiSakis, bool onlySI)
    {
        int ret = _coReceiptTensuModel.TenColCount(syukeiSakis, onlySI);

        return ret;
    }
    /// <summary>
    /// 点数欄合計点数
    /// </summary>
    /// <param name="syukeiSaki">集計先</param>
    /// <returns></returns>
    public double TenColTotalTen(string syukeiSaki)
    {
        double ret = _coReceiptTensuModel.TotalTen(syukeiSaki);

        return ret;
    }
    public double TenColTotalTen(List<string> syukeiSakis)
    {
        double ret = _coReceiptTensuModel.TotalTen(syukeiSakis);

        return ret;
    }
    /// <summary>
    /// 点数欄金額合計
    /// </summary>
    /// <param name="syukeiSakis">集計先</param>
    /// <returns></returns>
    public double TenColTotalKingaku(List<string> syukeiSakis)
    {
        double ret = _coReceiptTensuModel.TotalKingaku(syukeiSakis);

        return ret;
    }
    /// <summary>
    /// 点数欄点数公費分
    /// </summary>
    /// <param name="syukeiSakis"></param>
    /// <param name="kohiIndex"></param>
    /// <returns></returns>
    public double TenColTensuKohi(List<string> syukeiSakis, int kohiIndex)
    {
        double ret = _coReceiptTensuModel.TensuKohi(syukeiSakis, kohiIndex);

        return ret;
    }
    /// <summary>
    /// 点数欄回数公費分
    /// </summary>
    /// <param name="syukeiSakis"></param>
    /// <param name="kohiIndex"></param>
    /// <returns></returns>
    public int TenColCountKohi(List<string> syukeiSakis, int kohiIndex)
    {
        int ret = _coReceiptTensuModel.TenColCountKohi(syukeiSakis, kohiIndex);

        return ret;
    }
    /// <summary>
    /// 点数欄合計公費分
    /// </summary>
    /// <param name="syukeiSakis"></param>
    /// <param name="kohiIndex"></param>
    /// <returns></returns>
    public double TenColTotalTenKohi(List<string> syukeiSakis, int kohiIndex)
    {
        double ret = _coReceiptTensuModel.TotalTenKohi(syukeiSakis, kohiIndex);

        return ret;
    }
    public List<(int count, double kingaku)> TenColKingakuSonota(string syukeiSaki)
    {
        return _coReceiptTensuModel.TenColKingakuSonota(syukeiSaki);
    }

    public List<string> TenColFutanKbns
    {
        get
        {
            return _coReceiptTensuModel.FutanKbns;
        }
    }

    /// <summary>
    /// 傷病データ
    /// </summary>
    public List<SyobyoDataModel> SyobyoData
    {
        get { return _syobyoDataModels; }
    }

    /// <summary>
    /// 診療明細データ
    /// </summary>
    public List<SinMeiDataModel> SinMeiData
    {
        get { return _sinMeiDataModels; }
    }

    /// <summary>
    /// 診療明細の数を返す
    /// </summary>
    public int SinMeiCount
    {
        get
        {
            return _sinMeiDataModels.Count;
        }
    }

    /// <summary>
    /// 指定の負担区分に関わる公費のインデックスを返す
    /// </summary>
    /// <param name="futanKbn"></param>
    /// <returns></returns>
    public List<int> FutanKbnToKohiIndex(string futanKbn)
    {
        return _coReceiptTensuModel.FutanKbnToKohiIndex(futanKbn);
    }
    /// <summary>
    /// 公費の保険ID
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int KohiHokenId(int index)
    {
        int ret = 0;

        if (index >= 1 && index <= _kohiDataModels.Count)
        {
            ret = _kohiDataModels[index - 1].HokenId;
        }

        return ret;
    }

    public int KohiHokenIdAll(int index)
    {
        int ret = 0;

        if (index >= 1 && index <= _kohiDataModelsAll.Count)
        {
            ret = _kohiDataModelsAll[index - 1].HokenId;
        }

        return ret;
    }

    public int KohiHokenNo(int index)
    {
        int ret = 0;

        if (index >= 1 && index <= _kohiDataModels.Count)
        {
            ret = _kohiDataModels[index - 1].HokenNo;
        }

        return ret;

    }
    public int KohiHokenEdaNo(int index)
    {
        int ret = 0;

        if (index >= 1 && index <= _kohiDataModels.Count)
        {
            ret = _kohiDataModels[index - 1].HokenEdaNo;
        }

        return ret;

    }

    public int KohiPrefNo(int index)
    {
        int ret = 0;

        if (index >= 1 && index <= _kohiDataModels.Count)
        {
            ret = _kohiDataModels[index - 1].PrefNo;
        }

        return ret;

    }

    /// <summary>
    /// 労災請求回数
    /// </summary>
    public int RousaiReceCount
    {
        get { return _receInfModel.RousaiCount; }
    }

    /// <summary>
    /// 労災新継再別
    /// </summary>
    public int RousaiSinkei
    {
        get => _rousaiReceiptModel.Sinkei;
    }
    /// <summary>
    /// 労災転帰
    /// </summary>
    public int RousaiTenki
    {
        get => _rousaiReceiptModel.Tenki;
    }
    /// <summary>
    /// 労災交付番号
    /// </summary>
    public string RousaiKofu
    {
        get => _hokenDataModel.RousaiKofu;
    }
    /// <summary>
    /// 傷病開始日
    /// </summary>
    public int RousaiSyobyoDate
    {
        get => _hokenDataModel.RousaiSyobyoDate;
    }

    /// <summary>
    /// 療養開始日
    /// </summary>
    public int RyoyoStartDate
    {
        get
        {
            int ret = 0;
            ret = _rousaiReceiptModel == null ? 0 : _rousaiReceiptModel.RyoyoStartDate;
            return ret;
        }
    }

    /// <summary>
    /// 療養終了日
    /// </summary>
    public int RyoyoEndDate
    {
        get
        {
            int ret = 0;
            ret = _rousaiReceiptModel == null ? 0 : _rousaiReceiptModel.RyoyoEndDate;
            return ret;
        }
    }
    /// <summary>
    /// 労災実日数
    /// </summary>
    public int? RousaiJituNissu
    {
        get
        {
            int? ret = 0;
            ret = _rousaiReceiptModel == null ? 0 : _rousaiReceiptModel.JituNissu;
            return ret;
        }
    }

    /// <summary>
    /// 労災合計金額
    /// </summary>
    public int RousaiTotal
    {
        get => _rousaiReceiptModel == null ? 0 : _rousaiReceiptModel.Gokei;
    }

    /// <summary>
    /// 事業所名
    /// </summary>
    public string JigyosyoName
    {
        get => _rousaiReceiptModel == null ? "" : _rousaiReceiptModel.JigyosyoName;
    }
    /// <summary>
    /// 事業所住所（都道府県）
    /// </summary>
    public string RousaiPrefName
    {
        get => _rousaiReceiptModel == null ? "" : _rousaiReceiptModel.RousaiPrefName;
    }

    /// <summary>
    /// 事業所住所（市町村）
    /// </summary>
    public string RousaiCityName
    {
        get => _rousaiReceiptModel == null ? "" : _rousaiReceiptModel.RousaiCityName;
    }
    /// <summary>
    /// 労災小計
    /// </summary>
    public int? RousaiSyokei
    {
        get => _rousaiReceiptModel == null ? 0 : _rousaiReceiptModel.Syokei;
    }
    /// <summary>
    /// 労災イ
    /// </summary>
    public int? RousaiSyokeiGaku_I
    {
        get => _rousaiReceiptModel == null ? 0 : _rousaiReceiptModel.SyokeiGaku_I;
    }
    /// <summary>
    /// 労災ロ
    /// </summary>
    public int? RousaiSyokeiGaku_RO
    {
        get => _rousaiReceiptModel == null ? 0 : _rousaiReceiptModel.SyokeiGaku_RO;
    }
    /// <summary>
    /// アフターケア合計額
    /// </summary>
    public int AfterTotal
    {
        get => AfterSyokeiGaku_I + AfterSyokeiGaku_RO;
    }
    /// <summary>
    /// アフターケア小計額
    /// </summary>
    public int AfterSyokei
    {
        get => _afterSyokei;
        set { _afterSyokei = value; }
    }
    /// <summary>
    /// アフターケア小計額イ
    /// </summary>
    public int AfterSyokeiGaku_I
    {
        get => _afterSyokeiGaku_I;
        set { _afterSyokeiGaku_I = value; }
    }
    /// <summary>
    /// アフターケア小計額ロ
    /// </summary>
    public int AfterSyokeiGaku_RO
    {
        get => _afterSyokeiGaku_RO;
        set { _afterSyokeiGaku_RO = value; }
    }
    /// <summary>
    /// 診療日（アフターケア用）
    /// </summary>
    public int SinDate
    {
        get => _sinDate;
        set { _sinDate = value; }
    }
    /// <summary>
    /// 検査日（アフターケア用）
    /// </summary>
    public int KensaDate
    {
        get => _kensaDate;
        set { _kensaDate = value; }
    }
    /// <summary>
    /// 前回検査日（アフターケア用）
    /// </summary>
    public int ZenkaiKensaDate
    {
        get => _zenkaiKensaDate;
        set { _zenkaiKensaDate = value; }
    }
    /// <summary>
    /// 傷病コード
    /// </summary>
    public string SyobyoCd
    {
        get => _rousaiReceiptModel.SyobyoCd;
    }
    /// <summary>
    /// 自賠受傷日
    /// </summary>
    public int JibaiJyusyouDate
    {
        get => _receInfModel.JibaiJyusyouDate;
    }
    /// <summary>
    /// 自賠イ点数
    /// </summary>
    public int JibaiITensu
    {
        get => _receInfModel.JibaiITensu;
    }
    /// <summary>
    /// 自賠ロ点数
    /// </summary>
    public int JibaiRoTensu
    {
        get => _receInfModel.JibaiRoTensu;
    }
    /// <summary>
    /// 自賠ハ負担
    /// </summary>
    public int JibaiHaFutan
    {
        get => _receInfModel.JibaiHaFutan;
    }
    /// <summary>
    /// 自賠ニ負担
    /// </summary>
    public int JibaiNiFutan
    {
        get => _receInfModel.JibaiNiFutan;
    }
    /// <summary>
    /// 自賠ホ診断
    /// </summary>
    public int JibaiHoSindan
    {
        get => _receInfModel.JibaiHoSindan;
    }
    /// <summary>
    /// 自賠ヘ明細
    /// </summary>
    public int JibaiHeMeisai
    {
        get => _receInfModel.JibaiHeMeisai;
    }

    /// <summary>
    /// 自賠A
    /// </summary>
    public int JibaiAFutan
    {
        get => _receInfModel.JibaiAFutan;
    }
    /// <summary>
    /// 自賠B
    /// </summary>
    public int JibaiBFutan
    {
        get => _receInfModel.JibaiBFutan;
    }
    /// <summary>
    /// 自賠C
    /// </summary>
    public int JibaiCFutan
    {
        get => _receInfModel.JibaiCFutan;
    }
    /// <summary>
    /// 自賠D
    /// </summary>
    public int JibaiDFutan
    {
        get => _receInfModel.JibaiDFutan;
    }
    /// <summary>
    /// 自賠ABCD負担合計
    /// </summary>
    public int JibaiABCDFutan
    {
        get => _receInfModel.JibaiAFutan + _receInfModel.JibaiBFutan + _receInfModel.JibaiCFutan + _receInfModel.JibaiDFutan;
    }
    /// <summary>
    /// 通院日（自賠責用）
    /// </summary>
    public List<int> TuuinDays
    {
        get => _tuuinDays;
        set { _tuuinDays = value; }
    }
    /// <summary>
    /// 自賠保険会社名
    /// </summary>
    public string JibaiHokenName
    {
        get => _receInfModel.JibaiHokenName;
    }
    /// <summary>
    /// 開設者名
    /// </summary>
    public string KaisetuName
    {
        get => _hpInfModel == null ? "" : _hpInfModel.KaisetuName;
    }
    /// <summary>
    /// 自賠責初診日
    /// </summary>
    public int JibaiSyosinDate
    {
        get => _receInfModel.JibaiSyosinDate;
    }
    /// <summary>
    /// 自賠責診断書枚数
    /// </summary>
    public int JibaiHoSindanCount
    {
        get => _receInfModel.JibaiHoSindanCount;
    }
    /// <summary>
    /// 自賠責明細書枚数
    /// </summary>
    public int JibaiHeMeisaiCount
    {
        get => _receInfModel.JibaiHeMeisaiCount;
    }
    /// <summary>
    /// 自賠責健保準拠点数
    /// </summary>
    public int JibaiKenpoTensu
    {
        get => _receInfModel.JibaiKenpoTensu;
    }
    /// <summary>
    /// 自賠責健保準拠負担金額
    /// </summary>
    public int JibaiKenpoFutan
    {
        get => _receInfModel.JibaiKenpoFutan;
    }
    /// <summary>
    /// 請求区分
    /// 1:月遅れ 2:返戻 3:オンライン返戻
    /// </summary>
    public int SeikyuKbn
    {
        get => _receInfModel.SeikyuKbn;
    }
    /// <summary>
    /// 総医療費
    /// </summary>
    public int TotalIryoHi
    {
        get => _receInfModel.TotalIryohi;
    }
    /// <summary>
    /// 保険負担額
    /// </summary>
    public int HokenFutan
    {
        get => _receInfModel.HokenFutan;
    }
    /// <summary>
    /// 保険負担額10円単位
    /// </summary>
    public int HokenFutan10en
    {
        get => _receInfModel.HokenFutan10en;
    }
    /// <summary>
    /// 高額負担額
    /// </summary>
    public int KogakuFutan
    {
        get => _receInfModel.KogakuFutan;
    }
    /// <summary>
    /// 高額負担額10円単位
    /// </summary>
    public int KogakuFutan10en
    {
        get => _receInfModel.KogakuFutan10en;
    }
    /// <summary>
    /// 患者負担額
    /// </summary>
    public int PtFutan
    {
        get => _receInfModel.PtFutan;
    }
    /// <summary>
    /// 公費負担額
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int? KohiFutan(int index)
    {
        int? ret = null;
        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].Futan;
        }

        return ret;
    }
    public int? KohiFutanReceInf(int index)
    {
        int? ret = null;
        switch (index)
        {
            case 1:
                ret = _receInfModel.Kohi1Futan;
                break;
            case 2:
                ret = _receInfModel.Kohi2Futan;
                break;
            case 3:
                ret = _receInfModel.Kohi3Futan;
                break;
            case 4:
                ret = _receInfModel.Kohi4Futan;
                break;
        }
        return ret;
    }
    /// <summary>
    /// 公費負担額10円単位
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int? KohiFutan10en(int index)
    {
        int? ret = null;
        if (index >= 1 && index <= _kohiDataModels.Count())
        {
            ret = _kohiDataModels[index - 1].Futan10en;
        }

        return ret;
    }
    public int? KohiFutan10enReceInf(int index)
    {
        int? ret = null;

        switch (index)
        {
            case 1:
                ret = _receInfModel.Kohi1Futan10en;
                break;
            case 2:
                ret = _receInfModel.Kohi2Futan10en;
                break;
            case 3:
                ret = _receInfModel.Kohi3Futan10en;
                break;
            case 4:
                ret = _receInfModel.Kohi4Futan10en;
                break;
        }

        return ret;
    }

    /// <summary>
    /// 長野レセプト２枚目ソート用キー情報
    /// </summary>
    /// <returns></returns>
    public string NaganoRece2SortKey()
    {
        string ret = "";

        List<KohiDataModel> _kohiDatas = _kohiDataModels.FindAll(p => p.Houbetu == "99");

        if (_kohiDataModels.Any())
        {
            ret = CIUtil.Copy(_kohiDataModels.First().TokusyuNo, 1, 3);
        }

        return ret;
    }

    public string SyosinJikangai
    {
        get { return _coReceiptTensuModel.SyosinJikangai; }
    }

    public string KogakuKbnMessage
    {
        get
        {
            string ret = "";

            if (_receInfModel != null &&
                _receInfModel.KogakuOverKbn > 0 &&
                new int[] { 4, 5 }.Contains(_receInfModel.KogakuKbn) &&
                HospitalInfo.Instance.PrefCD == 25)
            {
                if (_receInfModel.KogakuKbn == 4)
                {
                    ret = "低所得Ⅰ";
                }
                else if (_receInfModel.KogakuKbn == 5)
                {
                    ret = "低所得Ⅱ";
                }
            }

            return ret;
        }
    }

    public bool IsStudent
    {
        get { return _ptInfModel.IsStudent; }
    }

    public bool IsElder
    {
        get { return _ptInfModel.IsElder; }
    }

}
