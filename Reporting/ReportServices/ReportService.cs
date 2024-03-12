using Domain.Models.Accounting;
using Domain.Models.KensaSet;
using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Domain.Models.SystemConf;
using Helper.Common;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reporting.Accounting.DB;
using Reporting.Accounting.Model;
using Reporting.Accounting.Model.Output;
using Reporting.Accounting.Service;
using Reporting.AccountingCard.DB;
using Reporting.AccountingCard.Service;
using Reporting.AccountingCardList.DB;
using Reporting.AccountingCardList.Model;
using Reporting.AccountingCardList.Service;
using Reporting.Byomei.DB;
using Reporting.Byomei.Service;
using CalculateService.Implementation;
using CalculateService.Interface;
using Reporting.CommonMasters.Config;
using Reporting.CommonMasters.Enums;
using Reporting.DailyStatic.DB;
using Reporting.DailyStatic.Service;
using Reporting.DrugInfo.DB;
using Reporting.DrugInfo.Model;
using Reporting.DrugInfo.Service;
using Reporting.DrugNoteSeal.DB;
using Reporting.DrugNoteSeal.Service;
using Reporting.GrowthCurve.DB;
using Reporting.GrowthCurve.Model;
using Reporting.GrowthCurve.Service;
using Reporting.InDrug.DB;
using Reporting.InDrug.Service;
using Reporting.Karte1.DB;
using Reporting.Karte1.Service;
using Reporting.Karte3.DB;
using Reporting.Karte3.Service;
using Reporting.KensaHistory.DB;
using Reporting.KensaHistory.Service;
using Reporting.KensaLabel.DB;
using Reporting.KensaLabel.Model;
using Reporting.KensaLabel.Service;
using Reporting.Kensalrai.DB;
using Reporting.Kensalrai.Service;
using Reporting.Mappers.Common;
using Reporting.MedicalRecordWebId.Service;
using Reporting.Memo.Service;
using Reporting.NameLabel.DB;
using Reporting.NameLabel.Service;
using Reporting.OrderLabel.Model;
using Reporting.OrderLabel.Service;
using Reporting.OutDrug.DB;
using Reporting.OutDrug.Service;
using Reporting.PatientManagement.DB;
using Reporting.PatientManagement.Models;
using Reporting.PatientManagement.Service;
using Reporting.ReadRseReportFile.Service;
using Reporting.Receipt.DB;
using Reporting.Receipt.Service;
using Reporting.ReceiptCheck.Service;
using Reporting.ReceiptList.Model;
using Reporting.ReceiptList.Service;
using Reporting.ReceiptPrint.Service;
using Reporting.ReceTarget.DB;
using Reporting.ReceTarget.Service;
using Reporting.Sijisen.DB;
using Reporting.Sijisen.Service;
using Reporting.Sokatu.AfterCareSeikyu.DB;
using Reporting.Sokatu.AfterCareSeikyu.Service;
using Reporting.Sokatu.Common.DB;
using Reporting.Sokatu.HikariDisk.DB;
using Reporting.Sokatu.HikariDisk.Service;
using Reporting.Sokatu.KokhoSeikyu.DB;
using Reporting.Sokatu.KokhoSeikyu.Service;
using Reporting.Sokatu.KokhoSokatu.DB;
using Reporting.Sokatu.KokhoSokatu.Service;
using Reporting.Sokatu.KoukiSeikyu.DB;
using Reporting.Sokatu.KoukiSeikyu.Service;
using Reporting.Sokatu.Syaho.DB;
using Reporting.Sokatu.Syaho.Service;
using Reporting.Sokatu.WelfareDisk.Service;
using Reporting.Sokatu.WelfareSeikyu.DB;
using Reporting.Sokatu.WelfareSeikyu.Service;
using Reporting.Statistics.Sta1001.DB;
using Reporting.Statistics.Sta1001.Service;
using Reporting.Statistics.Sta1002.DB;
using Reporting.Statistics.Sta1002.Service;
using Reporting.Statistics.Sta1010.DB;
using Reporting.Statistics.Sta1010.Service;
using Reporting.Statistics.Sta2001.DB;
using Reporting.Statistics.Sta2001.Service;
using Reporting.Statistics.Sta2002.DB;
using Reporting.Statistics.Sta2002.Service;
using Reporting.Statistics.Sta2003.DB;
using Reporting.Statistics.Sta2003.Service;
using Reporting.Statistics.Sta2010.DB;
using Reporting.Statistics.Sta2010.Service;
using Reporting.Statistics.Sta2011.DB;
using Reporting.Statistics.Sta2011.Service;
using Reporting.Statistics.Sta2020.DB;
using Reporting.Statistics.Sta2020.Service;
using Reporting.Statistics.Sta2021.DB;
using Reporting.Statistics.Sta2021.Service;
using Reporting.Statistics.Sta3001.DB;
using Reporting.Statistics.Sta3001.Service;
using Reporting.Statistics.Sta3010.DB;
using Reporting.Statistics.Sta3010.Service;
using Reporting.Statistics.Sta3020.DB;
using Reporting.Statistics.Sta3020.Service;
using Reporting.Statistics.Sta3030.DB;
using Reporting.Statistics.Sta3030.Service;
using Reporting.Statistics.Sta3040.DB;
using Reporting.Statistics.Sta3040.Service;
using Reporting.Statistics.Sta3041.DB;
using Reporting.Statistics.Sta3041.Service;
using Reporting.Statistics.Sta3050.DB;
using Reporting.Statistics.Sta3050.Service;
using Reporting.Statistics.Sta3060.DB;
using Reporting.Statistics.Sta3060.Service;
using Reporting.Statistics.Sta3061.DB;
using Reporting.Statistics.Sta3061.Service;
using Reporting.Statistics.Sta3070.DB;
using Reporting.Statistics.Sta3070.Service;
using Reporting.Statistics.Sta3071.DB;
using Reporting.Statistics.Sta3071.Service;
using Reporting.Statistics.Sta3080.DB;
using Reporting.Statistics.Sta3080.Service;
using Reporting.Statistics.Sta9000.DB;
using Reporting.Statistics.Sta9000.Models;
using Reporting.Statistics.Sta9000.Service;
using Reporting.SyojyoSyoki.DB;
using Reporting.SyojyoSyoki.Service;
using Reporting.Yakutai.DB;
using Reporting.Yakutai.Service;
using Reporting.Statistics.Sta3062.Service;
using Reporting.Statistics.Sta3062.DB;

namespace Reporting.ReportServices;

public class ReportService : IReportService
{
    private IOrderLabelCoReportService _orderLabelCoReportService;
    private IDrugInfoCoReportService _drugInfoCoReportService;
    private ISijisenReportService _sijisenReportService;
    private IByomeiService _byomeiService;
    private IKarte1Service _karte1Service;
    private INameLabelService? _nameLabelService;
    private IMedicalRecordWebIdReportService _medicalRecordWebIdReportService;
    private IReceiptCheckCoReportService _receiptCheckCoReportService;
    private IReceiptListCoReportService _receiptListCoReportService;
    private IOutDrugCoReportService _outDrugCoReportService;
    private IAccountingCoReportService _accountingCoReportService;
    private IStatisticService _statisticService;
    private IReceiptCoReportService _receiptCoReportService;
    private IPatientManagementService _patientManagementService;
    private ISyojyoSyokiCoReportService _syojyoSyokiCoReportService;
    private IKensaIraiCoReportService _kensaIraiCoReportService;
    private IReceiptPrintService _receiptPrintService;
    private IMemoMsgCoReportService _memoMsgCoReportService;
    private IReceTargetCoReportService _receTargetCoReportService;
    private IDrugNoteSealCoReportService _drugNoteSealCoReportService;
    private IYakutaiCoReportService _yakutaiCoReportService;
    private IAccountingCardCoReportService _accountingCardCoReportService;
    private ICoAccountingFinder _coAccountingFinder;
    private IKarte3CoReportService _karte3CoReportService;
    private IAccountingCardListCoReportService _accountingCardListCoReportService;
    private IInDrugCoReportService _inDrugCoReportService;
    private IGrowthCurveA4CoReportService _growthCurveA4CoReportService;
    private IGrowthCurveA5CoReportService _growthCurveA5CoReportService;
    private IKensaLabelCoReportService _kensaLabelCoReportService;
    private IReceiptPrintExcelService _receiptPrintExcelService;
    private IImportCSVCoReportService _importCSVCoReportService;
    private IStaticsticExportCsvService _staticsticExportCsvService;
    private ISta9000CoReportService _sta9000CoReportService;
    private IKensaHistoryCoReportService _kensaHistoryCoReportService;
    private IKensaResultMultiCoReportService _kensaResultMultiCoReportService;
    private IConfiguration _configuration;
    private ServiceProvider _serviceProvider;
    private ILogger<EmrLogger> _logger;
    private IOptions<AmazonS3Options> _option;
    private ITenantProvider _tenantProvider;

    public ReportService(IConfiguration configuration, ILogger<EmrLogger> logger, IOptions<AmazonS3Options> option, ITenantProvider tenantProvider)
    {
        _configuration = configuration;
        _logger = logger;
        _option = option;
        _tenantProvider = tenantProvider;
    }
    public void Instance(int service)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<ITenantProvider, TenantProvider>();
        serviceCollection.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        serviceCollection.AddScoped<IConfiguration>(_ => _configuration);
        if (service == 1)
        {
            serviceCollection.AddTransient<IOrderLabelCoReportService, OrderLabelCoReportService>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _orderLabelCoReportService = ActivatorUtilities.CreateInstance<OrderLabelCoReportService>(_serviceProvider);
        }
        if (service == 2)
        {
            serviceCollection.AddTransient<IDrugInfoCoReportService, DrugInfoCoReportService>();
            serviceCollection.AddTransient<ISystemConfRepository, SystemConfRepository>();
            serviceCollection.AddTransient<IAmazonS3Service, AmazonS3Service>();
            serviceCollection.AddTransient<ICoDrugInfFinder, CoDrugInfFinder>();
            serviceCollection.AddTransient<IOptions<AmazonS3Options>>(_ => _option);

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _drugInfoCoReportService = ActivatorUtilities.CreateInstance<DrugInfoCoReportService>(_serviceProvider);
        }
        else if (service == 3)
        {
            serviceCollection.AddTransient<ISijisenReportService, SijisenReportService>();
            serviceCollection.AddTransient<ICoSijisenFinder, CoSijisenFinder>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _sijisenReportService = ActivatorUtilities.CreateInstance<SijisenReportService>(_serviceProvider);
        }
        else if (service == 4)
        {
            serviceCollection.AddTransient<IByomeiService, ByomeiService>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<ICoPtByomeiFinder, CoPtByomeiFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _byomeiService = ActivatorUtilities.CreateInstance<ByomeiService>(_serviceProvider);
        }
        else if (service == 5)
        {
            serviceCollection.AddTransient<IKarte1Service, Karte1Service>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<ICoKarte1Finder, CoKarte1Finder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _karte1Service = ActivatorUtilities.CreateInstance<Karte1Service>(_serviceProvider);
        }
        else if (service == 6)
        {
            serviceCollection.AddTransient<INameLabelService, NameLabelService>();
            serviceCollection.AddTransient<ICoNameLabelFinder, CoNameLabelFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _nameLabelService = ActivatorUtilities.CreateInstance<NameLabelService>(_serviceProvider);
        }
        else if (service == 7)
        {
            serviceCollection.AddTransient<IMedicalRecordWebIdReportService, MedicalRecordWebIdReportService>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _medicalRecordWebIdReportService = ActivatorUtilities.CreateInstance<MedicalRecordWebIdReportService>(_serviceProvider);
        }
        else if (service == 8)
        {
            serviceCollection.AddTransient<IReceiptCheckCoReportService, ReceiptCheckCoReportService>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _receiptCheckCoReportService = ActivatorUtilities.CreateInstance<ReceiptCheckCoReportService>(_serviceProvider);
        }
        else if (service == 9)
        {
            serviceCollection.AddTransient<IReceiptListCoReportService, ReceiptListCoReportService>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _receiptListCoReportService = ActivatorUtilities.CreateInstance<ReceiptListCoReportService>(_serviceProvider);
        }
        else if (service == 10)
        {
            serviceCollection.AddTransient<IOutDrugCoReportService, OutDrugCoReportService>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<ICoOutDrugFinder, CoOutDrugFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _outDrugCoReportService = ActivatorUtilities.CreateInstance<OutDrugCoReportService>(_serviceProvider);
        }
        else if (service == 11)
        {
            serviceCollection.AddTransient<IAccountingCoReportService, AccountingCoReportService>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<IEmrLogger, EmrLogger>();
            serviceCollection.AddTransient<ILoggingHandler, LoggingHandler>();
            serviceCollection.AddTransient<ICoAccountingFinder, CoAccountingFinder>();
            serviceCollection.AddTransient<ISystemConfigProvider, SystemConfigProvider>();
            serviceCollection.AddTransient<ILogger<EmrLogger>>(_ => _logger);
            serviceCollection.AddTransient<DbContextOptions>(_ => _tenantProvider.CreateNewTrackingAdminDbContextOption());

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _accountingCoReportService = ActivatorUtilities.CreateInstance<AccountingCoReportService>(_serviceProvider);
        }
        else if (service == 12)
        {
            serviceCollection.AddTransient<IStatisticService, StatisticService>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<IDailyStatisticCommandFinder, DailyStatisticCommandFinder>();
            serviceCollection.AddTransient<ISta1002CoReportService, Sta1002CoReportService>();
            serviceCollection.AddTransient<ISta1010CoReportService, Sta1010CoReportService>();
            serviceCollection.AddTransient<ISta2001CoReportService, Sta2001CoReportService>();
            serviceCollection.AddTransient<ISta2003CoReportService, Sta2003CoReportService>();
            serviceCollection.AddTransient<ISta1001CoReportService, Sta1001CoReportService>();
            serviceCollection.AddTransient<ISta2002CoReportService, Sta2002CoReportService>();
            serviceCollection.AddTransient<ISta2010CoReportService, Sta2010CoReportService>();
            serviceCollection.AddTransient<ISta2011CoReportService, Sta2011CoReportService>();
            serviceCollection.AddTransient<ISta2021CoReportService, Sta2021CoReportService>();
            serviceCollection.AddTransient<ISta3020CoReportService, Sta3020CoReportService>();
            serviceCollection.AddTransient<ISta3080CoReportService, Sta3080CoReportService>();
            serviceCollection.AddTransient<ISta3071CoReportService, Sta3071CoReportService>();
            serviceCollection.AddTransient<ISta2020CoReportService, Sta2020CoReportService>();
            serviceCollection.AddTransient<ISta3010CoReportService, Sta3010CoReportService>();
            serviceCollection.AddTransient<ISta3030CoReportService, Sta3030CoReportService>();
            serviceCollection.AddTransient<ISta3001CoReportService, Sta3001CoReportService>();
            serviceCollection.AddTransient<ISta3040CoReportService, Sta3040CoReportService>();
            serviceCollection.AddTransient<ISta3041CoReportService, Sta3041CoReportService>();
            serviceCollection.AddTransient<ISta3050CoReportService, Sta3050CoReportService>();
            serviceCollection.AddTransient<ISta3060CoReportService, Sta3060CoReportService>();
            serviceCollection.AddTransient<ISta3061CoReportService, Sta3061CoReportService>();
            serviceCollection.AddTransient<ISta3062CoReportService, Sta3062CoReportService>();
            serviceCollection.AddTransient<ISta3070CoReportService, Sta3070CoReportService>();


            serviceCollection.AddTransient<ICoSta1002Finder, CoSta1002Finder>();
            serviceCollection.AddTransient<ICoSta1010Finder, CoSta1010Finder>();
            serviceCollection.AddTransient<Reporting.Statistics.DB.ICoHpInfFinder, Reporting.Statistics.DB.CoHpInfFinder>();
            serviceCollection.AddTransient<ICoSta1001Finder, CoSta1001Finder>();
            serviceCollection.AddTransient<ICoSta2001Finder, CoSta2001Finder>();
            serviceCollection.AddTransient<ICoSta2003Finder, CoSta2003Finder>();
            serviceCollection.AddTransient<ICoSta2002Finder, CoSta2002Finder>();
            serviceCollection.AddTransient<ICoSta2010Finder, CoSta2010Finder>();
            serviceCollection.AddTransient<ICoSta2011Finder, CoSta2011Finder>();
            serviceCollection.AddTransient<ICoSta2021Finder, CoSta2021Finder>();
            serviceCollection.AddTransient<ICoSta3020Finder, CoSta3020Finder>();
            serviceCollection.AddTransient<ICoSta3080Finder, CoSta3080Finder>();
            serviceCollection.AddTransient<ICoSta3071Finder, CoSta3071Finder>();
            serviceCollection.AddTransient<ICoSta2020Finder, CoSta2020Finder>();
            serviceCollection.AddTransient<ICoSta3010Finder, CoSta3010Finder>();
            serviceCollection.AddTransient<ICoSta3030Finder, CoSta3030Finder>();
            serviceCollection.AddTransient<ICoSta3001Finder, CoSta3001Finder>();
            serviceCollection.AddTransient<ICoSta3040Finder, CoSta3040Finder>();
            serviceCollection.AddTransient<ICoSta3041Finder, CoSta3041Finder>();
            serviceCollection.AddTransient<ICoSta3050Finder, CoSta3050Finder>();
            serviceCollection.AddTransient<ICoSta3060Finder, CoSta3060Finder>();
            serviceCollection.AddTransient<ICoSta3061Finder, CoSta3061Finder>();
            serviceCollection.AddTransient<ICoSta3062Finder, CoSta3062Finder>();
            serviceCollection.AddTransient<ICoSta3070Finder, CoSta3070Finder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _statisticService = ActivatorUtilities.CreateInstance<StatisticService>(_serviceProvider);
        }
        else if (service == 13)
        {
            serviceCollection.AddTransient<IReceiptCoReportService, ReceiptCoReportService>();
            serviceCollection.AddTransient<ISystemConfRepository, SystemConfRepository>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<IEmrLogger, EmrLogger>();
            serviceCollection.AddTransient<ILoggingHandler, LoggingHandler>();
            serviceCollection.AddTransient<ICoReceiptFinder, CoReceiptFinder>();
            serviceCollection.AddTransient<ISystemConfigProvider, SystemConfigProvider>();
            serviceCollection.AddTransient<IAccountingRepository, AccountingRepository>();
            serviceCollection.AddTransient<ILogger<EmrLogger>>(_ => _logger);
            serviceCollection.AddTransient<DbContextOptions>(_ => _tenantProvider.CreateNewTrackingAdminDbContextOption());

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _receiptCoReportService = ActivatorUtilities.CreateInstance<ReceiptCoReportService>(_serviceProvider);
        }
        else if (service == 14)
        {
            serviceCollection.AddTransient<IPatientManagementService, PatientManagementService>();
            serviceCollection.AddTransient<IPatientManagementFinder, PatientManagementFinder>();
            serviceCollection.AddTransient<ISta9000CoReportService, Sta9000CoReportService>();
            serviceCollection.AddTransient<ICoSta9000Finder, CoSta9000Finder>();
            serviceCollection.AddTransient<Reporting.Statistics.DB.ICoHpInfFinder, Reporting.Statistics.DB.CoHpInfFinder>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _patientManagementService = ActivatorUtilities.CreateInstance<PatientManagementService>(_serviceProvider);
        }
        else if (service == 15)
        {
            serviceCollection.AddTransient<ISyojyoSyokiCoReportService, SyojyoSyokiCoReportService>();
            serviceCollection.AddTransient<ICoSyojyoSyokiFinder, CoSyojyoSyokiFinder>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _syojyoSyokiCoReportService = ActivatorUtilities.CreateInstance<SyojyoSyokiCoReportService>(_serviceProvider);
        }
        else if (service == 16)
        {
            serviceCollection.AddTransient<IKensaIraiCoReportService, KensaIraiCoReportService>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<ICoKensaIraiFinder, CoKensaIraiFinder>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _kensaIraiCoReportService = ActivatorUtilities.CreateInstance<KensaIraiCoReportService>(_serviceProvider);
        }
        else if (service == 17)
        {
            serviceCollection.AddTransient<IReceiptPrintService, ReceiptPrintService>();
            serviceCollection.AddTransient<IP28KokhoSokatuCoReportService, P28KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP11KokhoSokatuCoReportService, P11KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IHikariDiskCoReportService, HikariDiskCoReportService>();
            serviceCollection.AddTransient<IP28KoukiSeikyuCoReportService, P28KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP29KoukiSeikyuCoReportService, P29KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IAfterCareSeikyuCoReportService, AfterCareSeikyuCoReportService>();
            serviceCollection.AddTransient<ISyahoCoReportService, SyahoCoReportService>();
            serviceCollection.AddTransient<IP30KoukiSeikyuCoReportService, P30KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP33KoukiSeikyuCoReportService, P33KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP34KoukiSeikyuCoReportService, P34KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP35KoukiSeikyuCoReportService, P35KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP37KoukiSeikyuCoReportService, P37KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP40KoukiSeikyuCoReportService, P40KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP42KoukiSeikyuCoReportService, P42KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP45KoukiSeikyuCoReportService, P45KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP09KoukiSeikyuCoReportService, P09KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP12KoukiSeikyuCoReportService, P12KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP13KoukiSeikyuCoReportService, P13KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP08KokhoSokatuCoReportService, P08KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP41KoukiSeikyuCoReportService, P41KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP44KoukiSeikyuCoReportService, P44KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP08KoukiSeikyuCoReportService, P08KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP11KoukiSeikyuCoReportService, P11KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP14KoukiSeikyuCoReportService, P14KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP17KoukiSeikyuCoReportService, P17KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP20KoukiSeikyuCoReportService, P20KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP25KokhoSokatuCoReportService, P25KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP13WelfareSeikyuCoReportService, P13WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP08KokhoSeikyuCoReportService, P08KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP22WelfareSeikyuCoReportService, P22WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP21KoukiSeikyuCoReportService, P21KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP22KoukiSeikyuCoReportService, P22KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP23KoukiSeikyuCoReportService, P23KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP24KoukiSeikyuCoReportService, P24KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP25KoukiSeikyuCoReportService, P25KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP27KoukiSeikyuCoReportService, P27KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP14KokhoSokatuCoReportService, P14KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP17KokhoSokatuCoReportService, P17KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP20KokhoSokatuCoReportService, P20KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP22KokhoSokatuCoReportService, P22KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP23KokhoSokatuCoReportService, P23KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP26KokhoSokatuInCoReportService, P26KokhoSokatuInCoReportService>();
            serviceCollection.AddTransient<IP33KokhoSokatuCoReportService, P33KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP34KokhoSokatuCoReportService, P34KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP35KokhoSokatuCoReportService, P35KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP37KokhoSokatuCoReportService, P37KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP37KoukiSokatuCoReportService, P37KoukiSokatuCoReportService>();
            serviceCollection.AddTransient<IP26KokhoSokatuOutCoReportService, P26KokhoSokatuOutCoReportService>();
            serviceCollection.AddTransient<IP40KokhoSokatuCoReportService, P40KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP41KokhoSokatuCoReportService, P41KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP42KokhoSokatuCoReportService, P42KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP12KokhoSokatuCoReportService, P12KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP13KokhoSokatuCoReportService, P13KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP43KokhoSokatuCoReportService, P43KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP43KoukiSokatuCoReportService, P43KoukiSokatuCoReportService>();
            serviceCollection.AddTransient<IP44KokhoSokatuCoReportService, P44KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP45KoukiSokatuCoReportService, P45KoukiSokatuCoReportService>();
            serviceCollection.AddTransient<IP45KokhoSokatuCoReportService, P45KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP12KokhoSeikyuCoReportService, P12KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP13KokhoSeikyuCoReportService, P13KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP14KokhoSeikyuCoReportService, P14KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP20KokhoSeikyuCoReportService, P20KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP21KokhoSeikyuCoReportService, P21KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP22KokhoSeikyuCoReportService, P22KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP23KokhoSeikyuCoReportService, P23KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP24KokhoSeikyuCoReportService, P24KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IReceiptCoReportService, ReceiptCoReportService>();
            serviceCollection.AddTransient<IP26KokhoSeikyuOutCoReportService, P26KokhoSeikyuOutCoReportService>();
            serviceCollection.AddTransient<IP27KokhoSeikyuInCoReportService, P27KokhoSeikyuInCoReportService>();
            serviceCollection.AddTransient<IP27KokhoSeikyuOutCoReportService, P27KokhoSeikyuOutCoReportService>();
            serviceCollection.AddTransient<IP28KokhoSeikyuCoReportService, P28KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP29KokhoSeikyuCoReportService, P29KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP30KokhoSeikyuCoReportService, P30KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP42KokhoSeikyuCoReportService, P42KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP43KokhoSeikyuCoReportService, P43KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP20WelfareSokatuCoReportService, P20WelfareSokatuCoReportService>();
            serviceCollection.AddTransient<IP21WelfareSeikyuCoReportService, P21WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP21WelfareSokatuCoReportService, P21WelfareSokatuCoReportService>();
            serviceCollection.AddTransient<IP09KokhoSeikyuCoReportService, P09KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP23NagoyaSeikyuCoReportService, P23NagoyaSeikyuCoReportService>();
            serviceCollection.AddTransient<IP23WelfareSeikyuCoReportService, P23WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP24WelfareSofuDiskCoReportService, P24WelfareSofuDiskCoReportService>();
            serviceCollection.AddTransient<IP24WelfareSofuPaperCoReportService, P24WelfareSofuPaperCoReportService>();
            serviceCollection.AddTransient<IP24WelfareSyomeiCoReportService, P24WelfareSyomeiCoReportService>();
            serviceCollection.AddTransient<IP24WelfareSyomeiListCoReportService, P24WelfareSyomeiListCoReportService>();
            serviceCollection.AddTransient<IP24WelfareSyomeiSofuCoReportService, P24WelfareSyomeiSofuCoReportService>();
            serviceCollection.AddTransient<IP26VaccineSokatuCoReportService, P26VaccineSokatuCoReportService>();
            serviceCollection.AddTransient<IP27IzumisanoSeikyuCoReportService, P27IzumisanoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP35WelfareSeikyuCoReportService, P35WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP35WelfareSokatuCoReportService, P35WelfareSokatuCoReportService>();
            serviceCollection.AddTransient<IP43KikuchiMeisai41CoReportService, P43KikuchiMeisai41CoReportService>();
            serviceCollection.AddTransient<IP43KikuchiMeisai43CoReportService, P43KikuchiMeisai43CoReportService>();
            serviceCollection.AddTransient<IP43KikuchiSeikyu41CoReportService, P43KikuchiSeikyu41CoReportService>();
            serviceCollection.AddTransient<IP43KikuchiSeikyu43CoReportService, P43KikuchiSeikyu43CoReportService>();
            serviceCollection.AddTransient<IP43KumamotoSeikyuCoReportService, P43KumamotoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP44WelfareSeikyu84CoReportService, P44WelfareSeikyu84CoReportService>();
            serviceCollection.AddTransient<IP43ReihokuSeikyu41CoReportService, P43ReihokuSeikyu41CoReportService>();
            serviceCollection.AddTransient<IP43AmakusaSeikyu41CoReportService, P43AmakusaSeikyu41CoReportService>();
            serviceCollection.AddTransient<IP43AmakusaSeikyu42CoReportService, P43AmakusaSeikyu42CoReportService>();
            serviceCollection.AddTransient<IReceTargetCoReportService, ReceTargetCoReportService>();
            serviceCollection.AddTransient<ISyojyoSyokiCoReportService, SyojyoSyokiCoReportService>();
            serviceCollection.AddTransient<IP11KokhoSeikyuCoReportService, P11KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP14WelfareSeikyuCoReportService, P14WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP17KokhoSeikyuCoReportService, P17KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP17WelfareSeikyuCoReportService, P17WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP25WelfareSeikyuCoReportService, P25WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP29WelfareSeikyuCoReportService, P29WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP33KokhoSeikyuCoReportService, P33KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP26KoukiSokatuInCoReportService, P26KoukiSokatuInCoReportService>();
            serviceCollection.AddTransient<IP25KokhoSeikyuCoReportService, P25KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP26WelfareSeikyuCoReportService, P26WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP34KokhoSeikyuCoReportService, P34KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP35KokhoSeikyuCoReportService, P35KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP37KokhoSeikyuCoReportService, P37KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP40KokhoSeikyuCityCoReportService, P40KokhoSeikyuCityCoReportService>();
            serviceCollection.AddTransient<IP40KokhoSeikyuKumiaiCoReportService, P40KokhoSeikyuKumiaiCoReportService>();
            serviceCollection.AddTransient<IP40WelfareSeikyuCoReportService, P40WelfareSeikyuCoReportService>();
            serviceCollection.AddTransient<IP41KokhoSeikyuCoReportService, P41KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP44KokhoSeikyuCoReportService, P44KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP45KokhoSeikyuCoReportService, P45KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP46KokhoSokatuCoReportService, P46KokhoSokatuCoReportService>();
            serviceCollection.AddTransient<IP46KokhoSeikyuCoReportService, P46KokhoSeikyuCoReportService>();
            serviceCollection.AddTransient<IP46KoukiSeikyuCoReportService, P46KoukiSeikyuCoReportService>();
            serviceCollection.AddTransient<IP46WelfareSofu99CoReportService, P46WelfareSofu99CoReportService>();
            serviceCollection.AddTransient<IP46WelfareSeikyu99CoReportService, P46WelfareSeikyu99CoReportService>();

            serviceCollection.AddTransient<ICoKokhoSokatuFinder, CoKokhoSokatuFinder>();
            serviceCollection.AddTransient<ICoHpInfFinder, CoHpInfFinder>();
            serviceCollection.AddTransient<ICoHokensyaMstFinder, CoHokensyaMstFinder>();
            serviceCollection.AddTransient<ICoKokhoSeikyuFinder, CoKokhoSeikyuFinder>();
            serviceCollection.AddTransient<ICoHokenMstFinder, CoHokenMstFinder>();
            serviceCollection.AddTransient<ICoHikariDiskFinder, CoHikariDiskFinder>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();
            serviceCollection.AddTransient<ICoKoukiSeikyuFinder, CoKoukiSeikyuFinder>();
            serviceCollection.AddTransient<ICoAfterCareSeikyuFinder, CoAfterCareSeikyuFinder>();
            serviceCollection.AddTransient<ICoSyahoFinder, CoSyahoFinder>();
            serviceCollection.AddTransient<ICoWelfareSeikyuFinder, CoWelfareSeikyuFinder>();
            serviceCollection.AddTransient<IReceiptCoReportService, ReceiptCoReportService>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<ICoReceTargetFinder, CoReceTargetFinder>();
            serviceCollection.AddTransient<ICoSyojyoSyokiFinder, CoSyojyoSyokiFinder>();
            serviceCollection.AddTransient<ICoReceiptFinder, CoReceiptFinder>();
            serviceCollection.AddTransient<ISystemConfRepository, SystemConfRepository>();
            serviceCollection.AddTransient<ISystemConfigProvider, SystemConfigProvider>();
            serviceCollection.AddTransient<IAccountingRepository, AccountingRepository>();
            serviceCollection.AddTransient<IEmrLogger, EmrLogger>();
            serviceCollection.AddTransient<ILoggingHandler, LoggingHandler>();
            serviceCollection.AddTransient<ILogger<EmrLogger>>(_ => _logger);
            serviceCollection.AddTransient<DbContextOptions>(_ => _tenantProvider.CreateNewTrackingAdminDbContextOption());

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _receiptPrintService = ActivatorUtilities.CreateInstance<ReceiptPrintService>(_serviceProvider);
        }
        else if (service == 18)
        {
            serviceCollection.AddTransient<IMemoMsgCoReportService, MemoMsgCoReportService>();
            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _memoMsgCoReportService = ActivatorUtilities.CreateInstance<MemoMsgCoReportService>(_serviceProvider);
        }
        else if (service == 19)
        {
            serviceCollection.AddTransient<IReceTargetCoReportService, ReceTargetCoReportService>();
            serviceCollection.AddTransient<ICoReceTargetFinder, CoReceTargetFinder>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _receTargetCoReportService = ActivatorUtilities.CreateInstance<ReceTargetCoReportService>(_serviceProvider);
        }
        else if (service == 20)
        {
            serviceCollection.AddTransient<IDrugNoteSealCoReportService, DrugNoteSealCoReportService>();
            serviceCollection.AddTransient<ICoDrugNoteSealFinder, CoDrugNoteSealFinder>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _drugNoteSealCoReportService = ActivatorUtilities.CreateInstance<DrugNoteSealCoReportService>(_serviceProvider);
        }
        else if (service == 21)
        {
            serviceCollection.AddTransient<IYakutaiCoReportService, YakutaiCoReportService>();
            serviceCollection.AddTransient<ICoYakutaiFinder, CoYakutaiFinder>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _yakutaiCoReportService = ActivatorUtilities.CreateInstance<YakutaiCoReportService>(_serviceProvider);
        }
        else if (service == 22)
        {
            serviceCollection.AddTransient<IAccountingCardCoReportService, AccountingCardCoReportService>();
            serviceCollection.AddTransient<ICoAccountingCardFinder, CoAccountingCardFinder>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<IEmrLogger, EmrLogger>();
            serviceCollection.AddTransient<ILoggingHandler, LoggingHandler>();
            serviceCollection.AddTransient<ISystemConfigProvider, SystemConfigProvider>();
            serviceCollection.AddTransient<ILogger<EmrLogger>>(_ => _logger);
            serviceCollection.AddTransient<DbContextOptions>(_ => _tenantProvider.CreateNewTrackingAdminDbContextOption());
            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _accountingCardCoReportService = ActivatorUtilities.CreateInstance<AccountingCardCoReportService>(_serviceProvider);
        }
        else if (service == 23)
        {
            serviceCollection.AddTransient<ICoAccountingFinder, CoAccountingFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _coAccountingFinder = ActivatorUtilities.CreateInstance<CoAccountingFinder>(_serviceProvider);
        }
        else if (service == 24)
        {
            serviceCollection.AddTransient<IKarte3CoReportService, Karte3CoReportService>();
            serviceCollection.AddTransient<ICoKarte3Finder, CoKarte3Finder>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _karte3CoReportService = ActivatorUtilities.CreateInstance<Karte3CoReportService>(_serviceProvider);
        }
        else if (service == 25)
        {
            serviceCollection.AddTransient<IAccountingCardListCoReportService, AccountingCardListCoReportService>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<ICoAccountingCardListFinder, CoAccountingCardListFinder>();
            serviceCollection.AddTransient<ISystemConfigProvider, SystemConfigProvider>();
            serviceCollection.AddTransient<IEmrLogger, EmrLogger>();
            serviceCollection.AddTransient<ILoggingHandler, LoggingHandler>();
            serviceCollection.AddTransient<ILogger<EmrLogger>>(_ => _logger);
            serviceCollection.AddTransient<DbContextOptions>(_ => _tenantProvider.CreateNewTrackingAdminDbContextOption());

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _accountingCardListCoReportService = ActivatorUtilities.CreateInstance<AccountingCardListCoReportService>(_serviceProvider);
        }
        else if (service == 26)
        {
            serviceCollection.AddTransient<IInDrugCoReportService, InDrugCoReportService>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<ICoInDrugFinder, CoInDrugFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _inDrugCoReportService = ActivatorUtilities.CreateInstance<InDrugCoReportService>(_serviceProvider);
        }
        else if (service == 27)
        {
            serviceCollection.AddTransient<IGrowthCurveA4CoReportService, GrowthCurveA4CoReportService>();
            serviceCollection.AddTransient<ISpecialNoteFinder, SpecialNoteFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _growthCurveA4CoReportService = ActivatorUtilities.CreateInstance<GrowthCurveA4CoReportService>(_serviceProvider);
        }
        else if (service == 28)
        {
            serviceCollection.AddTransient<IGrowthCurveA5CoReportService, GrowthCurveA5CoReportService>();
            serviceCollection.AddTransient<ISpecialNoteFinder, SpecialNoteFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _growthCurveA5CoReportService = ActivatorUtilities.CreateInstance<GrowthCurveA5CoReportService>(_serviceProvider);
        }
        else if (service == 29)
        {
            serviceCollection.AddTransient<IKensaLabelCoReportService, KensaLabelCoReportService>();
            serviceCollection.AddTransient<IKensaLabelFinder, KensaLabelFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _kensaLabelCoReportService = ActivatorUtilities.CreateInstance<KensaLabelCoReportService>(_serviceProvider);
        }
        else if (service == 30)
        {
            serviceCollection.AddTransient<IReceiptPrintExcelService, ReceiptPrintExcelService>();
            serviceCollection.AddTransient<IP24WelfareDiskService, P24WelfareDiskService>();
            serviceCollection.AddTransient<ICoWelfareSeikyuFinder, CoWelfareSeikyuFinder>();
            serviceCollection.AddTransient<Reporting.Sokatu.Common.DB.ICoHpInfFinder, Reporting.Sokatu.Common.DB.CoHpInfFinder>();
            serviceCollection.AddTransient<ICoHokensyaMstFinder, CoHokensyaMstFinder>();
            serviceCollection.AddTransient<ICoHokenMstFinder, CoHokenMstFinder>();
            serviceCollection.AddTransient<IP44WelfareDiskService, P44WelfareDiskService>();
            serviceCollection.AddTransient<ICoWelfareSeikyuFinder, CoWelfareSeikyuFinder>();
            serviceCollection.AddTransient<IP43Amakusa41DiskService, P43Amakusa41DiskService>();
            serviceCollection.AddTransient<ICoWelfareSeikyuFinder, CoWelfareSeikyuFinder>();
            serviceCollection.AddTransient<IP46WelfareDiskService, P46WelfareDiskService>();
            serviceCollection.AddTransient<ICoWelfareSeikyuFinder, CoWelfareSeikyuFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _receiptPrintExcelService = ActivatorUtilities.CreateInstance<ReceiptPrintExcelService>(_serviceProvider);
        }
        else if (service == 31)
        {
            serviceCollection.AddTransient<IImportCSVCoReportService, ImportCSVCoReportService>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _importCSVCoReportService = ActivatorUtilities.CreateInstance<ImportCSVCoReportService>(_serviceProvider);
        }
        else if (service == 32)
        {
            serviceCollection.AddTransient<IStaticsticExportCsvService, StaticsticExportCsvService>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();
            serviceCollection.AddTransient<IDailyStatisticCommandFinder, DailyStatisticCommandFinder>();
            serviceCollection.AddTransient<ISta1002CoReportService, Sta1002CoReportService>();
            serviceCollection.AddTransient<ISta1010CoReportService, Sta1010CoReportService>();
            serviceCollection.AddTransient<ISta2001CoReportService, Sta2001CoReportService>();
            serviceCollection.AddTransient<ISta2003CoReportService, Sta2003CoReportService>();
            serviceCollection.AddTransient<ISta1001CoReportService, Sta1001CoReportService>();
            serviceCollection.AddTransient<ISta2002CoReportService, Sta2002CoReportService>();
            serviceCollection.AddTransient<ISta2010CoReportService, Sta2010CoReportService>();
            serviceCollection.AddTransient<ISta2011CoReportService, Sta2011CoReportService>();
            serviceCollection.AddTransient<ISta2021CoReportService, Sta2021CoReportService>();
            serviceCollection.AddTransient<ISta3020CoReportService, Sta3020CoReportService>();
            serviceCollection.AddTransient<ISta3080CoReportService, Sta3080CoReportService>();
            serviceCollection.AddTransient<ISta3071CoReportService, Sta3071CoReportService>();
            serviceCollection.AddTransient<ISta2020CoReportService, Sta2020CoReportService>();
            serviceCollection.AddTransient<ISta3010CoReportService, Sta3010CoReportService>();
            serviceCollection.AddTransient<ISta3030CoReportService, Sta3030CoReportService>();
            serviceCollection.AddTransient<ISta3001CoReportService, Sta3001CoReportService>();
            serviceCollection.AddTransient<ISta3040CoReportService, Sta3040CoReportService>();
            serviceCollection.AddTransient<ISta3041CoReportService, Sta3041CoReportService>();
            serviceCollection.AddTransient<ISta3050CoReportService, Sta3050CoReportService>();
            serviceCollection.AddTransient<ISta3060CoReportService, Sta3060CoReportService>();
            serviceCollection.AddTransient<ISta3061CoReportService, Sta3061CoReportService>();
            serviceCollection.AddTransient<ISta3070CoReportService, Sta3070CoReportService>();


            serviceCollection.AddTransient<ICoSta1002Finder, CoSta1002Finder>();
            serviceCollection.AddTransient<ICoSta1010Finder, CoSta1010Finder>();
            serviceCollection.AddTransient<Reporting.Statistics.DB.ICoHpInfFinder, Reporting.Statistics.DB.CoHpInfFinder>();
            serviceCollection.AddTransient<ICoSta1001Finder, CoSta1001Finder>();
            serviceCollection.AddTransient<ICoSta2001Finder, CoSta2001Finder>();
            serviceCollection.AddTransient<ICoSta2003Finder, CoSta2003Finder>();
            serviceCollection.AddTransient<ICoSta2002Finder, CoSta2002Finder>();
            serviceCollection.AddTransient<ICoSta2010Finder, CoSta2010Finder>();
            serviceCollection.AddTransient<ICoSta2011Finder, CoSta2011Finder>();
            serviceCollection.AddTransient<ICoSta2021Finder, CoSta2021Finder>();
            serviceCollection.AddTransient<ICoSta3020Finder, CoSta3020Finder>();
            serviceCollection.AddTransient<ICoSta3080Finder, CoSta3080Finder>();
            serviceCollection.AddTransient<ICoSta3071Finder, CoSta3071Finder>();
            serviceCollection.AddTransient<ICoSta2020Finder, CoSta2020Finder>();
            serviceCollection.AddTransient<ICoSta3010Finder, CoSta3010Finder>();
            serviceCollection.AddTransient<ICoSta3030Finder, CoSta3030Finder>();
            serviceCollection.AddTransient<ICoSta3001Finder, CoSta3001Finder>();
            serviceCollection.AddTransient<ICoSta3040Finder, CoSta3040Finder>();
            serviceCollection.AddTransient<ICoSta3041Finder, CoSta3041Finder>();
            serviceCollection.AddTransient<ICoSta3050Finder, CoSta3050Finder>();
            serviceCollection.AddTransient<ICoSta3060Finder, CoSta3060Finder>();
            serviceCollection.AddTransient<ICoSta3061Finder, CoSta3061Finder>();
            serviceCollection.AddTransient<ICoSta3070Finder, CoSta3070Finder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _staticsticExportCsvService = ActivatorUtilities.CreateInstance<StaticsticExportCsvService>(_serviceProvider);
        }
        else if (service == 33)
        {
            serviceCollection.AddTransient<ISta9000CoReportService, Sta9000CoReportService>();
            serviceCollection.AddTransient<ICoSta9000Finder, CoSta9000Finder>();
            serviceCollection.AddTransient<Reporting.Statistics.DB.ICoHpInfFinder, Reporting.Statistics.DB.CoHpInfFinder>();
            serviceCollection.AddTransient<ISystemConfig, SystemConfig>();
            serviceCollection.AddTransient<IReadRseReportFileService, ReadRseReportFileService>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _sta9000CoReportService = ActivatorUtilities.CreateInstance<Sta9000CoReportService>(_serviceProvider);
        }
        else if (service == 34)
        {
            serviceCollection.AddTransient<IKensaHistoryCoReportService, KensaHistoryCoReportService>();
            serviceCollection.AddTransient<ICoKensaHistoryFinder, CoKensaHistoryFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _kensaHistoryCoReportService = ActivatorUtilities.CreateInstance<KensaHistoryCoReportService>(_serviceProvider);
        }
        else if (service == 35)
        {
            serviceCollection.AddTransient<IKensaResultMultiCoReportService, KensaResultMultiCoReportService>();
            serviceCollection.AddTransient<IKensaSetRepository, KensaSetRepository>();
            serviceCollection.AddTransient<ICoKensaHistoryFinder, CoKensaHistoryFinder>();

            // create service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _kensaResultMultiCoReportService = ActivatorUtilities.CreateInstance<KensaResultMultiCoReportService>(_serviceProvider);
        }
    }
    public void ReleaseResource()
    {
        _serviceProvider.Dispose();
        _tenantProvider.DisposeDataContext();
    }

    //Byomei
    public CommonReportingRequestModel GetByomeiReportingData(int hpId, long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds)
    {
        return _byomeiService.GetByomeiReportingData(hpId, ptId, fromDay, toDay, tenkiIn, hokenIds);
    }

    //Karte1
    public CommonReportingRequestModel GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
    {
        return _karte1Service.GetKarte1ReportingData(hpId, ptId, sinDate, hokenPid, tenkiByomei, syuByomei);
    }

    //NameLabel
    public CommonReportingRequestModel GetNameLabelReportingData(int hpId, long ptId, string kanjiName, int sinDate)
    {
        return _nameLabelService.GetNameLabelReportingData(hpId, ptId, kanjiName, sinDate);
    }

    //Sijisen
    public CommonReportingRequestModel GetSijisenReportingData(int hpId, int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr)
    {
        return _sijisenReportService.GetSijisenReportingData(hpId, formType, ptId, sinDate, raiinNo, odrKouiKbns, printNoOdr);
    }

    //OrderLabel
    public CommonReportingRequestModel GetOrderLabelReportingData(int mode, int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, List<RsvkrtOdrInfModel> rsvKrtOdrInfModels)
    {
        return _orderLabelCoReportService.GetOrderLabelReportingData(mode, hpId, ptId, sinDate, raiinNo, odrKouiKbns, rsvKrtOdrInfModels);
    }

    //OrderInfo
    public DrugInfoData SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _drugInfoCoReportService.SetOrderInfo(hpId, ptId, sinDate, raiinNo);
    }
    public CommonReportingRequestModel GetInDrugPrintData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _inDrugCoReportService.GetInDrugPrintData(hpId, ptId, sinDate, raiinNo);
    }
    public CommonReportingRequestModel GetAccountingCardListReportingData(int hpId, List<TargetItem> targets, bool includeOutDrug, string kaName, string tantoName, string uketukeSbt, string hoken)
    {
        return _accountingCardListCoReportService.GetAccountingCardListData(hpId, targets, includeOutDrug, kaName, tantoName, uketukeSbt, hoken);
    }

    //MedicalRecordWebId
    public CommonReportingRequestModel GetMedicalRecordWebIdReportingData(int hpId, long ptId, int sinDate)
    {
        return _medicalRecordWebIdReportService.GetMedicalRecordWebIdReportingData(hpId, ptId, sinDate);
    }

    //ReceiptCheckCoReport
    public CommonReportingRequestModel GetReceiptCheckCoReportService(int hpId, List<long> ptIds, int seikyuYm)
    {
        return _receiptCheckCoReportService.GetReceiptCheckCoReportingData(hpId, ptIds, seikyuYm);
    }

    //ReceiptListCoReport
    public CommonReportingRequestModel GetReceiptListReportingData(int hpId, int seikyuYm, List<ReceiptInputModel> receiptListModels)
    {
        return _receiptListCoReportService.GetReceiptListReportingData(hpId, seikyuYm, receiptListModels);
    }
    public CommonReportingRequestModel GetSyojyoSyokiReportingData(int hpId, long ptId, int seikyuYm, int hokenId)
    {
        return _syojyoSyokiCoReportService.GetSyojyoSyokiReportingData(hpId, ptId, seikyuYm, hokenId);
    }

    //KensaLabelCoReportService
    public CommonReportingRequestModel GetKensaLabelPrintData(int hpId, long ptId, long raiinNo, int sinDate, KensaPrinterModel printerModel)
    {
        return _kensaLabelCoReportService.GetKensaLabelPrintData(hpId, ptId, raiinNo, sinDate, printerModel);
    }

    /// <summary>
    /// OutDrug
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="sinDate"></param>
    /// <param name="raiinNo"></param>
    /// <returns></returns>
    public CommonReportingRequestModel GetOutDrugReportingData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _outDrugCoReportService.GetOutDrugReportingData(hpId, ptId, sinDate, raiinNo);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="raiinNos"></param>
    /// <param name="hokenId"></param>
    /// <param name="miseisanKbn"></param>
    /// <param name="saiKbn"></param>
    /// <param name="misyuKbn"></param>
    /// <param name="seikyuKbn"></param>
    /// <param name="hokenKbn"></param>
    /// <param name="hokenSeikyu"></param>
    /// <param name="jihiSeikyu"></param>
    /// <param name="nyukinBase"></param>
    /// <param name="hakkoDay"></param>
    /// <param name="memo"></param>
    /// <param name="printType"></param>
    /// <param name="formFileName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public AccountingResponse GetAccountingReportingData(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId = 0, int miseisanKbn = 0, int saiKbn = 0, int misyuKbn = 0, int seikyuKbn = 1, int hokenKbn = 0, bool hokenSeikyu = false, bool jihiSeikyu = false, bool nyukinBase = false, int hakkoDay = 0, string memo = "", int printType = 0, string formFileName = "")
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, ptId, startDate, endDate, raiinNos, hokenId, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hokenSeikyu, jihiSeikyu, nyukinBase, hakkoDay, memo, printType, formFileName);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="ptConditions"></param>
    /// <param name="grpConditions"></param>
    /// <param name="sort"></param>
    /// <param name="miseisanKbn"></param>
    /// <param name="saiKbn"></param>
    /// <param name="misyuKbn"></param>
    /// <param name="seikyuKbn"></param>
    /// <param name="hokenKbn"></param>
    /// <param name="hakkoDay"></param>
    /// <param name="memo"></param>
    /// <param name="formFileName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public AccountingResponse GetAccountingReportingData(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions, int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, int hakkoDay, string memo, string formFileName)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, startDate, endDate, ptConditions, grpConditions, sort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hakkoDay, memo, formFileName);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="coAccountingParamModels"></param>
    /// <returns></returns>
    public AccountingResponse GetAccountingReportingData(int hpId, List<CoAccountingParamModel> coAccountingParamModels)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, coAccountingParamModels);
    }
    public AccountingResponse GetAccountingData(int hpId, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> multiAccountDueListModels, bool isPrintMonth, bool ryoshusho, bool meisai)
    {
        List<CoAccountingParamModel> requestAccountting = new();
        List<CoAccountDueListModel> nyukinModels = _coAccountingFinder.GetAccountDueList(hpId, ptId);
        List<int> months = new();
        List<CoAccountDueListModel> accountDueListUnique = new();
        foreach (var model in multiAccountDueListModels)
        {
            if (accountDueListUnique.Any(item => item.SinDate == model.SinDate
            && item.NyukinKbn == model.NyukinKbn
            && item.RaiinNo == model.RaiinNo
            && item.OyaRaiinNo == model.OyaRaiinNo))
            {
                continue;
            }
            accountDueListUnique.Add(model);
        }
        foreach (var model in accountDueListUnique)
        {
            var selectedAccountDueListModel = model;
            var accountDueListModels = nyukinModels.FindAll(p => p.SinDate / 100 == model.SinDate / 100);
            if (isPrintMonth)
            {
                if (!months.Contains(model.SinDate / 100))
                {
                    var printItem = PrintWithoutThread(ryoshusho, meisai, mode, ptId, accountDueListModels, selectedAccountDueListModel, isPrintMonth, model.SinDate, model.OyaRaiinNo, accountDueListModels);
                    requestAccountting.AddRange(printItem);
                    months.Add(model.SinDate / 100);
                }
            }
            else
            {
                var printItem = PrintWithoutThread(ryoshusho, meisai, mode, ptId, accountDueListModels, selectedAccountDueListModel, isPrintMonth, model.SinDate, model.OyaRaiinNo);
                requestAccountting.AddRange(printItem);
            }
        }

        AccountingResponse result = null;
        try
        {
            result = _accountingCoReportService.GetAccountingReportingData(hpId, requestAccountting);
            Console.WriteLine("result AccountingResponse: " + result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex);
        }
        return result;
    }

    public List<CoAccountingParamModel> PrintWithoutThread(bool ryoshusho, bool meisai, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> accountDueListModels, CoAccountDueListModel selectedAccountDueListModel, bool isPrintMonth, int sinDate, long oyaRaiinNo, List<CoAccountDueListModel>? nyukinModels = null)
    {
        int GetMaxDateInMonth(int month)
        {
            var models = accountDueListModels.Where(x => x.Month == month).OrderBy(x => x.SinDate);
            return models.Last().SinDate;
        }

        int GetMinDateInMonth(int month)
        {
            var models = accountDueListModels.Where(x => x.Month == month).OrderBy(x => x.SinDate);
            return models.First().SinDate;
        }

        List<CoAccountingParamModel> result = new();
        List<(int startDate, int endDate)> dates = new();
        if (accountDueListModels.Count >= 1)
        {
            if (isPrintMonth)
            {
                var groups = accountDueListModels.GroupBy(x => x.Month);
                List<int> months = groups.Select(x => x.Key).ToList();
                foreach (var month in months)
                {
                    dates.Add((GetMinDateInMonth(month), GetMaxDateInMonth(month)));
                }
            }
            else
            {
                if (mode == ConfirmationMode.FromPrintBtn || mode == ConfirmationMode.FromMenu)
                {
                    dates.Add((selectedAccountDueListModel.SinDate, selectedAccountDueListModel.SinDate));
                }
                else
                {
                    dates.AddRange(from item in accountDueListModels
                                   select (item.SinDate, item.SinDate));
                    dates = dates.Distinct().ToList();
                }
            }
        }

        List<int> printTypes = new();
        if (isPrintMonth)
        {
            if (ryoshusho)
            {
                ///2:月間領収証
                ///3:月間明細
                printTypes.Add(2);
            }

            if (meisai)
            {
                printTypes.Add(3);
            }
        }
        else
        {
            if (ryoshusho)
            {
                ///0:領収証
                ///1:明細
                printTypes.Add(0);
            }

            if (meisai)
            {
                printTypes.Add(1);
            }
        }

        if (!isPrintMonth && mode == ConfirmationMode.FromPrintBtn || mode == ConfirmationMode.FromMenu)
        {
            foreach (var printType in printTypes)
            {
                var raiinNos = accountDueListModels.Where(x => x.OyaRaiinNo == (oyaRaiinNo > 0 ? oyaRaiinNo : selectedAccountDueListModel.OyaRaiinNo)).Select(x => x.RaiinNo).ToList();
                result.Add(new(
                              ptId,
                              sinDate > 0 ? sinDate : selectedAccountDueListModel.SinDate,
                              sinDate > 0 ? sinDate : selectedAccountDueListModel.SinDate,
                              raiinNos,
                              printType: printType));
            }
        }
        else
        {
            foreach (var (startDate, endDate) in dates)
            {
                if (isPrintMonth)
                {
                    List<long> raiinNos = new();
                    if (nyukinModels != null)
                    {
                        raiinNos = nyukinModels.Where(x => x.SinDate >= startDate && x.SinDate <= endDate)
                                                                         .Select(x => x.RaiinNo).ToList();
                    }
                    else
                    {
                        accountDueListModels.Where(x => x.SinDate >= startDate && x.SinDate <= endDate)
                                                                         .Select(x => x.RaiinNo).ToList();
                    }
                    foreach (var printType in printTypes)
                    {
                        result.Add(new(ptId, startDate, endDate, raiinNos, printType: printType));
                    }
                }
                else
                {
                    List<(long, List<long>)> raiinNos;
                    if (nyukinModels != null)
                    {
                        raiinNos = nyukinModels.Where(x => x.SinDate >= startDate && x.SinDate <= endDate)
                                               .GroupBy(x => x.OyaRaiinNo)
                                               .Select(x => (x.Key, x.Select(item => item.RaiinNo).ToList())).ToList();
                    }
                    else
                    {
                        raiinNos = accountDueListModels.Where(x => x.SinDate >= startDate && x.SinDate <= endDate)
                                                       .GroupBy(x => x.OyaRaiinNo)
                                                       .Select(x => (x.Key, x.Select(item => item.RaiinNo).ToList())).ToList();
                    }
                    foreach (var printType in printTypes)
                    {
                        foreach (var oya in raiinNos)
                        {
                            result.Add(new(ptId, startDate, endDate, oya.Item2, printType: printType));
                        }
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="printTypeInput"></param>
    /// <param name="raiinNoList"></param>
    /// <param name="raiinNoPayList"></param>
    /// <param name="isCalculateProcess"></param>
    /// <returns></returns>
    public AccountingResponse GetAccountingReportingData(int hpId, long ptId, int printTypeInput, List<long> raiinNoList, List<long> raiinNoPayList, bool isCalculateProcess)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, ptId, printTypeInput, raiinNoList, raiinNoPayList, isCalculateProcess);
    }

    public CommonReportingRequestModel GetStatisticReportingData(int hpId, string formName, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo, CoFileType? coFileType = null, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0)
    {
        return _statisticService.PrintExecute(hpId, formName, menuId, monthFrom, monthTo, dateFrom, dateTo, timeFrom, timeTo, coFileType, isPutTotalRow, tenkiDateFrom, tenkiDateTo, enableRangeFrom, enableRangeTo, ptNumFrom, ptNumTo);
    }

    public CommonReportingRequestModel GetPatientManagement(int hpId, PatientManagementModel patientManagementModel)
    {
        return _patientManagementService.PrintData(hpId, patientManagementModel);
    }

    //Receipt Preview
    public CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId, int seikyuYm, int hokenKbn, bool isIncludeOutDrug, bool isModePrint, bool isOpenedFromAccounting)
    {
        if (isOpenedFromAccounting)
        {
            return _receiptCoReportService.GetReceiptDataFromAccounting(hpId, ptId, sinYm, hokenId, isIncludeOutDrug, isModePrint);
        }
        else
        {
            return _receiptCoReportService.GetReceiptDataFromReceCheck(hpId, ptId, sinYm, seikyuYm, hokenId, hokenKbn, isIncludeOutDrug, isModePrint);
        }
    }

    public CommonReportingRequestModel GetKensalraiData(int hpId, int systemDate, int fromDate, int toDate, string centerCd)
    {
        return _kensaIraiCoReportService.GetKensalraiData(hpId, systemDate, fromDate, toDate, centerCd);
    }

    //public CommonReportingRequestModel GetReceiptPrint(int hpId, string formName, int prefNo, int reportId, int reportEdaNo, int dataKbn, long ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt, int welfareType, List<string> printHokensyaNos, List<long> printPtIds)
    //{
    //    return _receiptPrintService.GetReceiptPrint(hpId, formName, prefNo, reportId, reportEdaNo, dataKbn, ptId, seikyuYm, sinYm, hokenId, diskKind, diskCnt, welfareType, printHokensyaNos, printPtIds);
    //}

    public CommonReportingRequestModel GetReceiptPrint(int hpId, string formName, int prefNo, int reportId, int reportEdaNo, int dataKbn, long ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt, int welfareType, List<string> printHokensyaNos, int hokenKbn, ReseputoShubetsuModel selectedReseputoShubeusu, int departmentId, int doctorId, int printNoFrom, int printNoTo, bool includeTester, bool includeOutDrug, int sort, List<long> listPtId)
    {
        return _receiptPrintService.GetReceiptPrint(hpId, formName, prefNo, reportId, reportEdaNo, dataKbn, ptId, seikyuYm, sinYm, hokenId, diskKind, diskCnt, welfareType, printHokensyaNos, hokenKbn, selectedReseputoShubeusu, departmentId, doctorId, printNoFrom, printNoTo, includeTester, includeOutDrug, sort, listPtId);
    }

    public CommonReportingRequestModel GetMemoMsgReportingData(string reportName, string title, List<string> listMessage)
    {
        return _memoMsgCoReportService.GetMemoMsgReportingData(reportName, title, listMessage);
    }

    public CommonReportingRequestModel GetReceTargetPrint(int hpId, int seikyuYm)
    {
        return _receTargetCoReportService.GetReceTargetPrintData(hpId, seikyuYm);
    }

    public CommonReportingRequestModel GetDrugNoteSealPrintData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _drugNoteSealCoReportService.GetDrugNoteSealPrintData(hpId, ptId, sinDate, raiinNo);
    }

    public CommonReportingRequestModel GetYakutaiReportingData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _yakutaiCoReportService.GetYakutaiReportingData(hpId, ptId, sinDate, raiinNo);
    }

    public CommonReportingRequestModel GetAccountingCardReportingData(int hpId, long ptId, int sinYm, int hokenId, bool includeOutDrug)
    {
        return _accountingCardCoReportService.GetAccountingCardReportingData(hpId, ptId, sinYm, hokenId, includeOutDrug);
    }

    // Karte 3
    public CommonReportingRequestModel GetKarte3ReportingData(int hpId, long ptId, int startSinYm, int endSinYm, bool includeHoken, bool includeJihi)
    {
        return _karte3CoReportService.GetKarte3PrintData(hpId, ptId, startSinYm, endSinYm, includeHoken, includeJihi);
    }

    public CommonReportingRequestModel GetGrowthCurveA4PrintData(int hpId, GrowthCurveConfig growthCurveConfig)
    {
        return _growthCurveA4CoReportService.GetGrowthCurveA4PrintData(hpId, growthCurveConfig);
    }

    public CommonReportingRequestModel GetGrowthCurveA5PrintData(int hpId, GrowthCurveConfig growthCurveConfig)
    {
        return _growthCurveA5CoReportService.GetGrowthCurveA5PrintData(hpId, growthCurveConfig);
    }

    public CommonExcelReportingModel GetReceiptPrintExcel(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int seikyuYm)
    {
        return _receiptPrintExcelService.GetReceiptPrintExcel(hpId, prefNo, reportId, reportEdaNo, dataKbn, seikyuYm);
    }

    public CommonExcelReportingModel GetReceiptListExcel(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput receiptListModel, bool isIsExportTitle)
    {
        return _importCSVCoReportService.GetImportCSVCoReportServiceReportingData(hpId, seikyuYm, receiptListModel, isIsExportTitle);
    }

    public List<string> OutputExcelForPeriodReceipt(int hpId, int startDate, int endDate, List<Tuple<long, int>> ptConditions, List<Tuple<int, string>> grpConditions, int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn)
    {
        return _accountingCoReportService.ExportCsv(hpId, startDate, endDate, ptConditions, grpConditions, sort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn);
    }

    #region Period Report
    public AccountingResponse GetPeriodPrintData(int hpId, int startDate, int endDate, List<PtInfInputItem> sourcePt, List<(int grpId, string grpCd)> grpConditions, int printSort, bool isPrintList, bool printByMonth, bool printByGroup, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, int hakkoDay, string memo, string formFileName, bool nyukinBase)
    {
        DateTime startDateOrigin = CIUtil.IntToDate(startDate);
        DateTime endDateOrigin = CIUtil.IntToDate(endDate);
        List<CoAccountingParamListModel> requestModelList = new();
        List<CoAccountingParamModel> coAccountingParamModels = new();

        List<(int startDate, int endDate)> dates = GetDates(startDateOrigin, endDateOrigin, startDate, endDate, printByMonth);
        if (isPrintList)
        {
            List<(long ptId, int hokenId)> ptCoditions = GetPtCondition(sourcePt, printSort);

            if (printByGroup)
            {
                foreach (var group in grpConditions)
                {
                    foreach (var date in dates)
                    {
                        requestModelList.Add(new(date.startDate,
                                                 date.endDate,
                                                 ptCoditions,
                                                 new List<(int grpId, string grpCd)>() { group }));
                    }
                }
                return _accountingCoReportService.GetAccountingReportingData(hpId, requestModelList, printSort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hakkoDay, memo, formFileName);
            }
            else
            {
                foreach (var date in dates)
                {
                    requestModelList.Add(new(date.startDate,
                                             date.endDate,
                                             ptCoditions,
                                             grpConditions));
                }
                return _accountingCoReportService.GetAccountingReportingData(hpId, requestModelList, printSort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hakkoDay, memo, formFileName);
            }
        }
        else
        {
            foreach (var date in dates)
            {
                foreach (var ptInf in sourcePt)
                {
                    coAccountingParamModels.Add(new CoAccountingParamModel(
                                                    ptInf.PtId,
                                                    date.startDate,
                                                    date.endDate,
                                                    raiinNos: new List<long>(),
                                                    hokenId: ptInf.HokenId,
                                                    miseisanKbn,
                                                    saiKbn,
                                                    misyuKbn,
                                                    seikyuKbn,
                                                    hokenKbn,
                                                    nyukinBase: nyukinBase,
                                                    hakkoDay: hakkoDay,
                                                    memo: memo,
                                                    formFileName: formFileName));
                }
            }
        }
        return _accountingCoReportService.GetAccountingReportingData(hpId, coAccountingParamModels);
    }

    #region private function
    private List<(int startDate, int endDate)> GetDates(DateTime startDateOrigin, DateTime endDateOrigin, int startDate, int endDate, bool printByMonth)
    {
        List<(int startDate, int endDate)> dates = new();
        if (printByMonth)
        {
            int differenceMonth = CountMonth(startDateOrigin, endDateOrigin);
            int lastDayOfMonth = DateTime.DaysInMonth(startDateOrigin.Year, startDateOrigin.Month);
            int lastDateInMonth = startDateOrigin.Year * 10000 + startDateOrigin.Month * 100 + lastDayOfMonth;
            dates.Add((startDate, lastDateInMonth));
            for (int i = 1; i <= differenceMonth; i++)
            {
                if (i == differenceMonth)
                {
                    int firstDateInMonth = endDateOrigin.Year * 10000 + endDateOrigin.Month * 100 + 1;
                    dates.Add((firstDateInMonth, endDate));
                }
                else
                {
                    DateTime nexttimeInMonth = startDateOrigin.AddMonths(i);
                    int nextStartDateInMonth = nexttimeInMonth.Year * 10000 + nexttimeInMonth.Month * 100 + 1;
                    int nextEndDateInMonth = nexttimeInMonth.Year * 10000 + nexttimeInMonth.Month * 100 + DateTime.DaysInMonth(nexttimeInMonth.Year, nexttimeInMonth.Month);
                    dates.Add((nextStartDateInMonth, nextEndDateInMonth));
                }
            }
        }
        else
        {
            dates.Add((startDate, endDate));
        }
        return dates;
    }

    private int CountMonth(DateTime startD, DateTime endD)
    {
        return 12 * (endD.Year - startD.Year) + endD.Month - startD.Month;
    }

    private List<(long ptId, int hokenId)> GetPtCondition(List<PtInfInputItem> sourcePt, int printSort, bool exportCSV = false)
    {
        List<(long ptId, int hokenId)> ptCoditions = new();
        var listPt = GetPtListBySort(sourcePt, printSort, exportCSV);
        foreach (var item in listPt)
        {
            ptCoditions.Add((item.PtId, 0));
        }
        return ptCoditions;
    }

    private List<PtInfInputItem> GetPtListBySort(List<PtInfInputItem> sourcePt, int printSort, bool exportCSV = false)
    {
        if (printSort == 0)
        {
            sourcePt = sourcePt.OrderBy(item => item.PtNum).ToList();
        }
        else if (printSort == 1)
        {
            sourcePt = sourcePt.OrderBy(item => item.KanaName).ToList();
        }
        if (exportCSV) return sourcePt;
        return sourcePt.GroupBy(p => new { p.PtNum, p.HokenId }).Select(g => g.First()).ToList();
    }
    #endregion
    #endregion

    public CommonExcelReportingModel ExportCsv(int hpId, string menuName, int menuId, int timeFrom, int timeTo, int? monthFrom = 0, int? monthTo = 0, int? dateFrom = 0, int? dateTo = 0, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0, bool? isPutColName = false, CoFileType? coFileType = null)
    {
        return _staticsticExportCsvService.ExportCsv(hpId, menuName, menuId, timeFrom, timeTo, monthFrom, monthTo, dateFrom, dateTo, isPutTotalRow, tenkiDateFrom, tenkiDateTo, enableRangeFrom, enableRangeTo, ptNumFrom, ptNumTo, isPutColName, coFileType);
    }

    public (string message, CoPrintExitCode code, List<string> data) OutPutFileSta900(int hpId, List<string> outputColumns, bool isPutColName, CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf, CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf, CoSta9000KensaConf? kensaConf, List<long> ptIds, int sortOrder, int sortOrder2, int sortOrder3)
    {
        return _sta9000CoReportService.OutPutFile(hpId, outputColumns, isPutColName, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf, kensaConf, ptIds, sortOrder, sortOrder2, sortOrder3);
    }

    public CommonReportingRequestModel GetKensaHistoryPrint(int hpId, int userId, long ptId, int setId, int iraiDate, int startDate, int endDate, bool showAbnormalKbn, int sinDate)
    {
        return _kensaHistoryCoReportService.GetKensaHistoryPrintData(hpId, userId, ptId, setId, iraiDate, startDate, endDate, showAbnormalKbn, sinDate);
    }

    public CommonReportingRequestModel GetKensaResultMultiPrint(int hpId, int userId, long ptId, int setId, int startDate, int endDate, bool showAbnormalKbn, int sinDate)
    {
        return _kensaResultMultiCoReportService.GetKensaResultMultiPrintData(hpId, userId, ptId, setId, startDate, endDate, showAbnormalKbn, sinDate);
    }
}
