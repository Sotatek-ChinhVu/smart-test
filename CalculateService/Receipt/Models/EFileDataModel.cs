using CalculateService.Ika.Models;
using CalculateService.Receipt.DB.Finder;
using Helper.Constants;

namespace CalculateService.Receipt.Models;

public class EFileDataModel
{
    private const string ModuleName = ModuleNameConst.EmrCalculateEFFile;

    private ReceInfModel _receInfModel;
    private Ika.Models.PtInfModel _ptInfModel;
    private HokenDataModel _hokenDataModel;
    private List<KohiDataModel> _kohiDataModels;
    private List<SyobyoDataModel> _syobyoDataModels;
    private List<SinMeiDataModel> _sinMeiDataModels;

    private List<EDataModel> _eDataModels;

    private ReceMasterFinder _receFinder;

    /// <summary>
    /// Eファイル情報
    /// </summary>
    /// <param name="receInf">レセプト情報</param>
    /// <param name="ptInfModel">患者基本情報</param>
    /// <param name="hokenDataModel">保険情報</param>
    /// <param name="kohiDataModels">公費情報</param>
    /// <param name="syobyoDataModels">傷病情報</param>
    /// <param name="sinMeiDataModels">診療明細情報</param>
    public EFileDataModel(
        string siseteuCd,
        ReceInfModel receInf, Ika.Models.PtInfModel ptInfModel, HokenDataModel hokenDataModel, List<KohiDataModel> kohiDataModels,
        List<SyobyoDataModel> syobyoDataModels, List<SinMeiDataModel> sinMeiDataModels, List<EFRaiinInfModel> raiinInfs, ReceMasterFinder receFinder)
    {

        #region 'local method'

        int _getDayCount(SinMeiDataModel mei, int day)
        {
            int ret = 0;

            if (mei != null)
            {
                switch (day)
                {
                    case 1: ret = mei.Day1; break;
                    case 2: ret = mei.Day2; break;
                    case 3: ret = mei.Day3; break;
                    case 4: ret = mei.Day4; break;
                    case 5: ret = mei.Day5; break;
                    case 6: ret = mei.Day6; break;
                    case 7: ret = mei.Day7; break;
                    case 8: ret = mei.Day8; break;
                    case 9: ret = mei.Day9; break;
                    case 10: ret = mei.Day10; break;
                    case 11: ret = mei.Day11; break;
                    case 12: ret = mei.Day12; break;
                    case 13: ret = mei.Day13; break;
                    case 14: ret = mei.Day14; break;
                    case 15: ret = mei.Day15; break;
                    case 16: ret = mei.Day16; break;
                    case 17: ret = mei.Day17; break;
                    case 18: ret = mei.Day18; break;
                    case 19: ret = mei.Day19; break;
                    case 20: ret = mei.Day20; break;
                    case 21: ret = mei.Day21; break;
                    case 22: ret = mei.Day22; break;
                    case 23: ret = mei.Day23; break;
                    case 24: ret = mei.Day24; break;
                    case 25: ret = mei.Day25; break;
                    case 26: ret = mei.Day26; break;
                    case 27: ret = mei.Day27; break;
                    case 28: ret = mei.Day28; break;
                    case 29: ret = mei.Day29; break;
                    case 30: ret = mei.Day30; break;
                    case 31: ret = mei.Day31; break;
                }
            }

            return ret;
        }

        #endregion
        _receInfModel = receInf;
        _ptInfModel = ptInfModel;
        _hokenDataModel = hokenDataModel;
        _kohiDataModels = kohiDataModels;
        _syobyoDataModels = syobyoDataModels;
        _sinMeiDataModels = sinMeiDataModels;
        _receFinder = receFinder;

        _eDataModels = new List<EDataModel>();

        int dataSeqNo = 1;

        string receKaCd = string.Empty;
        string yosikiKaCd = string.Empty;
        int tantoId = 0;

        var raiinInf = raiinInfs.FindAll(p => p.PtId == receInf.PtId).OrderBy(p => p.SinDate).ThenBy(p => p.UketukeTime).First();

        receKaCd = raiinInf.ReceKaCd;
        yosikiKaCd = raiinInf.YousikiKaCd;
        tantoId = raiinInf.TantoId;


        foreach (SyobyoDataModel syobyo in _syobyoDataModels)
        {
            EDataModel eDataModel = new EDataModel();

            eDataModel.SisetuCd = siseteuCd;
            eDataModel.DataNo = _ptInfModel.PtNum;
            eDataModel.Birthday = _ptInfModel.Birthday;
            eDataModel.SinDay = receInf.SinYm * 100;
            eDataModel.DataKbn = "SY";
            eDataModel.SeqNo = dataSeqNo;
            eDataModel.MasterItemCd = syobyo.ByomeiCd;
            eDataModel.ReceItemCd = syobyo.ByomeiCd;
            eDataModel.KaisyakuNo = "";
            eDataModel.ItemName = syobyo.Byomei;
            eDataModel.KouiTen = 0;
            eDataModel.KouiYakuzai = 0;
            eDataModel.KouiZairyo = 0;
            eDataModel.EnTenKbn = 1;
            eDataModel.KouiCount = 1;
            eDataModel.HokensyaNo = _hokenDataModel.HokensyaNo.PadLeft(8, ' ');
            eDataModel.ReceSbt = "";
            eDataModel.JissiDay = syobyo.StartDate;
            eDataModel.ReceKaCd = receKaCd;
            eDataModel.SinKaCd = yosikiKaCd;
            eDataModel.DrCd = tantoId;

            _eDataModels.Add(eDataModel);

            dataSeqNo++;
        }

        int i = 0;
        int rpno = 0;
        int sinid = 0;

        while (i < _sinMeiDataModels.Count)
        {
            int seqno = 0;
            double yakuzai = 0;
            double zairyo = 0;
            double koui = 0;
            SinMeiDataModel s1mei = null;
            SinMeiDataModel smei = null;
            SinMeiDataModel ymei = null;
            SinMeiDataModel tmei = null;

            rpno = _sinMeiDataModels[i].RpNo;

            if (sinid != sinMeiDataModels[i].SinId)
            {
                sinid = sinMeiDataModels[i].SinId;
                dataSeqNo = 0;
            }

            while (i < _sinMeiDataModels.Count && _sinMeiDataModels[i].RpNo == rpno)
            {
                if (seqno != _sinMeiDataModels[i].SeqNo)
                {
                    seqno = _sinMeiDataModels[i].SeqNo;

                    // 点数をまとめておく
                    if (_sinMeiDataModels[i].InOutKbn == 0)
                    {
                        if (_sinMeiDataModels[i].KouiRecId == "IY")
                        {
                            yakuzai += _sinMeiDataModels[i].Ten;
                        }
                        else if (_sinMeiDataModels[i].KouiRecId == "TO")
                        {
                            zairyo += _sinMeiDataModels[i].Ten;
                        }
                        else
                        {
                            koui += _sinMeiDataModels[i].Ten;
                        }
                    }
                }

                // 主となる項目の候補を退避
                // 手技項目 > 診療行為 > 薬剤 > 特材
                if (s1mei == null && _sinMeiDataModels[i].RecId == "SI" && new string[] { "1", "3", "5" }.Contains(_sinMeiDataModels[i].Kokuji2))
                {
                    // 手技項目を優先
                    s1mei = _sinMeiDataModels[i];
                }
                else if (smei == null && _sinMeiDataModels[i].RecId == "SI")
                {
                    // 診療行為項目
                    smei = _sinMeiDataModels[i];
                }
                else if (ymei == null && _sinMeiDataModels[i].RecId == "IY")
                {
                    // 薬剤
                    ymei = _sinMeiDataModels[i];
                }
                else if (tmei == null && _sinMeiDataModels[i].RecId == "TO")
                {
                    // 特材
                    tmei = _sinMeiDataModels[i];
                }

                i++;
            }

            // 主となる項目の決定
            SinMeiDataModel tgtMei = null;

            if (s1mei != null)
            {
                tgtMei = s1mei;
            }
            else if (smei != null)
            {
                tgtMei = smei;
            }
            else if (ymei != null)
            {
                tgtMei = ymei;
            }
            else if (tmei != null)
            {
                tgtMei = tmei;
            }

            if (tgtMei != null)
            {
                // 日付ごとに分割
                for (int j = 1; j <= 31; j++)
                {
                    int dayCount = _getDayCount(tgtMei, j);
                    if (dayCount > 0)
                    {
                        dataSeqNo++;

                        EDataModel eDataModel = new EDataModel();

                        // 施設コード
                        eDataModel.SisetuCd = siseteuCd;
                        // データ番号
                        eDataModel.DataNo = _ptInfModel.PtNum;
                        // 生年月日
                        eDataModel.Birthday = _ptInfModel.Birthday;
                        // 診療日
                        eDataModel.SinDay = receInf.SinYm * 100 + j;
                        // データ区分
                        eDataModel.DataKbn = tgtMei.SinId.ToString();
                        // 順序番号
                        eDataModel.SeqNo = dataSeqNo;
                        // マスタ項目コード
                        eDataModel.MasterItemCd = tgtMei.ItemCd;
                        // レセ項目コード
                        eDataModel.ReceItemCd = tgtMei.ItemCd;
                        // 解釈番号
                        eDataModel.KaisyakuNo = tgtMei.KaisyakuNo;
                        // 項目名称
                        eDataModel.ItemName = tgtMei.ItemName;
                        // 行為点数
                        eDataModel.KouiTen = (tgtMei.EfFlg == 1 ? 0 : koui + yakuzai + zairyo);
                        // 行為薬剤料
                        eDataModel.KouiYakuzai = yakuzai;
                        // 行為材料料
                        eDataModel.KouiZairyo = zairyo;
                        // 円点区分
                        eDataModel.EnTenKbn = tgtMei.EnTenKbn;
                        // 行為回数
                        eDataModel.KouiCount = dayCount;
                        // 保険者番号
                        eDataModel.HokensyaNo = _hokenDataModel.HokensyaNo.PadLeft(8, ' ');
                        // レセ種別
                        eDataModel.ReceSbt = receInf.ReceSbt;
                        // 実施日
                        eDataModel.JissiDay = receInf.SinYm * 100 + j;

                        // 来院に関連する情報の取得
                        string sinReceKaCd = string.Empty;
                        string sinYosikiKaCd = string.Empty;
                        int sinTantoId = 0;

                        var sinRaiinInf = raiinInfs.FindAll(p =>
                            p.PtId == tgtMei.PtId &&
                            p.RpNo == tgtMei.SinRpNo &&
                            p.SeqNo == tgtMei.SinSeqNo &&
                            p.SinDate == receInf.SinYm * 100 + j)
                            .OrderBy(p => p.UketukeTime);
                        if (sinRaiinInf != null && sinRaiinInf.Any())
                        {
                            sinReceKaCd = sinRaiinInf.First().ReceKaCd;
                            sinYosikiKaCd = sinRaiinInf.First().YousikiKaCd;
                            sinTantoId = sinRaiinInf.First().TantoId;
                        }
                        // レセ電診療科コード
                        eDataModel.ReceKaCd = sinReceKaCd;
                        // 様式診療科コード
                        eDataModel.SinKaCd = sinYosikiKaCd;
                        // 医師コード
                        eDataModel.DrCd = sinTantoId;

                        _eDataModels.Add(eDataModel);
                    }
                }
            }
        }

    }

    public string EFileData
    {
        get
        {
            string ret = string.Empty;

            foreach (EDataModel eData in _eDataModels)
            {
                if (string.IsNullOrEmpty(ret) == false)
                {
                    ret += "\r\n";
                }

                ret += eData.EData;
            }

            return ret;
        }
    }
}
