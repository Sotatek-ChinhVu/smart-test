using Helper.Common;
using Helper.Constants;
using Reporting.InDrug.DB;
using Reporting.InDrug.Mapper;
using Reporting.InDrug.Model;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;

namespace Reporting.InDrug.Service
{
    public class InDrugCoReportService : IInDrugCoReportService
    {
        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private ICoInDrugFinder _indrugFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private CoInDrugModel coModel = new();

        /// <summary>
        /// 印刷項目情報
        /// </summary>
        private List<CoInDrugPrintDataModel> printOutData = new();

        private int dataCharCount;
        private int dataRowCount;
        private int suryoCharCount;
        private int unitCharCount;
        private DateTime printoutDateTime;

        #endregion

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private readonly IReadRseReportFileService _readRseReportFileService;
        private readonly string _formFileName = "fmInDrug.rse";

        #region Constructor and Init
        public InDrugCoReportService(ICoInDrugFinder indrugFinder, IReadRseReportFileService readRseReportFileService)
        {
            _indrugFinder = indrugFinder;
            _readRseReportFileService = readRseReportFileService;
            _singleFieldData = new();
            _setFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            _visibleAtPrint = new();
        }
        #endregion

        #region Init properties
        private int hpId;
        private long ptId;
        private int sinDate;
        private long raiinNo;
        private int currentPage;
        private bool hasNextPage;
        #endregion

        public CommonReportingRequestModel GetInDrugPrintData(int hpId, long ptId, int sinDate, long raiinNo)
        {
            try
            {
                this.hpId = hpId;
                this.ptId = ptId;
                this.sinDate = sinDate;
                this.raiinNo = raiinNo;
                coModel = GetData();
                if (coModel != new CoInDrugModel())
                {
                    GetRowCount("fmInDrug.rse");
                    currentPage = 1;
                    hasNextPage = true;

                    printoutDateTime = CIUtil.GetJapanDateTimeNow();

                    // リスト作成
                    MakeOdrDtlList();

                    // 印刷処理
                    while (hasNextPage)
                    {
                        UpdateDrawForm();
                        currentPage++;
                    }
                }

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
                _extralData.Add("totalPage", pageIndex.ToString());
                return new InDrugMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
            }
            finally
            {
                _indrugFinder.ReleaseResource();
            }
        }

        #region Private function
        /// <summary>
        /// オーダーリスト生成
        /// </summary>
        private void MakeOdrDtlList()
        {

            printOutData = new List<CoInDrugPrintDataModel>();

            HashSet<string> kensaContainers = new HashSet<string>();
            List<CoInDrugPrintDataModel> addPrintOutData;

            int rpNo = 0;

            for (int i = 0; i < coModel.OdrInfModels.Count; i++)
            {
                CoOdrInfModel odrInf = coModel.OdrInfModels[i];

                addPrintOutData = new List<CoInDrugPrintDataModel>();

                // Rp先頭行のみ*を付ける
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
                        addPrintOutData.AddRange(_addList(odrDtl.ItemName, dataCharCount, preSet));
                    }
                    else
                    {
                        string itemName = odrDtl.ItemName;

                        if (odrDtl.ItemCd == "@BUNKATU")
                        {
                            itemName += TenUtils.GetBunkatu(odrInf.OdrKouiKbn, odrDtl.Bunkatu);
                        }

                        addPrintOutData.AddRange(_addList(itemName, dataCharCount - suryoCharCount - unitCharCount, preSet));

                    }

                    preSet = "";

                    if (!string.IsNullOrEmpty(odrDtl.UnitName))
                    {
                        addPrintOutData.Last().Suuryo = odrDtl.Suryo.ToString();
                        addPrintOutData.Last().Tani = odrDtl.UnitName;
                    }
                }

                printOutData.AddRange(_appendBlankRows(addPrintOutData.Count()));
                printOutData.AddRange(addPrintOutData);

                if (dataRowCount - (printOutData.Count() % dataRowCount) > 0)
                {
                    // 1行空ける
                    printOutData.Add(new CoInDrugPrintDataModel());
                }

            }

            // アレルギー
            List<CoInDrugPrintDataModel> addAlrgyData = _getAlrgyList();
            // 患者コメント
            List<CoInDrugPrintDataModel> addPtCmtData = _getPtCmtList();
            // 処方箋コメント
            List<CoInDrugPrintDataModel> addSyohosenCommentData = _getSyohosenCommentList();
            // 処方箋備考
            List<CoInDrugPrintDataModel> addSyohosenBikoData = _getSyohosenBikoList();

            // アレルギー、患者コメントを追加する
            if (addAlrgyData.Any() || addPtCmtData.Any() || addSyohosenCommentData.Any() || addSyohosenBikoData.Any())
            {
                addPrintOutData = new List<CoInDrugPrintDataModel>();

                if (dataRowCount - (printOutData.Count() % dataRowCount) > 0)
                {
                    printOutData.Add(new CoInDrugPrintDataModel());
                    printOutData.Last().Data = new string('ー', dataCharCount);
                }

                if (addSyohosenCommentData.Any())
                {
                    printOutData.AddRange(addSyohosenCommentData);

                    if (addPtCmtData.Any() || addSyohosenCommentData.Any() || addSyohosenBikoData.Any())
                    {
                        if (dataRowCount - (printOutData.Count() % dataRowCount) > 0)
                        {
                            printOutData.Add(new CoInDrugPrintDataModel());
                        }
                    }
                }

                if (addSyohosenBikoData.Any())
                {
                    printOutData.AddRange(addSyohosenBikoData);

                    if (addPtCmtData.Any() || addSyohosenBikoData.Any())
                    {
                        if (dataRowCount - (printOutData.Count() % dataRowCount) > 0)
                        {
                            printOutData.Add(new CoInDrugPrintDataModel());
                        }
                    }
                }

                if (addAlrgyData.Any())
                {
                    printOutData.AddRange(addAlrgyData);

                    if (addPtCmtData.Any())
                    {
                        if (dataRowCount - (printOutData.Count() % dataRowCount) > 0)
                        {
                            printOutData.Add(new CoInDrugPrintDataModel());
                        }
                    }
                }

                if (addPtCmtData.Any())
                {
                    printOutData.AddRange(addPtCmtData);

                }
            }

        }

        /// <summary>
        /// リストに追加
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <param name="preset"></param>
        /// <returns></returns>
        private List<CoInDrugPrintDataModel> _addList(string str, int maxLength, string preset = "")
        {
            List<CoInDrugPrintDataModel> addPrintOutData = new List<CoInDrugPrintDataModel>();

            string line = str;

            while (line != "")
            {
                string tmp = line;
                if (CIUtil.LenB(line) > maxLength)
                {
                    // 文字列が最大幅より広い場合、カット
                    tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
                }

                CoInDrugPrintDataModel printOutData = new CoInDrugPrintDataModel();
                printOutData.Data = tmp;
                printOutData.RpNo = preset;
                preset = "";
                addPrintOutData.Add(printOutData);

                // 今回出力分の文字列を削除
                line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));

            }

            return addPrintOutData;
        }

        /// <summary>
        /// このページの印字済み行数
        /// 既に追加した行数 % 1ページの最大行数
        /// </summary>
        /// <returns></returns>
        int _getPrintedLineCount()
        {
            return printOutData.Count % dataRowCount;
        }

        /// <summary>
        /// このページに印字可能な残り行数
        /// 1ページの最大行数 - (既に追加した行数 % 1ページの最大行数)
        /// </summary>
        /// <returns></returns>
        int _getRemainingLineCount()
        {
            return dataRowCount - _getPrintedLineCount();
        }

        List<CoInDrugPrintDataModel> _getAlrgyList()
        {
            List<string> alrgys = coModel.Alrgy;
            List<CoInDrugPrintDataModel> addPrintOutData = new List<CoInDrugPrintDataModel>();

            if (alrgys.Any())
            {
                addPrintOutData.Add(new CoInDrugPrintDataModel());
                addPrintOutData.Last().Data = "[アレルギー情報]";

                foreach (string alrgy in alrgys)
                {
                    addPrintOutData.AddRange(_addList(alrgy, dataCharCount));
                }
            }

            return addPrintOutData;
        }

        List<CoInDrugPrintDataModel> _getPtCmtList()
        {
            List<string> ptCmts = coModel.PtCmtList;
            List<CoInDrugPrintDataModel> addPrintOutData = new List<CoInDrugPrintDataModel>();

            if (ptCmts.Any())
            {
                addPrintOutData.Add(new CoInDrugPrintDataModel());
                addPrintOutData.Last().Data = "[コメント]";

                foreach (string ptCmt in ptCmts)
                {
                    addPrintOutData.AddRange(_addList(ptCmt, dataCharCount));
                }
            }

            return addPrintOutData;
        }
        List<CoInDrugPrintDataModel> _getSyohosenCommentList()
        {
            List<string> syohosenComments = coModel.SyohosenComment;
            List<CoInDrugPrintDataModel> addPrintOutData = new List<CoInDrugPrintDataModel>();

            if (syohosenComments.Any())
            {
                addPrintOutData.Add(new CoInDrugPrintDataModel());
                addPrintOutData.Last().Data = "[処方箋コメント]";

                foreach (string alrgy in syohosenComments)
                {
                    addPrintOutData.AddRange(_addList(alrgy, dataCharCount));
                }
            }

            return addPrintOutData;
        }
        List<CoInDrugPrintDataModel> _getSyohosenBikoList()
        {
            List<string> syohosenComments = coModel.SyohosenBiko;
            List<CoInDrugPrintDataModel> addPrintOutData = new List<CoInDrugPrintDataModel>();

            if (syohosenComments.Any())
            {
                addPrintOutData.Add(new CoInDrugPrintDataModel());
                addPrintOutData.Last().Data = "[処方箋備考]";

                foreach (string alrgy in syohosenComments)
                {
                    addPrintOutData.AddRange(_addList(alrgy, dataCharCount));
                }
            }

            return addPrintOutData;
        }
        /// <summary>
        /// 追加予定の行数＋このページの印字済み行数が1ページの最大行数を超えてしまう場合、
        /// リストの最大行数までを埋める空行を生成する
        /// </summary>
        /// <param name="addRowCount">追加予定の行数</param>
        /// <returns></returns>
        List<CoInDrugPrintDataModel> _appendBlankRows(int addRowCount)
        {
            List<CoInDrugPrintDataModel> addPrintOutData = new List<CoInDrugPrintDataModel>();

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
                        addPrintOutData.Add(new CoInDrugPrintDataModel());
                    }
                }
            }

            return addPrintOutData;
        }

        /// <summary>
        /// 実際の印字処理
        /// </summary>
        /// <param name="hasNextPage"></param>
        /// <returns></returns>
        private bool UpdateDrawForm()
        {
            bool _hasNextPage = true;
            #region SubMethod

            // ヘッダー
            int UpdateFormHeader()
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
                }
                // 調剤日（診療日）
                void _printSinDate()
                {
                    SetFieldData("dfSinDateS", CIUtil.SDateToShowSDate(sinDate));
                    SetFieldData("dfSinDateW", CIUtil.SDateToShowWDate3(sinDate).Ymd);
                }
                // ページ
                void _printPage()
                {
                    SetFieldData("dfPage", currentPage.ToString());
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
                    SetFieldData("dfPtName", coModel.PtName);
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

                #endregion

                // 発行日
                _printPrintDate();

                // 発行時間
                _printPrintTime();

                // 発行日時
                _printPrintDateTime();

                // 調剤日
                _printSinDate();

                // ページ
                _printPage();

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

                // 患者コメント
                _printPtCmt();

                // 受付番号
                _printUketukeNo();

                // 診療科
                _printKaName();

                // 担当医
                _printTantoName();

                return 1;
            }

            // 本体
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                int dataIndex = (currentPage - 1) * dataRowCount;

                if (printOutData == null || printOutData.Count == 0)
                {
                    _hasNextPage = false;
                    return dataIndex;
                }

                for (short i = 0; i < dataRowCount; i++)
                {
                    listDataPerPage.Add(new("lsRpNo", 0, i, printOutData[dataIndex].RpNo));
                    listDataPerPage.Add(new("lsOrder", 0, i, printOutData[dataIndex].Data));
                    listDataPerPage.Add(new("lsSuryo", 0, i, printOutData[dataIndex].Suuryo));
                    listDataPerPage.Add(new("lsTani", 0, i, printOutData[dataIndex].Tani));

                    dataIndex++;
                    if (dataIndex >= printOutData.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                // 次ページ有無

                if (_hasNextPage)
                {
                    pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                    Dictionary<string, string> fieldDataPerPage = _setFieldData.ContainsKey(pageIndex) ? _setFieldData[pageIndex] : new();

                    fieldDataPerPage.Add("dfPageInfo", "--- 次頁あり ---");

                    if (!_setFieldData.ContainsKey(pageIndex))
                    {
                        _setFieldData.Add(pageIndex, fieldDataPerPage);
                    }
                }
                _listTextData.Add(pageIndex, listDataPerPage);

                return dataIndex;

            }

            #endregion

            if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
            {
                hasNextPage = _hasNextPage;
                return false;
            }

            hasNextPage = _hasNextPage;
            return true;
        }

        /// <summary>
        /// データ取得
        /// </summary>
        /// <returns></returns>
        private CoInDrugModel GetData()
        {

            // 患者情報
            CoPtInfModel ptInf = _indrugFinder.FindPtInf(hpId, ptId, sinDate);

            // 来院情報
            CoRaiinInfModel raiinInf = _indrugFinder.FindRaiinInfData(hpId, ptId, sinDate, raiinNo);

            // オーダー情報
            List<CoOdrInfModel> odrInfs = _indrugFinder.FindOdrInf(hpId, ptId, sinDate, raiinNo);

            // オーダー情報詳細
            List<CoOdrInfDetailModel> odrInfDtls = _indrugFinder.FindOdrInfDetail(hpId, ptId, sinDate, raiinNo);

            CoInDrugModel coInDrug = new CoInDrugModel(ptInf, raiinInf, odrInfs, odrInfDtls);

            if (odrInfs.Any())
            {
                // オーダーあり 
                return coInDrug;
            }
            else
            {
                return new();
            }
        }

        private void GetRowCount(string formFileName)
        {
            List<ObjectCalculate> fieldInputList = new();

            fieldInputList.Add(new ObjectCalculate("lsOrder", (int)CalculateTypeEnum.GetListFormatLendB));
            fieldInputList.Add(new ObjectCalculate("lsOrder", (int)CalculateTypeEnum.GetListRowCount));
            fieldInputList.Add(new ObjectCalculate("lsSuryo", (int)CalculateTypeEnum.GetListFormatLendB));
            fieldInputList.Add(new ObjectCalculate("lsTani", (int)CalculateTypeEnum.GetListFormatLendB));

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.InDrug, formFileName, fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            dataCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsOrder" && item.typeInt == (int)CalculateTypeEnum.GetListFormatLendB)?.result ?? 0;
            dataRowCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsOrder" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? 0;
            suryoCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsSuryo" && item.typeInt == (int)CalculateTypeEnum.GetListFormatLendB)?.result ?? 0;
            unitCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsTani" && item.typeInt == (int)CalculateTypeEnum.GetListFormatLendB)?.result ?? 0;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }
        #endregion
    }
}
