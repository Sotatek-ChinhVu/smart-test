using Helper.Enum;
using Reporting.Byomei.Service;
using Reporting.DrugInfo.Model;
using Reporting.DrugInfo.Service;
using Reporting.Karte1.Mapper;
using Reporting.Karte1.Service;
using Reporting.Mappers.Common;
using Reporting.MedicalRecordWebId.Service;
using Reporting.NameLabel.Service;
using Reporting.OrderLabel.Model;
using Reporting.OrderLabel.Service;
using Reporting.Receipt.Service;
using Reporting.Sijisen.Service;

namespace Reporting.ReportServices;

public class ReportService : IReportService
{
    private readonly IOrderLabelCoReportService _orderLabelCoReportService;
    private readonly IDrugInfoCoReportService _drugInfoCoReportService;
    private readonly ISijisenReportService _sijisenReportService;
    private readonly IByomeiService _byomeiService;
    private readonly IKarte1Service _karte1Service;
    private readonly INameLabelService _nameLabelService;
    private readonly IMedicalRecordWebIdReportService _medicalRecordWebIdReportService;
    private readonly IReceiptCoReportService _receiptCoReportService;

    public ReportService(IOrderLabelCoReportService orderLabelCoReportService, IDrugInfoCoReportService drugInfoCoReportService, ISijisenReportService sijisenReportService, IByomeiService byomeiService, IKarte1Service karte1Service, INameLabelService nameLabelService, IMedicalRecordWebIdReportService medicalRecordWebIdReportService, IReceiptCoReportService receiptCoReportService)
    {
        _orderLabelCoReportService = orderLabelCoReportService;
        _drugInfoCoReportService = drugInfoCoReportService;
        _sijisenReportService = sijisenReportService;
        _byomeiService = byomeiService;
        _karte1Service = karte1Service;
        _nameLabelService = nameLabelService;
        _medicalRecordWebIdReportService = medicalRecordWebIdReportService;
        _receiptCoReportService = receiptCoReportService;
    }

    //Byomei
    public CommonReportingRequestModel GetByomeiReportingData(long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds)
    {
        return _byomeiService.GetByomeiReportingData(ptId, fromDay, toDay, tenkiIn, hokenIds);
    }

    //Karte1
    public Karte1Mapper GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
    {
        return _karte1Service.GetKarte1ReportingData(hpId, ptId, sinDate, hokenPid, tenkiByomei, syuByomei);
    }

    //NameLabel
    public CommonReportingRequestModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate)
    {
        return _nameLabelService.GetNameLabelReportingData(ptId, kanjiName, sinDate);
    }

    //Sijisen
    public CommonReportingRequestModel GetSijisenReportingData(int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr)
    {
        return _sijisenReportService.GetSijisenReportingData(formType, ptId, sinDate, raiinNo, odrKouiKbns, printNoOdr);
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

    //MedicalRecordWebId
    public CommonReportingRequestModel GetMedicalRecordWebIdReportingData(int hpId, long ptId, int sinDate)
    {
        return _medicalRecordWebIdReportService.GetMedicalRecordWebIdReportingData(hpId, ptId, sinDate);
    }

    //Receipt Preview
    public CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId)
    {
        return _receiptCoReportService.GetReceiptData(hpId, ptId, seikyuYm, sinYm, hokenId);
    }
}
