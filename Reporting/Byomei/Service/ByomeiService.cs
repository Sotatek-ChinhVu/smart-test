using Entity.Tenant;
using Helper.Common;
using Reporting.Byomei.DB;
using Reporting.Byomei.Mapper;
using Reporting.Byomei.Model;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;

namespace Reporting.Byomei.Service;

public class ByomeiService : IByomeiService
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly Dictionary<string, bool> _visibleFieldList;
    private readonly ICoPtByomeiFinder _finder;

    private int dataCharCount;
    private int dataRowCount;
    private CoPtByomeiModel coModel;
    private int currentPage;
    private bool hasNextPage;
    private List<CoByomeiPrintDataModel> printOutData;

    private int hpId;
    private long ptId;
    private int fromDate;
    private int toDate;
    private List<int> hokenIds;
    private bool tenkiIn;

    public ByomeiService(ICoPtByomeiFinder finder, IReadRseReportFileService readRseReportFileService)
    {
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        _singleFieldData = new();
        _tableFieldData = new();
        _visibleFieldList = new();
        coModel = new();
        printOutData = new();
        hokenIds = new();
    }

    public CommonReportingRequestModel GetByomeiReportingData(int hpId, long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds)
    {
        try
        {
            this.hpId = hpId;
            this.ptId = ptId;
            fromDate = fromDay;
            toDate = toDay;
            this.hokenIds = hokenIds;
            this.tenkiIn = tenkiIn;

            var coModels = GetData();
            if (coModels != null && coModels.Any())
            {
                GetRowCount();

                foreach (CoPtByomeiModel ptByomeiModel in coModels)
                {
                    coModel = ptByomeiModel;
                    currentPage = 1;
                    printOutData = new();
                    MakePrintDataList();
                    hasNextPage = true;

                    //病名一覧印刷
                    while (hasNextPage)
                    {
                        UpdateDrawForm();
                        currentPage++;
                    }
                }
            }

            return new ByomeiMapper(_singleFieldData, _tableFieldData, _visibleFieldList).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }

    private void MakePrintDataList()
    {
        #region sub method
        List<CoByomeiPrintDataModel> _addList(string str, int byomeiIndex)
        {
            List<CoByomeiPrintDataModel> addPrintOutData = new();

            string line = $"({byomeiIndex})".PadRight(5) + str;
            int maxLength = dataCharCount;

            while (line != string.Empty)
            {
                string tmp = line;
                if (CIUtil.LenB(line) > maxLength)
                {
                    // 文字列が最大幅より広い場合、カット
                    tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
                }

                CoByomeiPrintDataModel prinData = new();
                prinData.Byomei = tmp;
                addPrintOutData.Add(prinData);

                // 今回出力分の文字列を削除
                line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));

                if (line != string.Empty)
                {
                    line = new string(' ', 5) + line;
                }
            }

            return addPrintOutData;
        }
        #endregion

        int i = 1;

        foreach (CoByomeiModel ptByomei in coModel.ListByomei)
        {
            List<CoByomeiPrintDataModel> addData = new();

            addData.AddRange(_addList(ptByomei.ByomeiName, i));

            if (addData.Any())
            {
                addData.Last().StartDate = ptByomei.StartDate;
                addData.Last().Tenki = ptByomei.DisplayTenki;
                addData.Last().TenkiDate = ptByomei.TenkiDate;
            }

            printOutData.AddRange(addData);
            i++;
        }

    }

    private void UpdateDrawForm()
    {
        #region SubMethod
        void UpdateFormHeader()
        {
            string bufFrom = string.Empty;
            if (coModel.FromDay != 0)
            {
                bufFrom = CIUtil.SDateToShowSDate(coModel.FromDay);
            }

            string bufTo = string.Empty;
            if (coModel.ToDay != 0)
            {
                bufTo = CIUtil.SDateToShowSDate(coModel.ToDay);
            }

            string sMakeYmd = CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd HH:mm");
            //HEAD印字
            //日付
            SetFieldData("dfPtNum", coModel.PtNum.ToString());
            SetFieldData("dfPtKanaName", coModel.KanaName);
            SetFieldData("dfPtName", coModel.KanjiName);
            SetFieldData("dfBirthDay", coModel.BirthDay);
            SetFieldData("dfSex", coModel.Sex);

            SetFieldData("bcPtNum", coModel.PtNum.ToString()); //患者番号バーコード

            int iAge = CIUtil.SDateToAge(coModel.BirthYmd, CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow()));
            SetFieldData("dfAge", iAge.ToString());

            if (string.IsNullOrEmpty(bufFrom) && string.IsNullOrEmpty(bufTo))
            {
                if (!_visibleFieldList.ContainsKey("lblTermTitle"))
                {
                    _visibleFieldList.Add("lblTermTitle", false);
                }
                else
                {
                    _visibleFieldList["lblTermTitle"] = false;
                }

                if (!_visibleFieldList.ContainsKey("lblTermTitle"))
                {
                    _visibleFieldList.Add("lblTermKara", false);
                }
                else
                {
                    _visibleFieldList["lblTermKara"] = false;
                }
            }
            else
            {
                if (!_visibleFieldList.ContainsKey("lblTermTitle"))
                {
                    _visibleFieldList.Add("lblTermTitle", true);
                }
                else
                {
                    _visibleFieldList["lblTermTitle"] = true;
                }

                if (!_visibleFieldList.ContainsKey("lblTermTitle"))
                {
                    _visibleFieldList.Add("lblTermKara", true);
                }
                else
                {
                    _visibleFieldList["lblTermKara"] = true;
                }

                if (!string.IsNullOrEmpty(bufFrom))
                {
                    SetFieldData("dfStartDate", bufFrom);
                }
                else
                {
                    //SetFieldData("dfStartDate", CON_UNSPECIFIED_TIME);
                }

                if (!string.IsNullOrEmpty(bufTo))
                {
                    SetFieldData("dfEndDate", bufTo);
                }
                else
                {
                    //SetFieldData("dfEndDate", CON_UNSPECIFIED_TIME);
                }
            }

            SetFieldData("dfPrintDateTime", sMakeYmd);

            //保険番号が"0"以外の場合にのみ、保険名称を印字する
            if (coModel.HokenPatternName != string.Empty)
            {
                SetFieldData("dfHokenPattern", coModel.HokenPatternName);
            }
        }

        void UpdateFormBody()
        {
            int byomeiIndex = (currentPage - 1) * dataCharCount;
            List<CoByomeiModel> listByomei = coModel.ListByomei;
            if (listByomei == null || listByomei.Count == 0)
            {
                hasNextPage = false;
            }

            for (short i = 0; i < dataRowCount; i++)
            {
                Dictionary<string, CellModel> data = new();
                AddListData(ref data, "lsByomei", printOutData[byomeiIndex].Byomei);
                AddListData(ref data, "lsStartDate", printOutData[byomeiIndex].StartDate);
                AddListData(ref data, "lsTenkiDate", printOutData[byomeiIndex].TenkiDate);
                AddListData(ref data, "lsTenki", printOutData[byomeiIndex].Tenki);

                _tableFieldData.Add(data);
                byomeiIndex++;
                if (byomeiIndex >= printOutData.Count)
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

    private List<CoPtByomeiModel> GetData()
    {
        var ptByomeis = _finder.GetPtByomei(hpId, ptId, fromDate, toDate, tenkiIn, hokenIds);
        var ptInf = _finder.FindPtInf(hpId, ptId);

        hokenIds = new List<int>();
        if (ptByomeis != null && ptByomeis.Any())
        {
            hokenIds = ptByomeis.GroupBy(p => p.HokenPid).Select(p => p.Key).ToList();
        }

        var ptHokenInfs = _finder.GetPtHokenInf(hpId, ptId, hokenIds, toDate);

        List<CoPtByomeiModel> results = new();

        if (ptHokenInfs == null || !ptHokenInfs.Any())
        {
            if (ptByomeis != null && ptByomeis.Any())
            {
                results.Add(new CoPtByomeiModel(fromDate, toDate, ptInf, null, ptByomeis));
            }
        }
        else if (ptHokenInfs.Count == 1)
        {
            // 使用されている保険が1つの場合、共通(0)とその保険分をまとめて出力
            if (ptByomeis != null && ptByomeis.Any())
            {
                results.Add(new CoPtByomeiModel(fromDate, toDate, ptInf, ptHokenInfs.First(), ptByomeis));
            }
        }
        else
        {
            IEnumerable<PtByomei> emByomeis;

            if (ptByomeis != null && ptByomeis.Any(p => p.HokenPid == 0))
            {
                emByomeis = ptByomeis.FindAll(p => p.HokenPid == 0);
                results.Add(new CoPtByomeiModel(fromDate, toDate, ptInf, null, emByomeis));
            }

            foreach (CoPtHokenInfModel ptHokenInf in ptHokenInfs)
            {
                if (ptByomeis != null && ptByomeis.Any(p => p.HokenPid == ptHokenInf.HokenId))
                {
                    emByomeis = ptByomeis.FindAll(p => p.HokenPid == ptHokenInf.HokenId);
                    results.Add(new CoPtByomeiModel(fromDate, toDate, ptInf, ptHokenInf, emByomeis));
                }
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

    private void GetRowCount()
    {
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate("lsByomei", (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("lsByomei", (int)CalculateTypeEnum.GetFormatLendB)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Byomei, string.Empty, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        dataCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsByomei" && item.typeInt == (int)CalculateTypeEnum.GetFormatLendB)?.result ?? dataCharCount;
        dataRowCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsByomei" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? dataRowCount;
    }
}
