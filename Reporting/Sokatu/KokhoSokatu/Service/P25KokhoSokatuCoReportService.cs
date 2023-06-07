using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public class P25KokhoSokatuCoReportService : IP25KokhoSokatuCoReportService
{
    /*#region Constant
    private const int MyPrefNo = 25;
    //県内保険者(固定枠)
    private List<string> prefInHokensyaNo = new List<string> {
            "253013", "250019", "250027", "250035", "250043", "250050", "250068", "250076", "250092", "250100",
            "250118", "250126", "250134", "250522", "250647", "250654", "250712", "250738", "250746", "250753"
        };
    //県外保険者(固定枠)
    private List<string> prefOutHokensyaNo = new List<string> {
            "095013", "133033", "133231", "133264", "133280", "133298", "233064", "263129", "273102"
        };
    //福祉
    private List<string> kohiHoubetus = new List<string> {
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "70", "71", "75", "76", "82", "83", "84", "85", "86"
        };
    #endregion

    #region Private properties
    /// <summary>
    /// Finder
    /// </summary>
    private ICoKokhoSokatuFinder _kokhoFinder;
    private ICoWelfareSeikyuFinder _welfareFinder;

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<string> hokensyaNos;
    private List<CoHokensyaMstModel> hokensyaNames;
    private List<CoReceInfModel> receInfs;
    private List<CoWelfareReceInfModel> welfareInfs;
    private CoHpInfModel hpInf;
    #endregion

    #region Constructor and Init
    public P25KokhoSokatuCoReportService() : base()
    {
        dbService = EmrConnectionFactory.StartNew();
        kokhoFinder = new CoKokhoSokatuFinder(dbService);
        welfareFinder = new CoWelfareSeikyuFinder(dbService);
    }
    #endregion

    #region Override method
    public override bool UpdateCrForm()
    {
        if (receInfs == null) return false;
        bool hasNextPage;
        bool bRet = UpdateDrawForm(out hasNextPage);
        return bRet && hasNextPage;
    }
    #endregion

    #region Init properties
    private int seikyuYm;
    private SeikyuType seikyuType;
    int diskKind;
    int diskCnt;

    public void InitParam(int seikyuYm, SeikyuType seikyuType, int diskKind, int diskCnt)
    {
        this.seikyuYm = seikyuYm;
        this.seikyuType = seikyuType;
        this.diskKind = diskKind;
        this.diskCnt = diskCnt;

        Log.WriteLogMsg(
            ModuleName, this, nameof(printOut),
            string.Format(
                "seikyuYm:{0} IsNormal:{1} IsPaper:{2} IsDelay:{3} IsHenrei:{4} IsOnLine:{5} diskKind:{6} diskCnt:{7}",
                seikyuYm, seikyuType.IsNormal, seikyuType.IsPaper, seikyuType.IsDelay, seikyuType.IsHenrei, seikyuType.IsOnline,
                diskKind, diskCnt
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

    private void printOut()
    {
        if (IsPrinterRunning) return;

        //媒体種類を記載するためにデータの有無に関わらず印刷する
        GetData();
        IsPrinterRunning = true;

        Task printSokatuTask = Task.Factory.StartNew(() =>
        {
            string formFile = Paths.P25KOKHOSOKATU_FORM_FILE_NAME;

            bool initResult = false;
            if (outputMode == CoOutputMode.Print)
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
                CoRep.FinishPrint();
                CurrentPage = 1;
                IsPrinterRunning = false;
            }
        }, TaskCreationOptions.LongRunning);
        printSokatuTask.ContinueWith((action) =>
        {
            printSokatuTask.Dispose();
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
    #endregion*/

    public CommonReportingRequestModel GetP25KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int diskKind, int diskCnt)
    {
        throw new NotImplementedException();
    }

   /* #region Private function
    private bool UpdateDrawForm(out bool hasNextPage)
    {
        bool _hasNextPage = true;

        #region SubMethod

        #region Header
        int UpdateFormHeader()
        {
            //医療機関コード
            CoRep.SetFieldData("hpCode", hpInf.ReceHpCd);
            //医療機関情報
            CoRep.SetFieldData("address1", hpInf.Address1);
            CoRep.SetFieldData("address2", hpInf.Address2);
            CoRep.SetFieldData("hpName", hpInf.ReceHpName);
            CoRep.SetFieldData("kaisetuName", hpInf.KaisetuName);
            CoRep.SetFieldData("hpTel", hpInf.Tel);
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
            //レセプト記載
            if (new int[] { 0, 1, 2 }.Contains(diskKind))
            {
                CoRep.SetFieldData("receMedia", "〇");
            }
            else
            {
                CoRep.SetFieldData("receOnline", "〇");
            }

            return 1;
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            #region Body
            if (CurrentPage == 1)
            {
                //1枚目のみ記載する

                //県内保険者
                for (short rowNo = 0; rowNo < prefInHokensyaNo.Count; rowNo++)
                {
                    int receCount = receInfs.Where(r => r.HokensyaNo == prefInHokensyaNo[rowNo]).Count();
                    CoRep.ListText(string.Format("prefInCount{0}", (short)Math.Floor((double)rowNo / 17)), 0, (short)(rowNo % 17), receCount);
                }
                //県外保険者(国保)
                for (short rowNo = 0; rowNo < prefOutHokensyaNo.Count; rowNo++)
                {
                    int receCount = receInfs.Where(r => r.HokensyaNo == prefOutHokensyaNo[rowNo]).Count();
                    CoRep.ListText("prefOutFixedCount", 0, rowNo, receCount);
                }
                //合計
                for (short rowNo = 0; rowNo < 4; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = null;
                    switch (rowNo)
                    {
                        case 0: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && r.IsPrefIn).ToList(); break;
                        case 1: wrkReces = receInfs.Where(r => r.IsKoukiAll && r.IsPrefIn).ToList(); break;
                        case 2: wrkReces = receInfs.Where(r => (r.IsNrAll || r.IsRetAll) && !r.IsPrefIn).ToList(); break;
                        case 3: wrkReces = receInfs.Where(r => r.IsKoukiAll && !r.IsPrefIn).ToList(); break;
                    }
                    if (wrkReces == null) continue;

                    CoRep.ListText("totalCount", 0, rowNo, wrkReces.Count);
                }
                //磁気媒体種類・枚数
                if (new int[] { 0, 1, 2 }.Contains(diskKind))
                {
                    CoRep.SetFieldData(string.Format("diskKind{0}", diskKind), "〇");
                    CoRep.SetFieldData("diskCnt", diskCnt);
                }
                //福祉医療費請求書                    
                CoRep.SetFieldData("welfarePaperCnt", Math.Ceiling((double)welfareInfs.Count / 6));
            }
            #endregion

            #region 県外保険者
            //県外保険者(国保・固定枠除く)
            const int maxKokhoRow = 4;
            int kokhoIndex = (CurrentPage - 1) * maxKokhoRow;

            var kokhoNos = receInfs.Where(
                r => (r.IsNrAll || r.IsRetAll) && !r.IsPrefIn && !prefOutHokensyaNo.Contains(r.HokensyaNo)
            ).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            bool kokhoNextPage = true;
            for (short rowNo = 0; rowNo < maxKokhoRow; rowNo++)
            {
                if (kokhoIndex < kokhoNos.Count)
                {
                    string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == kokhoNos[kokhoIndex])?.Name ?? "";
                    CoRep.ListText("prefOutName", 0, rowNo, hokensyaName == "" ? kokhoNos[kokhoIndex] : hokensyaName);
                    CoRep.ListText("prefOutNo", 0, rowNo, kokhoNos[kokhoIndex]);

                    int receCount = receInfs.Where(r => r.HokensyaNo == kokhoNos[kokhoIndex]).Count();
                    CoRep.ListText("prefOutCount", 0, rowNo, receCount);
                }

                kokhoIndex++;
                if (kokhoIndex >= kokhoNos.Count)
                {
                    kokhoNextPage = false;
                    break;
                }
            }

            //県外保険者(後期)
            const int maxKoukiRow = 5;
            int koukiIndex = (CurrentPage - 1) * maxKoukiRow;

            var koukiNos = receInfs.Where(
                r => r.IsKoukiAll && !r.IsPrefIn
            ).GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            if (kokhoNos.Count == 0 && koukiNos.Count == 0)
            {
                _hasNextPage = false;
                return 1;
            }

            bool koukiNextPage = true;
            for (short rowNo = 0; rowNo < maxKoukiRow; rowNo++)
            {
                if (koukiIndex < koukiNos.Count)
                {
                    int prefNo = koukiNos[koukiIndex].Substring(koukiNos[koukiIndex].Length - 6, 2).AsInteger();
                    CoRep.ListText("koukiName", 0, rowNo, PrefCode.PrefName(prefNo));
                    CoRep.ListText("koukiNo", 0, rowNo, koukiNos[koukiIndex]);

                    int receCount = receInfs.Where(r => r.HokensyaNo == koukiNos[koukiIndex]).Count();
                    CoRep.ListText("koukiCount", 0, rowNo, receCount);
                }

                koukiIndex++;
                if (koukiIndex >= koukiNos.Count)
                {
                    koukiNextPage = false;
                    break;
                }
            }

            if (!kokhoNextPage && !koukiNextPage)
            {
                _hasNextPage = false;
            }
            #endregion

            return 1;
        }
        #endregion

        #endregion

        try
        {
            if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
            {
                hasNextPage = _hasNextPage;
                return false;
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
        receInfs = kokhoFinder.GetReceInf(seikyuYm, seikyuType, KokhoKind.All, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.SumAll);
        //保険者番号リストを取得
        hokensyaNos = receInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();
        //保険者名を取得
        hokensyaNames = kokhoFinder.GetHokensyaName(hokensyaNos);

        //福祉           
        SeikyuType welSeikyutype = new SeikyuType(
            isNormal: true, isPaper: true, isDelay: true, isHenrei: true, isOnline: false
        );
        welfareInfs = welfareFinder.GetReceInf(seikyuYm, welSeikyutype, kohiHoubetus, FutanCheck.KohiFutan, HokenKbn.Syaho);

        return (receInfs?.Count ?? 0) > 0;
    }
    #endregion*/
}
