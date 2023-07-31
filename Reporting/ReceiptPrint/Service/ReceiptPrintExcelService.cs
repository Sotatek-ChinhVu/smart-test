using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReceiptList.Model;
using Reporting.ReceiptList.Service;
using Reporting.Sokatu.WelfareDisk.Service;
using Reporting.Structs;
using System.Collections.Generic;
using ReceiptListModel = Reporting.ReceiptList.Model.ReceiptListModel;

namespace Reporting.ReceiptPrint.Service
{
    public class ReceiptPrintExcelService : IReceiptPrintExcelService
    {
        #region properties
        private bool _isNormal { get; set; }
        private bool _isDelay { get; set; }
        private bool _isHenrei { get; set; }
        private bool _isPaper { get; set; }
        private bool _isOnline { get; set; }
        #endregion

        private readonly IP24WelfareDiskService _p24WelfareDiskService;
        private readonly IImportCSVCoReportService _importCSVCoReportService;

        public ReceiptPrintExcelService(IP24WelfareDiskService p24WelfareDiskService, IImportCSVCoReportService importCSVCoReportService)
        {
            _p24WelfareDiskService = p24WelfareDiskService;
            _importCSVCoReportService= importCSVCoReportService;
        }

        public CommonExcelReportingModel GetReceiptPrintExcel(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int seikyuYm, List<ReceiptListModel> receiptListModel, CoFileType printType)
        {
            CommonExcelReportingModel result = new();
            var seikyuType = GetSeikyuType(dataKbn);
            if (prefNo == 24 && reportId == 106 && reportEdaNo == 0)
            {
                _p24WelfareDiskService.GetDataP24WelfareDisk(hpId, seikyuYm, seikyuType);
            }
            else if (printType == CoFileType.Csv)
            {
                _importCSVCoReportService.GetImportCSVCoReportServiceReportingData(receiptListModel, printType, false);
            }
            return result;
        }

        private SeikyuType GetSeikyuType(int dataKbn)
        {
            int targetReceiptVal = (dataKbn >= 0 && dataKbn <= 2) ? (dataKbn + 1) : 0;
            switch (targetReceiptVal)
            {
                case 1:
                    _isNormal = true;
                    _isDelay = true;
                    _isHenrei = true;
                    _isPaper = true;
                    _isOnline = false;
                    break;
                case 2:
                    _isNormal = true;
                    _isDelay = true;
                    _isHenrei = false;
                    _isPaper = false;
                    _isOnline = false;
                    break;
                case 3:
                    _isNormal = false;
                    _isDelay = false;
                    _isHenrei = true;
                    _isPaper = true;
                    _isOnline = false;
                    break;
                default:
                    _isNormal = false;
                    _isDelay = false;
                    _isHenrei = false;
                    _isPaper = false;
                    _isOnline = false;
                    break;
            }

            return new SeikyuType(_isNormal, _isPaper, _isDelay, _isHenrei, _isOnline);
        }
    }
}
