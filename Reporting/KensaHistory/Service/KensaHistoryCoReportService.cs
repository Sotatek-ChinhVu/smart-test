using Domain.Models.HpInf;
using Domain.Models.KensaIrai;
using Domain.Models.KensaSet;
using Entity.Tenant;
using Helper.Common;
using Reporting.KensaHistory.DB;
using Reporting.KensaHistory.Mapper;
using Reporting.Mappers.Common;

namespace Reporting.KensaHistory.Service
{
    public class KensaHistoryCoReportService : IKensaHistoryCoReportService
    {
        private IKensaSetRepository _kokhoFinder;
        private ICoKensaHistoryFinder _coKensaHistoryFinder;
        private HpInfModel hpInf;
        private int hpId;
        private int userId;
        private long ptId;
        private int setId;
        private int iraiCd;
        private int seikyuYm;
        private int startDate;
        private bool showAbnormalKbn;
        private int itemQuantity;
        private PtInf ptInf;
        private ListKensaInfDetailModel kensaInfDetailModel;
        private List<ListKensaInfDetailItemModel> listKensaInfDetailItemModels = new();

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private string _formFileName = "kensaResult.rse";
        private readonly Dictionary<int, ReportConfigModel> _reportConfigPerPage;
        private readonly Dictionary<string, bool> _visibleAtPrint;

        public KensaHistoryCoReportService(IKensaSetRepository kokhoFinder, ICoKensaHistoryFinder coKensaHistoryFinder)
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

        public CommonReportingRequestModel GetKensaHistoryPrintData(int hpId, int userId, long ptId, int setId, int iraiCd, int seikyuYm, int startDate, int endDate, bool showAbnormalKbn, int itemQuantity)
        {
            this.hpId = hpId;
            this.userId = userId;
            this.ptId = ptId;
            this.setId = setId;
            this.iraiCd = iraiCd;
            this.seikyuYm = seikyuYm;
            this.startDate = startDate;
            this.showAbnormalKbn = showAbnormalKbn;
            this.itemQuantity = itemQuantity;
            var getData = GetData();

            if (getData)
            {
                UpdateDrawForm();
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
                //医療機関コード
                SetFieldData("hpCode", hpInf.HpCd);
                SetFieldData("hpName", hpInf.HpName);
                SetFieldData("ptNum", ptInf.PtNum.ToString());
                SetFieldData("name", ptInf.Name ?? string.Empty);
                SetFieldData("iraiDate", CIUtil.SDateToShowSDate(seikyuYm));
                SetFieldData("issuedDate", CIUtil.GetJapanDateTimeNow().ToString());
                //保険者

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage1 = new();
                List<ListTextObject> listDataPerPage0 = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                short maxRow = 30;
                short row = 0;
                int rowNo = 0;

                foreach (var item in listKensaInfDetailItemModels)
                {
                    listDataPerPage0.Add(new("itemName", 0, rowNo, item.KensaName));
                    listDataPerPage0.Add(new("resultValue", 0, rowNo, item.ResultVal));
                    listDataPerPage0.Add(new("abnormalFlag", 0, rowNo, item.AbnormalKbn));
                    listDataPerPage0.Add(new("unit", 0, rowNo, item.Unit));
                    listDataPerPage0.Add(new("standardValue", 0, rowNo, item.MaleStd));
                    rowNo++;
                    if (rowNo == maxRow)
                    {
                        break;
                    }
                }

                if (listKensaInfDetailItemModels.Count < maxRow)
                {
                    listKensaInfDetailItemModels.RemoveRange(0, listKensaInfDetailItemModels.Count - 1);
                    SetFieldData("pageNumber", pageIndex.ToString() + "/" + pageIndex.ToString());
                    _listTextData.Add(pageIndex, listDataPerPage0);
                    return 1;
                }
                else
                {
                    listKensaInfDetailItemModels.RemoveRange(0, maxRow);
                    _listTextData.Add(pageIndex, listDataPerPage0);
                    pageIndex = pageIndex + 1;
                    foreach (var item in listKensaInfDetailItemModels)
                    {
                        rowNo = 0;
                        listDataPerPage1.Add(new("itemName", 0, rowNo, item.KensaName));
                        listDataPerPage1.Add(new("resultValue", 0, rowNo, item.ResultVal));
                        listDataPerPage1.Add(new("abnormalFlag", 0, rowNo, item.AbnormalKbn));
                        listDataPerPage1.Add(new("unit", 0, rowNo, item.Unit));
                        listDataPerPage1.Add(new("standardValue", 0, rowNo, item.MaleStd));
                        rowNo++;
                        if (rowNo == maxRow)
                        {
                            break;
                        }
                    }
                }
                _listTextData.Add(pageIndex, listDataPerPage1);

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
            kensaInfDetailModel = _kokhoFinder.GetListKensaInfDetail(hpId, userId, ptId, setId, iraiCd, startDate, showAbnormalKbn, itemQuantity);
            var kensaInfDetails = kensaInfDetailModel.KensaInfDetailData.Select(x => x.DynamicArray);

            foreach (var item in kensaInfDetails)
            {
                foreach (var index in item)
                {
                    if (index.IraiDate == seikyuYm)
                    {
                        listKensaInfDetailItemModels.Add(index);
                    }
                }
            }

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
