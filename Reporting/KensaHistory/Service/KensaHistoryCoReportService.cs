using Domain.Models.HpInf;
using Domain.Models.KensaIrai;
using Domain.Models.KensaSet;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.KensaHistory.DB;
using Reporting.KensaHistory.Mapper;
using Reporting.Mappers.Common;
using Reporting.Sokatu;
using Reporting.Sokatu.Common.Utils;
using static Domain.Models.KensaIrai.ListKensaInfDetailModel;

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
        private int endDate;
        private bool showAbnormalKbn;
        private int itemQuantity;
        private PtInf ptInf;
        private ListKensaInfDetailModel kensaInfDetailModel;
        private List<ListKensaInfDetailItemModel> listKensaInfDetailItemModels = new();
        private bool hasNextPage;
        private int currentPage;

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private string _formFileName = "Form1.rse";
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
            this.endDate = endDate;
            this.showAbnormalKbn = showAbnormalKbn;
            this.itemQuantity = itemQuantity;
            var getData = GetData();

            if (startDate != 0 && endDate != 0)
            {
                _formFileName = "Form2";
            }
            if(getData) 
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
                SetFieldData("iraiDate", CIUtil.SDateToShowSDate(startDate));
                SetFieldData("issuedDate", CIUtil.GetJapanDateTimeNow().ToString());
                //保険者

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                List<ListTextObject> listDataPerPage = new();
                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                int row = 0;

                if (startDate != 0 && endDate != 0)
                {
                    
                }
                else
                {
                    foreach (var item in listKensaInfDetailItemModels)
                    {
                        listDataPerPage.Add(new("itemName", 0, row, item.KensaName));
                        listDataPerPage.Add(new("resultValue", 0, row, item.ResultVal));
                        listDataPerPage.Add(new("abnormalFlag", 0, row, item.AbnormalKbn));
                        listDataPerPage.Add(new("unit", 0, row, item.Unit));
                        listDataPerPage.Add(new("standardValue", 0, row, item.MaleStd));
                        row++;
                    }
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
            kensaInfDetailModel = _kokhoFinder.GetListKensaInfDetail(hpId, userId, ptId, setId, iraiCd, startDate, showAbnormalKbn, itemQuantity);
            var kensaInfDetails = kensaInfDetailModel.KensaInfDetailData.Select(x => x.DynamicArray);

            if (startDate != 0 && endDate != 0)
            {
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
            }
            else
            {
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

        private void SetVisibleFieldData(string field, bool value)
        {
            if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
            {
                _visibleFieldData.Add(field, value);
            }
        }
    }
}
