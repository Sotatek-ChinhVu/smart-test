using Reporting.Accounting.Model;
using Reporting.Accounting.Model.Output;
using Reporting.Accounting.Service;
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
using Reporting.OutDrug.Model.Output;
using Reporting.OutDrug.Service;
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
    private readonly IOutDrugCoReportService _outDrugCoReportService;
    private readonly IAccountingCoReportService _accountingCoReportService;

    public ReportService(IOrderLabelCoReportService orderLabelCoReportService, IDrugInfoCoReportService drugInfoCoReportService, ISijisenReportService sijisenReportService, IByomeiService byomeiService, IKarte1Service karte1Service, INameLabelService nameLabelService, IMedicalRecordWebIdReportService medicalRecordWebIdReportService, IOutDrugCoReportService outDrugCoReportService, IAccountingCoReportService accountingCoReportService)
    {
        _orderLabelCoReportService = orderLabelCoReportService;
        _drugInfoCoReportService = drugInfoCoReportService;
        _sijisenReportService = sijisenReportService;
        _byomeiService = byomeiService;
        _karte1Service = karte1Service;
        _nameLabelService = nameLabelService;
        _medicalRecordWebIdReportService = medicalRecordWebIdReportService;
        _outDrugCoReportService = outDrugCoReportService;
        _accountingCoReportService = accountingCoReportService;
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

    /// <summary>
    /// OutDrug
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="sinDate"></param>
    /// <param name="raiinNo"></param>
    /// <returns></returns>
    public CoOutDrugReportingOutputData GetOutDrugReportingData(int hpId, long ptId, int sinDate, long raiinNo)
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
    public AccountingOutputModel GetAccountingReportingData(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId = 0, int miseisanKbn = 0, int saiKbn = 0, int misyuKbn = 0, int seikyuKbn = 1, int hokenKbn = 0, bool hokenSeikyu = false, bool jihiSeikyu = false, bool nyukinBase = false, int hakkoDay = 0, string memo = "", int printType = 0, string formFileName = "")
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
    public AccountingOutputModel GetAccountingReportingData(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions, int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, int hakkoDay, string memo, string formFileName)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, startDate, endDate, ptConditions, grpConditions, sort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hakkoDay, memo, formFileName);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="coAccountingParamModels"></param>
    /// <returns></returns>
    public AccountingOutputModel GetAccountingReportingData(int hpId, List<CoAccountingParamModel> coAccountingParamModels)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, coAccountingParamModels);
    }
}
