﻿using CalculateService.Receipt.DB.Finder;
using Helper.Constants;

namespace CalculateService.Receipt.Models;

public class FFileDataModel
{
    private const string ModuleName = ModuleNameConst.EmrCalculateEFFile;

    private ReceInfModel _receInfModel;
    private Ika.Models.PtInfModel _ptInfModel;
    private HokenDataModel _hokenDataModel;
    private List<KohiDataModel> _kohiDataModels;
    private List<SyobyoDataModel> _syobyoDataModels;
    private List<SinMeiDataModel> _sinMeiDataModels;

    private List<FDataModel> _fDataModels;

    private int _outputYm;

    private int _receiptNo;
    private int _futanKbn;

    private int _recordYm;

    private ReceMasterFinder _receFinder;

    /// <summary>
    /// Fファイル情報
    /// </summary>
    /// <param name="receInf">レセプト情報</param>
    /// <param name="ptInfModel">患者基本情報</param>
    /// <param name="hokenDataModel">保険情報</param>
    /// <param name="kohiDataModels">公費情報</param>
    /// <param name="syobyoDataModels">傷病情報</param>
    /// <param name="sinMeiDataModels">診療明細情報</param>
    public FFileDataModel(
        string siseteuCd,
        ReceInfModel receInf, Ika.Models.PtInfModel ptInfModel, HokenDataModel hokenDataModel, List<KohiDataModel> kohiDataModels,
        List<SyobyoDataModel> syobyoDataModels, List<SinMeiDataModel> sinMeiDataModels, ReceMasterFinder receFinder)
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

        _fDataModels = new List<FDataModel>();

        int dataSeqNo = 1;
        foreach (SyobyoDataModel syobyo in _syobyoDataModels)
        {
            FDataModel fDataModel = new FDataModel();

            // 施設コード(F-1)
            fDataModel.SisetuCd = siseteuCd;
            // データ識別番号(F-2)
            fDataModel.DataNo = _ptInfModel.PtNum;
            // 生年月日(西暦)(F-3)
            fDataModel.Birthday = _ptInfModel.Birthday;
            // 外来受診年月日(西暦)(F-4)
            fDataModel.SinDay = receInf.SinYm * 100;
            // データ区分(F-5)
            fDataModel.DataKbn = "SY";
            // 順序番号(F-6)
            fDataModel.SeqNo = dataSeqNo;
            // 行為明細番号(F-7)
            fDataModel.RowNo = 1;
            // 病院点数マスタコード(F-8)
            fDataModel.MasterItemCd = syobyo.ByomeiCd;
            // レセプト電算処理システム用コード(F-9)
            fDataModel.ReceItemCd = syobyo.ByomeiCd;
            // 解釈番号（基本） (F-10)
            fDataModel.KaisyakuNo = "";
            // 診療行為名称(F-11)
            fDataModel.ItemName = syobyo.Byomei;
            // 数量(F-12)
            fDataModel.Suryo = 0;
            // 基準単位(F-13)
            fDataModel.Unit = 0;
            // 行為明細点数(F-14)
            fDataModel.MeisaiTen = 0;
            // 行為明細薬剤料(F-15)
            fDataModel.MeisaiYakuzai = 0;
            // 行為明細材料料(F-16)
            fDataModel.MeisaiZairyo = 0;
            // 円・点区分(F-17)
            fDataModel.EnTenKbn = 1;
            // 出来高実績点数 (F-18)
            fDataModel.TotalTen = 0;
            // ①院外処方区分
            fDataModel.OutDrug = 0;
            // ②一般名処方区分
            fDataModel.IpnDrug = 0;
            // ③性別
            fDataModel.Sex = ptInfModel.Sex;
            // ④転帰区分
            fDataModel.TenkiKbn = syobyo.ReceTenkiKbn;
            // ⑤主傷病
            fDataModel.SyubyoKbn = syobyo.SyubyoKbn;
            // ⑥医学管理料等包括項目区分
            fDataModel.HoukatuKbn = 0;
            // ⑦リフィル処方箋区分
            fDataModel.Refill = 0;

            _fDataModels.Add(fDataModel);

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

            List<FDataModel> rpFDatas = new List<FDataModel>();
            int rowNo = 0;
            while (i < _sinMeiDataModels.Count && _sinMeiDataModels[i].RpNo == rpno)
            {
                FDataModel fDataModel = new FDataModel();

                // 施設コード(F-1)
                fDataModel.SisetuCd = siseteuCd;
                // データ識別番号(F-2)
                fDataModel.DataNo = _ptInfModel.PtNum;
                // 生年月日(西暦)(F-3)
                fDataModel.Birthday = _ptInfModel.Birthday;
                // 外来受診年月日(西暦)(F-4)
                fDataModel.SinDay = 0;
                // データ区分(F-5)
                fDataModel.DataKbn = _sinMeiDataModels[i].SinId.ToString();
                // 順序番号(F-6)
                fDataModel.SeqNo = 0;
                // 行為明細番号(F-7)
                rowNo++;
                fDataModel.RowNo = rowNo;
                // 病院点数マスタコード(F-8)
                fDataModel.MasterItemCd = _sinMeiDataModels[i].ItemCd;
                // レセプト電算処理システム用コード(F-9)
                fDataModel.ReceItemCd = _sinMeiDataModels[i].ItemCd;
                // 解釈番号（基本） (F-10)
                fDataModel.KaisyakuNo = _sinMeiDataModels[i].KaisyakuNo;
                // 診療行為名称(F-11)
                fDataModel.ItemName = _sinMeiDataModels[i].ItemName;
                // 数量(F-12)
                if (_sinMeiDataModels[i].UnitCd > 0)
                {
                    fDataModel.Suryo = _sinMeiDataModels[i].Suryo;
                }
                // 基準単位(F-13)
                fDataModel.Unit = _sinMeiDataModels[i].UnitCd;

                if (_sinMeiDataModels[i].RecId == "IY")
                {
                    // 薬剤の場合

                    // 行為明細点数(F-14)
                    fDataModel.MeisaiTen = 0;
                    // 行為明細薬剤料(F-15)
                    fDataModel.MeisaiYakuzai = _sinMeiDataModels[i].MeisaiTen * 10;
                    // 行為明細材料料(F-16)
                    fDataModel.MeisaiZairyo = 0;
                    // 円・点区分(F-17)
                    fDataModel.EnTenKbn = 1;
                }
                else if (_sinMeiDataModels[i].RecId == "TO")
                {
                    // 特材の場合

                    // 行為明細点数(F-14)
                    fDataModel.MeisaiTen = 0;
                    // 行為明細薬剤料(F-15)
                    fDataModel.MeisaiYakuzai = 0;
                    // 行為明細材料料(F-16)
                    fDataModel.MeisaiZairyo = _sinMeiDataModels[i].MeisaiTen * 10;
                    // 円・点区分(F-17)
                    fDataModel.EnTenKbn = 1;
                }
                else if (_sinMeiDataModels[i].RecId != "CO")
                {
                    // コメント以外の場合

                    // 行為明細点数(F-14)
                    if (!(new int[] { 5, 6 }.Contains(_sinMeiDataModels[i].TenId)))
                    {
                        // 率加算・包括検査以外の場合

                        // 行為明細点数(F-14)
                        fDataModel.MeisaiTen = _sinMeiDataModels[i].MeisaiTen;
                        // 行為明細薬剤料(F-15)
                        fDataModel.MeisaiYakuzai = 0;
                        // 行為明細材料料(F-16)
                        fDataModel.MeisaiZairyo = 0;
                        // 円・点区分(F-17)
                        fDataModel.EnTenKbn = _sinMeiDataModels[i].EnTenKbn;
                    }
                }
                // 出来高実績点数 (F-18)
                if (_sinMeiDataModels[i].RecId != "CO" && _sinMeiDataModels[i].EfFlg == 0)
                {
                    if (_sinMeiDataModels[i].RecId == "IY" ||
                        _sinMeiDataModels[i].RecId == "TO" ||
                        _sinMeiDataModels[i].FmtKbn == 1)
                    {
                        // 薬剤、特材、包括検査の場合、行為点数をセット
                        // 後で、最後の行以外は0に戻す
                        fDataModel.TotalTen = _sinMeiDataModels[i].Ten;
                    }
                    else
                    {
                        fDataModel.TotalTen = _sinMeiDataModels[i].MeisaiTen;
                    }
                }
                // ①院外処方区分
                fDataModel.OutDrug = _sinMeiDataModels[i].InOutKbn;
                // ②一般名処方区分
                fDataModel.IpnDrug = _sinMeiDataModels[i].IpnFlg;
                // ③性別
                fDataModel.Sex = ptInfModel.Sex;
                // ④転帰区分
                fDataModel.TenkiKbn = 0;
                // ⑤主傷病
                fDataModel.SyubyoKbn = 0;
                // ⑥医学管理料等包括項目区分
                fDataModel.HoukatuKbn = _sinMeiDataModels[i].EfFlg;
                // ⑦リフィル処方箋区分
                fDataModel.Refill = _sinMeiDataModels[i].Refill;

                fDataModel.FmtKbn = _sinMeiDataModels[i].FmtKbn;

                rpFDatas.Add(fDataModel);

                if (seqno != _sinMeiDataModels[i].SeqNo)
                {
                    seqno = _sinMeiDataModels[i].SeqNo;

                    // 点数をまとめておく
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
                bool yChk = false;
                bool tChk = false;
                bool hokatu = false;

                // 最後以外の行の出来高実績点数を0にする
                for (int k = rpFDatas.Count - 1; k >= 0; k--)
                {
                    if (rpFDatas[k].MeisaiYakuzai > 0)
                    {
                        if (yChk == false)
                        {
                            yChk = true;
                        }
                        else
                        {
                            rpFDatas[k].TotalTen = 0;
                        }
                    }
                    else if (rpFDatas[k].MeisaiZairyo > 0)
                    {
                        if (tChk == false)
                        {
                            tChk = true;
                        }
                        else
                        {
                            rpFDatas[k].TotalTen = 0;
                        }
                    }
                    else if (rpFDatas[k].FmtKbn == 1)
                    {
                        if (hokatu == false)
                        {
                            hokatu = true;
                        }
                        else
                        {
                            rpFDatas[k].TotalTen = 0;
                        }
                    }
                }

                // 日付ごとに分割
                for (int j = 1; j <= 31; j++)
                {
                    int dayCount = _getDayCount(tgtMei, j);

                    if (dayCount > 0)
                    {
                        dataSeqNo++;

                        foreach (FDataModel rpFdata in rpFDatas)
                        {
                            FDataModel fDataModel = new FDataModel();

                            // 施設コード(F-1)
                            fDataModel.SisetuCd = rpFdata.SisetuCd;
                            // データ識別番号(F-2)
                            fDataModel.DataNo = rpFdata.DataNo;
                            // 生年月日(西暦)(F-3)
                            fDataModel.Birthday = rpFdata.Birthday;
                            // 外来受診年月日(西暦)(F-4)
                            fDataModel.SinDay = receInf.SinYm * 100 + j;
                            // データ区分(F-5)
                            fDataModel.DataKbn = rpFdata.DataKbn;
                            // 順序番号(F-6)
                            fDataModel.SeqNo = dataSeqNo;
                            // 行為明細番号(F-7)
                            fDataModel.RowNo = rpFdata.RowNo;
                            // 病院点数マスタコード(F-8)
                            fDataModel.MasterItemCd = rpFdata.MasterItemCd;
                            // レセプト電算処理システム用コード(F-9)
                            fDataModel.ReceItemCd = rpFdata.ReceItemCd;
                            // 解釈番号（基本） (F-10)
                            fDataModel.KaisyakuNo = rpFdata.KaisyakuNo;
                            // 診療行為名称(F-11)
                            fDataModel.ItemName = rpFdata.ItemName;
                            // 数量(F-12)
                            fDataModel.Suryo = rpFdata.Suryo;
                            // 基準単位(F-13)
                            fDataModel.Unit = rpFdata.Unit;
                            // 行為明細点数(F-14)
                            fDataModel.MeisaiTen = rpFdata.MeisaiTen;
                            // 行為明細薬剤料(F-15)
                            fDataModel.MeisaiYakuzai = rpFdata.MeisaiYakuzai;
                            // 行為明細材料料(F-16)
                            fDataModel.MeisaiZairyo = rpFdata.MeisaiZairyo;
                            // 円・点区分(F-17)
                            fDataModel.EnTenKbn = rpFdata.EnTenKbn;
                            // 出来高実績点数 (F-18)
                            fDataModel.TotalTen = rpFdata.TotalTen;
                            // ①院外処方区分
                            fDataModel.OutDrug = rpFdata.OutDrug;
                            // ②一般名処方区分
                            fDataModel.IpnDrug = rpFdata.IpnDrug;
                            // ③性別
                            fDataModel.Sex = rpFdata.Sex;
                            // ④転帰区分
                            fDataModel.TenkiKbn = rpFdata.TenkiKbn;
                            // ⑤主傷病
                            fDataModel.SyubyoKbn = rpFdata.SyubyoKbn;
                            // ⑥医学管理料等包括項目区分
                            fDataModel.HoukatuKbn = rpFdata.HoukatuKbn;
                            // ⑦リフィル処方箋区分
                            fDataModel.Refill = rpFdata.Refill;

                            _fDataModels.Add(fDataModel);
                        }
                    }
                }
            }
        }

    }

    public string FFileData
    {
        get
        {
            string ret = string.Empty;

            foreach (FDataModel fData in _fDataModels)
            {
                if (string.IsNullOrEmpty(ret) == false)
                {
                    ret += "\r\n";
                }

                ret += fData.FData;
            }

            return ret;
        }
    }
}