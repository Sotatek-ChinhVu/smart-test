using Domain.Models.HpInf;
using Domain.Models.KensaIrai;
using Domain.Models.KensaSet;
using Entity.Tenant;
using Helper.Common;
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
        private int row;
        private List<string> itemKensa1 = new();
        private PtInf ptInf;
        private List<CoKensaResultMultiModel> abc = new();
        private ListKensaInfDetailModel kensaInfDetailModel;
        private List<ListKensaInfDetailItemModel> listKensaInfDetailItemModels = new();
        private bool hasNextPage;
        private int currentPage;
        private List<string> itemKensa = new();

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

        public CommonReportingRequestModel GetKensaResultMultiPrintData(int hpId, int userId, long ptId, int setId, int iraiCd, int startDate, int endDate, bool showAbnormalKbn, int itemQuantity)
        {

            this.hpId = hpId;
            this.userId = userId;
            this.ptId = ptId;
            this.setId = setId;
            this.iraiCd = iraiCd;
            this.startDate = startDate;
            this.endDate = endDate;
            this.showAbnormalKbn = showAbnormalKbn;
            this.itemQuantity = itemQuantity;
            var getData = GetData();

            if (getData)
            {
                List<long> iraiDates = listKensaInfDetailItemModels.Select(x => x.IraiDate).Distinct().ToList();
                row = 0;
                foreach (var item in iraiDates)
                {
                    iraiDate = item;
                    currentPage = 1;
                    hasNextPage = true;
                    while (hasNextPage)
                    {
                        UpdateDrawForm();
                        currentPage++;
                    }
                    row++;
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
                SetFieldData("iraiStartDate", CIUtil.SDateToShowSDate(startDate));
                SetFieldData("iraiEndDate", CIUtil.SDateToShowSDate(endDate));
                SetFieldData("issuedDate", CIUtil.GetJapanDateTimeNow().ToString());
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                fieldDataPerPage.Add("pageNumber", pageIndex.ToString() + "/" + currentPage.ToString());
                _setFieldData.Add(pageIndex, fieldDataPerPage);
                //保険者

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage1 = new();
                List<ListTextObject> listDataPerPage0 = new();
                Dictionary<string, string> fieldDataPerPage = new();

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                short maxRow = 23;
                int rowNo = 0;
                hasNextPage = false;

                foreach (var item in listKensaInfDetailItemModels.Where(x => x.IraiDate == iraiDate))
                {
                    listDataPerPage0.Add(new("date" + row.ToString(), 0, rowNo, CIUtil.SDateToShowSDate((int)iraiDate)));
                    listDataPerPage0.Add(new("itemName", 0, rowNo, item.KensaName));
                    listDataPerPage0.Add(new("unit", 0, rowNo, item.Unit));
                    listDataPerPage0.Add(new("standardValue", 0, rowNo, item.MaleStd));
                    listDataPerPage0.Add(new("resultValue" + row.ToString(), 0, rowNo, item.ResultVal));
                    listDataPerPage0.Add(new("abnormalFlag" + row.ToString(), 0, rowNo, item.AbnormalKbn));
                    rowNo++;
                }

                _listTextData.Add(pageIndex, listDataPerPage0);
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
            abc = _coKensaHistoryFinder.GetListKensaInfDetail(hpId, userId, ptId, setId, iraiCd, startDate, showAbnormalKbn, itemQuantity);
            var kensaInfDetails = kensaInfDetailModel.KensaInfDetailData.Select(x => x.DynamicArray);

            foreach (var item in kensaInfDetails)
            {
                foreach (var index in item)
                {
                    if (index.IraiDate >= startDate && index.IraiDate <= endDate)
                    {
                        listKensaInfDetailItemModels.Add(index);
                    }
                }
            }

            itemKensa = listKensaInfDetailItemModels.Select(x => x.KensaName).Distinct().ToList();
            int count = 0;
            foreach (var item in itemKensa)
            {
                itemKensa1.Add(item);
                count++;
                if (count == 22)
                {
                    break;
                }
            }
            itemKensa.RemoveRange(0, 22);

            return listKensaInfDetailItemModels.Count > 0;
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
