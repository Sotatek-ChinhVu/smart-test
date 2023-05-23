﻿using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Reporting.DrugNoteSeal.DB;
using Reporting.DrugNoteSeal.Model;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;

namespace Reporting.DrugNoteSeal.Service;

public class DrugNoteSealCoReportService : IDrugNoteSealCoReportService
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly Dictionary<string, string> _extralData;

    private CoDrugNoteSealModel coModel;
    private int currentPage;
    private bool hasNextPage;
    private List<CoDrugNoteSealPrintDataModel> printOutData;
    private int _dataCharCount;
    private int _dataRowCount;
    private int _suryoTaniCharCount;
    private DateTime _printoutDateTime;
    private long ptId;
    private int hpId;
    private int sinDate;
    private long raiinNo;

    private readonly ICoDrugNoteSealFinder _finder;

    public DrugNoteSealCoReportService(ICoDrugNoteSealFinder finder)
    {
        _finder = finder;
    }

    private void UpdateDrawForm()
    {
        #region SubMethod
        // ヘッダー
        void UpdateFormHeader()
        {
            #region print method
            // 発行日
            void _printPrintDate()
            {
                SetFieldData("dfPrintDateS", _printoutDateTime.ToString("yyyy/MM/dd"));
                SetFieldData("dfPrintDateW", CIUtil.SDateToShowWDate3(CIUtil.DateTimeToInt(_printoutDateTime)).Ymd);
            }
            // 発行時間
            void _printPrintTime()
            {
                SetFieldData("dfPrintTime", _printoutDateTime.ToString("HH:mm"));
            }
            // 発行日時
            void _printPrintDateTime()
            {
                SetFieldData("dfPrintDateTimeS", _printoutDateTime.ToString("yyyy/MM/dd HH:mm"));
                SetFieldData("dfPrintDateTimeW", CIUtil.SDateToShowWDate3(CIUtil.DateTimeToInt(_printoutDateTime)).Ymd + _printoutDateTime.ToString(" HH:mm"));
            }
            // 診療日
            void _printSinDate()
            {
                SetFieldData("dfSinDateS", CIUtil.SDateToShowSDate(sinDate));
                SetFieldData("dfSinDateW", CIUtil.SDateToShowWDate3(sinDate).Ymd);
            }

            // 患者番号
            void _printPtNum()
            {
                SetFieldData("dfPtNum", coModel.PtNum.ToString());
            }

            // 患者カナ氏名
            void _printPtKanaName()
            {
                SetFieldData("dfPtKanaName", coModel.PtKanaName);
            }
            // 患者氏名
            void _printPtName()
            {
                SetFieldData("dfPtName", coModel.PtName + " 様");
            }
            // 性別
            void _printSex()
            {
                SetFieldData("dfSex", coModel.PtSex);
            }

            // 生年月日
            void _printBirthDay()
            {
                SetFieldData("dfBirthDayS", CIUtil.SDateToShowSDate(coModel.BirthDay));
                SetFieldData("dfBirthDayW", CIUtil.SDateToShowWDate3(coModel.BirthDay).Ymd);
            }

            // 年齢
            void _printAge()
            {
                SetFieldData("dfAge", coModel.Age.ToString());
            }

            // 受付番号
            void _printUketukeNo()
            {
                SetFieldData("dfUketukeNo", coModel.UketukeNo.ToString());
            }

            // 診療科
            void _printKaName()
            {
                SetFieldData("dfKa", coModel.KaName);
            }

            // 担当医
            void _printTantoName()
            {
                SetFieldData("dfTanto", coModel.TantoName);
            }

            // 病院名
            void _printHpName()
            {
                SetFieldData("dfHpName", coModel.HpName);
            }

            // 病院電話番号
            void _printHpTel()
            {
                SetFieldData("dfHpTel", coModel.HpTel);
            }

            // 医療機関所在地
            void _printHpAddress()
            {
                SetFieldData("dfHpAddress", coModel.HpAddress);
                SetFieldData("dfHpAddress1", coModel.HpAddress1);
                SetFieldData("dfHpAddress2", coModel.HpAddress2);
            }

            // 医療機関FAX番号
            void _printHpFaxNo()
            {
                SetFieldData("dfHpFaxNo", coModel.HpFaxNo);
            }

            // 医療機関その他連絡先
            void _printHpOtherContacts()
            {
                SetFieldData("dfHpOtherContacts", coModel.HpOtherContacts);
            }
            #endregion

            // 発行日
            _printPrintDate();

            // 発行時間
            _printPrintTime();

            // 発行日時
            _printPrintDateTime();

            // 調剤日
            _printSinDate();

            // 患者番号
            _printPtNum();

            // 患者カナ氏名
            _printPtKanaName();

            // 患者氏名
            _printPtName();

            // 性別
            _printSex();

            // 生年月日
            _printBirthDay();

            // 年齢
            _printAge();

            // 受付番号
            _printUketukeNo();

            // 診療科
            _printKaName();

            // 担当医
            _printTantoName();

            // 医療機関名
            _printHpName();

            // 医療機関電話番号
            _printHpTel();

            // 医療機関所在地
            _printHpAddress();

            // 医療機関FAX番号
            _printHpFaxNo();

            // 医療機関その他連絡先
            _printHpOtherContacts();
        }

        // 本体
        void UpdateFormBody()
        {
            int sijisenIndex = (currentPage - 1) * _dataRowCount;

            if (printOutData == null || printOutData.Count == 0)
            {
                hasNextPage = false;
                return;
            }

            for (short i = 0; i < _dataRowCount; i++)
            {
                Dictionary<string, CellModel> data = new();
                AddListData(ref data, "lsRpNo", printOutData[sijisenIndex].RpNo);
                AddListData(ref data, "lsOrder", printOutData[sijisenIndex].Data);
                AddListData(ref data, "lsSuryo", printOutData[sijisenIndex].Suuryo);
                AddListData(ref data, "lsTani", printOutData[sijisenIndex].Tani);
                AddListData(ref data, "lsSuryoTani", printOutData[sijisenIndex].Suuryo + printOutData[sijisenIndex].Tani);

                _tableFieldData.Add(data);
                sijisenIndex++;
                if (sijisenIndex >= printOutData.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }

            // 次ページ有無

            if (hasNextPage)
            {
                SetFieldData("dfPageInfo", "--- 次頁あり ---");
            }
        }

        #endregion
        UpdateFormHeader();
        UpdateFormBody();
    }

    private CoDrugNoteSealModel? GetData()
    {
        // 病院情報
        CoHpInfModel hpInf = _finder.FindHpInf(hpId, sinDate);

        // 患者情報
        CoPtInfModel ptInf = _finder.FindPtInf(hpId, ptId, sinDate);

        // 来院情報
        CoRaiinInfModel raiinInf = _finder.FindRaiinInfData(hpId, ptId, sinDate, raiinNo);

        // オーダー情報
        List<CoOdrInfModel> odrInfs;

        // オーダー情報詳細
        List<CoOdrInfDetailModel> odrInfDtls;

        odrInfs = _finder.FindOdrInf(hpId, ptId, sinDate, raiinNo);
        odrInfDtls = _finder.FindOdrInfDetail(hpId, ptId, sinDate, raiinNo);

        CoDrugNoteSealModel coSijisen = new CoDrugNoteSealModel(hpInf, ptInf, raiinInf, odrInfs, odrInfDtls);

        if (odrInfs.Any())
        {
            // オーダーあり 
            return coSijisen;
        }
        else
        {
            return null;
        }
    }

    private void MakeOdrDtlList()
    {
        printOutData = new();

        List<CoDrugNoteSealPrintDataModel> addPrintOutData;

        int rpNo = 0;

        for (int i = 0; i < coModel.OdrInfModels.Count; i++)
        {
            CoOdrInfModel odrInf = coModel.OdrInfModels[i];

            addPrintOutData = new List<CoDrugNoteSealPrintDataModel>();

            // Rp先頭行にRp番号を付ける
            rpNo++;
            string preSet = $"{rpNo:D2})";

            foreach (CoOdrInfDetailModel odrDtl in coModel.OdrInfDetailModels.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo))
            {
                if (odrDtl.ItemCd == ItemCdConst.ChusyaJikocyu)
                {
                    // 「自己注射」は印字しない
                    continue;
                }
                else if (odrDtl.ItemCd == "" || odrDtl.ItemCd.StartsWith("C") || (odrDtl.ItemCd.StartsWith("8") && odrDtl.ItemCd.Length == 9))
                {
                    // コメント
                    addPrintOutData.AddRange(AddList(odrDtl.ItemName, _dataCharCount, preSet));
                }
                else
                {
                    string itemName = odrDtl.ItemName;

                    if (odrDtl.ItemCd == "@BUNKATU")
                    {
                        itemName += TenUtils.GetBunkatu(odrInf.OdrKouiKbn, odrDtl.Bunkatu);
                    }

                    addPrintOutData.AddRange(AddList(itemName, _dataCharCount - _suryoTaniCharCount, preSet));

                }

                preSet = string.Empty;

                if (!string.IsNullOrEmpty(odrDtl.UnitName))
                {
                    addPrintOutData.Last().Suuryo = odrDtl.Suryo.ToString();
                    addPrintOutData.Last().Tani = odrDtl.UnitName;
                }
            }

            printOutData.AddRange(addPrintOutData);
        }
    }

    private string GetSikyu(int sikyuKbn, int tosekiKbn)
    {
        string sikyu = "";
        if (tosekiKbn == 1)
        {
            sikyu = "透前";
        }
        else if (tosekiKbn == 2)
        {
            sikyu = "透後";
        }

        if (sikyuKbn == 1)
        {
            if (sikyu == "")
            {
                sikyu = "至急";
            }
            else
            {
                sikyu = sikyu.Substring(1, 1) + "急";
            }
        }

        if (sikyu != "")
        {
            sikyu = $"《{sikyu}》";
        }
        return sikyu;
    }

    private List<CoDrugNoteSealPrintDataModel> AddList(string str, int maxLength, string preset = "")
    {
        List<CoDrugNoteSealPrintDataModel> addPrintOutData = new();

        string line = str;

        while (line != "")
        {
            string tmp = line;
            if (CIUtil.LenB(line) > maxLength)
            {
                // 文字列が最大幅より広い場合、カット
                tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
            }

            CoDrugNoteSealPrintDataModel printOutData = new();
            printOutData.Data = tmp;
            printOutData.RpNo = preset;
            preset = string.Empty;
            addPrintOutData.Add(printOutData);

            // 今回出力分の文字列を削除
            line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));
        }
        return addPrintOutData;
    }

    private int GetPrintedLineCount()
    {
        return printOutData.Count % _dataRowCount;
    }

    private int GetRemainingLineCount()
    {
        return _dataRowCount - GetPrintedLineCount();
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

    private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
        {
            dictionary.Add(field, new CellModel(value));
        }
    }

    private void GetRowCount(string fileName)
    {
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate(_rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2001, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }
}
