using Helper.Common;
using Helper.Constants;
using Reporting.CommonMasters.Enums;
using System.Linq;

namespace Reporting.OutDrug.Model;

public class CoOutDrugPrintData
{
    private readonly int _sinDate;
    private readonly CoPtInfModel _ptInf;
    private readonly CoPtHokenInfModel _ptHoken;
    private readonly List<CoPtKohiModel> _ptKohi;
    private readonly CoHpInfModel _hpInf;
    private readonly CoRaiinInfModel _raiinInf;

    public CoOutDrugPrintData(
        OutDrugPrintOutType printType, int sinDate, CoPtInfModel ptInf, CoPtHokenInfModel ptHoken, List<CoPtKohiModel> ptKohis,
        CoHpInfModel hpInf, CoRaiinInfModel raiinInf, int bunkatuMax, int bunkatuKai, int refillCount)
    {
        _sinDate = sinDate;
        _ptInf = ptInf;
        _ptHoken = ptHoken;
        _ptKohi = ptKohis;
        _hpInf = hpInf;
        _raiinInf = raiinInf;

        PrintType = printType;
        BunkatuMax = bunkatuMax;
        BunkatuKai = bunkatuKai;
        RefillCount = refillCount;

        RpInfs = new List<CoOutDrugPrintDataRpInf>();

        Biko = new();
    }
    /// <summary>
    /// 帳票タイプ
    /// 0-処方箋
    /// 1-分割続紙
    /// </summary>
    public OutDrugPrintOutType PrintType { get; set; }

    /// <summary>
    /// 印刷データ識別情報（患者ID_診療日_来院番号_保険ID
    /// </summary>
    public string PrintDataID
    {
        get
        {
            string ret;

            ret = $"{_ptInf.PtId}_{_sinDate}_{_raiinInf.RaiinNo}_{(_ptHoken == null ? 0 : _ptHoken.HokenId)}";

            return ret;
        }
    }
    /// <summary>
    /// 最大分割数
    /// </summary>
    public int BunkatuMax { get; set; }
    /// <summary>
    /// 分割回
    /// </summary>
    public int BunkatuKai { get; set; }
    /// <summary>
    /// リフィル回数
    /// </summary>
    public int RefillCount { get; set; } = 0;

    /// <summary>
    /// 患者カナ氏名
    /// </summary>
    public string PtKanaName
    {
        get { return _ptInf.KanaName; }
    }
    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName
    {
        get { return _ptInf.Name; }
    }
    /// <summary>
    /// 生年月日
    /// </summary>
    public string Birthday
    {
        get { return CIUtil.SDateToShowWDate3(_ptInf.Birthday).Ymd; }
    }
    /// <summary>
    /// 年齢
    /// </summary>
    public int Age
    {
        get { return CIUtil.SDateToAge(_ptInf.Birthday, _sinDate); }
    }
    /// <summary>
    /// 性別
    /// </summary>
    public int Sex
    {
        get { return _ptInf.Sex; }
    }

    /// <summary>
    /// 保険者番号
    /// </summary>
    public string HokensyaNo
    {
        get { return _ptHoken == null ? "" : _ptHoken.HokensyaNo; }
    }

    /// <summary>
    /// 枝番
    /// </summary>
    public string EdaNo
    {
        get
        {
            string ret = "";

            if (_ptHoken != null && !string.IsNullOrEmpty(_ptHoken.EdaNo))
            {
                ret = _ptHoken.EdaNo.PadLeft(2, '0');
            }
            return ret;
        }
    }
    /// <summary>
    /// 記号番号
    /// </summary>
    public string KigoBango
    {
        get
        {
            string ret = "";

            if (_ptHoken != null)
            {
                if (new int[] { 0, 1, 2 }.Contains(_ptHoken.HokenKbn))
                {
                    // 健保の場合
                    ret = _ptHoken.Kigo;
                    if (_ptHoken.Bango != "")
                    {
                        if (ret != "") ret += "・";
                        ret += _ptHoken.Bango;
                    }
                }
                else if (new int[] { 11, 12, 13 }.Contains(_ptHoken.HokenKbn))
                {
                    // 労災の場合、労働保険番号 or 年金証書番号 or 健康管理手帳番号
                    ret = _ptHoken.RousaiKofuNo;
                }
                else if (new int[] { 14 }.Contains(_ptHoken.HokenKbn))
                {
                    // 自賠の場合、保険会社名
                    ret = _ptHoken.JibaiHokenName;
                }
            }

            return ret;
        }
    }
    /// <summary>
    /// 負担率(%)
    /// </summary>
    public int? FutanRate
    {
        get
        {
            int? ret = 100; // PT_HOKEN=null && PT_KOHI=null の場合、自費算定の処理と考える

            if (_ptHoken != null)
            {
                if (new int[] { 1, 2 }.Contains(_ptHoken.HokenKbn))
                {

                    ret = GetHokenRate(_ptHoken.Rate, _ptHoken.HokenSbtKbn, _ptHoken.KogakuKbn, _ptHoken.Houbetu);

                }
                else if (_ptHoken.HokenKbn == 0)
                {
                    // 自費
                    ret = _ptHoken.Rate;
                }
                else
                {
                    // 労災・自賠
                    ret = _ptHoken.Rate;
                }
            }

            if (_ptKohi != null)
            {
                for (int i = 0; i < _ptKohi.Count; i++)
                {
                    if (_ptKohi[i].HokenMst.FutanKbn == 0)
                    {
                        ret = 0;
                    }
                    else if (_ptKohi[i].FutanRate > 0 && (ret == null || ret > _ptKohi[i].FutanRate))
                    {
                        ret = _ptKohi[i].FutanRate;
                    }
                }
            }

            return ret;
        }
    }

    /// <summary>
    /// 主保険負担率計算
    /// </summary>
    /// <param name="futanRate">負担率</param>
    /// <param name="hokenSbtKbn">保険種別区分</param>
    /// <param name="kogakuKbn">高額療養費区分</param>
    /// <param name="honkeKbn">本人家族区分</param>
    /// <param name="houbetu">法別番号</param>
    /// <param name="receSbt">レセプト種別</param>
    /// <returns></returns>
    public int GetHokenRate(int futanRate, int hokenSbtKbn, int kogakuKbn, string houbetu)
    {
        int wrkRate = futanRate;

        switch (hokenSbtKbn)
        {
            case 0:
                //主保険なし
                break;
            case 1:
                //主保険
                if (_ptInf.IsPreSchool())
                {
                    //６歳未満未就学児
                    wrkRate = 20;
                }
                else if (_ptInf.IsElder() && houbetu != "39")
                {
                    wrkRate = 10;
                    if (_ptInf.IsElder20per() || _ptInf.IsElderExpat())
                    {
                        wrkRate = 20;
                    }
                }

                if (_ptInf.IsElder() || houbetu == "39")
                {
                    if ((kogakuKbn == 3 && _sinDate < KaiseiDate.d20180801) ||
                        (new int[] { 26, 27, 28 }.Contains(kogakuKbn) && _sinDate >= KaiseiDate.d20180801))
                    {
                        //後期７割 or 高齢７割
                        wrkRate = 30;
                    }
                    else if (houbetu == "39" && kogakuKbn == 41 &&
                        _sinDate >= KaiseiDate.d20221001)
                    {
                        //後期８割
                        wrkRate = 20;
                    }
                }
                break;
            default:
                break;
        }

        return wrkRate;
    }

    /// <summary>
    /// 本人家族区分
    /// 1: 本人
    /// 2: 家族
    /// </summary>
    public int HonKeKbn
    {
        get { return _ptHoken == null ? 0 : _ptHoken.HonkeKbn; }
    }

    /// <summary>
    /// 公費情報取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public CoPtKohiModel? PtKohi(int index)
    {
        CoPtKohiModel? ret = null;

        if (_ptKohi != null && index >= 0 && index < _ptKohi.Count)
        {
            ret = _ptKohi[index];
        }
        return ret;
    }

    // 公費の数
    public int KohiCount
    {
        get { return _ptKohi == null ? 0 : _ptKohi.Count; }
    }

    /// <summary>
    /// 公費負担者番号
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string KohiFutansyaNo(int index)
    {
        string ret = "";

        CoPtKohiModel? kohi = PtKohi(index);
        if (kohi != null)
        {
            ret = kohi.FutansyaNo;
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

        CoPtKohiModel? kohi = PtKohi(index);
        if (kohi != null)
        {
            ret = kohi.JyukyusyaNo;
        }

        return ret;
    }
    /// <summary>
    /// 公費交付番号
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string KohiKofuNo(int index)
    {
        string ret = "";

        CoPtKohiModel? kohi = PtKohi(index);
        if (kohi != null)
        {
            ret = kohi.TokusyuNo;
        }

        return ret;
    }

    /// <summary>
    /// 公費１負担者番号
    /// </summary>
    public string Ko1FutansyaNo
    {
        get { return KohiFutansyaNo(0); }
    }
    /// <summary>
    /// 公費１受給者番号
    /// </summary>
    public string Ko1JyukyusyaNo
    {
        get { return KohiJyukyusyaNo(0); }
    }
    /// <summary>
    /// 公費１交付番号
    /// </summary>
    public string Ko1KofuNo
    {
        get { return KohiKofuNo(0); }
    }
    /// <summary>
    /// 公費２負担者番号
    /// </summary>
    public string Ko2FutansyaNo
    {
        get { return KohiFutansyaNo(1); }
    }
    /// <summary>
    /// 公費２受給者番号
    /// </summary>
    public string Ko2JyukyusyaNo
    {
        get { return KohiJyukyusyaNo(1); }
    }
    /// <summary>
    /// 公費２交付番号
    /// </summary>
    public string Ko2KofuNo
    {
        get { return KohiKofuNo(1); }
    }
    /// <summary>
    /// 公費３負担者番号
    /// </summary>
    public string Ko3FutansyaNo
    {
        get { return KohiFutansyaNo(2); }
    }
    /// <summary>
    /// 公費３受給者番号
    /// </summary>
    public string Ko3JyukyusyaNo
    {
        get { return KohiJyukyusyaNo(2); }
    }
    /// <summary>
    /// 公費３交付番号
    /// </summary>
    public string Ko3KofuNo
    {
        get { return KohiKofuNo(2); }
    }
    /// <summary>
    /// 公費４負担者番号
    /// </summary>
    public string Ko4FutansyaNo
    {
        get { return KohiFutansyaNo(3); }
    }
    /// <summary>
    /// 公費４受給者番号
    /// </summary>
    public string Ko4JyukyusyaNo
    {
        get { return KohiJyukyusyaNo(3); }
    }
    /// <summary>
    /// 公費４交付番号
    /// </summary>
    public string Ko4KofuNo
    {
        get { return KohiKofuNo(3); }
    }
    /// <summary>
    /// 交付日
    /// </summary>
    public string KofuDate
    {
        get { return CIUtil.SDateToShowWDate3(_sinDate).Ymd; }
    }
    /// <summary>
    /// 医療機関住所
    /// </summary>
    public string HpAddress
    {
        get { return _hpInf.Address1 + _hpInf.Address2; }
    }
    /// <summary>
    /// 医療機関名
    /// </summary>
    public string HpName
    {
        get { return _hpInf.HpName; }
    }
    /// <summary>
    /// 医療機関電話番号
    /// </summary>
    public string HpTel
    {
        get { return _hpInf.Tel; }
    }
    /// <summary>
    /// 医療機関FAX番号
    /// </summary>
    public string HpFaxNo
    {
        get { return _hpInf.FaxNo; }
    }
    /// <summary>
    /// 医療機関その他連絡先
    /// </summary>
    public string HpOtherContacts
    {
        get { return _hpInf.OtherContacts; }
    }
    /// <summary>
    /// 保険医師名
    /// </summary>
    public string DoctorName
    {
        get { return _raiinInf.TantoName; }
    }
    /// <summary>
    /// 受付順番
    /// </summary>
    public int UketukeNo
    {
        get { return _raiinInf.UketukeNo; }
    }
    /// <summary>
    /// 都道府県番号
    /// </summary>
    public int PrefNo
    {
        get { return _hpInf.PrefNo; }
    }
    /// <summary>
    /// 医療機関コード
    /// </summary>
    public string HpCd
    {
        get { return $"{_hpInf.HpCd.PadLeft(7, '0')}"; }
    }
    /// <summary>
    /// 労災医療機関コード
    /// </summary>
    public string RousaiHpCd
    {
        get { return $"{_hpInf.RousaiHpCd.PadLeft(7, '0')}"; }
    }
    /// <summary>
    /// 患者ＩＤ
    /// </summary>
    public long PtId
    {
        get { return _ptInf.PtId; }
    }
    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum
    {
        get { return _ptInf.PtNum; }
    }
    /// <summary>
    /// 診療日
    /// </summary>
    public int SinDate
    {
        get { return _sinDate; }
    }
    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo
    {
        get { return _raiinInf.RaiinNo; }
    }

    /// <summary>
    /// 備考
    /// </summary>
    public List<string> Biko { get; set; }
    /// <summary>
    /// 残薬確認疑義照会
    /// </summary>
    public bool ZanyakuGigi { get; set; }
    /// <summary>
    /// 残薬確認情報提供
    /// </summary>
    public bool ZanyakuTeikyo { get; set; }

    /// <summary>
    /// 変更不可の薬剤がある場合、trueを返す
    /// </summary>
    public bool HenkoFuka
    {
        get
        {
            bool ret = false;
            foreach (var _ in RpInfs.Where(rpInf => rpInf.DrugInfs.Any(p => p.HenkouMark == "×")).Select(rpInf => new { }))
            {
                ret = true;
            }

            return ret;
        }
    }
    public List<CoOutDrugPrintDataRpInf> RpInfs { get; set; }
}

/// <summary>
/// Rp情報管理クラス
/// </summary>
public class CoOutDrugPrintDataRpInf
{

    public CoOutDrugPrintDataRpInf(int rpNo, int kohiFutan)
    {
        RpNo = rpNo;
        KohiFutan = kohiFutan;

        DrugInfs = new List<CoOutDrugPrintDataDrugInf>();
    }
    /// <summary>
    /// Rp番号
    /// </summary>
    public int RpNo { get; set; }
    /// <summary>
    /// 公費負担区分
    /// </summary>
    public int KohiFutan { get; set; }
    /// <summary>
    /// 薬剤情報追加
    /// </summary>
    /// <param name="drugInf"></param>
    public void AddDrugInf(CoOutDrugPrintDataDrugInf drugInf)
    {
        DrugInfs.Add(drugInf);
    }
    public void AddDrugInf(int itemType, string henkouMark, string data, double suryo, string unitName)
    {
        DrugInfs.Add(new CoOutDrugPrintDataDrugInf(itemType, henkouMark, data, suryo, unitName));
    }

    public List<CoOutDrugPrintDataDrugInf> DrugInfs { get; set; }
}

/// <summary>
/// 薬剤情報管理クラス
/// </summary>
public class CoOutDrugPrintDataDrugInf
{
    public CoOutDrugPrintDataDrugInf(int itemType, string henkouMark, string data, double suryo, string unitName)
    {
        ItemType = itemType;
        HenkouMark = henkouMark;
        Data = data;
        Suryo = suryo;
        UnitName = unitName;
    }

    /// <summary>
    /// 項目タイプ
    /// 0:薬剤/特材
    /// 1:用法
    /// 2:補助用法
    /// 9:コメント
    /// 10:分割のときの総量コメント
    /// </summary>
    public int ItemType { get; set; }
    /// <summary>
    /// 変更不可
    /// </summary>
    public string HenkouMark { get; set; }
    /// <summary>
    /// 薬剤名、コメント等
    /// </summary>
    public string Data { get; set; }
    /// <summary>
    /// 数量
    /// </summary>
    public double Suryo { get; set; }
    /// <summary>
    /// 単位名称
    /// </summary>
    public string UnitName { get; set; }

}
