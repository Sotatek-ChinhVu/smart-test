using Domain.Models.HpInf;
using Domain.Models.KensaSet;
using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using Reporting.KensaHistory.DB;
using Reporting.KensaHistory.Mapper;
using Reporting.KensaHistory.Models;
using Reporting.Mappers.Common;

namespace Reporting.KensaHistory.Service
{
    public class KensaResultMultiCoReportService : IKensaResultMultiCoReportService
    {
        private IKensaSetRepository _kokhoFinder;
        private ICoKensaHistoryFinder _coKensaHistoryFinder;
        private HpInfModel hpInf;
        private int hpId;
        private int userId;
        private long ptId;
        private int setId;
        private int iraiCd;
        private int startDate;
        private int endDate;
        private bool showAbnormalKbn;
        private int itemQuantity;
        private long iraiDate;
        private long iraiStart;
        private long iraiEnd;
        private int row;
        private PtInf ptInf;
        private (List<CoKensaResultMultiModel>, List<long>) data = new();
        private List<CoKensaResultMultiModel> kensaInfDetails = new();
        private List<long> date = new();
        private int totalPage;
        private bool hasNextPage;
        private int currentPage;

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private string _formFileName = "kensaResultMulti.rse";
        private readonly Dictionary<int, ReportConfigModel> _reportConfigPerPage;
        private readonly Dictionary<string, bool> _visibleAtPrint;

        public KensaResultMultiCoReportService(IKensaSetRepository kokhoFinder, ICoKensaHistoryFinder coKensaHistoryFinder)
        {
            _kokhoFinder = kokhoFinder;
            _setFieldData = new();
            _singleFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            _visibleAtPrint = new();
            _coKensaHistoryFinder = coKensaHistoryFinder;
        }

        public CommonReportingRequestModel GetKensaResultMultiPrintData(int hpId, int userId, long ptId, int setId, int startDate, int endDate, bool showAbnormalKbn)
        {

            this.hpId = hpId;
            this.userId = userId;
            this.ptId = ptId;
            this.setId = setId;
            this.startDate = startDate;
            this.endDate = endDate;
            this.showAbnormalKbn = showAbnormalKbn;
            var getData = GetData();

            if (getData)
            {
                currentPage = 1;
                hasNextPage = true;
                while (hasNextPage)
                {
                    UpdateDrawForm();
                    currentPage++;
                }
            }

            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new KensaHistoryMapper(_reportConfigPerPage, _setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
        }

        private bool UpdateDrawForm()
        {
            #region SubMethod

            #region Header
            int UpdateFormHeader()
            {
                Dictionary<string, string> fieldDataPerPage = new();
                //医療機関コード
                SetFieldData("hpCode", hpInf.HpCd);
                SetFieldData("hpName", hpInf.HpName);
                SetFieldData("ptNum", ptInf.PtNum.ToString());
                SetFieldData("name", ptInf.Name ?? string.Empty);
                SetFieldData("iraiStartDate", CIUtil.SDateToShowSDate(iraiStart.AsInteger()));
                SetFieldData("iraiEndDate", CIUtil.SDateToShowSDate(endDate.AsInteger()));
                SetFieldData("issuedDate", CIUtil.GetJapanDateTimeNow().ToString());
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                fieldDataPerPage.Add("pageNumber", pageIndex.ToString() + "/" + totalPage.ToString());
                _setFieldData.Add(pageIndex, fieldDataPerPage);
                //保険者

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                Dictionary<string, string> fieldDataPerPage = new();

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                short maxRow = 23;
                int rowNo = 0;
                int k = 0;
                short maxColDate = 9;
                short colDate = 1;

                if (currentPage == 1)
                {
                    foreach (var item in date.OrderBy(x => x))
                    {
                        listDataPerPage.Add(new("date" + k.ToString(), 0, rowNo, CIUtil.SDateToShowSDate((int)item)));
                        date.Remove(item);
                        colDate++;
                        if (colDate > maxColDate)
                        {
                            break;
                        }
                        k++;
                    }

                    foreach (var item in kensaInfDetails)
                    {
                        listDataPerPage.Add(new("itemName", 0, rowNo, item.ItemName));
                        listDataPerPage.Add(new("unit", 0, rowNo, item.Unit));
                        listDataPerPage.Add(new("standardValue", 0, rowNo, item.StandardValue));
                        int count = 0;
                        foreach (var itemKensa in item.KensaResultMultiItems)
                        {
                            listDataPerPage.Add(new("resultValue" + count.ToString(), 0, rowNo, itemKensa.ResultValue));
                            listDataPerPage.Add(new("abnormalFlag" + count.ToString(), 0, rowNo, itemKensa.AbnormalKbn));
                            count++;
                        }
                        rowNo++;
                        if (rowNo == maxRow)
                        {
                            break;
                        }
                    }

                    if (kensaInfDetails.Count < maxRow && date.Count == 0)
                    {
                        _listTextData.Add(pageIndex, listDataPerPage);
                        hasNextPage = false;
                        return 1;
                    }
                    else
                    {
                        hasNextPage = true;
                        int z = 0;
                        foreach (var item in kensaInfDetails)
                        {
                            item.KensaResultMultiItems.RemoveRange(0, 9);
                            z++;
                            if (z == kensaInfDetails.Count - 1)
                            {
                                break;
                            }
                        }
                        _listTextData.Add(pageIndex, listDataPerPage);
                        return 1;
                    }
                }

                rowNo = 0;

                int counts = kensaInfDetails.Count;

                foreach (var item in date.OrderBy(x => x))
                {
                    listDataPerPage.Add(new("date" + k.ToString(), 0, rowNo, CIUtil.SDateToShowSDate((int)item)));
                    date.Remove(item);
                    colDate++;
                    if (colDate > maxColDate)
                    {
                        break;
                    }
                    k++;
                }

                foreach (var item in kensaInfDetails)
                {
                    listDataPerPage.Add(new("itemName", 0, rowNo, item.ItemName));
                    listDataPerPage.Add(new("unit", 0, rowNo, item.Unit));
                    listDataPerPage.Add(new("standardValue", 0, rowNo, item.StandardValue));
                    int count = 0;

                    foreach (var itemKensa in item.KensaResultMultiItems)
                    {
                        listDataPerPage.Add(new("resultValue" + count.ToString(), 0, rowNo, itemKensa.ResultValue));
                        listDataPerPage.Add(new("abnormalFlag" + count.ToString(), 0, rowNo, itemKensa.AbnormalKbn));
                        count++;
                    }
                    rowNo++;
                    if (rowNo == maxRow)
                    {
                        break;
                    }
                }

                if (counts > maxRow)
                {
                    kensaInfDetails.RemoveRange(0, maxRow);
                }
                else
                {
                    hasNextPage = false;
                }

                _listTextData.Add(pageIndex, listDataPerPage);

                return 1;
            }
            #endregion

            #endregion

            if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
            {
                return false;
            }
            return true;
        }

        private bool GetData()
        {
            hpInf = _coKensaHistoryFinder.GetHpInf(hpId);
            ptInf = _coKensaHistoryFinder.GetPtInf(hpId, ptId);
            data = _coKensaHistoryFinder.GetListKensaInfDetail(hpId, userId, ptId, setId, startDate, endDate, showAbnormalKbn);
            kensaInfDetails = data.Item1;
            date = data.Item2;
            date = date.OrderBy(x => x).ToList();
            iraiStart = date.First();
            iraiEnd = date.Last();
            totalPage = (kensaInfDetails.Count / 23) + 1 + (date.Count / 9);
            return kensaInfDetails.Count > 0;
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