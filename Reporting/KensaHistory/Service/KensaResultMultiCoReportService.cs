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
        private int sinDate;
        private int row;
        private PtInf ptInf;
        private (List<CoKensaResultMultiModel>, List<long>) data = new();
        private List<CoKensaResultMultiModel> kensaInfDetails = new();
        private List<CoKensaResultMultiModel> kensaInfDetailsItem = new();
        private List<long> date = new();
        private int totalPage;
        private bool hasNextPage;
        private int currentPage;
        private short t;

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

        public CommonReportingRequestModel GetKensaResultMultiPrintData(int hpId, int userId, long ptId, int setId, int startDate, int endDate, bool showAbnormalKbn, int sinDate)
        {
            try
            {
                this.hpId = hpId;
                this.userId = userId;
                this.ptId = ptId;
                this.setId = setId;
                this.startDate = startDate;
                this.endDate = endDate;
                this.showAbnormalKbn = showAbnormalKbn;
                this.sinDate = sinDate;
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
                int i = 1;

                foreach (var item in _setFieldData)
                {
                    item.Value.Clear();
                    item.Value.Add("pageNumber", i.ToString() + "/" + pageIndex.ToString());
                    i++;
                    if (i > pageIndex)
                    {
                        break;
                    }
                }

                return new KensaHistoryMapper(_reportConfigPerPage, _setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();

            }
            finally
            {
                _kokhoFinder.ReleaseResource();
                _coKensaHistoryFinder.ReleaseResource();
            }
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
                SetFieldData("iraiStartDate", CIUtil.SDateToShowSDate(startDate.AsInteger()));
                SetFieldData("iraiEndDate", CIUtil.SDateToShowSDate(endDate.AsInteger()));
                SetFieldData("issuedDate", CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd HH:mm:ss"));
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                fieldDataPerPage.Add("pageNumber", pageIndex.ToString() + "/" + totalPage.ToString());
                _setFieldData.Add(pageIndex, fieldDataPerPage);
                //保険者

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBodyP1()
            {
                List<ListTextObject> listDataPerPage = new();
                Dictionary<string, string> fieldDataPerPage = new();

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                short maxRow = 20;
                int rowNo = 0;
                int k = 0;
                short maxColDate = 9;
                short colDate = 1;
                hasNextPage = true;

                if (date.Count <= maxColDate && kensaInfDetails.Count - 1 <= maxRow)
                {
                    foreach (var item in date)
                    {
                        listDataPerPage.Add(new("date" + k.ToString(), 0, rowNo, CIUtil.SDateToShowSDate((int)item)));
                        colDate++;
                        if (colDate > maxColDate)
                        {
                            break;
                        }
                        k++;
                    }

                    foreach (var item in kensaInfDetails)
                    {
                        listDataPerPage.Add(new("itemName", 0, rowNo, item.ItemName.TrimEnd()));
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

                    hasNextPage = false;
                    _listTextData.Add(pageIndex, listDataPerPage);

                    return 1;
                }

                if (date.Count > maxColDate && kensaInfDetails.Count - 1 <= maxRow)
                {
                    foreach (var item in date)
                    {
                        listDataPerPage.Add(new("date" + k.ToString(), 0, rowNo, CIUtil.SDateToShowSDate((int)item)));
                        colDate++;
                        if (colDate > maxColDate)
                        {
                            break;
                        }
                        k++;
                    }

                    foreach (var item in kensaInfDetails)
                    {
                        listDataPerPage.Add(new("itemName", 0, rowNo, item.ItemName.TrimEnd()));
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

                    date.RemoveRange(0, maxColDate);
                    int z = 0;
                    foreach (var item in kensaInfDetails)
                    {
                        item.KensaResultMultiItems.RemoveRange(0, maxColDate);
                        z++;
                        if (z == kensaInfDetails.Count - 1)
                        {
                            break;
                        }
                    }
                    _listTextData.Add(pageIndex, listDataPerPage);

                    return 1;
                }

                if (date.Count <= maxColDate && kensaInfDetails.Count - 1 > maxRow)
                {
                    foreach (var item in date)
                    {
                        listDataPerPage.Add(new("date" + k.ToString(), 0, rowNo, CIUtil.SDateToShowSDate((int)item)));
                        colDate++;
                        if (colDate > maxColDate)
                        {
                            break;
                        }
                        k++;
                    }

                    foreach (var item in kensaInfDetails)
                    {
                        listDataPerPage.Add(new("itemName", 0, rowNo, item.ItemName.TrimEnd()));
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

                    kensaInfDetails.RemoveRange(0, maxRow);
                    _listTextData.Add(pageIndex, listDataPerPage);

                    return 1;
                }

                return 1;
            }

            int UpdateFormBodyP2()
            {
                List<ListTextObject> listDataPerPage = new();
                Dictionary<string, string> fieldDataPerPage = new();

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                short maxRow = 20;
                int rowNo = 0;
                int k = 0;
                short maxColDate = 9;
                short colDate = 1;
                hasNextPage = true;

                if (currentPage == 1)
                {
                    foreach (var item in date)
                    {
                        listDataPerPage.Add(new("date" + k.ToString(), 0, rowNo, CIUtil.SDateToShowSDate((int)item)));
                        colDate++;
                        if (colDate > maxColDate)
                        {
                            break;
                        }
                        k++;
                    }

                    foreach (var item in kensaInfDetailsItem)
                    {
                        listDataPerPage.Add(new("itemName", 0, rowNo, item.ItemName.TrimEnd()));
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

                    kensaInfDetailsItem.RemoveRange(0, maxRow);
                    _listTextData.Add(pageIndex, listDataPerPage);

                    return 1;
                }

                if (currentPage == 2)
                {
                    foreach (var item in date)
                    {
                        listDataPerPage.Add(new("date" + k.ToString(), 0, rowNo, CIUtil.SDateToShowSDate((int)item)));
                        colDate++;
                        if (colDate > maxColDate)
                        {
                            break;
                        }
                        k++;
                    }

                    foreach (var item in kensaInfDetailsItem)
                    {
                        listDataPerPage.Add(new("itemName", 0, rowNo, item.ItemName.TrimEnd()));
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

                    date.RemoveRange(0, maxColDate);

                    _listTextData.Add(pageIndex, listDataPerPage);

                    return 1;

                }

                if (currentPage == 3)
                {
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

                    foreach (var item in date)
                    {
                        listDataPerPage.Add(new("date" + k.ToString(), 0, rowNo, CIUtil.SDateToShowSDate((int)item)));
                        colDate++;
                        if (colDate > maxColDate)
                        {
                            break;
                        }
                        k++;
                    }

                    foreach (var item in kensaInfDetails)
                    {
                        listDataPerPage.Add(new("itemName", 0, rowNo, item.ItemName.TrimEnd()));
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

                    kensaInfDetails.RemoveRange(0, maxRow);
                    _listTextData.Add(pageIndex, listDataPerPage);

                    return 1;
                }

                if (currentPage == 4)
                {
                    foreach (var item in date)
                    {
                        listDataPerPage.Add(new("date" + k.ToString(), 0, rowNo, CIUtil.SDateToShowSDate((int)item)));
                        colDate++;
                        if (colDate > maxColDate)
                        {
                            break;
                        }
                        k++;
                    }

                    foreach (var item in kensaInfDetails)
                    {
                        listDataPerPage.Add(new("itemName", 0, rowNo, item.ItemName.TrimEnd()));
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

                    hasNextPage = false;
                    _listTextData.Add(pageIndex, listDataPerPage);

                    return 1;
                }

                return 1;
            }
            #endregion

            #endregion
            switch (t)
            {
                case 1:
                    if (UpdateFormHeader() < 0 || UpdateFormBodyP2() < 0)
                    {
                        return false;
                    }
                    break;
                default:
                    if (UpdateFormHeader() < 0 || UpdateFormBodyP1() < 0)
                    {
                        return false;
                    }
                    break;
            }


            return true;
        }

        private bool GetData()
        {
            hpInf = _coKensaHistoryFinder.GetHpInf(hpId, sinDate);
            ptInf = _coKensaHistoryFinder.GetPtInf(hpId, ptId);
            data = _coKensaHistoryFinder.GetListKensaInfDetail(hpId, userId, ptId, setId, startDate, endDate, showAbnormalKbn);

            List<CoKensaResultMultiModel> coKensaResultMultiModels = new();
            Dictionary<int, CoKensaResultMultiModel> parents = new();

            if (showAbnormalKbn)
            {
                foreach (var item in data.Item1)
                {
                    int count = 0;
                    foreach (var itemDynamicArray in item.KensaResultMultiItems)
                    {
                        if (itemDynamicArray.AbnormalKbn == "")
                        {
                            count++;
                        }

                        if (count == data.Item2.Count)
                        {
                            coKensaResultMultiModels.Add(item);
                        }
                    }
                }

                foreach (var item in coKensaResultMultiModels)
                {
                    data.Item1.Remove(item);
                }

                foreach (var item in coKensaResultMultiModels.Where(x => x.SeqParentNo == 0))
                {
                    var childrens = data.Item1.Where(x => x.SeqParentNo > 0 && item.RowSeqId.Contains(x.SeqParentNo.ToString()));
                    if (childrens != null)
                    {
                        var index = 99999999;
                        foreach (var itemChildren in childrens)
                        {
                            var indexNew = data.Item1.IndexOf(itemChildren);
                            if (indexNew < index)
                            {
                                index = indexNew;
                            }
                        }
                        parents.Add(index, item);
                    }
                }

                foreach (var item in parents)
                {
                    data.Item1.Insert(item.Key, item.Value);
                }
            }

            kensaInfDetails = new List<CoKensaResultMultiModel>(data.Item1);
            date = data.Item2;

            if (kensaInfDetails.Count > 0 && date.Count > 0)
            {
                iraiStart = date.First();
                iraiEnd = date.Last();
            }

            foreach (var item in kensaInfDetails)
            {

                foreach (var itemKensa in item.KensaResultMultiItems)
                {
                    switch (itemKensa.ResultType)
                    {
                        case "E": itemKensa.ChangeResultVal(itemKensa.ResultValue + "以下"); break;
                        case "L": itemKensa.ChangeResultVal(itemKensa.ResultValue + "未満"); break;
                        case "U": itemKensa.ChangeResultVal(itemKensa.ResultValue + "以上"); break;
                        default: break;
                    }
                }
            }

            if (date.Count > 9 && kensaInfDetails.Count - 1 > 20)
            {
                t = 1;
            }

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