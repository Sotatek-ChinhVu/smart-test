using Domain.Models.HpInf;
using Domain.Models.KensaIrai;
using Entity.Tenant;
using Helper.Common;
using Reporting.KensaHistory.DB;
using Reporting.KensaHistory.Mapper;
using Reporting.Mappers.Common;
using static Domain.Models.KensaIrai.ListKensaInfDetailModel;

namespace Reporting.KensaHistory.Service
{
    public class KensaHistoryCoReportService : IKensaHistoryCoReportService
    {
        private readonly ICoKensaHistoryFinder _coKensaHistoryFinder;
        private HpInfModel hpInf = new();
        private int hpId;
        private int userId;
        private long ptId;
        private int setId;
        private int iraiDate;
        private int startDate;
        private int sinDate;
        private bool showAbnormalKbn;
        private PtInf ptInf = new();
        private ListKensaInfDetailModel kensaInfDetailModel = new();
        private List<ListKensaInfDetailItemModel> listKensaInfDetailItemModels = new();
        private List<ListKensaInfDetailItemModel> listKensaInfDetailItemModelItems = new();
        private bool hasNextPage;
        private int currentPage;
        private int totalPage;

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly string _formFileName = "kensaResult.rse";
        private readonly Dictionary<int, ReportConfigModel> _reportConfigPerPage = new();
        private readonly Dictionary<string, bool> _visibleAtPrint;

        public KensaHistoryCoReportService(ICoKensaHistoryFinder coKensaHistoryFinder)
        {
            _setFieldData = new();
            _singleFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            _visibleAtPrint = new();
            _coKensaHistoryFinder = coKensaHistoryFinder;
        }

        public CommonReportingRequestModel GetKensaHistoryPrintData(int hpId, int userId, long ptId, int setId, int iraiDate, int startDate, int endDate, bool showAbnormalKbn, int sinDate)
        {
            try
            {
                this.hpId = hpId;
                this.userId = userId;
                this.ptId = ptId;
                this.setId = setId;
                this.iraiDate = iraiDate;
                this.startDate = iraiDate;
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
                SetFieldData("iraiDate", CIUtil.SDateToShowSDate(iraiDate));
                SetFieldData("issuedDate", CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd HH:mm:ss"));
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
                short maxRow = 20;
                int rowNo = 0;

                if (currentPage == 1)
                {
                    foreach (var item in listKensaInfDetailItemModelItems)
                    {
                        listDataPerPage.Add(new("itemName", 0, rowNo, item.KensaName));
                        listDataPerPage.Add(new("resultValue", 0, rowNo, item.ResultVal));
                        listDataPerPage.Add(new("abnormalFlag", 0, rowNo, item.AbnormalKbn));
                        listDataPerPage.Add(new("unit", 0, rowNo, item.Unit));

                        switch (ptInf.Sex)
                        {
                            case 1: listDataPerPage.Add(new("standardValue", 0, rowNo, item.MaleStd)); break;
                            case 2: listDataPerPage.Add(new("standardValue", 0, rowNo, item.FemaleStd)); break;
                        }
                        rowNo++;
                        if (rowNo == maxRow)
                        {
                            break;
                        }
                    }

                    if (listKensaInfDetailItemModelItems.Count <= maxRow)
                    {
                        _listTextData.Add(pageIndex, listDataPerPage);
                        hasNextPage = false;
                        return 1;
                    }
                    else
                    {
                        hasNextPage = true;
                        listKensaInfDetailItemModelItems.RemoveRange(0, maxRow);
                        _listTextData.Add(pageIndex, listDataPerPage);
                        return 1;
                    }
                }

                rowNo = 0;
                int count = listKensaInfDetailItemModelItems.Count;

                foreach (var item in listKensaInfDetailItemModelItems)
                {
                    listDataPerPage.Add(new("itemName", 0, rowNo, item.KensaName));
                    listDataPerPage.Add(new("resultValue", 0, rowNo, item.ResultVal));
                    listDataPerPage.Add(new("abnormalFlag", 0, rowNo, item.AbnormalKbn));
                    listDataPerPage.Add(new("unit", 0, rowNo, item.Unit));

                    switch (ptInf.Sex)
                    {
                        case 1: listDataPerPage.Add(new("standardValue", 0, rowNo, item.MaleStd)); break;
                        case 2: listDataPerPage.Add(new("standardValue", 0, rowNo, item.FemaleStd)); break;
                    }

                    rowNo++;
                    if (rowNo == maxRow)
                    {
                        break;
                    }
                }

                if (count > maxRow)
                {
                    listKensaInfDetailItemModelItems.RemoveRange(0, maxRow);
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
            hpInf = _coKensaHistoryFinder.GetHpInf(hpId, sinDate);
            ptInf = _coKensaHistoryFinder.GetPtInf(hpId, ptId);
            kensaInfDetailModel = _coKensaHistoryFinder.GetListKensaInf(hpId, userId, ptId, setId, 0, false, showAbnormalKbn, startDate);

            var kensaInfDetailItems = kensaInfDetailModel.KensaInfDetailData.ToList();
            List<KensaInfDetailDataModel> KensaInfDetailDataAbnormal = new();
            Dictionary<int, KensaInfDetailDataModel> parents = new();

            if (showAbnormalKbn)
            {
                foreach (var item in kensaInfDetailItems)
                {
                    if (item.DynamicArray.Where(x => x.AbnormalKbn == "").Count() == item.DynamicArray.Count())
                    {
                        KensaInfDetailDataAbnormal.Add(item);
                    }
                }

                foreach (var item in KensaInfDetailDataAbnormal)
                {
                    kensaInfDetailItems.Remove(item);
                }

                int i = 0;

                foreach (var item in KensaInfDetailDataAbnormal.Where(x => x.SeqParentNo == 0))
                {
                    var childrens = kensaInfDetailItems.Where(x => x.SeqParentNo > 0 && item.RowSeqId.Contains(x.SeqParentNo.ToString()));

                    if (childrens != null)
                    {
                        var index = 99999999;
                        foreach (var itemChildren in childrens)
                        {
                            var indexNew = kensaInfDetailItems.IndexOf(itemChildren);

                            if (childrens.Count() > 1)
                            {
                                if (indexNew < index)
                                {
                                    index = indexNew;
                                }
                            }
                            else
                            {
                                index = indexNew;
                            }
                        }

                        if (index != 99999999)
                        {
                            parents.Add(index + i, item);
                            i++;
                        }
                    }
                }

                foreach (var item in parents)
                {
                    kensaInfDetailItems.Insert(item.Key, item.Value);
                }
            }

            var kensaInfDetails = kensaInfDetailItems.Select(x => x.DynamicArray).ToList();

            foreach (var item in kensaInfDetails)
            {
                foreach (var index in item)
                {
                    if (index.IraiDate == iraiDate)
                    {
                        listKensaInfDetailItemModels.Add(index);
                    }
                }
            }

            var listKensaInfDetail = listKensaInfDetailItemModels.GroupBy(x => x.IraiCd);

            foreach (var item in listKensaInfDetail)
            {
                foreach (var index in item)
                {
                    listKensaInfDetailItemModelItems.Add(new ListKensaInfDetailItemModel(index.IraiCd, index.KensaName, index.ResultVal, index.AbnormalKbn, index.Unit, index.MaleStd, index.FemaleStd, index.ResultType, index.RowSeqId, index.SeqParentNo));
                }
            }

            foreach (var item in listKensaInfDetailItemModelItems)
            {
                switch (item.ResultType)
                {
                    case "E": item.ChangeResultVal(item.ResultVal + "以下"); break;
                    case "L": item.ChangeResultVal(item.ResultVal + "未満"); break;
                    case "U": item.ChangeResultVal(item.ResultVal + "以上"); break;
                    default: break;
                }
            }

            return listKensaInfDetailItemModelItems.Count > 0;
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
