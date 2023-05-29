using Reporting.Mappers.Common;
using Reporting.Sokatu.KokhoSokatu.Service;
using Reporting.Sokatu.KoukiSeikyu.Service;
using Reporting.Structs;

namespace Reporting.ReceiptPrint.Service
{
    public class ReceiptPrintService : IReceiptPrintService
    {
        #region properties
        private bool _isNormal { get; set; }
        private bool _isDelay { get; set; }
        private bool _isHenrei { get; set; }
        private bool _isPaper { get; set; }
        private bool _isOnline { get; set; }

        #endregion

        private readonly IP28KokhoSokatuCoReportService _p28KokhoSokatuCoReportService;
        private readonly IP11KokhoSokatuCoReportService _p11KokhoSokatuCoReportService;
        private readonly IP28KoukiSeikyuCoReportService _p28KoukiSeikyuCoReportService;
        private readonly IP29KoukiSeikyuCoReportService _p29KoukiSeikyuCoReportService;
        private readonly IP09KoukiSeikyuCoReportService _p09KoukiSeikyuCoReportService;

        public ReceiptPrintService(IP28KokhoSokatuCoReportService p28KokhoSokatuCoReportService
            , IP11KokhoSokatuCoReportService p11KokhoSokatuCoReportService
            , IP28KoukiSeikyuCoReportService p28KoukiSeikyuCoReportService
            , IP29KoukiSeikyuCoReportService p29KoukiSeikyuCoReportService
            , IP09KoukiSeikyuCoReportService p09KoukiSeikyuCoReportService    

                                  )
        {
            _p28KokhoSokatuCoReportService = p28KokhoSokatuCoReportService;
            _p11KokhoSokatuCoReportService = p11KokhoSokatuCoReportService;
            _p28KoukiSeikyuCoReportService = p28KoukiSeikyuCoReportService;
            _p29KoukiSeikyuCoReportService = p29KoukiSeikyuCoReportService;
            _p09KoukiSeikyuCoReportService = p09KoukiSeikyuCoReportService;
        }

        public CommonReportingRequestModel GetReceiptPrint(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int ptId, int seikyuYm, int sinYm, int hokenId)
        {
            var seikyuType = GetSeikyuType(dataKbn);

            if (prefNo == 28 && reportId == 102 && reportEdaNo == 0)
            {
                return _p28KokhoSokatuCoReportService.GetP28KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
            }
            else if (prefNo == 11 && reportId == 102 && reportEdaNo == 0)
            {
                return _p11KokhoSokatuCoReportService.GetP11KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
            }else if (prefNo == 28 && reportId == 104 && reportEdaNo == 0)
            {
                return _p28KoukiSeikyuCoReportService.GetP28KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
            }else if (prefNo == 29 && reportId == 104 && reportEdaNo == 0)
            {
                return _p29KoukiSeikyuCoReportService.GetP29KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
            }else if (prefNo == 09 && reportId == 104 && reportEdaNo == 0)
            {
                return _p09KoukiSeikyuCoReportService.GetP09KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
            }

            return new();
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
