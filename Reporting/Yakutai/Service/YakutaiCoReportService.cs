using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Reporting.Calculate.Constants;
using Reporting.CommonMasters.Config;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Yakutai.DB;
using Reporting.Yakutai.Mapper;
using Reporting.Yakutai.Model;

namespace Reporting.Yakutai.Service
{
    public class YakutaiCoReportService : IYakutaiCoReportService
    {
        #region contructor and init
        public const string YAKUTAI_FORM_FILE_NAME = "fmYakutai.rse";
        public const string YAKUTAI_NAIFUKU_SMALL_FORM_FILE_NAME = "fmYakutaiN1.rse";
        public const string YAKUTAI_NAIFUKU_NORMAL_FORM_FILE_NAME = "fmYakutaiN2.rse";
        public const string YAKUTAI_NAIFUKU_BIG_FORM_FILE_NAME = "fmYakutaiN3.rse";
        public const string YAKUTAI_TONPUKU_SMALL_FORM_FILE_NAME = "fmYakutaiT1.rse";
        public const string YAKUTAI_TONPUKU_NORMAL_FORM_FILE_NAME = "fmYakutaiT2.rse";
        public const string YAKUTAI_TONPUKU_BIG_FORM_FILE_NAME = "fmYakutaiT3.rse";
        public const string YAKUTAI_GAIYO_SMALL_FORM_FILE_NAME = "fmYakutaiG1.rse";
        public const string YAKUTAI_GAIYO_NORMAL_FORM_FILE_NAME = "fmYakutaiG2.rse";
        public const string YAKUTAI_GAIYO_BIG_FORM_FILE_NAME = "fmYakutaiG3.rse";

        private readonly ICoYakutaiFinder _finder;
        private readonly ISystemConfig _systemConfig;
        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoYakutaiModel> coModels;
        private CoYakutaiModel coModel;

        /// <summary>
        /// ファイル名
        /// </summary>
        private string OutputFileName;
        /// <summary>
        /// プリンタ名
        /// </summary>
        private string PrinterName;

        /// <summary>
        /// 印刷項目情報
        /// </summary>
        private List<CoYakutaiPrintDataModel> printOutData;

        private int _dataCharCount;
        private int _dataRowCount;
        private int _suryoCharCount;
        private int _unitCharCount;
        private int _yohoCmtCharCount;
        private DateTime _printoutDateTime;

        private bool _hasNextPage;
        private int _currentPage;
        private int _sinDate;
        private int _hpId;
        private long _ptId;
        private int _raiinNo;
        private string _formFileName;

        private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM = new Dictionary<int, Dictionary<string, string>>();
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _fileNamePageMap = new Dictionary<string, string>();
        private readonly Dictionary<int, List<ListTextObject>> _listTextData = new Dictionary<int, List<ListTextObject>>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly IReadRseReportFileService _readRseReportFileService;

        public YakutaiCoReportService(ICoYakutaiFinder finder, ISystemConfig systemConfig, IReadRseReportFileService readRseReportFileService)
        {
            _finder = finder;
            _systemConfig = systemConfig;
            _readRseReportFileService = readRseReportFileService;
        }

        public CommonReportingRequestModel GetYakutaiReportingData(int hpId, long ptId, int sinDate, int raiinNo)
        {
            _hpId = hpId;
            _ptId = ptId;
            _sinDate = sinDate;
            _raiinNo = raiinNo;
            _printoutDateTime = CIUtil.GetJapanDateTimeNow();
            coModels = GetData();
            _currentPage = 1;

            if (coModels != null && coModels.Any())
            {
                foreach (CoYakutaiModel coYakutaiModel in coModels)
                {
                    try
                    {
                        coModel = coYakutaiModel;

                        _formFileName = GetFormFilePrinterName(coYakutaiModel).Item1;
                        AddFileNamePageMap("1", _formFileName);
                        _hasNextPage = true;

                        GetRowCount(_formFileName);
                        MakeOdrDtlList();
                        //印刷
                        while (_hasNextPage)
                        {
                            UpdateDrawForm();
                            _currentPage++;
                        }
                    }
                    finally
                    {
                        _currentPage = 1;
                    }
                }
            }

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new YakutaiMapper(_singleFieldData, _fileNamePageMap, string.Empty, _singleFieldDataM, _listTextData, _extralData).GetData();
        }
        #endregion

        private void MakeOdrDtlList()
        {

            printOutData = new List<CoYakutaiPrintDataModel>();

            List<CoYakutaiPrintDataModel> addPrintOutData;

            for (int i = 0; i < coModel.OdrInfModels.Count; i++)
            {
                CoOdrInfModel odrInf = coModel.OdrInfModels[i];

                addPrintOutData = new List<CoYakutaiPrintDataModel>();

                bool yoho = false;

                foreach (CoOdrInfDetailModel odrDtl in coModel.OdrInfDetailModels.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo))
                {
                    if (odrDtl.ItemCd == ItemCdConst.ChusyaJikocyu)
                    {
                        // 「自己注射」は印字しない
                        continue;
                    }
                    if (odrDtl.YohoKbn == 1)
                    {
                        yoho = true;

                        coModel.Yoho = odrDtl.ItemName;

                        if (!(odrInf.OdrKouiKbn == 23 && odrDtl.UnitName == "調剤"))
                        {
                            // 外用用法で単位が"調剤"のときは印字しないようにする
                            coModel.YohoSuryo = odrDtl.Suryo;
                            coModel.YohoTani = odrDtl.UnitName;
                        }
                    }
                    else
                    {
                        if (odrDtl.ItemCd == "" || odrDtl.ItemCd.StartsWith("C") || (odrDtl.ItemCd.StartsWith("8") && odrDtl.ItemCd.Length == 9))
                        {
                            // コメント
                            if (yoho)
                            {
                                // 基本用法から後のコメントは、用法のコメントとして扱う
                                coModel.YohoComments.AddRange(_addStringList(odrDtl.ItemName, _yohoCmtCharCount));
                            }
                            else
                            {
                                // 基本用法までのコメントは、薬剤のコメントとして扱う
                                addPrintOutData.AddRange(_addList(odrDtl.ItemName, _dataCharCount));
                            }
                        }
                        else
                        {
                            string itemName = odrDtl.ItemName;

                            if (odrDtl.ItemCd == "@BUNKATU")
                            {
                                itemName += TenUtils.GetBunkatu(odrInf.OdrKouiKbn, odrDtl.Bunkatu);
                            }

                            addPrintOutData.AddRange(_addList(itemName, _dataCharCount - _suryoCharCount - _unitCharCount));

                            if (!string.IsNullOrEmpty(odrDtl.UnitName))
                            {
                                if (coModel.IsOnceAmount && _systemConfig.YakutaiOnceAmount() == 1)
                                {
                                    // 1回量に換算（小数2桁まで)
                                    addPrintOutData.Last().Suuryo = (Math.Floor(odrDtl.SuryoDsp / coModel.CnvToOnceValue * coModel.OnceValue * 100) / 100).ToString();
                                }
                                else
                                {
                                    addPrintOutData.Last().Suuryo = odrDtl.SuryoDsp.ToString();
                                }
                                addPrintOutData.Last().Tani = odrDtl.UnitNameDsp;
                            }
                        }
                    }
                }

                printOutData.AddRange(_appendBlankRows(addPrintOutData.Count()));
                printOutData.AddRange(addPrintOutData);

            }
        }

        List<CoYakutaiPrintDataModel> _appendBlankRows(int addRowCount)
        {
            List<CoYakutaiPrintDataModel> addPrintOutData = new List<CoYakutaiPrintDataModel>();

            if ((addRowCount + _getPrintedLineCount()) > _dataRowCount)
            {
                // 追加する行数 + このページの印字済み行数 > 1ページの最大行数(最終Rpの場合は0, その他は-1する）
                // つまり、このRpのデータを追加すると、ページの行数を超えてしまう場合、
                // 区切りの文字と残り行を埋める空行を追加する
                // このRpのデータは次ページに印字する

                // 追加する行数を決定する
                int appendRowCount = _getRemainingLineCount();
                if (appendRowCount % _dataRowCount != 0)
                {
                    for (int j = 0; j < appendRowCount; j++)
                    {
                        // 空行で埋める
                        addPrintOutData.Add(new CoYakutaiPrintDataModel());
                    }
                }
            }

            return addPrintOutData;
        }

        int _getPrintedLineCount()
        {
            return printOutData.Count % _dataRowCount;
        }

        int _getRemainingLineCount()
        {
            return _dataRowCount - _getPrintedLineCount();
        }

        private List<CoYakutaiPrintDataModel> _addList(string str, int maxLength)
        {
            List<CoYakutaiPrintDataModel> addPrintOutData = new List<CoYakutaiPrintDataModel>();

            string line = str;

            while (line != "")
            {
                string tmp = line;
                if (CIUtil.LenB(line) > maxLength)
                {
                    // 文字列が最大幅より広い場合、カット
                    tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
                }

                CoYakutaiPrintDataModel printOutData = new CoYakutaiPrintDataModel();
                printOutData.Data = tmp;

                addPrintOutData.Add(printOutData);

                // 今回出力分の文字列を削除
                line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));

            }

            return addPrintOutData;
        }

        private List<string> _addStringList(string str, int maxLength)
        {
            List<string> addPrintOutData = new List<string>();

            string line = str;

            while (line != "")
            {
                string tmp = line;
                if (CIUtil.LenB(line) > maxLength)
                {
                    // 文字列が最大幅より広い場合、カット
                    tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
                }

                addPrintOutData.Add(tmp);

                // 今回出力分の文字列を削除
                line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));

            }

            return addPrintOutData;
        }

        private void UpdateDrawForm()
        {
            _hasNextPage = true;
            #region SubMethod
            List<ListTextObject> listDataPerPage = new();
            // ヘッダー
            int UpdateFormHeader()
            {
                Dictionary<string, string> fieldDataPerPage = new();
                #region print method
                // 発行日
                void _printPrintDate()
                {
                    SetFieldData("dfPrintDate", _printoutDateTime.ToString("yyyy/MM/dd"));
                }
                // 発行時間
                void _printPrintTime()
                {
                    SetFieldData("dfPrintTime", _printoutDateTime.ToString("HH:mm"));
                }
                // 発行日時
                void _printPrintDateTime()
                {
                    SetFieldData("dfPrintDateTime", _printoutDateTime.ToString("yyyy/MM/dd HH:mm"));
                }
                // 調剤日（診療日）
                void _printSinDate()
                {
                    SetFieldData("dfSinDate", CIUtil.SDateToShowWDate3(_sinDate).Ymd);
                }
                // ページ
                void _printPage()
                {
                    SetFieldData("dfPage", _currentPage.ToString());
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

                // 担当医
                void _printTantoName()
                {
                    SetFieldData("dfTanto", coModel.TantoName);
                }

                // 行為区分
                void _printDrugKbn()
                {
                    SetFieldData("dfDrugKbn", coModel.DrugKbn);
                }
                // 用法
                void _printYoho()
                {
                    fieldDataPerPage.Add("dfYoho", coModel.Yoho);
                }

                // 日数回数
                void _printDayCount()
                {
                    if (coModel.YohoSuryo > 0)
                    {
                        fieldDataPerPage.Add("dfNisu", $"{coModel.YohoSuryo}{coModel.YohoTani}");
                    }
                }

                // 用法コメント
                void _printYohoCmt()
                {
                    short i = 0;
                    List<ListTextObject> listDataPerPage = new();
                    foreach (string yohoCmt in coModel.YohoComments)
                    {
                        listDataPerPage.Add(new("lsYohoCmt", 0, i, yohoCmt));
                        i++;

                    }
                }

                // 医療機関住所
                void _printHpAddress()
                {
                    SetFieldData("dfHpAddress", coModel.HpAddress);
                }
                // 医療機関名称
                void _printHpName()
                {
                    SetFieldData("dfHpName", coModel.HpName);
                }
                // 医療機関電話番号
                void _printHpTel()
                {
                    SetFieldData("dfHpTel", coModel.HpTel);
                }
                // 医療機関郵便番号
                void _printHpPostCd()
                {
                    SetFieldData("dfHpPostCd", coModel.HpPostCdDsp);
                }
                // 医療機関FAX番号
                void _printHpFax()
                {
                    SetFieldData("dfHpFaxNo", coModel.HpFaxNo);
                }
                // 医療機関その他連絡先
                void _printHpOtherContacts()
                {
                    SetFieldData("dfHpOtherContacts", coModel.HpOtherContacts);
                }
                // １回に（１日に）
                void _printIkkairyo()
                {
                    if (coModel.DrugKbnCd == OdrKouiKbnConst.Naifuku)
                    {
                        // 内服
                        if (coModel.YohoTani != "調剤")
                        {
                            // "調剤" の場合は出さない
                            if ((coModel.IsOnceAmount && _systemConfig.YakutaiOnceAmount() == 1) || coModel.IsFukuyojiIppo)
                            {
                                // 1回量に換算する場合
                                SetFieldData("dfIkkairyo", "１回に");
                                SetFieldData("dfIkkairyo2", "【１回量】");
                            }
                            else
                            {
                                SetFieldData("dfIkkairyo", "１日に");
                                SetFieldData("dfIkkairyo2", "【１日量】");
                            }
                        }
                    }
                    else if (coModel.DrugKbnCd == OdrKouiKbnConst.Tonpuku)
                    {
                        // 頓服
                        if (coModel.YohoTani != "調剤")
                        {
                            // "調剤" の場合は出さない
                            SetFieldData("dfIkkairyo", "１回に");
                            SetFieldData("dfIkkairyo2", "【１回量】");
                        }
                    }

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

                // 患者氏名（様つき）
                _printPtName();

                // 担当医
                _printTantoName();

                // 行為
                _printDrugKbn();
                // 用法
                _printYoho();
                // 日数回数
                _printDayCount();
                // 用法コメント
                _printYohoCmt();
                // 医療機関住所
                _printHpAddress();
                // 医療機関名称
                _printHpName();
                // 医療機関電話番号
                _printHpTel();
                // 医療機関郵便番号
                _printHpPostCd();
                // 医療機関FAX
                _printHpFax();
                // 医療機関その他連絡先
                _printHpOtherContacts();

                // １回に（１日に）
                _printIkkairyo();

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                _singleFieldDataM.Add(pageIndex, fieldDataPerPage);
                return 1;
            }

            // 本体
            int UpdateFormBody()
            {

                int dataIndex = (_currentPage - 1) * _dataRowCount;

                if (printOutData == null || printOutData.Count == 0)
                {
                    _hasNextPage = false;
                    return dataIndex;
                }

                for (short i = 0; i < _dataRowCount; i++)
                {
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

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                _listTextData.Add(pageIndex, listDataPerPage);

                return dataIndex;

            }

            #endregion

            UpdateFormHeader();
            UpdateFormBody();
        }

        private List<CoYakutaiModel> GetData()
        {
            // 病院情報
            CoHpInfModel hpInf = _finder.FindHpInf(_hpId, _sinDate);

            // 患者情報
            CoPtInfModel ptInf = _finder.FindPtInf(_hpId, _ptId, _sinDate);

            // 来院情報
            CoRaiinInfModel raiinInf = _finder.FindRaiinInfData(_hpId, _ptId, _sinDate, _raiinNo);

            // オーダー情報
            List<CoOdrInfModel> odrInfs = _finder.FindOdrInf(_hpId, _ptId, _sinDate, _raiinNo);

            // オーダー情報詳細
            List<CoOdrInfDetailModel> odrInfDtls = _finder.FindOdrInfDetail(_hpId, _ptId, _sinDate, _raiinNo);

            // 1回料表示単位マスタ
            List<CoSingleDoseMstModel> singleDoses = _finder.FindSingleDoseMst(_hpId);

            // 一包化指示項目を含むかどうか
            List<CoOdrInfModel> appendOdrInfs = new();
            List<CoOdrInfDetailModel> appendOdrDtls = new ();

            foreach (CoOdrInfModel odrInf in odrInfs.OrderBy(p => p.OdrKouiKbn).ThenBy(p => p.SortNo).ThenBy(p => p.RpNo).ThenBy(p => p.RpEdaNo))
            {
                List<int> fukuyojis = new ()
                {
                    0,0,0,0,0
                };

                bool ippo = !string.IsNullOrEmpty(_systemConfig.YakutaiFukuyojiIppokaItemCd()) &&
                    odrInfDtls.Any(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo && p.ItemCd == _systemConfig.YakutaiFukuyojiIppokaItemCd());

                // 用法の服用時設定を確認
                if (ippo && odrInfDtls.Any(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo && p.YohoKbn == 1))
                {
                    var yohoOdrDtl = odrInfDtls.First(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo && p.YohoKbn == 1);
                    if (yohoOdrDtl.FukuyoRise + yohoOdrDtl.FukuyoMorning + yohoOdrDtl.FukuyoDaytime + yohoOdrDtl.FukuyoNight + yohoOdrDtl.FukuyoSleep <= 0)
                    {
                        ippo = false;
                    }
                    else
                    {
                        fukuyojis[0] = yohoOdrDtl.FukuyoRise;
                        fukuyojis[1] = yohoOdrDtl.FukuyoMorning;
                        fukuyojis[2] = yohoOdrDtl.FukuyoDaytime;
                        fukuyojis[3] = yohoOdrDtl.FukuyoNight;
                        fukuyojis[4] = yohoOdrDtl.FukuyoSleep;
                    }
                }

                // 一回量対象外の単位がないかチェック
                if (ippo)
                {
                    foreach (CoOdrInfDetailModel odrDtl in odrInfDtls.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo && p.DrugKbn > 0))
                    {
                        if (singleDoses.Any(p => p.UnitName == odrDtl.UnitNameDsp) == false)
                        {
                            ippo = false;
                            break;
                        }
                    }
                }

                if (ippo)
                {
                    // 服用時一包化対象の場合

                    // こいつは削除
                    odrInf.Delete = true;
                    int totakuYoji = fukuyojis.Sum(p => p);

                    for (int i = 0; i < fukuyojis.Count; i++)
                    {
                        if (fukuyojis[i] > 0)
                        {
                            // 服用時設定別にOdrInfを生成
                            long rpEdaNo = odrInfs.Where(p => p.RpNo == odrInf.RpNo).Max(p => p.RpEdaNo);

                            if (appendOdrInfs.Any(p => p.RpNo == odrInf.RpNo))
                            {
                                long rpEdaNo2 = appendOdrInfs.Where(p => p.RpNo == odrInf.RpNo).Max(p => p.RpEdaNo);

                                if (rpEdaNo2 > rpEdaNo)
                                {
                                    rpEdaNo = rpEdaNo2;
                                }
                            }

                            rpEdaNo++;

                            appendOdrInfs.Add(new CoOdrInfModel(new OdrInf()
                            {
                                HpId = odrInf.HpId,
                                PtId = odrInf.PtId,
                                SinDate = odrInf.SinDate,
                                RaiinNo = odrInf.RaiinNo,
                                RpNo = odrInf.RpNo,
                                RpEdaNo = rpEdaNo,
                                //Id = 0;
                                HokenPid = odrInf.HokenPid,
                                OdrKouiKbn = odrInf.OdrKouiKbn,
                                RpName = odrInf.RpName,
                                InoutKbn = odrInf.InoutKbn,
                                SikyuKbn = odrInf.SikyuKbn,
                                SyohoSbt = odrInf.SyohoSbt,
                                SanteiKbn = odrInf.SanteiKbn,
                                TosekiKbn = odrInf.TosekiKbn,
                                DaysCnt = odrInf.DaysCnt,
                                SortNo = odrInf.SortNo,
                                IsDeleted = odrInf.IsDeleted
                            }));

                            // Deteilを設定
                            int rowNo = 0;
                            foreach (CoOdrInfDetailModel odrInfDtl in odrInfDtls.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo).OrderBy(p => p.RowNo))
                            {
                                odrInfDtl.Delete = true;

                                if (odrInfDtl.ItemCd != _systemConfig.YakutaiFukuyojiIppokaItemCd())
                                {
                                    string itemCd = odrInfDtl.ItemCd;
                                    string itemName = odrInfDtl.ItemName;
                                    int fukuyoRise = 0;
                                    int fukuyoMorning = 0;
                                    int fukuyoDaytime = 0;
                                    int fukuyoNight = 0;
                                    int fukuyoSleep = 0;
                                    double suryo = odrInfDtl.Suryo;

                                    if (odrInfDtl.YohoKbn == 1)
                                    {
                                        string suffix = odrInfDtl.YohoSuffix;

                                        if (string.IsNullOrEmpty(suffix))
                                        {
                                            // suffixが未指定の場合、用法名から検索
                                            List<string> checkStrs = new List<string>
                                            {
                                                "食前","食後","食直前","食直後","食中","食間"
                                            };

                                            foreach (string checkStr in checkStrs)
                                            {
                                                if (odrInfDtl.ItemName.Contains(checkStr))
                                                {
                                                    suffix = checkStr;
                                                    break;
                                                }
                                            }
                                        }

                                        switch (i)
                                        {
                                            case 0:
                                                fukuyoRise = fukuyojis[i];
                                                itemName = "起床時";
                                                itemCd = itemName;
                                                break;
                                            case 1:
                                                fukuyoMorning = fukuyojis[i];
                                                itemName = "朝" + suffix;
                                                itemCd = itemName;
                                                break;
                                            case 2:
                                                fukuyoDaytime = fukuyojis[i];
                                                itemName = "昼" + suffix;
                                                itemCd = itemName;
                                                break;
                                            case 3:
                                                fukuyoNight = fukuyojis[i];
                                                if (string.IsNullOrEmpty(suffix))
                                                {
                                                    itemName = "夜";
                                                }
                                                else
                                                {
                                                    itemName = "夕" + suffix;
                                                }
                                                itemCd = itemName;
                                                break;
                                            case 4:
                                                fukuyoSleep = fukuyojis[i];
                                                itemName = "寝る前";
                                                itemCd = itemName;
                                                break;
                                        }
                                    }
                                    else if (odrInfDtl.DrugKbn > 0)
                                    {
                                        suryo = Math.Floor(suryo / totakuYoji * fukuyojis[i] * 100) / 100;
                                    }

                                    appendOdrDtls.Add(new CoOdrInfDetailModel(
                                        new OdrInfDetail()
                                        {
                                            HpId = odrInf.HpId,
                                            PtId = odrInf.PtId,
                                            SinDate = odrInf.SinDate,
                                            RaiinNo = odrInf.RaiinNo,
                                            RpNo = odrInf.RpNo,
                                            RpEdaNo = rpEdaNo,
                                            RowNo = rowNo++,
                                            SinKouiKbn = odrInfDtl.SinKouiKbn,
                                            ItemCd = itemCd,
                                            ItemName = itemName,
                                            Suryo = suryo,
                                            UnitName = odrInfDtl.UnitName,
                                            UnitSBT = odrInfDtl.UnitSBT,
                                            TermVal = odrInfDtl.TermVal,
                                            KohatuKbn = odrInfDtl.KohatuKbn,
                                            SyohoKbn = odrInfDtl.SyohoKbn,
                                            SyohoLimitKbn = odrInfDtl.SyohoLimitKbn,
                                            DrugKbn = odrInfDtl.DrugKbn,
                                            YohoKbn = odrInfDtl.YohoKbn,
                                            Kokuji1 = odrInfDtl.Kokuji1,
                                            Kokiji2 = odrInfDtl.Kokiji2,
                                            IsNodspRece = odrInfDtl.IsNodspRece,
                                            IpnCd = odrInfDtl.IpnCd,
                                            IpnName = odrInfDtl.IpnName,
                                            JissiKbn = odrInfDtl.JissiKbn,
                                            JissiDate = odrInfDtl.JissiDate,
                                            JissiId = odrInfDtl.JissiId,
                                            JissiMachine = odrInfDtl.JissiMachine,
                                            ReqCd = odrInfDtl.ReqCd,
                                            Bunkatu = odrInfDtl.Bunkatu,
                                            CmtName = odrInfDtl.CmtName,
                                            CmtOpt = odrInfDtl.CmtOpt,
                                            FontColor = odrInfDtl.FontColor
                                        },
                                    appendOdrInfs.Last().OdrInf,
                                    odrInfDtl.TenMst,
                                    odrInfDtl.YohoInfMst,
                                    _systemConfig
                                    )
                                    {
                                        FukuyoRise = fukuyoRise,
                                        FukuyoMorning = fukuyoMorning,
                                        FukuyoDaytime = fukuyoDaytime,
                                        FukuyoNight = fukuyoNight,
                                        FukuyoSleep = fukuyoSleep,
                                        IsIppoYoho = true
                                    });
                                }
                            }
                        }
                    }
                }
            }
            odrInfs.RemoveAll(p => p.Delete == true);
            odrInfDtls.RemoveAll(p => p.Delete == true);

            odrInfs.AddRange(appendOdrInfs);
            odrInfDtls.AddRange(appendOdrDtls);

            List<CoYakutaiModel> results = new List<CoYakutaiModel>();

            // 用法の種類を抽出する
            HashSet<string> yohoKeys = new HashSet<string>();

            foreach (CoOdrInfModel odrInf in odrInfs.OrderBy(p => p.OdrKouiKbn).ThenBy(p => p.SortNo).ThenBy(p => p.RpNo).ThenBy(p => p.RpEdaNo))
            {
                List<CoOdrInfDetailModel> filteredOdrDtls =
                    odrInfDtls.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo);
                if (filteredOdrDtls.Any(p =>
                        p.YohoKbn == 0 &&
                        p.ItemCd != ItemCdConst.ChusyaJikocyu &&
                        !(string.IsNullOrEmpty(p.ItemCd)) &&
                        !(p.TenMst != null && p.TenMst.MasterSbt == "C")))
                {
                    bool bYoho = false;
                    List<CoOdrInfDetailModel> yohoOdrInfDtls = new List<CoOdrInfDetailModel>();

                    foreach (CoOdrInfDetailModel odrInfDtl in odrInfDtls.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo).OrderBy(p => p.RowNo))
                    {
                        if (bYoho || odrInfDtl.YohoKbn > 0)
                        {
                            bYoho = true;
                            yohoOdrInfDtls.Add(odrInfDtl);
                        }
                        else if (odrInfDtl.ItemCd == ItemCdConst.ChusyaJikocyu)
                        {
                            bYoho = true;
                            odrInf.YohoKey = "9999999999,自己注射,9999999999";
                            yohoKeys.Add(odrInf.YohoKey);
                        }
                    }

                    if (yohoOdrInfDtls.Any())
                    {
                        yohoOdrInfDtls = yohoOdrInfDtls.OrderBy(p => p.ItemCd).ThenBy(p => p.ItemName).ToList();

                        string key = "";

                        foreach (CoOdrInfDetailModel yohoOdrInfDtl in yohoOdrInfDtls)
                        {
                            if (_systemConfig.YakutaiPrintUnit() == 1 && yohoOdrInfDtl.IsIppoYoho == false)
                            {
                                //Rp毎に印刷する
                                key += $"({yohoOdrInfDtl.RpNo},{yohoOdrInfDtl.ItemCd},{yohoOdrInfDtl.ItemName},{yohoOdrInfDtl.Suryo})";
                            }
                            else
                            {
                                //用法＋用法コメントが同じRpは１つにまとめる
                                key += $"({yohoOdrInfDtl.ItemCd},{yohoOdrInfDtl.ItemName},{yohoOdrInfDtl.Suryo})";
                            }
                        }

                        yohoKeys.Add(key);

                        odrInf.YohoKey = key;
                    }
                }
            }

            // 用法の種類ごとに、CoYakutaiModelを生成する
            foreach (string yohoKey in yohoKeys)
            {
                List<CoOdrInfModel> addOdrInfs = new List<CoOdrInfModel>();
                List<CoOdrInfDetailModel> addOdrInfDtls = new List<CoOdrInfDetailModel>();

                foreach (CoOdrInfModel yohoOdrInf in odrInfs.FindAll(p => p.YohoKey == yohoKey))
                {
                    addOdrInfs.Add(yohoOdrInf);
                    addOdrInfDtls.AddRange(odrInfDtls.FindAll(p => p.RpNo == yohoOdrInf.RpNo && p.RpEdaNo == yohoOdrInf.RpEdaNo));
                }

                //if (addOdrInfs.Any() && 
                //    addOdrInfDtls.Any(p=>
                //        p.YohoKbn == 0 && 
                //        !(string.IsNullOrEmpty(p.ItemCd)) && 
                //        !(p.TenMst != null && p.TenMst.MasterSbt == "C")))
                if (addOdrInfs.Any() && addOdrInfDtls.Any(p => p.YohoKbn == 0))
                {
                    // 用法・コメント以外のオーダーあり 
                    CoYakutaiModel result = new CoYakutaiModel(hpInf, ptInf, raiinInf, addOdrInfs, addOdrInfDtls, singleDoses);

                    if (addOdrInfDtls.Any(p => p.IsIppoYoho))
                    {
                        // 服用時点別一包化の用法の場合
                        result.IsFukuyojiIppo = true;
                    }
                    results.Add(result);
                }
            }

            return results;

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

        private void AddFileNamePageMap(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_fileNamePageMap.ContainsKey(field))
            {
                _fileNamePageMap.Add(field, value);
            }
        }

        private void GetRowCount(string formFileName)
        {
            List<ObjectCalculate> fieldInputList = new();

            fieldInputList.Add(new ObjectCalculate("lsOrder", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsSuryo", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsTani", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsYohoCmt", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsOrder", (int)CalculateTypeEnum.GetListRowCount));

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Yakutai, formFileName, fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _dataRowCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsOrder" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? 0;
            _dataCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsOrder" && item.typeInt == (int)CalculateTypeEnum.GetFormatLength)?.result ?? 0;
            _suryoCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsSuryo" && item.typeInt == (int)CalculateTypeEnum.GetFormatLength)?.result ?? 0;
            _unitCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsTani" && item.typeInt == (int)CalculateTypeEnum.GetFormatLength)?.result ?? 0;
            _yohoCmtCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsYohoCmt" && item.typeInt == (int)CalculateTypeEnum.GetFormatLength)?.result ?? 0;
        }

        private (string, string) GetFormFilePrinterName(CoYakutaiModel yakutaiModel)
        {
            string formFileName = YAKUTAI_FORM_FILE_NAME;
            string printerName = PrinterName;

            #region "local method"

            // 小中大のうち、条件に合うものを選択
            void _choiceSetting(
                double smallValue, string smallPrinter, string smallForm,
                double normalValue, string normalPrinter, string normalForm,
                double bigValue, string bigPrinter, string bigForm)
            {
                int ret = 0;

                // 小中大のうち、プリンタの指定がある設定で最小のものを初期値とする
                if (string.IsNullOrEmpty(smallPrinter) == false)
                {
                    // 小
                    ret = 1;
                }
                else if (string.IsNullOrEmpty(normalPrinter) == false)
                {
                    // 中
                    ret = 2;
                }
                else if (string.IsNullOrEmpty(bigPrinter) == false)
                {
                    // 大
                    ret = 3;
                }

                // 大中の最小服用量を超えたら、その設定を採用
                if (bigValue > 0 &&
                    string.IsNullOrEmpty(bigPrinter) == false &&
                    yakutaiModel.Fukuyoryo >= bigValue)
                {
                    // 大
                    ret = 3;
                }
                else if (normalValue > 0 &&
                    string.IsNullOrEmpty(normalPrinter) == false &&
                    yakutaiModel.Fukuyoryo >= normalValue)
                {
                    // 中
                    ret = 2;
                }

                // 判断結果から、フォームファイル、プリンタをセット
                switch (ret)
                {
                    case 1:
                        formFileName = smallForm;
                        printerName = smallPrinter;
                        break;
                    case 2:
                        formFileName = normalForm;
                        printerName = normalPrinter;
                        break;
                    case 3:
                        formFileName = bigForm;
                        printerName = bigPrinter;
                        break;
                }
            }
            #endregion

            if (_systemConfig.YakutaiPaperSize() == 1)
            {
                if (yakutaiModel.DrugKbnCd == OdrKouiKbnConst.Naifuku)
                {
                    // 内服
                    _choiceSetting(
                        _systemConfig.YakutaiNaifukuPaperSmallMinValue(), _systemConfig.YakutaiNaifukuPaperSmallPrinter(), YAKUTAI_NAIFUKU_SMALL_FORM_FILE_NAME,
                        _systemConfig.YakutaiNaifukuPaperNormalMinValue(), _systemConfig.YakutaiNaifukuPaperNormalPrinter(), YAKUTAI_NAIFUKU_NORMAL_FORM_FILE_NAME,
                        _systemConfig.YakutaiNaifukuPaperBigMinValue(), _systemConfig.YakutaiNaifukuPaperBigPrinter(), YAKUTAI_NAIFUKU_BIG_FORM_FILE_NAME
                        );
                }
                else if (yakutaiModel.DrugKbnCd == OdrKouiKbnConst.Tonpuku)
                {
                    // 頓服
                    _choiceSetting(
                        _systemConfig.YakutaiTonpukuPaperSmallMinValue(), _systemConfig.YakutaiTonpukuPaperSmallPrinter(), YAKUTAI_TONPUKU_SMALL_FORM_FILE_NAME,
                        _systemConfig.YakutaiTonpukuPaperNormalMinValue(), _systemConfig.YakutaiTonpukuPaperNormalPrinter(), YAKUTAI_TONPUKU_NORMAL_FORM_FILE_NAME,
                        _systemConfig.YakutaiTonpukuPaperBigMinValue(), _systemConfig.YakutaiTonpukuPaperBigPrinter(), YAKUTAI_TONPUKU_BIG_FORM_FILE_NAME
                        );
                }
                else if (yakutaiModel.DrugKbnCd == OdrKouiKbnConst.Gaiyo || yakutaiModel.DrugKbnCd == OdrKouiKbnConst.JikoCyu)
                {
                    // 外用
                    _choiceSetting(
                        _systemConfig.YakutaiGaiyoPaperSmallMinValue(), _systemConfig.YakutaiGaiyoPaperSmallPrinter(), YAKUTAI_GAIYO_SMALL_FORM_FILE_NAME,
                        _systemConfig.YakutaiGaiyoPaperNormalMinValue(), _systemConfig.YakutaiGaiyoPaperNormalPrinter(), YAKUTAI_GAIYO_NORMAL_FORM_FILE_NAME,
                        _systemConfig.YakutaiGaiyoPaperBigMinValue(), _systemConfig.YakutaiGaiyoPaperBigPrinter(), YAKUTAI_GAIYO_BIG_FORM_FILE_NAME
                        );
                }
            }

            return (formFileName, printerName);
        }
    }
}
