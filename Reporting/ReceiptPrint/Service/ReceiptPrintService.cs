using Reporting.Mappers.Common;
using Reporting.Sokatu.AfterCareSeikyu.Service;
using Reporting.Sokatu.HikariDisk.Service;
using Reporting.Sokatu.KokhoSokatu.Service;
using Reporting.Sokatu.KoukiSeikyu.Service;
using Reporting.Sokatu.Syaho.Service;
using Reporting.Structs;

namespace Reporting.ReceiptPrint.Service;

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
    private readonly IHikariDiskCoReportService _hikariDiskCoReportService;
    private readonly IP28KoukiSeikyuCoReportService _p28KoukiSeikyuCoReportService;
    private readonly IP29KoukiSeikyuCoReportService _p29KoukiSeikyuCoReportService;
    private readonly IAfterCareSeikyuCoReportService _afterCareSeikyuCoReportService;
    private readonly ISyahoCoReportService _syahoCoReportService;
    private readonly IP30KoukiSeikyuCoReportService _p30KoukiSeikyuCoReportService;
    private readonly IP33KoukiSeikyuCoReportService _p33KoukiSeikyuCoReportService;
    private readonly IP34KoukiSeikyuCoReportService _p34KoukiSeikyuCoReportService;
    private readonly IP35KoukiSeikyuCoReportService _p35KoukiSeikyuCoReportService;
    private readonly IP37KoukiSeikyuCoReportService _p37KoukiSeikyuCoReportService;
    private readonly IP40KoukiSeikyuCoReportService _p40KoukiSeikyuCoReportService;

    public ReceiptPrintService(IP28KokhoSokatuCoReportService p28KokhoSokatuCoReportService, IP11KokhoSokatuCoReportService p11KokhoSokatuCoReportService, IHikariDiskCoReportService hikariDiskCoReportService, IP28KoukiSeikyuCoReportService p28KoukiSeikyuCoReportService, IP29KoukiSeikyuCoReportService p29KoukiSeikyuCoReportService, IAfterCareSeikyuCoReportService afterCareSeikyuCoReportService, ISyahoCoReportService syahoCoReportService, IP33KoukiSeikyuCoReportService p33KoukiSeikyuCoReportService, IP34KoukiSeikyuCoReportService p34KoukiSeikyuCoReportService, IP35KoukiSeikyuCoReportService p35KoukiSeikyuCoReportService, IP37KoukiSeikyuCoReportService p37KoukiSeikyuCoReportService, IP40KoukiSeikyuCoReportService p40KoukiSeikyuCoReportService, IP30KoukiSeikyuCoReportService p30KoukiSeikyuCoReportService)
    {
        _p28KokhoSokatuCoReportService = p28KokhoSokatuCoReportService;
        _p11KokhoSokatuCoReportService = p11KokhoSokatuCoReportService;
        _hikariDiskCoReportService = hikariDiskCoReportService;
        _p28KoukiSeikyuCoReportService = p28KoukiSeikyuCoReportService;
        _p29KoukiSeikyuCoReportService = p29KoukiSeikyuCoReportService;
        _afterCareSeikyuCoReportService = afterCareSeikyuCoReportService;
        _syahoCoReportService = syahoCoReportService;
        _p33KoukiSeikyuCoReportService = p33KoukiSeikyuCoReportService;
        _p34KoukiSeikyuCoReportService = p34KoukiSeikyuCoReportService;
        _p35KoukiSeikyuCoReportService = p35KoukiSeikyuCoReportService;
        _p37KoukiSeikyuCoReportService = p37KoukiSeikyuCoReportService;
        _p40KoukiSeikyuCoReportService = p40KoukiSeikyuCoReportService;
        _p30KoukiSeikyuCoReportService = p30KoukiSeikyuCoReportService;
    }

    public CommonReportingRequestModel GetReceiptPrint(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt)
    {
        var seikyuType = GetSeikyuType(dataKbn);

        if (prefNo == 28 && reportId == 102 && reportEdaNo == 0)
        {
            return _p28KokhoSokatuCoReportService.GetP28KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 11 && reportId == 102 && reportEdaNo == 0)
        {
            return _p11KokhoSokatuCoReportService.GetP11KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 28 && reportId == 104 && reportEdaNo == 0)
        {
            return _p28KoukiSeikyuCoReportService.GetP28KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 29 && reportId == 104 && reportEdaNo == 0)
        {
            return _p29KoukiSeikyuCoReportService.GetP29KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (reportId == 2 && reportEdaNo == 0)
        {
            int hokenKbn = 1;
            return _hikariDiskCoReportService.GetHikariDiskPrintData(hpId, seikyuYm, hokenKbn, diskKind, diskCnt);
        }
        else if (reportId == 4 && reportEdaNo == 0)
        {
            return _afterCareSeikyuCoReportService.GetAfterCareSeikyuPrintData(hpId, seikyuYm, GetSeikyuType(dataKbn));
        }
        else if (reportId == 101 && reportEdaNo == 0)
        {
            return _syahoCoReportService.GetSyahoPrintData(hpId, seikyuYm, GetSeikyuType(dataKbn));
        }
        else if (reportId == 104 && reportEdaNo == 0)
        {
            return _p33KoukiSeikyuCoReportService.GetP33KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (reportId == 104 && reportEdaNo == 0)
        {
            return _p34KoukiSeikyuCoReportService.GetP34KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (reportId == 104 && reportEdaNo == 0)
        {
            return _p35KoukiSeikyuCoReportService.GetP35KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (reportId == 104 && reportEdaNo == 0)
        {
            return _p37KoukiSeikyuCoReportService.GetP37KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (reportId == 104 && reportEdaNo == 0)
        {
            return _p40KoukiSeikyuCoReportService.GetP40KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (reportId == 104 && reportEdaNo == 0)
        {
            return _p30KoukiSeikyuCoReportService.GetP30KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
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

    public enum TargetReceipt
    {
        All = 1,//すべて
        DenshiSeikyu = 2, //電子請求
        KamiSeikyu = 3, //紙請求
    }
}
