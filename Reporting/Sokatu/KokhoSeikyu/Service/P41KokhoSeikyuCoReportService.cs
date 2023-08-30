using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public class P41KokhoSeikyuCoReportService : IP41KokhoSeikyuCoReportService
    {
        #region Constant
        private const int myPrefNo = 41;
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private DB.ICoKokhoSeikyuFinder kokhoFinder;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private PrintUnit currentPrintUnit;
        private List<PrintUnit> printUnits;
        private List<CoHokensyaMstModel> hokensyaNames;
        private List<CoKohiHoubetuMstModel> kohiHoubetuMsts;
        private List<CoReceInfModel> receInfs;
        private CoHpInfModel hpInf;

        struct PrintUnit
        {
            public bool IsPrefIn;
            public bool IsKumiai;
            public string HokensyaNo;
            public int HokenRate;
        }
        #endregion

        #region Constructor and Init
        public P41KokhoSeikyuCoReportService()
        {
            dbService = EmrConnectionFactory.StartNew();
            kokhoFinder = new CoKokhoSeikyuFinder(dbService);
        }
        #endregion

        #region Override method
        public override bool UpdateCrForm()
        {
            if (receInfs == null || currentPrintUnit.AsString() == "") return false;
            bool hasNextPage;
            bool bRet = UpdateDrawForm(out hasNextPage);
            return bRet && hasNextPage;
        }
        #endregion

        #region Init properties
        private int seikyuYm;
        private SeikyuType seikyuType;
        private List<string> printHokensyaNos;

        public void InitParam(int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos)
        {
            this.seikyuYm = seikyuYm;
            this.seikyuType = seikyuType;
            this.printHokensyaNos = printHokensyaNos;

            Log.WriteLogMsg(
                ModuleName, this, nameof(printOut),
                string.Format(
                    "seikyuYm:{0} IsNormal:{1} IsPaper:{2} IsDelay:{3} IsHenrei:{4} IsOnLine:{5} printHokensyaNos:{6}",
                    seikyuYm, seikyuType.IsNormal, seikyuType.IsPaper, seikyuType.IsDelay, seikyuType.IsHenrei, seikyuType.IsOnline,
                    printHokensyaNos == null ? "" : string.Join(",", printHokensyaNos)
                )
            );
        }
        #endregion

        #region Printer method
        private CoOutputMode outputMode;
        private string printerName;
        CoFileType outputFileType;
        string outputDirectory;
        string outputFileName;

        public void PrintOut(string printerName)
        {
            outputMode = CoOutputMode.Print;
            this.printerName = printerName;

            printOut();
        }

        public void PrintOut(CoFileType fileType, string outputDirectory, string outputFileName)
        {
            outputMode = CoOutputMode.File;
            outputFileType = fileType;
            this.outputDirectory = outputDirectory;
            this.outputFileName = outputFileName;

            printOut();
        }

        public void printOut()
        {
            if (IsPrinterRunning) return;

            if (!GetData())
            {
                PrintExitCode = CoPrintExitCode.EndNoData;
                return;
            }
            IsPrinterRunning = true;

            Task printSeikyuTask = Task.Factory.StartNew(() =>
            {
                string formFile = Paths.P41KOKHOSEIKYU_P1_FORM_FILE_NAME;

                bool initResult = initPrint(CoOutputMode.File, formFile);
                if (!initResult) return;

                try
                {
                    //保険者単位で出力
                    foreach (PrintUnit currentUnit in printUnits)
                    {
                        currentPrintUnit = currentUnit;

                        initResult = initPrint(CoOutputMode.Print, formFile);
                        if (!initResult) return;

                        try
                        {
                            bool isNextPageExits = true;
                            CurrentPage = 1;
                            //印刷
                            while (isNextPageExits)
                            {
                                isNextPageExits = CoRep.PrintOut();
                                CurrentPage++;
                                if (CurrentPage >= 2)
                                {
                                    CoRep.OpenForm(Paths.P41KOKHOSEIKYU_P2_FORM_FILE_NAME);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLogError(ModuleName, this, nameof(printOut), ex);
                            PrintExitCode = CoPrintExitCode.EndError;
                            PrintExitMessage = ex.Message;
                        }
                        finally
                        {
                            finishPrint(CoOutputMode.Print);
                            CurrentPage = 1;
                        }
                    }
                }
                finally
                {
                    finishPrint(CoOutputMode.File);
                    IsPrinterRunning = false;
                }
            }, TaskCreationOptions.LongRunning);
            printSeikyuTask.ContinueWith((action) =>
            {
                printSeikyuTask.Dispose();
                PrintExitCode = CoPrintExitCode.EndSuccess;
                IsPrinterRunning = false;
            });
        }

        public bool IsPrinting()
        {
            return IsPrinterRunning;
        }

        public CoPrintExitCode ExitCode()
        {
            return PrintExitCode;
        }

        public string ExitMessage()
        {
            return PrintExitMessage;
        }
        #endregion

        #region Private function
        private bool initPrint(CoOutputMode executeMode, string formFile)
        {
            if (outputMode != executeMode)
            {
                if (executeMode == CoOutputMode.Print)
                {
                    CoRep.OpenForm(formFile);
                }
                return true;
            }

            bool initResult = false;
            if (executeMode == CoOutputMode.Print)
            {
                initResult = CoRep.IniParamPrinter(
                    Paths.SOKATU_FORM_PATH, formFile,
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    printerName
                );
            }
            else
            {
                initResult = CoRep.InitParamDocument(
                    Paths.SOKATU_FORM_PATH, formFile,
                    outputDirectory, outputFileName, (Hos.CnDraw.Constants.ConFileType)outputFileType
                );
            }
            return initResult;
        }

        private void finishPrint(CoOutputMode executeMode)
        {
            if (outputMode != executeMode) return;

            CoRep.FinishPrint();
        }

        private bool UpdateDrawForm(out bool hasNextPage)
        {
            bool _hasNextPage = true;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                //医療機関コード
                CoRep.SetFieldData("hpCode", hpInf.HpCd);

                //医療機関情報
                CoRep.SetFieldData("address1", hpInf.Address1);
                CoRep.SetFieldData("address2", hpInf.Address2);
                CoRep.SetFieldData("hpName", hpInf.ReceHpName);
                CoRep.SetFieldData("hpTel", hpInf.Tel);
                CoRep.SetFieldData("kaisetuName", hpInf.KaisetuName);
                //請求年月
                CIUtil.WarekiYmd wrkYmd = CIUtil.SDateToShowWDate3(seikyuYm * 100 + 1);
                CoRep.SetFieldData("seikyuGengo", wrkYmd.Gengo);
                CoRep.SetFieldData("seikyuYear", wrkYmd.Year);
                CoRep.SetFieldData("seikyuMonth", wrkYmd.Month);
                //提出年月日
                wrkYmd = CIUtil.SDateToShowWDate3(
                    CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd"))
                );
                CoRep.SetFieldData("reportGengo", wrkYmd.Gengo);
                CoRep.SetFieldData("reportYear", wrkYmd.Year);
                CoRep.SetFieldData("reportMonth", wrkYmd.Month);
                CoRep.SetFieldData("reportDay", wrkYmd.Day);
                //保険者
                string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == currentPrintUnit.HokensyaNo)?.Name ?? "";
                CoRep.SetFieldData("hokensyaName", hokensyaName);
                CoRep.SetFieldData("hokensyaNo", currentPrintUnit.HokensyaNo.PadLeft(6, '0'));
                //国保組合は給付割合に印をつける
                if (currentPrintUnit.IsKumiai)
                {
                    CoRep.SetFieldData("kyufu7", currentPrintUnit.HokenRate != 30 ? "×" : string.Empty);
                    CoRep.SetFieldData("kyufu8", currentPrintUnit.HokenRate == 20 ? "○" : string.Empty);
                    CoRep.SetFieldData("kyufu9", currentPrintUnit.HokenRate == 10 ? "○" : string.Empty);
                    CoRep.SetFieldData("kyufu10", currentPrintUnit.HokenRate == 100 ? "○" : string.Empty);
                }

                return 1;
            }
            #endregion

            #region BodyP1
            int UpdateFormBodyP1()
            {
                var curReceInfs = currentPrintUnit.IsKumiai ?
                    receInfs.Where(r => r.HokensyaNo == currentPrintUnit.HokensyaNo && r.HokenRate == currentPrintUnit.HokenRate) :
                    receInfs.Where(r => r.HokensyaNo == currentPrintUnit.HokensyaNo);

                const int maxRow = 9;

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        //国保
                        case 0: wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList(); break;
                        case 1: wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList(); break;
                        case 2: wrkReces = curReceInfs.Where(r => r.IsNrMine || r.IsNrFamily).ToList(); break;
                        case 3: wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList(); break;
                        //退職
                        case 4: wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList(); break;
                        case 5: wrkReces = curReceInfs.Where(r => r.IsRetElderIppan).ToList(); break;
                        case 6: wrkReces = curReceInfs.Where(r => r.IsRetElderUpper).ToList(); break;
                        case 7: wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList(); break;
                        case 8: wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    CoRep.ListText("count", 0, rowNo, wrkData.Count);
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.HokenNissu);
                    CoRep.ListText("nissu", 0, rowNo, wrkData.Nissu);
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.Tensu);
                    CoRep.ListText("tensu", 0, rowNo, wrkData.Tensu);
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.HokenReceFutan);
                    CoRep.ListText("futan", 0, rowNo, wrkData.Futan);

                }

                _hasNextPage = curReceInfs.Where(r => r.IsHeiyo).Count() >= 1;

                return 1;
            }
            #endregion

            #region BodyP2
            int UpdateFormBodyP2()
            {
                var curReceInfs = currentPrintUnit.IsKumiai ?
                    receInfs.Where(r => r.HokensyaNo == currentPrintUnit.HokensyaNo && r.HokenRate == currentPrintUnit.HokenRate) :
                    receInfs.Where(r => r.HokensyaNo == currentPrintUnit.HokensyaNo);

                const int maxKohiRow = 6;
                int kohiIndex = (CurrentPage - 2) * maxKohiRow;

                var kohiHoubetus = SokatuUtil.GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
                if (kohiHoubetus.Count == 0)
                {
                    _hasNextPage = false;
                    return 1;
                }

                //集計
                for (short rowNo = 0; rowNo < maxKohiRow; rowNo++)
                {
                    var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[kohiIndex])).ToList();

                    //法別番号
                    CoRep.ListText("kohiHoubetu", 0, rowNo, kohiHoubetus[kohiIndex]);
                    //制度略称
                    CoRep.SetFieldData(string.Format("kohiName{0}", rowNo), SokatuUtil.GetKohiName(kohiHoubetuMsts, myPrefNo, kohiHoubetus[kohiIndex]));

                    countData wrkData = new countData();
                    //件数
                    wrkData.Count = wrkReces.Count;
                    CoRep.ListText("kohiCount", 0, rowNo, wrkData.Count);
                    //日数
                    wrkData.Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[kohiIndex]));
                    CoRep.ListText("kohiNissu", 0, rowNo, wrkData.Nissu);
                    //点数
                    wrkData.Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[kohiIndex]));
                    CoRep.ListText("kohiTensu", 0, rowNo, wrkData.Tensu);
                    //一部負担金
                    wrkData.Futan = wrkReces.Sum(r => r.SagaKohiReceFutan(kohiHoubetus[kohiIndex]));
                    CoRep.ListText("kohiFutan", 0, rowNo, wrkData.Futan);
                    //患者負担額
                    wrkData.PtFutan = wrkReces.Sum(r => r.SagaKohiIchibuFutan(kohiHoubetus[kohiIndex]));
                    CoRep.ListText("kohiPtFutan", 0, rowNo, wrkData.PtFutan);

                    kohiIndex++;
                    if (kohiIndex >= kohiHoubetus.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                return 1;
            }
            #endregion

            #endregion

            try
            {
                switch (CurrentPage)
                {
                    case 1:
                        if (UpdateFormHeader() < 0 || UpdateFormBodyP1() < 0)
                        {
                            hasNextPage = _hasNextPage;
                            return false;
                        }
                        break;
                    default:
                        if (UpdateFormHeader() < 0 || UpdateFormBodyP2() < 0)
                        {
                            hasNextPage = _hasNextPage;
                            return false;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Log.WriteLogError(ModuleName, this, nameof(UpdateDrawForm), e);
                hasNextPage = _hasNextPage;
                return false;
            }

            hasNextPage = _hasNextPage;
            return true;
        }

        private bool GetData()
        {
            hpInf = kokhoFinder.GetHpInf(seikyuYm);
            receInfs = kokhoFinder.GetReceInf(seikyuYm, seikyuType, KokhoKind.Kokho, PrefKbn.PrefAll, myPrefNo, HokensyaNoKbn.SumAll);
            //保険者番号の指定がある場合は絞り込み
            var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
                receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();

            //保険者番号リストを取得
            printUnits = wrkReceInfs.Where(r => !r.IsKumiai)
                .GroupBy(r => new { r.IsPrefIn, r.HokensyaNo })
                .Select(r => new PrintUnit { IsKumiai = false, IsPrefIn = r.Key.IsPrefIn, HokensyaNo = r.Key.HokensyaNo, HokenRate = -1 })
                .ToList();

            //国保組合は給付割合ごとに用紙を変えて印刷する
            printUnits.AddRange(
                wrkReceInfs.Where(r => r.IsKumiai)
                .GroupBy(r => new { r.IsPrefIn, r.HokensyaNo, r.HokenRate })
                .Select(r => new PrintUnit { IsKumiai = true, IsPrefIn = r.Key.IsPrefIn, HokensyaNo = r.Key.HokensyaNo, HokenRate = r.Key.HokenRate })
                .ToList()
            );

            //県外→県内
            printUnits = printUnits.OrderBy(p => p.IsPrefIn).ThenBy(p => p.HokensyaNo).ThenByDescending(p => p.HokenRate).ToList();

            //保険者名リスト取得
            var hokensyaNos = printUnits.Select(p => p.HokensyaNo).ToList();
            hokensyaNames = kokhoFinder.GetHokensyaName(hokensyaNos);

            //公費法別番号リストを取得
            kohiHoubetuMsts = kokhoFinder.GetKohiHoubetuMst(seikyuYm);

            return (wrkReceInfs?.Count ?? 0) > 0;
        }
        #endregion

        public CommonReportingRequestModel GetP40KokhoSeikyuKumiaiReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos)
        {
            throw new NotImplementedException();
        }
    }
}
