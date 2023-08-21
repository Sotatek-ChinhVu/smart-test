using Helper.Common;
using Reporting.KensaLabel.DB;
using Reporting.KensaLabel.Mapper;
using Reporting.KensaLabel.Model;
using Reporting.Mappers.Common;

namespace Reporting.KensaLabel.Service
{
    public class KensaLabelCoReportService : IKensaLabelCoReportService
    {
        private readonly IKensaLabelFinder _finder;
        private int hpId;
        private long ptId;
        private long raiinNo;
        private int sinDate;
        private KensaPrinterModel kensaPrinter;
        private PtInfModel ptInfModel;

        public bool canOutputTraceLog { get; set; } = true;

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly Dictionary<string, bool> _visibleAtPrint;
        private string _formFileName = "fmKensaLabel.rse";
        public KensaLabelCoReportService(IKensaLabelFinder finder)
        {
            _finder = finder;
            _singleFieldData = new();
            _setFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            _visibleAtPrint = new();
        }

        public CommonReportingRequestModel GetKensaLabelPrintData(int hpId, long ptId, long raiinNo, int sinDate, KensaPrinterModel printerModel)
        {
            this.hpId = hpId;
            this.ptId = ptId;
            this.raiinNo = raiinNo;
            this.sinDate = sinDate;
            this.kensaPrinter = printerModel;

            ptInfModel = _finder.GetPtInfModel(hpId, ptId);
            UpdateDrawForm();

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new KensaLabelMapper(_setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
        }

        private bool UpdateDrawForm()
        {
            try
            {
                for (int i = 0; i <= 10; i++)
                {
                    if (i == 0)
                    {
                        SetFieldData("dfPtNum", $"{ptInfModel.PtNum}");
                    }
                    else
                    {
                        SetFieldData("dfPtNum" + i, $"{ptInfModel.PtNum}".PadLeft(i, '0'));
                    }
                }
                SetFieldData("dfPtKanName", ptInfModel.KanaName ?? string.Empty);
                SetFieldData("dfPtName", ptInfModel.Name ?? string.Empty);

                for (int i = 0; i <= 10; i++)
                {
                    if (i == 0)
                    {
                        SetFieldData("bcPtNum", $"{ptInfModel.PtNum}");
                    }
                    else
                    {
                        SetFieldData("bcPtNum" + i, $"{ptInfModel.PtNum}".PadLeft(i, '0'));
                    }
                }
                SetFieldData("dfPrintDate", CIUtil.SDateToShowSDate(CIUtil.DateTimeToInt(DateTime.Now)));

                SetFieldData("dfSex", ptInfModel.Sex == 1 ? "男" : "女");
                SetFieldData("dfAge", ptInfModel.Age);
                SetFieldData("dfBirtyDayS", CIUtil.SDateToShowSDate(ptInfModel.BirthDay));
                SetFieldData("dfBirtyDaySk", CIUtil.IntToDate(ptInfModel.BirthDay).ToString("yyyy年MM月dd日"));
                SetFieldData("dfBirtyDayW", CIUtil.SDateToShowWDate(ptInfModel.BirthDay));
                SetFieldData("dfBirtyDayWk", CIUtil.SDateToShowWDate2(ptInfModel.BirthDay));
                SetFieldData("dfContainerName", kensaPrinter.ContainerName);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }
    }
}
