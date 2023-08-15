using Reporting.Mappers.Common;
using Reporting.Sokatu.WelfareDisk.Service;
using Reporting.Structs;

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
        private readonly IP44WelfareDiskService _p44WelfareDiskService;
        private readonly IP43Amakusa41DiskService _p43Amakusa41DiskService;
        private readonly IP46WelfareDiskService _p46WelfareDiskService;

        public ReceiptPrintExcelService(IP24WelfareDiskService p24WelfareDiskService, IP44WelfareDiskService p44WelfareDiskService, IP43Amakusa41DiskService p43Amakusa41DiskService, IP46WelfareDiskService p46WelfareDiskService)
        {
            _p24WelfareDiskService = p24WelfareDiskService;
            _p44WelfareDiskService = p44WelfareDiskService;
            _p43Amakusa41DiskService = p43Amakusa41DiskService;
            _p46WelfareDiskService = p46WelfareDiskService;
        }

        public CommonExcelReportingModel GetReceiptPrintExcel(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int seikyuYm)
        {
            CommonExcelReportingModel result = new();
            var seikyuType = GetSeikyuType(dataKbn);
            if (prefNo == 24 && reportId == 106 && reportEdaNo == 0)
            {
                result = _p24WelfareDiskService.GetDataP24WelfareDisk(hpId, seikyuYm, seikyuType);
            }
            else if (prefNo == 44 && reportId == 106 && reportEdaNo == 0)
            {
                result = _p44WelfareDiskService.GetDataP44WelfareDisk(hpId, seikyuYm, seikyuType);
            }
            else if (prefNo == 43 && reportId == 106 && reportEdaNo == 0)
            {
                result = _p43Amakusa41DiskService.GetDataP43Amakusa41Disk(hpId, seikyuYm, seikyuType);
            }
            else if (prefNo == 46 && reportId == 106 && reportEdaNo == 0)
            {
                result = _p46WelfareDiskService.GetDataP46WelfareDisk(hpId, seikyuYm, seikyuType);
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
