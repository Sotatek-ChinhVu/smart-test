using Domain.Constant;
using Helper.Common;
using Reporting.Karte1.DB;
using Reporting.Karte1.Mapper;
using Reporting.Karte1.Model;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.ReceTarget.Mapper;

namespace Reporting.Karte1.Service;

public class Karte1Service : IKarte1Service
{
    private readonly ICoKarte1Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<int, ReportConfigModel> _reportConfigModelPerPage;
    private CoKarte1Model coModel;
    private List<CoKarte1PrintDataModel> printOutData;

    private List<string> objectNamePage1;
    private List<ObjectCalculateResponse> responsesPage1;
    private List<string> objectNamePage2;
    private List<ObjectCalculateResponse> responsesPage2;

    private int dataCharCount;
    private int dataRowCount;
    private int dataTarget;
    private int dataCharCountP2;
    private int dataRowCountP2;
    private int dataTargetP2;
    private DateTime printoutDateTime;

    private int hpId;
    private long ptId;
    private int sinDate;
    private int hokenPid;
    private bool syuByomei;
    private bool tenkiByomei;
    private int currentPage;
    private bool hasNextPage;

    public Karte1Service(ICoKarte1Finder finder, IReadRseReportFileService readRseReportFileService)
    {
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        _reportConfigModelPerPage = new();
        _singleFieldData = new();
        _listTextData = new();
        _extralData = new();
        coModel = new();
        printOutData = new();
        objectNamePage1 = new();
        objectNamePage2 = new();
        responsesPage1 = new();
        responsesPage2 = new();
    }

    public CommonReportingRequestModel GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
    {
        try
        {
            this.hpId = hpId;
            this.ptId = ptId;
            this.sinDate = sinDate;
            this.hokenPid = hokenPid;
            this.syuByomei = syuByomei;
            this.tenkiByomei = tenkiByomei;

            currentPage = 1;

            dataCharCount = 0;
            dataRowCount = 0;
            dataTarget = 0;
            dataCharCountP2 = 0;
            dataRowCountP2 = 0;
            dataTargetP2 = 0;

            coModel = GetData();

            if (this.ptId != 0 && coModel != null)
            {
                ReadRseFileP1();
                ReadRseFileP2();

                if (SetListProperty("lsByomei", ref dataCharCount, ref dataRowCount))
                {
                    dataTarget = 1;
                }
                else if (SetDataFieldProperty("ByoMei_", ref dataCharCount, ref dataRowCount))
                {
                    dataTarget = 2;
                }
                else if (SetDataFieldProperty("ByoMeiH_", ref dataCharCount, ref dataRowCount))
                {
                    dataTarget = 3;
                }

                currentPage = 2;

                if (SetListProperty("lsByomei", ref dataCharCountP2, ref dataRowCountP2))
                {
                    dataTargetP2 = 1;
                }
                else if (SetDataFieldProperty("ByoMei_", ref dataCharCountP2, ref dataRowCountP2))
                {
                    dataTargetP2 = 2;
                }
                else if (SetDataFieldProperty("ByoMeiH_", ref dataCharCountP2, ref dataRowCountP2))
                {
                    dataTargetP2 = 3;
                }

                printOutData = new List<CoKarte1PrintDataModel>();

                if (dataTarget > 0)
                {
                    MakePrintDataList();
                }

                printoutDateTime = CIUtil.GetJapanDateTimeNow();
                currentPage = 1;
                hasNextPage = true;
                while (hasNextPage)
                {
                    UpdateDrawForm();
                    currentPage++;
                }
            }

            _extralData.Add("totalPage", (currentPage - 1).ToString());
            return new Karte1Mapper(_extralData, _singleFieldData, _listTextData, _reportConfigModelPerPage).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }

    #region sub method
    private bool SetListProperty(string field, ref int charcount, ref int rowcount)
    {
        bool ret = false;
        var objectName = currentPage == 1 ? objectNamePage1 : objectNamePage2;
        var responses = currentPage == 1 ? responsesPage1 : responsesPage2;
        if (objectName.Contains(field))
        {
            ret = true;
            charcount = responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetFormatLendB && item.listName == field)!.result;
            rowcount = responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetListRowCount && item.listName == field)!.result;
        }
        return ret;
    }

    private bool SetDataFieldProperty(string field, ref int charcount, ref int rowcount)
    {
        bool ret = false;
        var objectName = currentPage == 1 ? objectNamePage1 : objectNamePage2;
        var responses = currentPage == 1 ? responsesPage1 : responsesPage2;
        if (objectName.Contains($"{field}1"))
        {
            ret = true;
            charcount = responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetFormatLendB && item.listName == $"{field}1")!.result;
            int i = 1;
            while (objectName.Contains($"{field}{i}"))
            {
                i++;
            }
            rowcount = i - 1;
        }
        return ret;
    }
    #endregion

    private void MakePrintDataList()
    {
        int rowCount = 0;
        int charCount = 0;
        int listRow = 0;
        int page = 0;

        if (dataRowCount > 0)
        {
            page = 1;
            rowCount = dataRowCount;
            charCount = dataCharCount;
        }
        else if (dataRowCountP2 > 0)
        {
            page = 2;
            rowCount = dataRowCountP2;
            charCount = dataCharCountP2;
        }
        else if (dataRowCount <= 0 && dataRowCountP2 <= 0)
        {
            return;
        }

        #region sub method

        List<CoKarte1PrintDataModel> _addList(string str, bool existNextByomei)
        {
            List<CoKarte1PrintDataModel> addPrintOutData = new List<CoKarte1PrintDataModel>();

            string line = str;
            int maxLength = charCount;

            while (line != string.Empty)
            {
                string tmp = line;
                if (CIUtil.LenB(line) > maxLength)
                {
                    // 文字列が最大幅より広い場合、カット
                    tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
                }

                CoKarte1PrintDataModel printOutData = new CoKarte1PrintDataModel();
                printOutData.Byomei = tmp;
                addPrintOutData.Add(printOutData);

                // 今回出力分の文字列を削除
                line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));

                listRow++;

                if (page == 1 && rowCount <= listRow)
                {
                    // 1ページ目の行数を超える場合
                    if (dataRowCountP2 > 0)
                    {
                        // 2ページ目以降がある場合
                        page = 2;
                        listRow = 0;
                        rowCount = dataRowCountP2;
                        charCount = dataCharCountP2;
                    }
                    else if (existNextByomei || line != string.Empty)
                    {
                        // 2ページ目以降がない場合で、次の病名が存在する場合、または病名の途中
                        addPrintOutData.Last().Byomei = CIUtil.CiCopyStrWidth(addPrintOutData.Last().Byomei, 1, maxLength - 4) + "　他";
                        break;
                    }
                }
            }

            return addPrintOutData;
        }
        #endregion

        for (int i = 0; i < coModel.PtByomeiModels.Count; i++)
        {
            List<CoKarte1PrintDataModel> addData = new List<CoKarte1PrintDataModel>();
            CoPtByomeiModel ptByomei = coModel.PtByomeiModels[i];

            string addByomei = ptByomei.Byomei;

            if (syuByomei && ptByomei.SyobyoKbn == 1)
            {
                addByomei = "（主）" + addByomei;
            }

            if (dataTarget != 2 && ptByomei.HosokuCmt.Trim() != string.Empty)
            {
                addByomei += "（" + ptByomei.HosokuCmt.Trim() + "）";
            }
            addData.AddRange(_addList(addByomei, i < coModel.PtByomeiModels.Count - 1));
            addData.Last().StartDate = ptByomei.StartDate;
            addData.Last().TenkiKbn = ptByomei.TenkiKbn;
            addData.Last().TenkiDate = ptByomei.TenkiDate;

            printOutData.AddRange(addData);

            if (page == 1 && printOutData.Count >= rowCount)
            {
                if (dataRowCountP2 > 0)
                {
                    page = 2;
                    rowCount = dataRowCountP2;
                    charCount = dataCharCountP2;
                }
                else
                {
                    break;
                }
            }
        }

    }

    private void UpdateDrawForm()
    {
        List<ListTextObject> listDataPerPage = new();
        Dictionary<string, bool> visibleFieldList = new();

        if (printOutData == null || printOutData.Count == 0)
        {
            hasNextPage = false;
            return;
        }

        #region SubMethod
        // ヘッダー
        void UpdateFormHeader()
        {
            #region sub method
            // システム日付（西暦年）
            string _getSysYearS()
            {
                return printoutDateTime.ToString("yyyy");
            }
            // システム日付（和暦年）
            string _getSysYearW()
            {
                return printoutDateTime.ToString("yy");
            }
            // システム日付（月）
            string _getSysMonth()
            {
                return printoutDateTime.ToString("MM");
            }
            // システム日付（日）
            string _getSysDay()
            {
                return printoutDateTime.ToString("dd");
            }
            #endregion

            #region print method
            // システム日付
            void _pirntSysDate()
            {
                // システム日時
                SetFieldData("dfSysDateTimeS", printoutDateTime.ToString("yyyy/MM/dd HH:mm"));
                SetFieldData("dfSysDateTimeW", CIUtil.SDateToShowWDate3(CIUtil.DateTimeToInt(printoutDateTime)).Ymd + printoutDateTime.ToString(" HH:mm"));

                // システム日付（西暦年）
                SetFieldData("dfSysYearS", _getSysYearS());
                // システム日付（和暦年）
                SetFieldData("dfSysYearW", _getSysYearW());
                // システム日付（月）
                SetFieldData("dfSysMonth", _getSysMonth());
                // システム日付（日）
                SetFieldData("dfSysDay", _getSysDay());

                // 下位互換
                // システム日付（西暦年）
                SetFieldData("SYS_YEAR", _getSysYearS());
                // システム日付（和暦年）
                SetFieldData("SYS_JPYEAR", _getSysYearW());
                // システム日付（月）
                SetFieldData("SYS_MONTH", _getSysMonth());
                // システム日付（日）
                SetFieldData("SYS_DAY", _getSysDay());

            }
            // 患者番号
            void _printPtNum()
            {
                SetFieldData("dfPtNum", coModel.PtNum);
                SetFieldData("bcPtNum", coModel.PtNum);
                // 下位互換
                SetFieldData("KaruteNo", coModel.PtNum);
                SetFieldData("KanID", coModel.PtNum);
                SetFieldData("KanID_BC", coModel.PtNum);
            }

            // 患者氏名
            void _printPtName()
            {
                SetFieldData("dfPtName", coModel.PtName);
                // 下位互換
                SetFieldData("KjNM", coModel.PtName);
            }
            // 患者カナ氏名
            void _printPtKanaName()
            {
                SetFieldData("dfPtKanaName", coModel.PtKanaName);
                // 下位互換
                SetFieldData("KanaNM", coModel.PtKanaName);
            }
            // 患者生年月日
            void _printPtBirthDay()
            {
                // 元号〇
                SetFieldData($"dfBirthGengoMaru{CIUtil.SDateToWDate(coModel.BirthDay) / 1000000}", "〇");
                // 下位互換
                SetFieldData("Gen_" + CIUtil.SDateToWDate(coModel.BirthDay), "〇");

                // 元号
                CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(coModel.BirthDay);

                SetFieldData("dfBirthGengo", wareki.Gengo);
                // 下位互換
                SetFieldData("Gen", wareki.Gengo);
                SetFieldData("BirthGen", wareki.Gengo);

                // 西暦年
                SetFieldData("dfBirthYearS", coModel.BirthDay / 10000);

                // 和暦年
                SetFieldData("dfBirthYearW", wareki.Year);
                // 下位互換
                SetFieldData("BirthNen", wareki.Year);

                // 月
                SetFieldData("dfBirthMonth", wareki.Month);
                // 下位互換
                SetFieldData("BirthTuki", wareki.Month);

                // 日
                SetFieldData("dfBirthDay", wareki.Day);
                // 下位互換
                SetFieldData("BirthHi", wareki.Day);

                // 年月日
                SetFieldData("dfBirthDateS", CIUtil.SDateToShowSDate(coModel.BirthDay));
                SetFieldData("dfBirthDateW", wareki.Ymd);
                // 下位互換
                SetFieldData("BirthYmd", wareki.Ymd);
            }
            // 年齢
            void _printPtAge()
            {
                SetFieldData("dfAge", coModel.Age);
                // 下位互換
                SetFieldData("Age", coModel.Age);
            }
            // 性別
            void _printPtSex()
            {
                // 〇
                SetFieldData($"dfSexMaru{coModel.Sex}", "〇");

                SetFieldData("dfSex", coModel.PtSex);
                // 下位互換
                if (coModel.Sex == 1)
                {
                    SetFieldData("Sex_M", "〇");
                }
                else
                {
                    SetFieldData("Sex_F", "〇");
                }
                SetFieldData("Sex", coModel.PtSex);

            }
            // 患者郵便番号
            void _printPtPostCd()
            {
                SetFieldData("dfPtPostCode", coModel.PtPostCdDsp);
                // 下位互換
                SetFieldData("Zip", coModel.PtPostCdDsp);
            }
            // 患者住所
            void _printPtHomeAddress()
            {
                SetFieldData("dfPtAddress", coModel.PtHomeAddress);
                SetFieldData("dfPtAddress1", coModel.PtHomeAddress1);
                SetFieldData("dfPtAddress2", coModel.PtHomeAddress2);
            }
            void _printPtHomeAddressWordWrap()
            {
                // 下位互換
                string value = coModel.PtHomeAddress;
                for (int i = 1; i <= 3; i++)
                {
                    string outputValue = CIUtil.CiCopyStrWidthDst(value, 0, 34, 0, ref value);

                    SetFieldData($"Address{i}", outputValue);
                }
            }
            // 患者電話番号
            void _printPtTel()
            {
                void _printSplitTel(string field, string tel)
                {
                    string[] tmp = tel.Split('-');
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        SetFieldData($"{field}{i + 1}", tmp[i]);
                    }

                }

                SetFieldData("dfPtTel", coModel.PtTel);
                SetFieldData("dfPtTel1", coModel.PtTel1);
                SetFieldData("dfPtTel2", coModel.PtTel2);
                SetFieldData("dfPtRenrakuTel", coModel.PtRenrakuTel);

                _printSplitTel("dfPtTel1_", coModel.PtTel1);
                _printSplitTel("dfPtTel2_", coModel.PtTel1);
                _printSplitTel("dfPtRenrakuTel_", coModel.PtRenrakuTel);

                // 下位互換
                SetFieldData("TEL1", coModel.PtTel);
                SetFieldData("TEL_Jitaku", coModel.PtTel1);
                SetFieldData("TEL_Keitai", coModel.PtTel2);
                SetFieldData("TEL_Renraku", coModel.PtRenrakuTel);
                SetFieldData("TEL_Kinkyu", coModel.PtRenrakuTel);

                _printSplitTel("TEL_Jitaku", coModel.PtTel1);
                _printSplitTel("TEL_Keitai", coModel.PtTel1);
                _printSplitTel("TEL_Renraku", coModel.PtRenrakuTel);

            }
            // 世帯主
            void _printSetaiNusi()
            {
                SetFieldData("dfSetainusi", coModel.SetaiNusi);
                // 下位互換
                SetFieldData("HiHoNM2", coModel.SetaiNusi);

                if (coModel.HonkeKbn == 2)
                {
                    SetFieldData("dfSetaiNusiKazoku", coModel.SetaiNusi);
                    // 下位互換
                    SetFieldData("HiHoNM", coModel.SetaiNusi);
                }
            }
            // 記号番号
            void _printKigoBango()
            {
                SetFieldData("dfKigoBango", coModel.KigoBango);
                SetFieldData("dfKigo", coModel.Kigo);
                SetFieldData("dfBango", coModel.Bango);
                SetFieldData("dfEdaban", coModel.EdaNo);

                // 下位互換
                SetFieldData("Ho_KigoBango_8", coModel.KigoBango);
                SetFieldData("Ho_Kigo_10", coModel.Kigo);
                SetFieldData("Ho_Bango", coModel.Bango);
                SetFieldData("Ho_Edaban", coModel.EdaNo);
            }
            // 保険有効期限
            void _printHokenKigen()
            {
                SetFieldData("dfHokenKigenS", CIUtil.SDateToShowSDate(coModel.HokenEndDate));

                CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(coModel.HokenEndDate);

                SetFieldData("dfHokenKigenW", wareki.Ymd);

                SetFieldData("dfHokenKigenYearS", coModel.HokenEndDate / 10000);
                SetFieldData("dfHokenKigenGengo", wareki.Gengo);
                SetFieldData("dfHokenKigenYearW", wareki.Year);
                SetFieldData("dfHokenKigenMonth", wareki.Month);
                SetFieldData("dfHokenKigenDay", wareki.Day);


                // 下位互換
                SetFieldData("Ho_YukoYmd", CIUtil.SDateToShowWDate3(coModel.HokenEndDate).Ymd);
                SetFieldData("Ho_YukoGen", wareki.Gengo);
                SetFieldData("Ho_YukoNen", wareki.Year);
                SetFieldData("Ho_YukoTuki", wareki.Month);
                SetFieldData("Ho_YukoHi", wareki.Day);

            }
            // 保険取得日
            void _printHokenSyutoku()
            {
                SetFieldData("dfHokenSyutokuS", CIUtil.SDateToShowSDate(coModel.HokenSyutokuDate));

                CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(coModel.HokenSyutokuDate);

                SetFieldData("dfHokenSyutokuW", wareki.Ymd);

                SetFieldData("dfHokenSyutokuYearS", coModel.HokenSyutokuDate / 10000);
                SetFieldData("dfHokenSyutokuGengo", wareki.Gengo);
                SetFieldData("dfHokenSyutokuYearW", wareki.Year);
                SetFieldData("dfHokenSyutokuMonth", wareki.Month);
                SetFieldData("dfHokenSyutokuDay", wareki.Day);

                // 下位互換
                SetFieldData("Ho_GetYmd", CIUtil.SDateToShowWDate3(coModel.HokenSyutokuDate).Ymd);
                SetFieldData("Ho_GetGen", wareki.Gengo);
                SetFieldData("Ho_GetNen", wareki.Year);
                SetFieldData("Ho_GetTuki", wareki.Month);
                SetFieldData("Ho_GetHi", wareki.Day);
            }
            // 保険者番号
            void _printHokensyaNo()
            {
                string value = coModel.HokensyaNo.PadLeft(8, ' ');
                SetFieldData("dfHokensyaNo", value);
                // 下位互換
                short i = 0;
                while (i < 8 && i < value.Length)
                {
                    listDataPerPage.Add(new("Ho_No", i, 0, value[i].ToString()));
                    i++;
                }
            }
            // 本人家族
            void _printHonke()
            {
                if (coModel.HonkeKbn == 2)
                {
                    SetFieldData("dfHonkeMaru2", "〇");
                    SetFieldData("dfHonke", "家族");
                    // 下位互換
                    SetFieldData("HonKaKb_Ka", "〇");
                }
                else
                {
                    SetFieldData("dfHonkeMaru1", "〇");
                    SetFieldData("dfHonke", "本人");
                    // 下位互換
                    SetFieldData("HonKaKb_Hon", "〇");
                }
            }
            // 保険種別
            void _printHokenSbt()
            {
                SetFieldData("dfHokenSbt", coModel.HokenSbt);
                // 下位互換
                SetFieldData("HoKind", coModel.HokenSbt);
            }

            // 患者負担率
            void _printFutanRate()
            {
                SetFieldData("dfFutanRate", coModel.FutanRate == null ? string.Empty : coModel.FutanRate.ToString() ?? string.Empty);
                // 下位互換
                SetFieldData("Ho_Futan", coModel.FutanRate == null ? string.Empty : coModel.FutanRate.ToString() ?? string.Empty);
            }
            // 保険給付率
            void _printKyufuRate()
            {
                SetFieldData("dfKyufuRate", coModel.KyufuRate);
                // 下位互換
                SetFieldData("Ho_Kyufu", coModel.KyufuRate);
            }
            // 保険者名
            void _printHokensyaName()
            {
                SetFieldData("dfHokensyaName", coModel.HokensyaName);
                // 下位互換
                SetFieldData("HokenNM", coModel.HokensyaName);
            }
            // 保険者所在地
            void _printHokensyaAddress()
            {
                SetFieldData("dfHokensyaAddress", coModel.HokensyaAddress);
                // 下位互換
                SetFieldData("HokenAdd", coModel.HokensyaAddress);
            }
            // 保険者電話番号
            void _printHokensyaTel()
            {
                void _printSplitTel(string field, string tel)
                {
                    string[] tmp = tel.Split('-');

                    for (int i = 0; i < tmp.Length; i++)
                    {
                        SetFieldData($"{field}{i + 1}", tmp[i]);
                    }
                }

                SetFieldData("dfHokensyaTel", coModel.HokensyaTel);
                _printSplitTel("dfHokensyaTel_", coModel.HokensyaTel);

                // 下位互換
                SetFieldData("HokenTel", coModel.HokensyaTel);
                _printSplitTel("HokenTel", coModel.HokensyaTel);

            }
            // 公費負担者番号受給者番号
            void _printKohiFutansyaJyukyusya()
            {
                void _SetFieldDateData(string field, int index, int date)
                {
                    SetFieldData($"{field}S_K{index}", CIUtil.SDateToShowSDate(date));

                    CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(date);
                    SetFieldData($"{field}W_K{index}", wareki.Ymd);
                    SetFieldData($"{field}YearS_K{index}", date / 10000);
                    SetFieldData($"{field}Gengo_K{index}", wareki.Gengo);
                    SetFieldData($"{field}YearW_K{index}", CIUtil.ToStringIgnoreZero(wareki.Year));
                    SetFieldData($"{field}Month_K{index}", CIUtil.ToStringIgnoreZero(wareki.Month));
                    SetFieldData($"{field}Day_K{index}", CIUtil.ToStringIgnoreZero(wareki.Day));
                }

                for (int kohiIndex = 1; kohiIndex <= 4; kohiIndex++)
                {
                    if (coModel.KohiId != null && coModel.KohiId(kohiIndex) > 0)
                    {
                        string futansyaNo = coModel.KohiFutansyaNo(kohiIndex).PadLeft(8, ' ');
                        string jyukyusyaNo = coModel.KohiJyukyusyaNo(kohiIndex).PadLeft(7, ' ');
                        int startDate = coModel.KohiStartDate(kohiIndex);
                        int endDate = coModel.KohiEndDate(kohiIndex);
                        int sikakuDate = coModel.KohiSikakuDate(kohiIndex);
                        int kofuDate = coModel.KohiKofuDate(kohiIndex);

                        SetFieldData($"dfFutansyaNo_K{kohiIndex}", futansyaNo);
                        SetFieldData($"dfJyukyusyaNo_K{kohiIndex}", jyukyusyaNo);

                        _SetFieldDateData("dfStartDate", kohiIndex, startDate);
                        _SetFieldDateData("dfEndDate", kohiIndex, endDate);

                        _SetFieldDateData($"dfEndDate", kohiIndex, endDate);
                        _SetFieldDateData($"dfSikakuDate", kohiIndex, sikakuDate);
                        _SetFieldDateData($"dfKofuDate", kohiIndex, kofuDate);

                        // 下位互換
                        short i = 0;

                        while (i < 8 && i < futansyaNo.Length)
                        {
                            listDataPerPage.Add(new($"Ko{kohiIndex}_FutanNo", i, 0, futansyaNo[i].ToString()));
                            i++;
                        }
                        i = 0;
                        while (i < 7 && i < jyukyusyaNo.Length)
                        {
                            listDataPerPage.Add(new($"Ko{kohiIndex}_JyukyuNo", i, 0, jyukyusyaNo[i].ToString()));
                            i++;
                        }

                        SetFieldData($"Ko{kohiIndex}_YukoYmd", CIUtil.SDateToShowWDate2(endDate));
                    }
                }
            }
            // 勤務先名
            void _printOffice()
            {
                SetFieldData("dfOffice", coModel.OfficeName);
                // 下位互換
                SetFieldData("OfficeNM", coModel.OfficeName);
            }
            // 勤務先所在地
            void _printOfficeAddress()
            {
                SetFieldData("dfOfficeAddress", coModel.OfficeAddress);
                // 下位互換
                SetFieldData("OfficeAdd1", coModel.OfficeAddress);
            }
            // 勤務先電話番号
            void _printOfficeTel()
            {
                SetFieldData("dfOfficeTel", coModel.OfficeTel);
            }
            // 勤務先郵便番号
            void _printOfficePostCd()
            {
                SetFieldData("dfOfficePostCd", coModel.OfficePostCdDsp);
            }
            // 勤務先メモ
            void _printOfficeBiko()
            {
                SetFieldData("dfOfficeBiko", coModel.OfficeMemo);
            }
            // メールアドレス
            void _printMail()
            {
                SetFieldData("dfMail", coModel.Mail);
            }
            // 続柄
            void _printZokugara()
            {
                SetFieldData("dfZokugara", coModel.Zokugara);
            }
            // 職業
            void _printJob()
            {
                SetFieldData("dfJob", coModel.Job);
            }
            // 患者コメント
            void _printPtCmt()
            {
                SetFieldData("txPtCmt", coModel.PtCmtText);

                // 下位互換
                int fieldIndex = 1;
                int dataIndex = 0;

                while ((objectNamePage1.Contains($"KAN_CMT_TEN_{fieldIndex}") || objectNamePage1.Contains($"KAN_CMT_{fieldIndex}")) &&
                      (dataIndex < coModel.PtCmtList.Count))
                {
                    if (!objectNamePage1.Contains($"KAN_CMT_TEN_{fieldIndex + 1}") && dataIndex + 1 < coModel.PtCmtList.Count)
                    {
                        SetFieldData($"KAN_CMT_TEN_{fieldIndex}", coModel.PtCmtList[dataIndex] + "...");
                    }
                    else
                    {
                        SetFieldData($"KAN_CMT_TEN_{fieldIndex}", coModel.PtCmtList[dataIndex]);
                    }
                    SetFieldData($"KAN_CMT_{fieldIndex}", coModel.PtCmtList[dataIndex]);

                    fieldIndex++;
                    dataIndex++;

                    if (!objectNamePage1.Contains($"KAN_CMT_TEN_{fieldIndex}") && objectNamePage1.Contains($"KAN_CMT_{fieldIndex}") == false)
                    {
                        break;
                    }
                }
            }
            // 患者メモ
            void _printPtMemo()
            {
                SetFieldData("txPtMemo", coModel.PtMemoText);
            }
            #endregion

            // システム日付
            _pirntSysDate();

            // 患者番号
            _printPtNum();
            // 患者氏名
            _printPtName();
            // 患者カナ氏名
            _printPtKanaName();
            // 生年月日
            _printPtBirthDay();
            // 年齢
            _printPtAge();

            // 性別
            _printPtSex();

            // 患者郵便番号
            _printPtPostCd();
            // 患者住所
            _printPtHomeAddress();
            _printPtHomeAddressWordWrap();

            // 患者電話番号
            _printPtTel();

            // メールアドレス
            _printMail();
            // 続柄
            _printZokugara();
            // 職業
            _printJob();

            // 世帯主
            _printSetaiNusi();

            // 記号番号
            _printKigoBango();
            // 保険有効期限
            _printHokenKigen();
            // 保険取得日
            _printHokenSyutoku();
            // 保険者番号
            _printHokensyaNo();
            // 本人家族
            _printHonke();
            // 保険の種類
            _printHokenSbt();
            // 患者負担率
            _printFutanRate();
            // 保険給付率
            _printKyufuRate();

            // 保険者所在地
            _printHokensyaAddress();
            // 保険者名称
            _printHokensyaName();
            // 保険者電話番号
            _printHokensyaTel();

            // 公費負担者番号受給者番号
            _printKohiFutansyaJyukyusya();

            // 勤務先名
            _printOffice();
            // 勤務先所在地
            _printOfficeAddress();
            // 勤務先電話番号
            _printOfficeTel();
            // 勤務先郵便番号
            _printOfficePostCd();
            // 勤務先備考
            _printOfficeBiko();
            // 患者コメント
            _printPtCmt();
            // 患者メモ
            _printPtMemo();
        }

        // 本体
        void UpdateFormBody()
        {
            string _getSDateFormat(int date)
            {
                string ret = string.Empty;

                if (date > 0)
                {
                    ret = $"{date / 10000}年{date % 10000 / 100}月{date % 100}日";
                }

                return ret;
            }
            string _getWDateFormat(int date)
            {
                string ret = string.Empty;

                if (date > 0)
                {
                    ret = CIUtil.SDateToShowWDate3(date).Ymd;
                }

                return ret;
            }
            string _getSDate(int date)
            {
                string ret = string.Empty;

                if (date > 0)
                {
                    ret = $"{date / 10000,4} {date % 10000 / 100,4} {date % 100,4}  ";
                }

                return ret;

            }
            string _getWDate(int date)
            {
                string ret = string.Empty;

                if (date > 0)
                {
                    CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(date);

                    ret = $"{wareki.Year,2} {wareki.Month,2} {wareki.Day,2}";
                }

                return ret;
            }

            int dataIndex = (currentPage - 1) * dataRowCount;
            int maxListRow = dataRowCount;
            int target = dataTarget;

            if (currentPage >= 2)
            {
                if (dataRowCountP2 <= 0)
                {
                    hasNextPage = false;
                    return;
                }
                else
                {
                    dataIndex = dataRowCount + (currentPage - 2) * dataRowCountP2;
                    maxListRow = dataRowCountP2;
                    target = dataTargetP2;
                }
            }

            if (printOutData == null || printOutData.Count == 0)
            {
                hasNextPage = false;
                return;
            }

            for (short i = 0; i < maxListRow; i++)
            {
                switch (target)
                {
                    case 1:
                        listDataPerPage.Add(new("lsByomei", 0, i, printOutData[dataIndex].Byomei));
                        listDataPerPage.Add(new("lsByomeiStartDateS", 0, i, _getSDate(printOutData[dataIndex].StartDate)));
                        listDataPerPage.Add(new("lsByomeiStartDateW", 0, i, _getWDate(printOutData[dataIndex].StartDate)));
                        listDataPerPage.Add(new("lsByomeiStartDateSFormat", 0, i, _getSDateFormat(printOutData[dataIndex].StartDate)));
                        listDataPerPage.Add(new("lsByomeiStartDateWFormat", 0, i, _getWDateFormat(printOutData[dataIndex].StartDate)));
                        listDataPerPage.Add(new("lsByomeiTenki", 0, i, printOutData[dataIndex].Tenki));
                        listDataPerPage.Add(new("lsByomeiTenkiDateS", 0, i, _getSDate(printOutData[dataIndex].TenkiDate)));
                        listDataPerPage.Add(new("lsByomeiTenkiDateW", 0, i, _getWDate(printOutData[dataIndex].TenkiDate)));
                        listDataPerPage.Add(new("lsByomeiTenkiDateSFormat", 0, i, _getSDateFormat(printOutData[dataIndex].TenkiDate)));
                        listDataPerPage.Add(new("lsByomeiTenkiDateWFormat", 0, i, _getWDateFormat(printOutData[dataIndex].TenkiDate)));

                        switch (printOutData[dataIndex].TenkiKbn)
                        {
                            case TenkiKbnConst.Cured:
                                listDataPerPage.Add(new("lsTenkiTiyuMaru", 0, i, "〇"));
                                break;
                            case TenkiKbnConst.Dead:
                                listDataPerPage.Add(new("lsTenkiSiboMaru", 0, i, "〇"));
                                break;
                            case TenkiKbnConst.Canceled:
                                listDataPerPage.Add(new("lsTenkiChusiMaru", 0, i, "〇"));
                                break;
                            case TenkiKbnConst.Other:
                                listDataPerPage.Add(new("lsTenkiSonota", 0, i, "〇"));
                                break;
                        }
                        break;
                    case 2:
                    case 3:
                        SetFieldData($"ByoMei_{i + 1}", printOutData[dataIndex].Byomei);
                        SetFieldData($"ByoMeiH_{i + 1}", printOutData[dataIndex].Byomei);
                        SetFieldData($"ByoYmd_{i + 1}", _getWDate(printOutData[dataIndex].StartDate));
                        SetFieldData($"TenYmd_{i + 1}", _getWDate(printOutData[dataIndex].TenkiDate));

                        visibleFieldList.Add($"TenHeal_{i + 1}", false);
                        visibleFieldList.Add($"TenAbort_{i + 1}", false);
                        visibleFieldList.Add($"TenDeath_{i + 1}", false);
                        visibleFieldList.Add($"TenSonota_{i + 1}", false);

                        switch (printOutData[dataIndex].TenkiKbn)
                        {
                            case TenkiKbnConst.Cured:
                                visibleFieldList.Add($"TenHeal_{i}", true);
                                break;
                            case TenkiKbnConst.Dead:
                                visibleFieldList.Add($"TenDeath_{i}", true);
                                break;
                            case TenkiKbnConst.Canceled:
                                visibleFieldList.Add($"TenAbort_{i}", true);
                                break;
                            case TenkiKbnConst.Other:
                                visibleFieldList.Add($"TenSonota_{i}", true);
                                break;
                        }
                        break;
                }

                dataIndex++;
                if (dataIndex >= printOutData.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }

            _listTextData.Add(currentPage, listDataPerPage);
            _reportConfigModelPerPage.Add(currentPage, new ReportConfigModel()
            {
                VisibleFieldList = visibleFieldList
            });
        }

        #endregion

        if (ptId == 0)
        {
            // 白紙印刷の場合
            return;
        }
        UpdateFormHeader();
        UpdateFormBody();
    }

    private CoKarte1Model GetData()
    {
        // 白紙印刷の場合、データ取得しない
        if (ptId == 0) return new();

        // 患者情報
        CoPtInfModel ptInf = _finder.FindPtInf(hpId, ptId, sinDate);

        // 病名情報
        List<CoPtByomeiModel> ptByomeis = _finder.FindPtByomei(hpId, ptId, hokenPid, tenkiByomei);

        // 患者保険情報
        CoPtHokenInfModel ptHokenInf = _finder.FindPtHokenInf(hpId, ptId, hokenPid, sinDate);

        return new CoKarte1Model(ptInf, ptByomeis, ptHokenInf);
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

    private void SetFieldData(string field, long value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value.ToString());
        }
    }

    private void ReadRseFileP1()
    {
        string formFileName = "fmKarte1.rse";
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate("lsByomei", (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("lsByomei", (int)CalculateTypeEnum.GetFormatLendB),
            new ObjectCalculate("ByoMei_", (int)CalculateTypeEnum.GetFormatLendB),
            new ObjectCalculate("ByoMeiH_", (int)CalculateTypeEnum.GetFormatLendB)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Karte1, formFileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);

        responsesPage1 = javaOutputData.responses;
        objectNamePage1 = javaOutputData.objectNames;
    }

    private void ReadRseFileP2()
    {
        string formFileName = "fmKarte1_2.rse";
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate("lsByomei", (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("lsByomei", (int)CalculateTypeEnum.GetFormatLendB),
            new ObjectCalculate("ByoMei_", (int)CalculateTypeEnum.GetFormatLendB),
            new ObjectCalculate("ByoMeiH_", (int)CalculateTypeEnum.GetFormatLendB)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Karte1, formFileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);

        responsesPage2 = javaOutputData.responses;
        objectNamePage2 = javaOutputData.objectNames;
    }
}
