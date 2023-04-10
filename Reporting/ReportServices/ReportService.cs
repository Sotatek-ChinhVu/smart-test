using Infrastructure.Interfaces;
using Reporting.Byomei.Service;
using Reporting.DrugInfo.Model;
using Reporting.DrugInfo.Service;
using Reporting.Interface;
using Reporting.Karte1.Mapper;
using Reporting.Karte1.Service;
using Reporting.Mappers.Common;
using Reporting.NameLabel.DB;
using Reporting.NameLabel.Models;
using Reporting.OrderLabel.Model;
using Reporting.OrderLabel.Service;
using Reporting.Sijisen.Service;
using LabelCoPtInfModel = Reporting.NameLabel.Models.CoPtInfModel;

namespace Reporting.ReportServices;

public class ReportService : IReportService
{
    private readonly ITenantProvider _tenantProvider;
    private readonly IOrderLabelCoReportService _orderLabelCoReportService;
    private readonly IDrugInfoCoReportService _drugInfoCoReportService;

    public ReportService(ITenantProvider tenantProvider, IOrderLabelCoReportService orderLabelCoReportService, IDrugInfoCoReportService drugInfoCoReportService)
    {
        _tenantProvider = tenantProvider;
        _orderLabelCoReportService = orderLabelCoReportService;
        _drugInfoCoReportService = drugInfoCoReportService;
    }

    //Byomei
    public CommonReportingRequestModel GetByomeiReportingData(long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds)
    {
        var service = new ByomeiService(_tenantProvider);
        return service.GetByomeiReportingData(ptId, fromDay, toDay, tenkiIn, hokenIds);
    }

    //Karte1
    public Karte1Mapper GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
    {
        var service = new Karte1Service(_tenantProvider);
        return service.GetKarte1ReportingData(hpId, ptId, sinDate, hokenPid, tenkiByomei, syuByomei);
    }

    public CoNameLabelModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate)
    {
        using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
        {
            var finder = new CoNameLabelFinder(noTrackingDataContext);

            // 患者情報
            LabelCoPtInfModel ptInf = finder.FindPtInf(ptId);

            return new CoNameLabelModel(ptInf, kanjiName, sinDate);
        }
    }

    //Sijisen
    public CommonReportingRequestModel GetSijisenReportingData(int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr)
    {
        var sijisenService = new SijisenReportService(_tenantProvider);
        return sijisenService.GetSijisenReportingData(formType, ptId, sinDate, raiinNo, odrKouiKbns, printNoOdr);
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
}
