using Helper.Common;
using Reporting.CommonMasters.Config;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Sijisen.DB;
using Reporting.Sijisen.Mapper;
using Reporting.Sijisen.Model;
using System.Text;

namespace Reporting.Sijisen.Service;

public class SijisenReportService : ISijisenReportService
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private readonly ICoSijisenFinder _finder;
    private readonly ISystemConfig _systemConfig;

    public SijisenReportService(ICoSijisenFinder finder, ISystemConfig systemConfig, IReadRseReportFileService readRseReportFileService)
    {
        _finder = finder;
        _systemConfig = systemConfig;
        _readRseReportFileService = readRseReportFileService;
        _singleFieldData = new();
        _tableFieldData = new();
        printOutData = new();
        RaiinKbnMstModels = new();
        coModel = new();
        odrKouiKbns = new();
    }

    private List<CoSijisenPrintDataModel> printOutData;

    private List<CoRaiinKbnMstModel> RaiinKbnMstModels;

    private CoSijisenModel coModel;

    private int dataCharCount;
    private int dataRowCount;
    private int suryoCharCount;
    private int unitCharCount;
    private DateTime printoutDateTime;
    private CoSijisenFormType formType;
    private int hpId;
    private long ptId;
    private int sinDate;
    private long raiinNo;
    List<(int from, int to)> odrKouiKbns;
    private bool printNoOdr;
    private int currentPage;
    private bool hasNextPage;


    public CommonReportingRequestModel GetSijisenReportingData(int hpId, int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr)
    {
        this.hpId = hpId;
        this.ptId = ptId;
        this.sinDate = sinDate;
        this.formType = (CoSijisenFormType)formType;
        this.raiinNo = raiinNo;
        if (odrKouiKbns == null)
        {
            this.odrKouiKbns = new();
        }
        else
        {
            this.odrKouiKbns = odrKouiKbns;
        }
        this.printNoOdr = printNoOdr;
        printoutDateTime = CIUtil.GetJapanDateTimeNow();
        currentPage = 1;

        coModel = GetData() ?? new();
        if (coModel != null)
        {
            GetRowCount();
            MakeOdrDtlList();
            hasNextPage = true;
            while (hasNextPage)
            {
                UpdateDrawForm();
                currentPage++;
            }
        }

        return new SijisenMapper(_singleFieldData, _tableFieldData, "lsOrder", GetJobName(formType, coModel!.PtNum), this.formType).GetData();
    }

    private string GetJobName(int formType, long ptNum)
    {
        string jobName = $"指示箋_{ptNum:D9}";

        if (formType == (int)CoSijisenFormType.JyusinHyo)
        {
            // 受診票
            jobName = "受診票";
        }

        return jobName;
    }

    private void MakeOdrDtlList()
    {
        const string conAst = "＊";
        const string conNoAst = "　";

        printOutData = new List<CoSijisenPrintDataModel>();

        string preKouiName = "";
        HashSet<string> kensaContainers = new HashSet<string>();
        List<CoSijisenPrintDataModel> addPrintOutData;

        if (dataRowCount <= 0) return;

        //foreach(CoOdrInfModel odrInf in coModel.OdrInfModels)
        for (int i = 0; i < coModel.OdrInfModels.Count; i++)
        {
            CoCommonOdrInfModel odrInf = coModel.OdrInfModels[i];

            addPrintOutData = new List<CoSijisenPrintDataModel>();

            // 行為名を除くRp先頭行のみ*を付ける
            string ast = conAst;

            string kouiName = _getKouiName(odrInf.OdrKouiKbn, odrInf.InoutKbn);
            if (preKouiName != kouiName)
            {
                addPrintOutData.AddRange(_addList(kouiName, dataCharCount));

                addPrintOutData.Last().UnderLine = true;
                preKouiName = kouiName;
            }

            if (!string.IsNullOrEmpty(odrInf.RpName) &&
                (
                    (formType == CoSijisenFormType.Sijisen && _systemConfig.SijisenRpName() == 1) ||
                    (formType == CoSijisenFormType.JyusinHyo && _systemConfig.JyusinHyoRpName() == 1)
                )
              )
            {
                addPrintOutData.AddRange(_addList($"【{odrInf.RpName}】", dataCharCount, ast));
                ast = conNoAst;

                string sikyu = _getSikyu(odrInf.SikyuKbn, odrInf.TosekiKbn);

                if (sikyu != "")
                {
                    addPrintOutData.Last().Sikyu = sikyu;
                }
            }

            foreach (CoCommonOdrInfDetailModel odrDtl in
                coModel.OdrInfDetailModels.FindAll(p => p.RaiinNo == odrInf.RaiinNo && p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo))
            {
                if (string.IsNullOrEmpty(odrDtl.ItemCd) || odrDtl.ItemCd.StartsWith("C") || (odrDtl.ItemCd.StartsWith("8") && odrDtl.ItemCd.Length == 9))
                {
                    // コメント
                    addPrintOutData.AddRange(_addList(odrDtl.ItemName, dataCharCount, ast));
                }
                else
                {
                    string itemName = odrDtl.ItemName;



                    if (odrDtl.ItemCd == "@BUNKATU")
                    {
                        itemName += TenUtils.GetBunkatu(odrInf.OdrKouiKbn, odrDtl.Bunkatu);
                    }
                    else if (odrDtl.ItemCd == "@SHIN")
                    {
                        // 初再診
                        switch (odrDtl.Suryo)
                        {
                            case 0:
                                itemName = "初再診なし";
                                break;
                            case 1:
                                itemName = "初診";
                                break;
                            case 3:
                                itemName = "再診";
                                break;
                            case 4:
                                itemName = "電話再診";
                                break;
                            case 5:
                                itemName = "医科計算なし";
                                break;
                            case 6:
                                itemName = "初診２科目";
                                break;
                            case 7:
                                itemName = "再診２科目";
                                break;
                            case 8:
                                itemName = "電話再診２科目";
                                break;
                            default:
                                itemName = "";
                                break;
                        }
                    }
                    else if (odrDtl.ItemCd == "@JIKAN")
                    {
                        // 時間枠
                        if (odrDtl.Suryo == 0) continue;

                        switch (odrDtl.Suryo)
                        {
                            case 1:
                                itemName = "時間外";
                                break;
                            case 2:
                                itemName = "休日";
                                break;
                            case 3:
                                itemName = "深夜";
                                break;
                            case 4:
                                itemName = "夜・早";
                                break;
                        }
                    }

                    addPrintOutData.AddRange(_addList(itemName, dataCharCount - suryoCharCount - unitCharCount, ast));

                    if (odrInf.OdrKouiKbn >= 60 && odrInf.OdrKouiKbn <= 69)
                    {
                        // 検査の場合、容器チェック
                        if (odrDtl.ContainerName != "")
                        {
                            kensaContainers.Add(odrDtl.ContainerName);
                        }
                    }
                }

                if (ast == conAst)
                {
                    // ast != "" ということは、このRp最初の項目
                    // このとき、至急チェックを行う
                    string sikyu = _getSikyu(odrInf.SikyuKbn, odrInf.TosekiKbn);

                    if (sikyu != "")
                    {
                        addPrintOutData.Last().Sikyu = sikyu;
                    }
                }

                ast = conNoAst;

                if (!string.IsNullOrEmpty(odrDtl.UnitName))
                {
                    addPrintOutData.Last().Suuryo = odrDtl.Suryo.ToString();
                    addPrintOutData.Last().Tani = odrDtl.UnitName;
                }
            }

            printOutData.AddRange(_appendBlankRows(addPrintOutData.Count));
            printOutData.AddRange(addPrintOutData);

            if (dataRowCount - (printOutData.Count % dataRowCount) > 0)
            {
                // 1行空ける
                printOutData.Add(new CoSijisenPrintDataModel());
            }

        }

        // アレルギー
        List<CoSijisenPrintDataModel> addAlrgyData = new List<CoSijisenPrintDataModel>();
        if ((formType == CoSijisenFormType.Sijisen && _systemConfig.SijisenAlrgy() == 0) ||
                (formType == CoSijisenFormType.JyusinHyo && _systemConfig.JyusinHyoAlrgy() == 0))
        {
            addAlrgyData = _getAlrgyList();
        }


        // 患者コメント
        List<CoSijisenPrintDataModel> addPtCmtData = new List<CoSijisenPrintDataModel>();
        if ((formType == CoSijisenFormType.Sijisen && _systemConfig.SijisenPtCmt() == 0) || (formType == CoSijisenFormType.JyusinHyo && _systemConfig.JyusinHyoPtCmt() == 0))
        {
            addPtCmtData = _getPtCmtList();
        }

        // 検査容器
        List<CoSijisenPrintDataModel> addYokiData = new List<CoSijisenPrintDataModel>();
        if (
            (
                (formType == CoSijisenFormType.Sijisen && _systemConfig.SijisenKensaYokiZairyo() == 1) || (formType == CoSijisenFormType.JyusinHyo && _systemConfig.JyusinHyoKensaYokiZairyo() == 1)
            ) && kensaContainers.Any())
        {
            StringBuilder containers = new();

            foreach (string container in kensaContainers)
            {
                if (containers.Length > 0)
                {
                    containers.Append(",");
                }
                containers.Append(container);
            }

            if (containers.Length > 0)
            {

                addYokiData.Add(new CoSijisenPrintDataModel());
                addYokiData.Last().Data = "[容器情報]";

                addYokiData.AddRange(_addList(containers.ToString(), dataCharCount));
            }
        }

        // アレルギー、患者コメント、検査容器情報を追加する
        if (addAlrgyData.Any() || addPtCmtData.Any() || addYokiData.Any())
        {
            if (dataRowCount - (printOutData.Count % dataRowCount) > 0)
            {
                printOutData.Add(new CoSijisenPrintDataModel());
                printOutData.Last().Data = new string('ー', dataCharCount);
            }

            if (addAlrgyData.Any())
            {
                printOutData.AddRange(addAlrgyData);

                if ((addPtCmtData.Any() || addYokiData.Any()) && dataRowCount - (printOutData.Count % dataRowCount) > 0)
                {
                    printOutData.Add(new CoSijisenPrintDataModel());
                }
            }

            if (addPtCmtData.Any())
            {
                printOutData.AddRange(addPtCmtData);

                if (addYokiData.Any() && dataRowCount - (printOutData.Count % dataRowCount) > 0)
                {
                    printOutData.Add(new CoSijisenPrintDataModel());
                }
            }

            if (addYokiData.Any())
            {
                printOutData.AddRange(addYokiData);
            }
        }
    }

    /// <summary>
    /// オーダー行為区分から行為名称を取得する
    /// </summary>

    private string _getKouiName(int odrKouiKbn, int inoutKbn)
    {
        string ret = "";
        string[] inout = new string[] { "（院内）", "（院外）" };

        if (odrKouiKbn >= 10 && odrKouiKbn <= 12)
        {
            ret = "初再診";
        }
        else if (odrKouiKbn == 13)
        {
            ret = "医学管理";
        }
        else if (odrKouiKbn == 14)
        {
            ret = "在宅";
        }
        else if ((odrKouiKbn >= 20 && odrKouiKbn <= 29) || (odrKouiKbn >= 100 && odrKouiKbn <= 101))
        {
            ret = "投薬" + inout[inoutKbn];
        }
        else if (odrKouiKbn >= 30 && odrKouiKbn <= 39)
        {
            ret = "注射";
        }
        else if (odrKouiKbn >= 40 && odrKouiKbn <= 49)
        {
            ret = "処置";
        }
        else if (odrKouiKbn >= 50 && odrKouiKbn <= 59)
        {
            ret = "手術";
        }
        else if (odrKouiKbn >= 60 && odrKouiKbn <= 69)
        {
            ret = "検査" + inout[inoutKbn];
        }
        else if (odrKouiKbn >= 70 && odrKouiKbn <= 79)
        {
            ret = "画像";
        }
        else if (odrKouiKbn >= 80 && odrKouiKbn <= 89)
        {
            ret = "その他";
        }
        else if (odrKouiKbn == 96)
        {
            ret = "自費";
        }

        return ret;
    }

    private string _getSikyu(int sikyuKbn, int tosekiKbn)
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

    private List<CoSijisenPrintDataModel> _addList(string str, int maxLength, string preset = "")
    {
        List<CoSijisenPrintDataModel> addPrintOutData = new();

        if (maxLength > 0)
        {
            string line = preset + str;

            while (line != "")
            {
                string tmp = line;
                if (CIUtil.LenB(line) > maxLength)
                {
                    // 文字列が最大幅より広い場合、カット
                    tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
                }

                CoSijisenPrintDataModel printOutData = new CoSijisenPrintDataModel();
                printOutData.Data = tmp;
                addPrintOutData.Add(printOutData);

                // 今回出力分の文字列を削除
                line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));

                // インデント
                if (line != "" && preset != "")
                {
                    line = new string(' ', CIUtil.LenB(preset)) + line;
                }
            }
        }

        return addPrintOutData;
    }

    int _getPrintedLineCount()
    {
        return printOutData.Count % dataRowCount;
    }

    int _getRemainingLineCount()
    {
        return dataRowCount - _getPrintedLineCount();
    }

    List<CoSijisenPrintDataModel> _getAlrgyList()
    {
        List<string> alrgys = coModel.Alrgy;
        List<CoSijisenPrintDataModel> addPrintOutData = new List<CoSijisenPrintDataModel>();

        if (alrgys.Any())
        {
            addPrintOutData.Add(new CoSijisenPrintDataModel());
            addPrintOutData.Last().Data = "[アレルギー情報]";

            foreach (string alrgy in alrgys)
            {
                addPrintOutData.AddRange(_addList(alrgy, dataCharCount));
            }
        }

        return addPrintOutData;
    }

    List<CoSijisenPrintDataModel> _getPtCmtList()
    {
        List<string> ptCmts = coModel.PtCmtList;
        List<CoSijisenPrintDataModel> addPrintOutData = new List<CoSijisenPrintDataModel>();

        if (ptCmts.Any())
        {
            addPrintOutData.Add(new CoSijisenPrintDataModel());
            addPrintOutData.Last().Data = "[コメント]";

            foreach (string ptCmt in ptCmts)
            {
                addPrintOutData.AddRange(_addList(ptCmt, dataCharCount));
            }
        }

        return addPrintOutData;
    }

    List<CoSijisenPrintDataModel> _appendBlankRows(int addRowCount)
    {
        List<CoSijisenPrintDataModel> addPrintOutData = new List<CoSijisenPrintDataModel>();

        if ((addRowCount + _getPrintedLineCount()) > dataRowCount)
        {
            // 追加する行数 + このページの印字済み行数 > 1ページの最大行数(最終Rpの場合は0, その他は-1する）
            // つまり、このRpのデータを追加すると、ページの行数を超えてしまう場合、
            // 区切りの文字と残り行を埋める空行を追加する
            // このRpのデータは次ページに印字する

            // 追加する行数を決定する
            int appendRowCount = _getRemainingLineCount();
            if (appendRowCount % dataRowCount != 0)
            {
                for (int j = 0; j < appendRowCount; j++)
                {
                    // 空行で埋める
                    addPrintOutData.Add(new CoSijisenPrintDataModel());
                }
            }
        }

        return addPrintOutData;
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
                SetFieldData("dfPrintDateS", printoutDateTime.ToString("yyyy/MM/dd"));
                SetFieldData("dfPrintDateW", CIUtil.SDateToShowWDate3(CIUtil.DateTimeToInt(printoutDateTime)).Ymd);
            }
            // 発行時間
            void _printPrintTime()
            {
                SetFieldData("dfPrintTime", printoutDateTime.ToString("HH:mm"));
            }
            // 発行日時
            void _printPrintDateTime()
            {
                SetFieldData("dfPrintDateTimeS", printoutDateTime.ToString("yyyy/MM/dd HH:mm"));
                SetFieldData("dfPrintDateTimeW", CIUtil.SDateToShowWDate3(CIUtil.DateTimeToInt(printoutDateTime)).Ymd + printoutDateTime.ToString(" HH:mm"));
                // 下位互換
                SetFieldData("TimeStmp", printoutDateTime.ToString("yyyy/MM/dd HH:mm"));
            }

            // 患者番号
            void _printPtNum()
            {
                SetFieldData("dfPtNum", coModel.PtNum.ToString());
                // 下位互換
                SetFieldData("KanId", coModel.PtNum.ToString());
            }
            // 患者番号バーコード
            void _printBcPtNum()
            {
                SetFieldData("bcPtNum", coModel.PtNum.ToString());
                SetFieldData("bcPtNum9", $"{coModel.PtNum:D9}");
                // 下位互換
                SetFieldData("KanID_BC", coModel.PtNum.ToString());
                SetFieldData("KanID_BC9", $"{coModel.PtNum:D9}");
            }
            // 患者カナ氏名
            void _printPtKanaName()
            {
                SetFieldData("dfPtKanaName", coModel.PtKanaName);
                // 下位互換
                SetFieldData("KanKana", coModel.PtKanaName);
            }
            // 患者氏名
            void _printPtName()
            {
                SetFieldData("dfPtName", coModel.PtName);
                // 下位互換
                SetFieldData("KanNm", coModel.PtName);
            }
            // 性別
            void _printSex()
            {
                SetFieldData("dfSex", coModel.PtSex);
                // 下位互換
                SetFieldData("KanSex", coModel.PtSex);
            }

            // 生年月日
            void _printBirthDay()
            {
                SetFieldData("dfBirthDayS", CIUtil.SDateToShowSDate(coModel.BirthDay));
                SetFieldData("dfBirthDayW", CIUtil.SDateToShowWDate3(coModel.BirthDay).Ymd);
                // 下位互換
                SetFieldData("KanBirth", $"{CIUtil.SDateToShowWDate3(coModel.BirthDay).Ymd} ({coModel.Age}歳)");
            }

            // 年齢
            void _printAge()
            {
                SetFieldData("dfAge", coModel.Age.ToString());
            }
            void _printAgeDsp()
            {
                SetFieldData("dfAgeDsp", coModel.AgeDsp);
            }
            void _printAgeDspMonth()
            {
                SetFieldData("dfAgeDspMonth", coModel.AgeDspMonth);
            }
            void _printAgeDspDay()
            {
                SetFieldData("dfAgeDspDay", coModel.AgeDspDay);
            }
            // 郵便番号
            void _printPostCd()
            {
                SetFieldData("dfPostCd", coModel.PtPostCdDsp);
            }

            // 患者住所
            void _printAddress()
            {
                SetFieldData("dfAddress", coModel.PtAddress);
            }

            // 電話番号
            void _printTel()
            {
                SetFieldData("dfTel", coModel.PtTel);
            }

            // 患者コメント
            void _printPtCmt()
            {
                SetFieldData("dfPtCmt", coModel.PtCmt);
            }

            // 受付番号
            void _printUketukeNo()
            {
                SetFieldData("dfUketukeNo", coModel.UketukeNo.ToString());
            }

            // 受付種別
            void _printUketukeSbt()
            {
                SetFieldData("dfUketukeKbn", coModel.UketukeSbtName);
            }

            // 受付区分（同日他来院の分）
            void _printUketukeSbtOther()
            {
                SetFieldData("dfUketukeKbnEtc", coModel.UketukeSbtNameEtc);
            }

            // 来院区分タイトル
            void _printRaiinKbnTitle()
            {
                foreach (CoRaiinKbnMstModel raiinKbnMst in RaiinKbnMstModels)
                {
                    SetFieldData($"dfRaiinKbnTitle{raiinKbnMst.GrpCd}", raiinKbnMst.GrpName);
                    SetFieldData($"dfRaiinKbnTitle{raiinKbnMst.GrpCd}_2", raiinKbnMst.GrpName);
                }
            }

            // 来院区分
            void _printRaiinKbn()
            {
                foreach (CoRaiinKbnInfModel raiinKbnInf in coModel.RaiinKbnInfModels)
                {
                    SetFieldData($"dfRaiinKbn{raiinKbnInf.GrpId}", raiinKbnInf.KbnName);
                    SetFieldData($"dfRaiinKbn{raiinKbnInf.GrpId}_2", raiinKbnInf.KbnName);
                }
            }

            // 受付時間
            void _printUketukeTime()
            {
                SetFieldData("dfUketukeTime", coModel.UketukeTime);
            }

            // 診療科
            void _printKaName()
            {
                SetFieldData("dfKa", coModel.KaName);
                // 下位互換
                SetFieldData("SkaNm", coModel.KaName);
            }

            // 担当医
            void _printTantoName()
            {
                SetFieldData("dfTanto", coModel.TantoName);
                // 下位互換
                SetFieldData("Tanto", coModel.TantoName);
            }

            // 来院コメント
            void _printRaiinCmt()
            {
                SetFieldData("dfRaiinCmt", coModel.RaiinCmt);
            }

            // 予約時間
            void _printYoyakuTime()
            {
                SetFieldData("dfYoyakuTime", coModel.YoyakuTime);
            }
            // JunNavi順番
            void _printJunNaviUketukeNo()
            {
                if (coModel.RaiinCmt.Length == 3 && CIUtil.StrToIntDef(coModel.RaiinCmt, 0) > 0)
                {
                    SetFieldData("dfJunBango", coModel.RaiinCmt);
                    // 下位互換
                    SetFieldData("JunBango", coModel.RaiinCmt);
                }
            }

            // 最終来院日
            void _printLastSinDate()
            {
                SetFieldData("dfLastSinDateS", CIUtil.SDateToShowSDate(coModel.LastSinDate));
                SetFieldData("dfLastSinDateW", CIUtil.SDateToShowWDate3(coModel.LastSinDate).Ymd);
                // 下位互換
                SetFieldData("LastSinDay", CIUtil.SDateToShowSDate(coModel.LastSinDate));
            }
            #endregion

            // 発行日
            _printPrintDate();

            // 発行時間
            _printPrintTime();

            // 発行日時
            _printPrintDateTime();

            // 患者番号
            _printPtNum();

            // 患者番号バーコード
            _printBcPtNum();

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
            _printAgeDsp();
            _printAgeDspMonth();
            _printAgeDspDay();

            // 患者住所郵便番号
            _printPostCd();

            // 患者住所
            _printAddress();

            // 電話番号
            _printTel();

            // 患者コメント
            _printPtCmt();

            // 受付番号
            _printUketukeNo();

            // 受付種別
            _printUketukeSbt();

            // 受付区分（同日他来院の分）
            _printUketukeSbtOther();

            // 来院区分タイトル
            _printRaiinKbnTitle();

            // 来院区分
            _printRaiinKbn();

            // 受付時間
            _printUketukeTime();

            // 診療科
            _printKaName();

            // 担当医
            _printTantoName();

            // 来院コメント
            _printRaiinCmt();

            // 予約時間
            _printYoyakuTime();

            // JunNavi順番
            _printJunNaviUketukeNo();

            // 最終来院日
            _printLastSinDate();
        }

        // 本体
        void UpdateFormBody()
        {
            int dataIndex = (currentPage - 1) * dataRowCount;

            if (printOutData == null || printOutData.Count == 0)
            {
                hasNextPage = false;
                return;
            }

            for (short i = 0; i < dataRowCount; i++)
            {
                Dictionary<string, CellModel> data = new();
                AddListData(ref data, "lsOdrKbn", new CellModel(printOutData[dataIndex].Sikyu));
                AddListData(ref data, "lsOrder", new CellModel(printOutData[dataIndex].Data, printOutData[dataIndex].UnderLine));
                AddListData(ref data, "lsSuryo", new CellModel(printOutData[dataIndex].Suuryo));
                AddListData(ref data, "lsTani", new CellModel(printOutData[dataIndex].Tani));

                _tableFieldData.Add(data);
                dataIndex++;
                if (dataIndex >= printOutData.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }
        }

        #endregion
        UpdateFormHeader();
        UpdateFormBody();
    }

    private CoSijisenModel? GetData()
    {
        // 患者情報
        CoPtInfModel ptInf = _finder.FindPtInf(hpId, ptId, sinDate);

        // 来院情報
        CoRaiinInfModel raiinInf = _finder.FindRaiinInfData(hpId, ptId, sinDate, raiinNo);
        List<CoRaiinInfModel> raiinInfOther = _finder.FindOtherRaiinInfData(hpId, ptId, sinDate, raiinNo);


        List<CoCommonOdrInfModel> commonOdrInfs = new();
        List<CoCommonOdrInfDetailModel> commonOdrDtls = new();

        if (formType == CoSijisenFormType.Sijisen)
        {
            // オーダー情報
            List<CoOdrInfModel> odrInfs = _finder.FindOdrInf(hpId, ptId, sinDate, raiinNo, odrKouiKbns);

            // オーダー情報詳細
            List<CoOdrInfDetailModel> odrInfDtls = _finder.FindOdrInfDetail(hpId, ptId, sinDate, raiinNo, odrKouiKbns);

            commonOdrInfs = CommonOdrInfListFactory(odrInfs);
            commonOdrDtls = CommonOdrInfDetailListFactory(odrInfDtls);
        }
        else if (formType == CoSijisenFormType.JyusinHyo)
        {

            // 予約オーダー情報
            List<CoRsvkrtOdrInfModel> rsvkrtOdrInfs = _finder.FindRsvKrtOdrInf(hpId, ptId, sinDate, odrKouiKbns);

            // 予約オーダー情報詳細
            List<CoRsvkrtOdrInfDetailModel> rsvkrtOdrInfDtls = _finder.FindRsvKrtOdrInfDetail(hpId, ptId, sinDate, odrKouiKbns);

            commonOdrInfs = CommonOdrInfListFactory(rsvkrtOdrInfs);
            commonOdrDtls = CommonOdrInfDetailListFactory(rsvkrtOdrInfDtls);
        }

        // 来院区分
        List<CoRaiinKbnInfModel> raiinKbnInfs = _finder.FindRaiinKbnInf(hpId, ptId, sinDate, raiinNo);

        if (formType == CoSijisenFormType.JyusinHyo && !string.IsNullOrEmpty(_systemConfig.JyusinHyoRaiinKbn()))
        {
            // 受診票 来院区分指定印刷

            List<string> raiinKbns = _systemConfig.JyusinHyoRaiinKbn().Split(',').ToList();
            foreach (string raiinKbn in raiinKbns)
            {
                List<string> splitRaiinKbn = raiinKbn.Split('=').ToList();
                if (splitRaiinKbn.Count == 2 && raiinKbnInfs.Any(p => p.GrpId == CIUtil.StrToIntDef(splitRaiinKbn[0], 0) && p.KbnCd == CIUtil.StrToIntDef(splitRaiinKbn[1], 0)))
                {
                    break;
                }
            }
        }

        // 最終来院日
        int lastSinDate = _finder.GetLastSinDate(hpId, ptId);

        CoSijisenModel coSijisen = new CoSijisenModel(ptInf, raiinInf, commonOdrInfs, commonOdrDtls, raiinKbnInfs, raiinInfOther, lastSinDate);

        // 来院区分マスタ
        RaiinKbnMstModels = _finder.FindRaiinKbnMst(hpId);

        // オーダーあり or
        return coSijisen;
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

    private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, CellModel value)
    {
        if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
        {
            dictionary.Add(field, value);
        }
    }

    private void GetRowCount()
    {
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate("lsOrder", (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("lsOrder", (int)CalculateTypeEnum.GetListFormatLendB),
            new ObjectCalculate("lsSuryo", (int)CalculateTypeEnum.GetListFormatLendB),
            new ObjectCalculate("lsTani", (int)CalculateTypeEnum.GetListFormatLendB),
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sijisen, string.Empty, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        var response = javaOutputData.responses;
        dataRowCount = response?.FirstOrDefault(item => item.listName == "lsOrder" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? dataRowCount;
        dataCharCount = response?.FirstOrDefault(item => item.listName == "lsOrder" && item.typeInt == (int)CalculateTypeEnum.GetListFormatLendB)?.result ?? dataCharCount;
        suryoCharCount = response?.FirstOrDefault(item => item.listName == "lsSuryo" && item.typeInt == (int)CalculateTypeEnum.GetListFormatLendB)?.result ?? suryoCharCount;
        unitCharCount = response?.FirstOrDefault(item => item.listName == "lsTani" && item.typeInt == (int)CalculateTypeEnum.GetListFormatLendB)?.result ?? unitCharCount;
    }

    #region Factory Method
    // オーダー情報
    List<CoCommonOdrInfModel> CommonOdrInfListFactory(List<CoOdrInfModel> odrInfs)
    {
        List<CoCommonOdrInfModel> results = new List<CoCommonOdrInfModel>();

        foreach (CoOdrInfModel odrInf in odrInfs)
        {
            results.Add(CommonOdrInfFactory(odrInf));
        }

        return results;
    }

    List<CoCommonOdrInfModel> CommonOdrInfListFactory(List<CoRsvkrtOdrInfModel> odrInfs)
    {
        List<CoCommonOdrInfModel> results = new List<CoCommonOdrInfModel>();

        foreach (CoRsvkrtOdrInfModel odrInf in odrInfs)
        {
            results.Add(CommonOdrInfFactory(odrInf));
        }

        return results;
    }

    List<CoCommonOdrInfDetailModel> CommonOdrInfDetailListFactory(List<CoOdrInfDetailModel> odrDtls)
    {
        List<CoCommonOdrInfDetailModel> results = new List<CoCommonOdrInfDetailModel>();

        foreach (CoOdrInfDetailModel odrDtl in odrDtls)
        {
            results.Add(CommonOdrDtlFactory(odrDtl));
        }

        return results;
    }

    List<CoCommonOdrInfDetailModel> CommonOdrInfDetailListFactory(List<CoRsvkrtOdrInfDetailModel> odrDtls)
    {
        List<CoCommonOdrInfDetailModel> results = new List<CoCommonOdrInfDetailModel>();

        foreach (CoRsvkrtOdrInfDetailModel odrDtl in odrDtls)
        {
            results.Add(CommonOdrDtlFactory(odrDtl));
        }

        return results;
    }

    private CoCommonOdrInfModel CommonOdrInfFactory(CoOdrInfModel odrInf)
    {
        return new CoCommonOdrInfModel(
            hpId: odrInf.HpId, ptId: odrInf.PtId, sinDate: odrInf.SinDate,
            raiinNo: odrInf.RaiinNo, rpNo: odrInf.RpNo, rpEdaNo: odrInf.RpEdaNo,
            odrKouiKbn: odrInf.OdrKouiKbn, rpName: odrInf.RpName,
            inoutKbn: odrInf.InoutKbn, sikyuKbn: odrInf.SikyuKbn, syohoSbt: odrInf.SyohoSbt,
            santeiKbn: odrInf.SanteiKbn, tosekiKbn: odrInf.TosekiKbn, daysCnt: odrInf.DaysCnt, sortNo: odrInf.SortNo);
    }

    private CoCommonOdrInfModel CommonOdrInfFactory(CoRsvkrtOdrInfModel odrInf)
    {
        return new CoCommonOdrInfModel(
            hpId: odrInf.HpId, ptId: odrInf.PtId, sinDate: odrInf.RsvDate,
            raiinNo: odrInf.RsvkrtNo, rpNo: odrInf.RpNo, rpEdaNo: odrInf.RpEdaNo,
            odrKouiKbn: odrInf.OdrKouiKbn, rpName: odrInf.RpName,
            inoutKbn: odrInf.InoutKbn, sikyuKbn: odrInf.SikyuKbn, syohoSbt: odrInf.SyohoSbt,
            santeiKbn: odrInf.SanteiKbn, tosekiKbn: odrInf.TosekiKbn, daysCnt: odrInf.DaysCnt, sortNo: odrInf.SortNo);
    }

    private CoCommonOdrInfDetailModel CommonOdrDtlFactory(CoOdrInfDetailModel odrDtl)
    {
        return new CoCommonOdrInfDetailModel(
            hpId: odrDtl.HpId, ptId: odrDtl.PtId, sinDate: odrDtl.SinDate,
            raiinNo: odrDtl.RaiinNo, rpNo: odrDtl.RpNo, rpEdaNo: odrDtl.RpEdaNo, rowNo: odrDtl.RowNo,
            sinKouiKbn: odrDtl.SinKouiKbn, itemCd: odrDtl.ItemCd, itemName: odrDtl.ItemName,
            suryo: odrDtl.Suryo, unitName: odrDtl.UnitName,
            bunkatu: odrDtl.Bunkatu,
            materialName: odrDtl.MaterialName, containerName: odrDtl.ContainerName
            );
    }

    private CoCommonOdrInfDetailModel CommonOdrDtlFactory(CoRsvkrtOdrInfDetailModel odrDtl)
    {
        return new CoCommonOdrInfDetailModel(
            hpId: odrDtl.HpId, ptId: odrDtl.PtId, sinDate: odrDtl.RsvDate,
            raiinNo: odrDtl.RsvkrtNo, rpNo: odrDtl.RpNo, rpEdaNo: odrDtl.RpEdaNo, rowNo: odrDtl.RowNo,
            sinKouiKbn: odrDtl.SinKouiKbn, itemCd: odrDtl.ItemCd, itemName: odrDtl.ItemName,
            suryo: odrDtl.Suryo, unitName: odrDtl.UnitName,
            bunkatu: odrDtl.Bunkatu,
            materialName: odrDtl.MaterialName, containerName: odrDtl.ContainerName
            );
    }
    #endregion
}
