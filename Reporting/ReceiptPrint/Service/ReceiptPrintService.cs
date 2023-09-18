using Reporting.Mappers.Common;
using Reporting.Receipt.Service;
using Reporting.ReceTarget.Service;
using Reporting.Sokatu;
using Reporting.Sokatu.AfterCareSeikyu.Service;
using Reporting.Sokatu.HikariDisk.Service;
using Reporting.Sokatu.KokhoSeikyu.Service;
using Reporting.Sokatu.KokhoSokatu.Service;
using Reporting.Sokatu.KoukiSeikyu.Service;
using Reporting.Sokatu.Syaho.Service;
using Reporting.Sokatu.WelfareSeikyu.Service;
using Reporting.Structs;
using Reporting.SyojyoSyoki.Service;

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

    #region Contructor
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
    private readonly IP42KoukiSeikyuCoReportService _p42KoukiSeikyuCoReportService;
    private readonly IP45KoukiSeikyuCoReportService _p45KoukiSeikyuCoReportService;
    private readonly IP09KoukiSeikyuCoReportService _p09KoukiSeikyuCoReportService;
    private readonly IP12KoukiSeikyuCoReportService _p12KoukiSeikyuCoReportService;
    private readonly IP13KoukiSeikyuCoReportService _p13KoukiSeikyuCoReportService;
    private readonly IP08KokhoSokatuCoReportService _p08KokhoSokatuCoReportService;
    private readonly IP41KoukiSeikyuCoReportService _p41KoukiSeikyuCoReportService;
    private readonly IP44KoukiSeikyuCoReportService _p44KoukiSeikyuCoReportService;
    private readonly IP08KoukiSeikyuCoReportService _p08KoukiSeikyuCoReportService;
    private readonly IP11KoukiSeikyuCoReportService _p11KoukiSeikyuCoReportService;
    private readonly IP14KoukiSeikyuCoReportService _p14KoukiSeikyuCoReportService;
    private readonly IP17KoukiSeikyuCoReportService _p17KoukiSeikyuCoReportService;
    private readonly IP20KoukiSeikyuCoReportService _p20KoukiSeikyuCoReportService;
    private readonly IP25KokhoSokatuCoReportService _p25KokhoSokatuCoReportService;
    private readonly IP13WelfareSeikyuCoReportService _p13WelfareSeikyuCoReportService;
    private readonly IP08KokhoSeikyuCoReportService _p08KokhoSeikyuCoReportService;
    private readonly IP22WelfareSeikyuCoReportService _p22WelfareSeikyuCoReportService;
    private readonly IP21KoukiSeikyuCoReportService _p21KoukiSeikyuCoReportService;
    private readonly IP22KoukiSeikyuCoReportService _p22KoukiSeikyuCoReportService;
    private readonly IP23KoukiSeikyuCoReportService _p23KoukiSeikyuCoReportService;
    private readonly IP24KoukiSeikyuCoReportService _p24KoukiSeikyuCoReportService;
    private readonly IP25KoukiSeikyuCoReportService _p25KoukiSeikyuCoReportService;
    private readonly IP27KoukiSeikyuCoReportService _p27KoukiSeikyuCoReportService;
    private readonly IP14KokhoSokatuCoReportService _p14KokhoSokatuCoReportService;
    private readonly IP17KokhoSokatuCoReportService _p17KokhoSokatuCoReportService;
    private readonly IP20KokhoSokatuCoReportService _p20KokhoSokatuCoReportService;
    private readonly IP22KokhoSokatuCoReportService _p22KokhoSokatuCoReportService;
    private readonly IP23KokhoSokatuCoReportService _p23KokhoSokatuCoReportService;
    private readonly IP26KokhoSokatuInCoReportService _p26KokhoSokatuInCoReportService;
    private readonly IP33KokhoSokatuCoReportService _p33KokhoSokatuCoReportService;
    private readonly IP34KokhoSokatuCoReportService _p34KokhoSokatuCoReportService;
    private readonly IP35KokhoSokatuCoReportService _p35KokhoSokatuCoReportService;
    private readonly IP37KokhoSokatuCoReportService _p37KokhoSokatuCoReportService;
    private readonly IP37KoukiSokatuCoReportService _p37KoukiSokatuCoReportService;
    private readonly IP26KokhoSokatuOutCoReportService _p26KokhoSokatuOutCoReportService;
    private readonly IP40KokhoSokatuCoReportService _p40KokhoSokatuCoReportService;
    private readonly IP41KokhoSokatuCoReportService _p41KokhoSokatuCoReportService;
    private readonly IP42KokhoSokatuCoReportService _p42KokhoSokatuCoReportService;
    private readonly IP12KokhoSokatuCoReportService _p12KokhoSokatuCoReportService;
    private readonly IP13KokhoSokatuCoReportService _p13KokhoSokatuCoReportService;
    private readonly IP43KokhoSokatuCoReportService _P43KokhoSokatuCoReportService;
    private readonly IP43KoukiSokatuCoReportService _p43KoukiSokatuCoReportService;
    private readonly IP44KokhoSokatuCoReportService _p44KokhoSokatuCoReportService;
    private readonly IP45KokhoSokatuCoReportService _p45KokhoSokatuCoReportService;
    private readonly IP45KoukiSokatuCoReportService _p45KoukiSokatuCoReportService;
    private readonly IP12KokhoSeikyuCoReportService _p12KokhoSeikyuCoReportService;
    private readonly IP13KokhoSeikyuCoReportService _p13KokhoSeikyuCoReportService;
    private readonly IP14KokhoSeikyuCoReportService _p14KokhoSeikyuCoReportService;
    private readonly IP20KokhoSeikyuCoReportService _p20KokhoSeikyuCoReportService;
    private readonly IP21KokhoSeikyuCoReportService _p21KokhoSeikyuCoReportService;
    private readonly IP22KokhoSeikyuCoReportService _p22KokhoSeikyuCoReportService;
    private readonly IP23KokhoSeikyuCoReportService _p23KokhoSeikyuCoReportService;
    private readonly IP24KokhoSeikyuCoReportService _p24KokhoSeikyuCoReportService;
    private readonly IReceiptCoReportService _receiptCoReportService;
    private readonly IP26KokhoSeikyuOutCoReportService _p26KokhoSeikyuOutCoReportService;
    private readonly IP27KokhoSeikyuInCoReportService _p27KokhoSeikyuInCoReportService;
    private readonly IP27KokhoSeikyuOutCoReportService _p27KokhoSeikyuOutCoReportService;
    private readonly IP28KokhoSeikyuCoReportService _p28KokhoSeikyuCoReportService;
    private readonly IP29KokhoSeikyuCoReportService _p29KokhoSeikyuCoReportService;
    private readonly IP30KokhoSeikyuCoReportService _p30KokhoSeikyuCoReportService;
    private readonly IP42KokhoSeikyuCoReportService _p42KokhoSeikyuCoReportService;
    private readonly IP43KokhoSeikyuCoReportService _p43KokhoSeikyuCoReportService;
    private readonly IP20WelfareSokatuCoReportService _p20WelfareSokatuCoReportService;
    private readonly IP21WelfareSeikyuCoReportService _p21WelfareSeikyuCoReportService;
    private readonly IP21WelfareSokatuCoReportService _p21WelfareSokatuCoReportService;
    private readonly IP09KokhoSeikyuCoReportService _p09KokhoSeikyuCoReportService;
    private readonly IP23NagoyaSeikyuCoReportService _p23NagoyaSeikyuCoReportService;
    private readonly IP23WelfareSeikyuCoReportService _p23WelfareSeikyuCoReportService;
    private readonly IP24WelfareSofuDiskCoReportService _p24WelfareSofuDiskCoReportService;
    private readonly IP24WelfareSofuPaperCoReportService _p24WelfareSofuPaperCoReportService;
    private readonly IP24WelfareSyomeiCoReportService _p24WelfareSyomeiCoReportService;
    private readonly IP24WelfareSyomeiListCoReportService _p24WelfareSyomeiListCoReportService;
    private readonly IP24WelfareSyomeiSofuCoReportService _p24WelfareSyomeiSofuCoReportService;
    private readonly IP26VaccineSokatuCoReportService _p26VaccineSokatuCoReportService;
    private readonly IP27IzumisanoSeikyuCoReportService _p27IzumisanoSeikyuCoReportService;
    private readonly IP35WelfareSeikyuCoReportService _p35WelfareSeikyuCoReportService;
    private readonly IP35WelfareSokatuCoReportService _p35WelfareSokatuCoReportService;
    private readonly IP43KikuchiMeisai41CoReportService _p43KikuchiMeisai41CoReportService;
    private readonly IP43KikuchiMeisai43CoReportService _p43KikuchiMeisai43CoReportService;
    private readonly IP43KikuchiSeikyu41CoReportService _p43KikuchiSeikyu41CoReportService;
    private readonly IP43KikuchiSeikyu43CoReportService _p43KikuchiSeikyu43CoReportService;
    private readonly IP43KumamotoSeikyuCoReportService _p43KumamotoSeikyuCoReportService;
    private readonly IP44WelfareSeikyu84CoReportService _p44WelfareSeikyu84CoReportService;
    private readonly IP43ReihokuSeikyu41CoReportService _p43ReihokuSeikyu41CoReportService;
    private readonly IP43AmakusaSeikyu41CoReportService _p43AmakusaSeikyu41CoReportService;
    private readonly IP43AmakusaSeikyu42CoReportService _p43AmakusaSeikyu42CoReportService;
    private readonly IReceTargetCoReportService _receTargetCoReportService;
    private readonly ISyojyoSyokiCoReportService _syojyoSyokiCoReportService;
    private readonly IP11KokhoSeikyuCoReportService _p11KokhoSeikyuCoReportService;
    private readonly IP14WelfareSeikyuCoReportService _p14WelfareSeikyuCoReportService;
    private readonly IP17KokhoSeikyuCoReportService _p17KokhoSeikyuCoReportService;
    private readonly IP17WelfareSeikyuCoReportService _p17WelfareSeikyuCoReportService;
    private readonly IP25WelfareSeikyuCoReportService _p25WelfareSeikyuCoReportService;
    private readonly IP29WelfareSeikyuCoReportService _p29WelfareSeikyuCoReportService;
    private readonly IP33KokhoSeikyuCoReportService _p33KokhoSeikyuCoReportService;
    private readonly IP26KoukiSokatuInCoReportService _p26KoukiSokatuInCoReportService;
    private readonly IP25KokhoSeikyuCoReportService _p25KokhoSeikyuCoReportService;
    private readonly IP26WelfareSeikyuCoReportService _p26WelfareSeikyuCoReportService;
    private readonly IP34KokhoSeikyuCoReportService _p34KokhoSeikyuCoReportService;
    private readonly IP35KokhoSeikyuCoReportService _p35KokhoSeikyuCoReportService;
    private readonly IP37KokhoSeikyuCoReportService _p37KokhoSeikyuCoReportService;
    private readonly IP40KokhoSeikyuCityCoReportService _p40KokhoSeikyuCityCoReportService;
    private readonly IP40KokhoSeikyuKumiaiCoReportService _p40KokhoSeikyuKumiaiCoReportService;
    private readonly IP40WelfareSeikyuCoReportService _p40WelfareSeikyuCoReportService;
    private readonly IP41KokhoSeikyuCoReportService _p41KokhoSeikyuCoReportService;
    private readonly IP44KokhoSeikyuCoReportService _p44KokhoSeikyuCoReportService;
    private readonly IP45KokhoSeikyuCoReportService _p45KokhoSeikyuCoReportService;
    private readonly IP46KokhoSokatuCoReportService _p46KokhoSokatuCoReportService;
    private readonly IP46KokhoSeikyuCoReportService _p46KokhoSeikyuCoReportService;
    private readonly IP46KoukiSeikyuCoReportService _p46KoukiSeikyuCoReportService;
    private readonly IP46WelfareSofu99CoReportService _p46WelfareSofu99CoReportService;
    private readonly IP46WelfareSeikyu99CoReportService _p46WelfareSeikyu99CoReportService;

    public ReceiptPrintService(IP28KokhoSokatuCoReportService p28KokhoSokatuCoReportService, IP11KokhoSokatuCoReportService p11KokhoSokatuCoReportService, IHikariDiskCoReportService hikariDiskCoReportService, IP28KoukiSeikyuCoReportService p28KoukiSeikyuCoReportService, IP29KoukiSeikyuCoReportService p29KoukiSeikyuCoReportService, IAfterCareSeikyuCoReportService afterCareSeikyuCoReportService, ISyahoCoReportService syahoCoReportService, IP45KoukiSeikyuCoReportService p45KoukiSeikyuCoReportService, IP33KoukiSeikyuCoReportService p33KoukiSeikyuCoReportService, IP34KoukiSeikyuCoReportService p34KoukiSeikyuCoReportService, IP35KoukiSeikyuCoReportService p35KoukiSeikyuCoReportService, IP37KoukiSeikyuCoReportService p37KoukiSeikyuCoReportService, IP40KoukiSeikyuCoReportService p40KoukiSeikyuCoReportService, IP42KoukiSeikyuCoReportService p42KoukiSeikyuCoReportService, IP09KoukiSeikyuCoReportService p09KoukiSeikyuCoReportService, IP12KoukiSeikyuCoReportService p12KoukiSeikyuCoReportService, IP13KoukiSeikyuCoReportService p13KoukiSeikyuCoReportService, IP30KoukiSeikyuCoReportService p30KoukiSeikyuCoReportService, IP41KoukiSeikyuCoReportService p41KoukiSeikyuCoReportService, IP08KokhoSokatuCoReportService p08KokhoSokatuCoReportService, IP44KoukiSeikyuCoReportService p44KoukiSeikyuCoReportService, IP08KoukiSeikyuCoReportService p08KoukiSeikyuCoReportService, IP11KoukiSeikyuCoReportService p11KoukiSeikyuCoReportService, IP14KoukiSeikyuCoReportService p14KoukiSeikyuCoReportService, IP17KoukiSeikyuCoReportService p17KoukiSeikyuCoReportService
                              , IP20KoukiSeikyuCoReportService p20KoukiSeikyuCoReportService, IP25KokhoSokatuCoReportService p25KokhoSokatuCoReportService, IP13WelfareSeikyuCoReportService p13WelfareSeikyuCoReportService, IP08KokhoSeikyuCoReportService p08KokhoSeikyuCoReportService, IP22WelfareSeikyuCoReportService p22WelfareSeikyuCoReportService, IP21KoukiSeikyuCoReportService p21KoukiSeikyuCoReportService, IP22KoukiSeikyuCoReportService p22KoukiSeikyuCoReportService, IP23KoukiSeikyuCoReportService p23KoukiSeikyuCoReportService, IP24KoukiSeikyuCoReportService p24KoukiSeikyuCoReportService, IP25KoukiSeikyuCoReportService p25KoukiSeikyuCoReportService, IP27KoukiSeikyuCoReportService p27KoukiSeikyuCoReportService, IP14KokhoSokatuCoReportService p14KokhoSokatuCoReportService, IP17KokhoSokatuCoReportService p17KokhoSokatuCoReportService, IP20KokhoSokatuCoReportService p20KokhoSokatuCoReportService, IP22KokhoSokatuCoReportService p22KokhoSokatuCoReportService, IP23KokhoSokatuCoReportService p23KokhoSokatuCoReportService, IP26KokhoSokatuInCoReportService p26KokhoSokatuInCoReportService, IP33KokhoSokatuCoReportService p33KokhoSokatuCoReportService, IP34KokhoSokatuCoReportService p34KokhoSokatuCoReportService, IP35KokhoSokatuCoReportService p35KokhoSokatuCoReportService, IP37KokhoSokatuCoReportService p37KokhoSokatuCoReportService, IP37KoukiSokatuCoReportService p37KoukiSokatuCoReportService, IP26KokhoSokatuOutCoReportService p26KokhoSokatuOutCoReportService, IP40KokhoSokatuCoReportService p40KokhoSokatuCoReportService
                              , IP41KokhoSokatuCoReportService p41KokhoSokatuCoReportService, IP42KokhoSokatuCoReportService p42KokhoSokatuCoReportService, IP12KokhoSokatuCoReportService p12KokhoSokatuCoReportService, IP13KokhoSokatuCoReportService p13KokhoSokatuCoReportService, IP43KokhoSokatuCoReportService p43KokhoSokatuCoReportService, IP43KoukiSokatuCoReportService p43KoukiSokatuCoReportService, IP44KokhoSokatuCoReportService p44KokhoSokatuCoReportService, IP45KokhoSokatuCoReportService p45KokhoSokatuCoReportService, IP45KoukiSokatuCoReportService p45KoukiSokatuCoReportService, IP12KokhoSeikyuCoReportService p12KokhoSeikyuCoReportService, IP13KokhoSeikyuCoReportService p13KokhoSeikyuCoReportService, IP14KokhoSeikyuCoReportService p14KokhoSeikyuCoReportService, IP20KokhoSeikyuCoReportService p20KokhoSeikyuCoReportService, IP21KokhoSeikyuCoReportService p21KokhoSeikyuCoReportService, IP22KokhoSeikyuCoReportService p22KokhoSeikyuCoReportService, IP23KokhoSeikyuCoReportService p23KokhoSeikyuCoReportService, IP24KokhoSeikyuCoReportService p24KokhoSeikyuCoReportService, IP26KokhoSeikyuOutCoReportService p26KokhoSeikyuOutCoReportService, IP27KokhoSeikyuInCoReportService p27KokhoSeikyuInCoReportService, IP27KokhoSeikyuOutCoReportService p27KokhoSeikyuOutCoReportService, IP28KokhoSeikyuCoReportService p28KokhoSeikyuCoReportService, IP29KokhoSeikyuCoReportService p29KokhoSeikyuCoReportService, IP30KokhoSeikyuCoReportService p30KokhoSeikyuCoReportService, IP42KokhoSeikyuCoReportService p42KokhoSeikyuCoReportService, IP43KokhoSeikyuCoReportService p43KokhoSeikyuCoReportService
                              , IP20WelfareSokatuCoReportService p20WelfareSokatuCoReportService, IP21WelfareSeikyuCoReportService p21WelfareSeikyuCoReportService, IP21WelfareSokatuCoReportService p21WelfareSokatuCoReportService, IP09KokhoSeikyuCoReportService p09KokhoSeikyuCoReportService, IP23NagoyaSeikyuCoReportService p23NagoyaSeikyuCoReportService, IP23WelfareSeikyuCoReportService p23WelfareSeikyuCoReportService, IP24WelfareSofuDiskCoReportService p24WelfareSofuDiskCoReportService, IP24WelfareSofuPaperCoReportService p24WelfareSofuPaperCoReportService, IP24WelfareSyomeiCoReportService p24WelfareSyomeiCoReportService, IP24WelfareSyomeiListCoReportService p24WelfareSyomeiListCoReportService, IP24WelfareSyomeiSofuCoReportService p24WelfareSyomeiSofuCoReportService, IP26VaccineSokatuCoReportService p26VaccineSokatuCoReportService, IP35WelfareSeikyuCoReportService p35WelfareSeikyuCoReportService, IP35WelfareSokatuCoReportService p35WelfareSokatuCoReportService, IP43KikuchiMeisai41CoReportService p43KikuchiMeisai41CoReportService, IP43KikuchiMeisai43CoReportService p43KikuchiMeisai43CoReportService, IP43KikuchiSeikyu41CoReportService p43KikuchiSeikyu41CoReportService, IP43KikuchiSeikyu43CoReportService p43KikuchiSeikyu43CoReportService, IP43KumamotoSeikyuCoReportService p43KumamotoSeikyuCoReportService, IP44WelfareSeikyu84CoReportService p44WelfareSeikyu84CoReportService, IReceiptCoReportService receiptCoReportService, IP27IzumisanoSeikyuCoReportService p27IzumisanoSeikyuCoReportService, IP43ReihokuSeikyu41CoReportService p43ReihokuSeikyu41CoReportService
                              , IP43AmakusaSeikyu41CoReportService p43AmakusaSeikyu41CoReportService, IP43AmakusaSeikyu42CoReportService p43AmakusaSeikyu42CoReportService, IReceTargetCoReportService receTargetCoReportService, ISyojyoSyokiCoReportService syojyoSyokiCoReportService, IP11KokhoSeikyuCoReportService p11KokhoSeikyuCoReportService, IP14WelfareSeikyuCoReportService p14WelfareSeikyuCoReportService, IP17KokhoSeikyuCoReportService p17KokhoSeikyuCoReportService, IP17WelfareSeikyuCoReportService p17WelfareSeikyuCoReportService, IP25WelfareSeikyuCoReportService p25WelfareSeikyuCoReportService, IP29WelfareSeikyuCoReportService p29WelfareSeikyuCoReportService, IP33KokhoSeikyuCoReportService p33KokhoSeikyuCoReportService, IP26KoukiSokatuInCoReportService p26KoukiSokatuInCoReportService, IP25KokhoSeikyuCoReportService p25KokhoSeikyuCoReportService, IP26WelfareSeikyuCoReportService p26WelfareSeikyuCoReportService, IP34KokhoSeikyuCoReportService p34KokhoSeikyuCoReportService, IP35KokhoSeikyuCoReportService p35KokhoSeikyuCoReportService, IP37KokhoSeikyuCoReportService p37KokhoSeikyuCoReportService, IP40KokhoSeikyuCityCoReportService p40KokhoSeikyuCityCoReportService, IP40KokhoSeikyuKumiaiCoReportService p40KokhoSeikyuKumiaiCoReportService, IP40WelfareSeikyuCoReportService p40WelfareSeikyuCoReportService, IP41KokhoSeikyuCoReportService p41KokhoSeikyuCoReportService, IP44KokhoSeikyuCoReportService p44KokhoSeikyuCoReportService, IP45KokhoSeikyuCoReportService p45KokhoSeikyuCoReportService, IP46KokhoSokatuCoReportService p46KokhoSokatuCoReportService 
                              ,  IP46KokhoSeikyuCoReportService p46KokhoSeikyuCoReportService, IP46KoukiSeikyuCoReportService p46KoukiSeikyuCoReportService, IP46WelfareSofu99CoReportService p46WelfareSofu99CoReportService, IP46WelfareSeikyu99CoReportService p46WelfareSeikyu99CoReportService)
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
        _p42KoukiSeikyuCoReportService = p42KoukiSeikyuCoReportService;
        _p45KoukiSeikyuCoReportService = p45KoukiSeikyuCoReportService;
        _p09KoukiSeikyuCoReportService = p09KoukiSeikyuCoReportService;
        _p12KoukiSeikyuCoReportService = p12KoukiSeikyuCoReportService;
        _p13KoukiSeikyuCoReportService = p13KoukiSeikyuCoReportService;
        _p30KoukiSeikyuCoReportService = p30KoukiSeikyuCoReportService;
        _p41KoukiSeikyuCoReportService = p41KoukiSeikyuCoReportService;
        _p08KokhoSokatuCoReportService = p08KokhoSokatuCoReportService;
        _p44KoukiSeikyuCoReportService = p44KoukiSeikyuCoReportService;
        _p08KoukiSeikyuCoReportService = p08KoukiSeikyuCoReportService;
        _p11KoukiSeikyuCoReportService = p11KoukiSeikyuCoReportService;
        _p14KoukiSeikyuCoReportService = p14KoukiSeikyuCoReportService;
        _p17KoukiSeikyuCoReportService = p17KoukiSeikyuCoReportService;
        _p20KoukiSeikyuCoReportService = p20KoukiSeikyuCoReportService;
        _p25KokhoSokatuCoReportService = p25KokhoSokatuCoReportService;
        _p13WelfareSeikyuCoReportService = p13WelfareSeikyuCoReportService;
        _p08KokhoSeikyuCoReportService = p08KokhoSeikyuCoReportService;
        _p22WelfareSeikyuCoReportService = p22WelfareSeikyuCoReportService;
        _p21KoukiSeikyuCoReportService = p21KoukiSeikyuCoReportService;
        _p22KoukiSeikyuCoReportService = p22KoukiSeikyuCoReportService;
        _p23KoukiSeikyuCoReportService = p23KoukiSeikyuCoReportService;
        _p24KoukiSeikyuCoReportService = p24KoukiSeikyuCoReportService;
        _p25KoukiSeikyuCoReportService = p25KoukiSeikyuCoReportService;
        _p27KoukiSeikyuCoReportService = p27KoukiSeikyuCoReportService;
        _p14KokhoSokatuCoReportService = p14KokhoSokatuCoReportService;
        _p17KokhoSokatuCoReportService = p17KokhoSokatuCoReportService;
        _p20KokhoSokatuCoReportService = p20KokhoSokatuCoReportService;
        _p22KokhoSokatuCoReportService = p22KokhoSokatuCoReportService;
        _p23KokhoSokatuCoReportService = p23KokhoSokatuCoReportService;
        _p26KokhoSokatuInCoReportService = p26KokhoSokatuInCoReportService;
        _p33KokhoSokatuCoReportService = p33KokhoSokatuCoReportService;
        _p34KokhoSokatuCoReportService = p34KokhoSokatuCoReportService;
        _p35KokhoSokatuCoReportService = p35KokhoSokatuCoReportService;
        _p37KokhoSokatuCoReportService = p37KokhoSokatuCoReportService;
        _p37KoukiSokatuCoReportService = p37KoukiSokatuCoReportService;
        _p26KokhoSokatuOutCoReportService = p26KokhoSokatuOutCoReportService;
        _p40KokhoSokatuCoReportService = p40KokhoSokatuCoReportService;
        _p41KokhoSokatuCoReportService = p41KokhoSokatuCoReportService;
        _p42KokhoSokatuCoReportService = p42KokhoSokatuCoReportService;
        _p12KokhoSokatuCoReportService = p12KokhoSokatuCoReportService;
        _p13KokhoSokatuCoReportService = p13KokhoSokatuCoReportService;
        _P43KokhoSokatuCoReportService = p43KokhoSokatuCoReportService;
        _p43KoukiSokatuCoReportService = p43KoukiSokatuCoReportService;
        _p44KokhoSokatuCoReportService = p44KokhoSokatuCoReportService;
        _p45KokhoSokatuCoReportService = p45KokhoSokatuCoReportService;
        _p45KoukiSokatuCoReportService = p45KoukiSokatuCoReportService;
        _p12KokhoSeikyuCoReportService = p12KokhoSeikyuCoReportService;
        _p13KokhoSeikyuCoReportService = p13KokhoSeikyuCoReportService;
        _p14KokhoSeikyuCoReportService = p14KokhoSeikyuCoReportService;
        _p20KokhoSeikyuCoReportService = p20KokhoSeikyuCoReportService;
        _p21KokhoSeikyuCoReportService = p21KokhoSeikyuCoReportService;
        _p22KokhoSeikyuCoReportService = p22KokhoSeikyuCoReportService;
        _p23KokhoSeikyuCoReportService = p23KokhoSeikyuCoReportService;
        _p24KokhoSeikyuCoReportService = p24KokhoSeikyuCoReportService;
        _p26KokhoSeikyuOutCoReportService = p26KokhoSeikyuOutCoReportService;
        _p27KokhoSeikyuInCoReportService = p27KokhoSeikyuInCoReportService;
        _p27KokhoSeikyuOutCoReportService = p27KokhoSeikyuOutCoReportService;
        _p28KokhoSeikyuCoReportService = p28KokhoSeikyuCoReportService;
        _p29KokhoSeikyuCoReportService = p29KokhoSeikyuCoReportService;
        _p30KokhoSeikyuCoReportService = p30KokhoSeikyuCoReportService;
        _p42KokhoSeikyuCoReportService = p42KokhoSeikyuCoReportService;
        _p43KokhoSeikyuCoReportService = p43KokhoSeikyuCoReportService;
        _p20WelfareSokatuCoReportService = p20WelfareSokatuCoReportService;
        _p21WelfareSeikyuCoReportService = p21WelfareSeikyuCoReportService;
        _p21WelfareSokatuCoReportService = p21WelfareSokatuCoReportService;
        _p09KokhoSeikyuCoReportService = p09KokhoSeikyuCoReportService;
        _p23NagoyaSeikyuCoReportService = p23NagoyaSeikyuCoReportService;
        _p23WelfareSeikyuCoReportService = p23WelfareSeikyuCoReportService;
        _p24WelfareSofuDiskCoReportService = p24WelfareSofuDiskCoReportService;
        _p24WelfareSofuPaperCoReportService = p24WelfareSofuPaperCoReportService;
        _p24WelfareSyomeiCoReportService = p24WelfareSyomeiCoReportService;
        _p24WelfareSyomeiListCoReportService = p24WelfareSyomeiListCoReportService;
        _p24WelfareSyomeiSofuCoReportService = p24WelfareSyomeiSofuCoReportService;
        _p26VaccineSokatuCoReportService = p26VaccineSokatuCoReportService;
        _p35WelfareSeikyuCoReportService = p35WelfareSeikyuCoReportService;
        _p35WelfareSokatuCoReportService = p35WelfareSokatuCoReportService;
        _p43KikuchiMeisai41CoReportService = p43KikuchiMeisai41CoReportService;
        _p43KikuchiMeisai43CoReportService = p43KikuchiMeisai43CoReportService;
        _p43KikuchiSeikyu41CoReportService = p43KikuchiSeikyu41CoReportService;
        _p43KikuchiSeikyu43CoReportService = p43KikuchiSeikyu43CoReportService;
        _p43KumamotoSeikyuCoReportService = p43KumamotoSeikyuCoReportService;
        _p44WelfareSeikyu84CoReportService = p44WelfareSeikyu84CoReportService;
        _receiptCoReportService = receiptCoReportService;
        _p27IzumisanoSeikyuCoReportService = p27IzumisanoSeikyuCoReportService;
        _p43ReihokuSeikyu41CoReportService = p43ReihokuSeikyu41CoReportService;
        _p43AmakusaSeikyu41CoReportService = p43AmakusaSeikyu41CoReportService;
        _p43AmakusaSeikyu42CoReportService = p43AmakusaSeikyu42CoReportService;
        _receTargetCoReportService = receTargetCoReportService;
        _syojyoSyokiCoReportService = syojyoSyokiCoReportService;
        _p11KokhoSeikyuCoReportService = p11KokhoSeikyuCoReportService;
        _p14WelfareSeikyuCoReportService = p14WelfareSeikyuCoReportService;
        _p17KokhoSeikyuCoReportService = p17KokhoSeikyuCoReportService;
        _p17WelfareSeikyuCoReportService = p17WelfareSeikyuCoReportService;
        _p25WelfareSeikyuCoReportService = p25WelfareSeikyuCoReportService;
        _p29WelfareSeikyuCoReportService = p29WelfareSeikyuCoReportService;
        _p33KokhoSeikyuCoReportService = p33KokhoSeikyuCoReportService;
        _p26KoukiSokatuInCoReportService = p26KoukiSokatuInCoReportService;
        _p25KokhoSeikyuCoReportService = p25KokhoSeikyuCoReportService;
        _p26WelfareSeikyuCoReportService = p26WelfareSeikyuCoReportService;
        _p34KokhoSeikyuCoReportService = p34KokhoSeikyuCoReportService;
        _p35KokhoSeikyuCoReportService = p35KokhoSeikyuCoReportService;
        _p37KokhoSeikyuCoReportService = p37KokhoSeikyuCoReportService;
        _p40KokhoSeikyuCityCoReportService = p40KokhoSeikyuCityCoReportService;
        _p40KokhoSeikyuKumiaiCoReportService = p40KokhoSeikyuKumiaiCoReportService;
        _p40WelfareSeikyuCoReportService = p40WelfareSeikyuCoReportService;
        _p41KokhoSeikyuCoReportService = p41KokhoSeikyuCoReportService;
        _p44KokhoSeikyuCoReportService = p44KokhoSeikyuCoReportService;
        _p45KokhoSeikyuCoReportService = p45KokhoSeikyuCoReportService;
        _p46KokhoSokatuCoReportService = p46KokhoSokatuCoReportService;
        _p46KokhoSeikyuCoReportService = p46KokhoSeikyuCoReportService;
        _p46KoukiSeikyuCoReportService = p46KoukiSeikyuCoReportService;
        _p46WelfareSofu99CoReportService = p46WelfareSofu99CoReportService;
        _p46WelfareSeikyu99CoReportService = p46WelfareSeikyu99CoReportService;
    }
    #endregion

    public CommonReportingRequestModel GetReceiptPrint(int hpId, string formName, int prefNo, int reportId, int reportEdaNo, int dataKbn, long ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt, int welfareType, List<string> printHokensyaNos, int hokenKbn, ReseputoShubetsuModel selectedReseputoShubeusu, int departmentId, int doctorId, int printNoFrom, int printNoTo, bool includeTester, bool includeOutDrug, int sort, List<long> printPtIds)
    {
        CommonReportingRequestModel result = new();
        var seikyuType = GetSeikyuType(dataKbn);
        var prefKbn = GetPrefKbn(reportEdaNo);

        #region 100 service
        if (prefNo == 28 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p28KokhoSokatuCoReportService.GetP28KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (reportId == 10 && reportEdaNo == 0)
        {
            result = _receTargetCoReportService.GetReceTargetPrintData(hpId, seikyuYm);
        }
        else if (reportId == 3 && reportEdaNo == 0)
        {
            result = _syojyoSyokiCoReportService.GetSyojyoSyokiReportingData(hpId, ptId, seikyuYm, hokenId);
        }
        else if (prefNo == 29 && reportId == 105 && reportEdaNo == 1)
        {
            result = _p29WelfareSeikyuCoReportService.GetP29WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 25 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p25WelfareSeikyuCoReportService.GetP25WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 33 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p33KokhoSeikyuCoReportService.GetP33KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 40 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p40WelfareSeikyuCoReportService.GetP40WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 46 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p46KokhoSokatuCoReportService.GetP46KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 37 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p37KokhoSeikyuCoReportService.GetP37KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 46 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p46KokhoSeikyuCoReportService.GetP46KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 44 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p44KokhoSeikyuCoReportService.GetP44KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 40 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p40KokhoSeikyuCityCoReportService.GetP40KokhoSeikyuCityReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 40 && reportId == 103 && reportEdaNo == 1)
        {
            result = _p40KokhoSeikyuKumiaiCoReportService.GetP40KokhoSeikyuKumiaiReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 45 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p45KokhoSeikyuCoReportService.GetP45KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 25 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p25KokhoSeikyuCoReportService.GetP25KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 41 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p41KokhoSeikyuCoReportService.GetP40KokhoSeikyuKumiaiReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 26 && reportId == 102 && reportEdaNo == 1)
        {
            result = _p26KoukiSokatuInCoReportService.GetP26KoukiSokatuInReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 26 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p26WelfareSeikyuCoReportService.GetP26WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 46 && reportId == 105 && reportEdaNo == 2)
        {
            result = _p46WelfareSeikyu99CoReportService.GetP46WelfareSeikyu99ReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 34 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p34KokhoSeikyuCoReportService.GetP34KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 17 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p17WelfareSeikyuCoReportService.GetP17WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 35 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p35KokhoSeikyuCoReportService.GetP35KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 11 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p11KokhoSeikyuCoReportService.GetP11KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 14 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p14WelfareSeikyuCoReportService.GetP14WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType, 1);
        }
        else if (prefNo == 14 && reportId == 105 && reportEdaNo == 1)
        {
            result = _p14WelfareSeikyuCoReportService.GetP14WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType, 2);
        }
        else if (prefNo == 14 && reportId == 105 && reportEdaNo == 2)
        {
            result = _p14WelfareSeikyuCoReportService.GetP14WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType, 0);
        }
        else if (prefNo == 14 && reportId == 105 && reportEdaNo == 3)
        {
            result = _p14WelfareSeikyuCoReportService.GetP14WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType, 3);
        }
        else if (prefNo == 14 && reportId == 105 && reportEdaNo == 4)
        {
            result = _p14WelfareSeikyuCoReportService.GetP14WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType, 4);
        }
        else if (prefNo == 46 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p46WelfareSofu99CoReportService.GetP46WelfareSofu99ReportingData(hpId, seikyuYm, seikyuType, 0);
        }
        else if (prefNo == 46 && reportId == 105 && reportEdaNo == 1)
        {
            result = _p46WelfareSofu99CoReportService.GetP46WelfareSofu99ReportingData(hpId, seikyuYm, seikyuType, 1);
        }
        else if (prefNo == 17 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p17KokhoSeikyuCoReportService.GetP17KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 46 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p46KoukiSeikyuCoReportService.GetP46KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 11 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p11KokhoSokatuCoReportService.GetP11KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 28 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p28KoukiSeikyuCoReportService.GetP28KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 20 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p20KoukiSeikyuCoReportService.GetP20KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 29 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p29KoukiSeikyuCoReportService.GetP29KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 44 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p44KoukiSeikyuCoReportService.GetP44KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 08 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p08KoukiSeikyuCoReportService.GetP08KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 11 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p11KoukiSeikyuCoReportService.GetP11KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (reportId == 2 && reportEdaNo == 0)
        {
            result = _hikariDiskCoReportService.GetHikariDiskPrintData(hpId, seikyuYm, hokenKbn, diskKind, diskCnt);
        }
        else if (reportId == 4 && reportEdaNo == 0)
        {
            result = _afterCareSeikyuCoReportService.GetAfterCareSeikyuPrintData(hpId, seikyuYm, GetSeikyuType(dataKbn));
        }
        else if (reportId == 101 && reportEdaNo == 0)
        {
            result = _syahoCoReportService.GetSyahoPrintData(hpId, seikyuYm, GetSeikyuType(dataKbn));
        }
        else if (prefNo == 41 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p41KoukiSeikyuCoReportService.GetP41KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 33 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p33KoukiSeikyuCoReportService.GetP33KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 34 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p34KoukiSeikyuCoReportService.GetP34KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 35 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p35KoukiSeikyuCoReportService.GetP35KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 37 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p37KoukiSeikyuCoReportService.GetP37KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 40 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p40KoukiSeikyuCoReportService.GetP40KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 42 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p42KoukiSeikyuCoReportService.GetP42KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 45 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p45KoukiSeikyuCoReportService.GetP45KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 09 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p09KoukiSeikyuCoReportService.GetP09KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 12 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p12KoukiSeikyuCoReportService.GetP12KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 13 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p13KoukiSeikyuCoReportService.GetP13KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 30 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p30KoukiSeikyuCoReportService.GetP30KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 14 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p14KoukiSeikyuCoReportService.GetP14KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 17 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p17KoukiSeikyuCoReportService.GetP17KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 08 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p08KokhoSokatuCoReportService.GetP08KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 25 && reportId == 102 && reportEdaNo == 2)
        {
            result = _p25KokhoSokatuCoReportService.GetP25KokhoSokatuReportingData(hpId, seikyuYm, seikyuType, diskKind, diskCnt);
        }
        else if (prefNo == 13 && reportId == 105 && reportEdaNo == 0 && welfareType == 0)
        {
            result = _p13WelfareSeikyuCoReportService.GetP13WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType, welfareType);
        }
        else if (prefNo == 13 && reportId == 105 && reportEdaNo == 0 && welfareType == 1)
        {
            result = _p13WelfareSeikyuCoReportService.GetP13WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType, welfareType);
        }
        else if (prefNo == 08 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p08KokhoSeikyuCoReportService.GetP08KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 22 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p22WelfareSeikyuCoReportService.GetP22WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType, 0);
        }
        else if (prefNo == 22 && reportId == 105 && reportEdaNo == 1)
        {
            result = _p22WelfareSeikyuCoReportService.GetP22WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType, 1);
        }
        else if (prefNo == 43 && reportId == 105 && reportEdaNo == 4)
        {
            result = _p43KikuchiMeisai41CoReportService.GetP43KikuchiMeisai41ReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 21 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p21KoukiSeikyuCoReportService.GetP21KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 22 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p22KoukiSeikyuCoReportService.GetP22KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 23 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p23KoukiSeikyuCoReportService.GetP23KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 24 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p24KoukiSeikyuCoReportService.GetP24KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 25 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p25KoukiSeikyuCoReportService.GetP25KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 27 && reportId == 104 && reportEdaNo == 0)
        {
            result = _p27KoukiSeikyuCoReportService.GetP27KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType, prefKbn, printHokensyaNos);
        }
        else if (prefNo == 27 && reportId == 104 && reportEdaNo == 1)
        {
            result = _p27KoukiSeikyuCoReportService.GetP27KoukiSeikyuReportingData(hpId, seikyuYm, seikyuType, prefKbn, printHokensyaNos);
        }
        else if (prefNo == 14 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p14KokhoSokatuCoReportService.GetP14KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 17 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p17KokhoSokatuCoReportService.GetP17KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 20 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p20KokhoSokatuCoReportService.GetP20KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 22 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p22KokhoSokatuCoReportService.GetP22KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 23 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p23KokhoSokatuCoReportService.GetP23KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 26 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p26KokhoSokatuInCoReportService.GetP26KokhoSokatuInReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 33 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p33KokhoSokatuCoReportService.GetP33KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 34 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p34KokhoSokatuCoReportService.GetP34KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 35 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p35KokhoSokatuCoReportService.GetP35KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 37 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p37KokhoSokatuCoReportService.GetP37KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 37 && reportId == 102 && reportEdaNo == 1)
        {
            result = _p37KoukiSokatuCoReportService.GetP37KoukiSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 26 && reportId == 102 && reportEdaNo == 2)
        {
            result = _p26KokhoSokatuOutCoReportService.GetP26KokhoSokatuOutReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 40 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p40KokhoSokatuCoReportService.GetP40KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 41 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p41KokhoSokatuCoReportService.GetP41KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 42 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p42KokhoSokatuCoReportService.GetP42KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 12 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p12KokhoSokatuCoReportService.GetP12KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 13 && reportId == 102 && reportEdaNo == 1)
        {
            result = _p13KokhoSokatuCoReportService.GetP13KokhoSokatuReportingData(hpId, seikyuYm, seikyuType, diskKind, diskCnt);
        }
        else if (prefNo == 43 && reportId == 102 && reportEdaNo == 0)
        {
            result = _P43KokhoSokatuCoReportService.GetP43KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 43 && reportId == 102 && reportEdaNo == 1)
        {
            result = _p43KoukiSokatuCoReportService.GetP43KoukiSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 44 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p44KokhoSokatuCoReportService.GetP44KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 45 && reportId == 102 && reportEdaNo == 0)
        {
            result = _p45KokhoSokatuCoReportService.GetP45KokhoSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 45 && reportId == 102 && reportEdaNo == 1)
        {
            result = _p45KoukiSokatuCoReportService.GetP45KoukiSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 12 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p12KokhoSeikyuCoReportService.GetP12KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 13 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p13KokhoSeikyuCoReportService.GetP13KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 14 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p14KokhoSeikyuCoReportService.GetP14KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 20 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p20KokhoSeikyuCoReportService.Get20KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 21 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p21KokhoSeikyuCoReportService.GetP21KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 22 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p22KokhoSeikyuCoReportService.GetP22KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }

        else if (prefNo == 23 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p23KokhoSeikyuCoReportService.GetP23KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 24 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p24KokhoSeikyuCoReportService.GetP24KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 26 && reportId == 103 && reportEdaNo == 2)
        {
            result = _p26KokhoSeikyuOutCoReportService.GetP26KokhoSeikyuOutReportingData(hpId, seikyuYm, seikyuType);
        }

        else if (prefNo == 27 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p27KokhoSeikyuInCoReportService.GetP27KokhoSeikyuInReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 27 && reportId == 103 && reportEdaNo == 1)
        {
            result = _p27KokhoSeikyuOutCoReportService.GetP27KokhoSeikyuOutReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 28 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p28KokhoSeikyuCoReportService.GetP28KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }

        else if (prefNo == 29 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p29KokhoSeikyuCoReportService.GetP29KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 30 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p30KokhoSeikyuCoReportService.GetP30KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 42 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p42KokhoSeikyuCoReportService.GetP42KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 43 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p43KokhoSeikyuCoReportService.GetP43KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 20 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p20WelfareSokatuCoReportService.GetP20WelfareSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 21 && reportId == 105 && reportEdaNo == 1)
        {
            result = _p21WelfareSeikyuCoReportService.GetP21WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 21 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p21WelfareSokatuCoReportService.GetP21WelfareSokatuCoReportService(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 09 && reportId == 103 && reportEdaNo == 0)
        {
            result = _p09KokhoSeikyuCoReportService.GetP09KokhoSeikyuReportingData(hpId, seikyuYm, seikyuType, printHokensyaNos);
        }
        else if (prefNo == 23 && reportId == 105 && reportEdaNo == 1)
        {
            result = _p23NagoyaSeikyuCoReportService.GetP23NagoyaSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 23 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p23WelfareSeikyuCoReportService.GetP23WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 24 && reportId == 105 && reportEdaNo == 3 && diskCnt == 1)
        {
            result = _p24WelfareSofuDiskCoReportService.GetP24WelfareSofuDiskReportingData(hpId, seikyuYm, seikyuType, diskCnt);
        }
        else if (prefNo == 24 && reportId == 105 && reportEdaNo == 4)
        {
            result = _p24WelfareSofuPaperCoReportService.GetP24WelfareSofuPaperReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 24 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p24WelfareSyomeiCoReportService.GetP24WelfareSyomeiReportingData(hpId, seikyuYm, seikyuType, printPtIds);
        }
        else if (prefNo == 24 && reportId == 105 && reportEdaNo == 2)
        {
            result = _p24WelfareSyomeiListCoReportService.GetP24WelfareSyomeiListReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 24 && reportId == 105 && reportEdaNo == 1)
        {
            result = _p24WelfareSyomeiSofuCoReportService.GetP24WelfareSyomeiSofuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 26 && reportId == 105 && reportEdaNo == 1)
        {
            result = _p26VaccineSokatuCoReportService.GetP26VaccineSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 27 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p27IzumisanoSeikyuCoReportService.GetP27IzumisanoSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 35 && reportId == 105 && reportEdaNo == 1)
        {
            result = _p35WelfareSeikyuCoReportService.GetP35WelfareSeikyuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 35 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p35WelfareSokatuCoReportService.GetP35WelfareSokatuReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 43 && reportId == 105 && reportEdaNo == 6)
        {
            result = _p43KikuchiMeisai43CoReportService.GetP43KikuchiMeisai43ReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 43 && reportId == 105 && reportEdaNo == 3)
        {
            result = _p43KikuchiSeikyu41CoReportService.GetP43KikuchiSeikyu41ReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 43 && reportId == 105 && reportEdaNo == 5)
        {
            result = _p43KikuchiSeikyu43CoReportService.GetP43KikuchiSeikyu43ReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 43 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p43KumamotoSeikyuCoReportService.GetP43KumamotoSeikyuReportingData(hpId, seikyuYm, seikyuType, welfareType);
        }
        else if (prefNo == 43 && reportId == 105 && reportEdaNo == 1)
        {
            result = _p43KumamotoSeikyuCoReportService.GetP43KumamotoSeikyuReportingData(hpId, seikyuYm, seikyuType, welfareType);
        }
        else if (prefNo == 43 && reportId == 105 && reportEdaNo == 2)
        {
            result = _p43KumamotoSeikyuCoReportService.GetP43KumamotoSeikyuReportingData(hpId, seikyuYm, seikyuType, welfareType);
        }
        else if (prefNo == 44 && reportId == 105 && reportEdaNo == 0)
        {
            result = _p44WelfareSeikyu84CoReportService.GetP44WelfareSeikyu84ReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 43 && reportId == 105 && reportEdaNo == 9)
        {
            result = _p43ReihokuSeikyu41CoReportService.GetP43ReihokuSeikyu41ReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 43 && reportId == 105 && reportEdaNo == 7)
        {
            result = _p43AmakusaSeikyu41CoReportService.GetP43AmakusaSeikyu41sReportingData(hpId, seikyuYm, seikyuType);
        }
        else if (prefNo == 43 && reportId == 105 && reportEdaNo == 8)
        {
            result = _p43AmakusaSeikyu42CoReportService.GetP43AmakusaSeikyu42ReportingData(hpId, seikyuYm, seikyuType);
        }
        #endregion
        else
        {
            // if (isPreview) return false;
            // Calculate target
            int target = hokenKbn;
            if (reportId >= 10001)
            {
                target = reportId;
            }
            else
            {
                if (hokenKbn == 3 && selectedReseputoShubeusu != null)
                {
                    var listReceSbtShaho = new List<ReseputoShubetsuModel>()
                    {
                        new ReseputoShubetsuModel ("11","--■社保一般（すべて）"),
                        new ReseputoShubetsuModel ("12","--■公費（すべて）	"),
                        new ReseputoShubetsuModel ("1112","社保単独・本人"),
                        new ReseputoShubetsuModel ("1114","社保単独・未就学"),
                        new ReseputoShubetsuModel ("1116","社保単独・家族"),
                        new ReseputoShubetsuModel ("1118","社保単独・高一/低"),
                        new ReseputoShubetsuModel ("1110","社保単独・高７"),
                        new ReseputoShubetsuModel ("1122","社保２併・本人"),
                        new ReseputoShubetsuModel ("1124","社保２併・未就学"),
                        new ReseputoShubetsuModel ("1126","社保２併・家族"),
                        new ReseputoShubetsuModel ("1128","社保２併・高一/低"),
                        new ReseputoShubetsuModel ("1120","社保２併・高７"),
                        new ReseputoShubetsuModel ("1132","社保３併・本人"),
                        new ReseputoShubetsuModel ("1134","社保３併・未就学"),
                        new ReseputoShubetsuModel ("1136","社保３併・家族"),
                        new ReseputoShubetsuModel ("1138","社保３併・高一/低"),
                        new ReseputoShubetsuModel ("1130","社保３併・高７"),
                        new ReseputoShubetsuModel ("1142","社保４併・本人"),
                        new ReseputoShubetsuModel ("1144","社保４併・未就学"),
                        new ReseputoShubetsuModel ("1146","社保４併・家族"),
                        new ReseputoShubetsuModel ("1148","社保４併・高一/低"),
                        new ReseputoShubetsuModel ("1140","社保４併・高７"),
                        new ReseputoShubetsuModel ("1152","社保５併・本人"),
                        new ReseputoShubetsuModel ("1154","社保５併・未就学"),
                        new ReseputoShubetsuModel ("1156","社保５併・家族"),
                        new ReseputoShubetsuModel ("1158","社保５併・高一/低"),
                        new ReseputoShubetsuModel ("1150","社保５併・高７"),
                        new ReseputoShubetsuModel ("1212","公費単独"),
                        new ReseputoShubetsuModel ("1222","公費２併"),
                        new ReseputoShubetsuModel ("1232","公費３併"),
                        new ReseputoShubetsuModel ("1242","公費４併")
                    };

                    var listReceSbtKokuho = new List<ReseputoShubetsuModel>()
                    {
                        new ReseputoShubetsuModel ("11","--■国保一般（すべて）"),
                        new ReseputoShubetsuModel ("14","--■退職（すべて）"),
                        new ReseputoShubetsuModel ("13","--■後期（すべて）"),
                        new ReseputoShubetsuModel ("1112","国保単独・本人"),
                        new ReseputoShubetsuModel ("1114","国保単独・未就学"),
                        new ReseputoShubetsuModel ("1116","国保単独・家族"),
                        new ReseputoShubetsuModel ("1118","国保単独・高一/低"),
                        new ReseputoShubetsuModel ("1110","国保単独・高７"),
                        new ReseputoShubetsuModel ("1122","国保２併・本人"),
                        new ReseputoShubetsuModel ("1124","国保２併・未就学"),
                        new ReseputoShubetsuModel ("1126","国保２併・家族"),
                        new ReseputoShubetsuModel ("1128","国保２併・高一/低"),
                        new ReseputoShubetsuModel ("1120","国保２併・高７"),
                        new ReseputoShubetsuModel ("1132","国保３併・本人"),
                        new ReseputoShubetsuModel ("1134","国保３併・未就学"),
                        new ReseputoShubetsuModel ("1136","国保３併・家族"),
                        new ReseputoShubetsuModel ("1138","国保３併・高一/低"),
                        new ReseputoShubetsuModel ("1130","国保３併・高７"),
                        new ReseputoShubetsuModel ("1142","国保４併・本人"),
                        new ReseputoShubetsuModel ("1144","国保４併・未就学"),
                        new ReseputoShubetsuModel ("1146","国保４併・家族"),
                        new ReseputoShubetsuModel ("1148","国保４併・高一/低"),
                        new ReseputoShubetsuModel ("1140","国保４併・高７"),
                        new ReseputoShubetsuModel ("1152","国保５併・本人"),
                        new ReseputoShubetsuModel ("1154","国保５併・未就学"),
                        new ReseputoShubetsuModel ("1156","国保５併・家族"),
                        new ReseputoShubetsuModel ("1158","国保５併・高一/低"),
                        new ReseputoShubetsuModel ("1150","国保５併・高７"),
                        new ReseputoShubetsuModel ("1318","後期単独・一低"),
                        new ReseputoShubetsuModel ("1310","後期単独・高７"),
                        new ReseputoShubetsuModel ("1328","後期２併・一低"),
                        new ReseputoShubetsuModel ("1320","後期２併・高７"),
                        new ReseputoShubetsuModel ("1338","後期３併・一低"),
                        new ReseputoShubetsuModel ("1330","後期３併・高７"),
                        new ReseputoShubetsuModel ("1348","後期４併・一低"),
                        new ReseputoShubetsuModel ("1340","後期４併・高７"),
                        new ReseputoShubetsuModel ("1358","後期５併・一低"),
                        new ReseputoShubetsuModel ("1350","後期５併・高７"),
                        new ReseputoShubetsuModel ("1412","退職単独・本人"),
                        new ReseputoShubetsuModel ("1414","退職単独・未就学"),
                        new ReseputoShubetsuModel ("1416","退職単独・家族"),
                        new ReseputoShubetsuModel ("1418","退職単独・高一/低"),
                        new ReseputoShubetsuModel ("1410","退職単独・高７"),
                        new ReseputoShubetsuModel ("1422","退職２併・本人"),
                        new ReseputoShubetsuModel ("1424","退職２併・未就学"),
                        new ReseputoShubetsuModel ("1426","退職２併・家族"),
                        new ReseputoShubetsuModel ("1428","退職２併・高一/低"),
                        new ReseputoShubetsuModel ("1420","退職２併・高７"),
                        new ReseputoShubetsuModel ("1432","退職３併・本人"),
                        new ReseputoShubetsuModel ("1434","退職３併・未就学"),
                        new ReseputoShubetsuModel ("1436","退職３併・家族"),
                        new ReseputoShubetsuModel ("1438","退職３併・高一/低"),
                        new ReseputoShubetsuModel ("1430","退職３併・高７"),
                        new ReseputoShubetsuModel ("1432","退職４併・本人"),
                        new ReseputoShubetsuModel ("1434","退職４併・未就学"),
                        new ReseputoShubetsuModel ("1436","退職４併・家族"),
                        new ReseputoShubetsuModel ("1438","退職４併・高一/低"),
                        new ReseputoShubetsuModel ("1440","退職４併・高７"),
                        new ReseputoShubetsuModel ("1452","退職５併・本人"),
                        new ReseputoShubetsuModel ("1454","退職５併・未就学"),
                        new ReseputoShubetsuModel ("1456","退職５併・家族"),
                        new ReseputoShubetsuModel ("1458","退職５併・高一/低"),
                        new ReseputoShubetsuModel ("1450","退職５併・高７")
                    };

                    var selectedReceSbtShaho = listReceSbtShaho.FirstOrDefault(x => x.Key == selectedReseputoShubeusu.Key && (x.Value.Contains(selectedReseputoShubeusu.Value) || selectedReseputoShubeusu.Value.Contains(x.Value)));
                    if (selectedReceSbtShaho != null)
                    {
                        target = 1;
                    }
                    var selectedReceSbtKokuho = listReceSbtKokuho.FirstOrDefault(x => x.Key == selectedReseputoShubeusu.Key && (x.Value.Contains(selectedReseputoShubeusu.Value) || selectedReseputoShubeusu.Value.Contains(x.Value)));
                    if (selectedReceSbtKokuho != null)
                    {
                        target = 2;
                    }
                }
            }

            result = _receiptCoReportService.GetReceiptDataByReceiptCheckList(hpId
                                                                            , seikyuYm
                                                                            , printPtIds
                                                                            , sinYm: 0
                                                                            , hokenId: 0
                                                                            , departmentId
                                                                            , doctorId
                                                                            , target
                                                                            , receSbt: selectedReseputoShubeusu?.Key ?? string.Empty
                                                                            , printNoFrom: printNoFrom
                                                                            , printNoTo: printNoTo
                                                                            , seikyuType: seikyuType
                                                                            , includeTester
                                                                            , includeOutDrug
                                                                            , sort);

        }
        result.JobName = formName;

        return result;
    }

    private PrefKbn GetPrefKbn(int reportEdaNo)
    {
        switch (reportEdaNo)
        {
            case 0:
                return PrefKbn.PrefIn;
            case 1:
                return PrefKbn.PrefOut;
            default:
                return PrefKbn.PrefAll;
        }
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


public class ReseputoShubetsuModel
{
    public ReseputoShubetsuModel(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public ReseputoShubetsuModel()
    {
        Key = string.Empty;
        Value = string.Empty;
    }
    public string Key { get; private set; }

    public string Value { get; private set; }
}