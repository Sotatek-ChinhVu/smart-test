using Domain.Models.Diseases;
using EmrCloudApi.Constants;
using EmrCloudApi.Controller;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.Family;
using EmrCloudApi.Requests.MedicalExamination;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MedicalExamination;
using Microsoft.AspNetCore.Mvc;
using UseCase.CommonChecker;
using UseCase.Core.Sync;
using UseCase.Diseases.Upsert;
using UseCase.Family;
using UseCase.MedicalExamination.Calculate;
using UseCase.MedicalExamination.CheckedAfter327Screen;
using UseCase.MedicalExamination.CheckOpenTrialAccounting;
using UseCase.MedicalExamination.GetCheckDisease;
using UseCase.MedicalExamination.GetCheckedOrder;
using UseCase.MedicalExamination.GetContainerMst;
using UseCase.MedicalExamination.GetDefaultSelectedTime;
using UseCase.MedicalExamination.GetHeaderVistitDate;
using UseCase.MedicalExamination.GetHistoryFollowSindate;
using UseCase.MedicalExamination.GetKensaAuditTrailLog;
using UseCase.MedicalExamination.GetMaxAuditTrailLogDateForPrint;
using UseCase.MedicalExamination.GetOrdersForOneOrderSheetGroup;
using UseCase.MedicalExamination.GetOrderSheetGroup;
using UseCase.MedicalExamination.GetSinkouCountInMonth;
using UseCase.MedicalExamination.InitKbnSetting;
using UseCase.MedicalExamination.SaveKensaIrai;
using UseCase.MedicalExamination.SaveMedical;
using UseCase.MedicalExamination.SummaryInf;
using UseCase.MedicalExamination.TrailAccounting;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.CheckedSpecialItem;
using CalculateResponseOfMedical = EmrCloudApi.Responses.MedicalExamination.CalculateResponse;

namespace EmrCloudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalExaminationController : BaseParamControllerBase
    {
        private readonly IWebSocketService _webSocketService;
        private readonly UseCaseBus _bus;

        public MedicalExaminationController(UseCaseBus bus, IWebSocketService webSocketService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
            _webSocketService = webSocketService;
        }

        [HttpPost(ApiPath.GetCheckDiseases)]
        public ActionResult<Response<GetCheckDiseaseResponse>> GetCheckDiseases([FromBody] GetCheckDiseaseRequest request)
        {
            var input = new GetCheckDiseaseInputData(HpId, request.SinDate, request.TodayByomeis.Select(b => new PtDiseaseModel(
                    HpId,
                    b.PtId,
                    b.SeqNo,
                    b.ByomeiCd,
                    b.SortNo,
                    b.PrefixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                    b.Byomei,
                    b.StartDate,
                    b.TenkiKbn,
                    b.TenkiDate,
                    b.SyubyoKbn,
                    b.SikkanKbn,
                    b.NanByoCd,
                    b.IsNodspRece,
                    b.IsNodspKarte,
                    b.IsDeleted,
                    b.Id,
                    b.IsImportant,
                    0,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    b.HokenPid,
                    b.HosokuCmt,
                    0,
                    0
                )).ToList(), request.TodayOdrs.Select(
                    o => new OdrInfItemInputData(
                            HpId,
                            o.RaiinNo,
                            o.RpNo,
                            o.RpEdaNo,
                            o.PtId,
                            o.SinDate,
                            o.HokenPid,
                            o.OdrKouiKbn,
                            o.RpName,
                            o.InoutKbn,
                            o.SikyuKbn,
                            o.SyohoSbt,
                            o.SanteiKbn,
                            o.TosekiKbn,
                            o.DaysCnt,
                            o.SortNo,
                            o.Id,
                            o.OdrDetails.Select(
                                    od => new OdrInfDetailItemInputData(
                                            HpId,
                                            od.RaiinNo,
                                            od.RpNo,
                                            od.RpEdaNo,
                                            od.RowNo,
                                            od.PtId,
                                            od.SinDate,
                                            od.SinKouiKbn,
                                            od.ItemCd,
                                            od.ItemName,
                                            od.Suryo,
                                            od.UnitName,
                                            od.UnitSbt,
                                            od.TermVal,
                                            od.KohatuKbn,
                                            od.SyohoKbn,
                                            od.SyohoLimitKbn,
                                            od.DrugKbn,
                                            od.YohoKbn,
                                            od.Kokuji1,
                                            od.Kokuji2,
                                            od.IsNodspRece,
                                            od.IpnCd,
                                            od.IpnName,
                                            od.JissiKbn,
                                            od.JissiDate,
                                            od.JissiId,
                                            od.JissiMachine,
                                            od.ReqCd,
                                            od.Bunkatu,
                                            od.CmtName,
                                            od.CmtOpt,
                                            od.FontColor,
                                            od.CommentNewline
                                        )
                                ).ToList(),
                            o.IsDeleted
                        )
                ).ToList());
            var output = _bus.Handle(input);

            var presenter = new GetCheckDiseasePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetCheckDiseaseResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetInfCheckedSpecialItem)]
        public ActionResult<Response<CheckedSpecialItemResponse>> GetInfCheckedSpecialItem([FromBody] CheckedSpecialItemRequest request)
        {
            var input = new CheckedSpecialItemInputData(HpId, UserId, request.PtId, request.SinDate, request.IBirthDay, request.CheckAge, request.RaiinNo, request.OdrInfs.Select(
                o => new OdrInfItemInputData(
                            HpId,
                            o.RaiinNo,
                            o.RpNo,
                            o.RpEdaNo,
                            o.PtId,
                            o.SinDate,
                            o.HokenPid,
                            o.OdrKouiKbn,
                            o.RpName,
                            o.InoutKbn,
                            o.SikyuKbn,
                            o.SyohoSbt,
                            o.SanteiKbn,
                            o.TosekiKbn,
                            o.DaysCnt,
                            o.SortNo,
                            o.Id,
                            o.OdrDetails.Select(
                                    od => new OdrInfDetailItemInputData(
                                            HpId,
                                            od.RaiinNo,
                                            od.RpNo,
                                            od.RpEdaNo,
                                            od.RowNo,
                                            od.PtId,
                                            od.SinDate,
                                            od.SinKouiKbn,
                                            od.ItemCd,
                                            od.ItemName,
                                            od.Suryo,
                                            od.UnitName,
                                            od.UnitSbt,
                                            od.TermVal,
                                            od.KohatuKbn,
                                            od.SyohoKbn,
                                            od.SyohoLimitKbn,
                                            od.DrugKbn,
                                            od.YohoKbn,
                                            od.Kokuji1,
                                            od.Kokuji2,
                                            od.IsNodspRece,
                                            od.IpnCd,
                                            od.IpnName,
                                            od.JissiKbn,
                                            od.JissiDate,
                                            od.JissiId,
                                            od.JissiMachine,
                                            od.ReqCd,
                                            od.Bunkatu,
                                            od.CmtName,
                                            od.CmtOpt,
                                            od.FontColor,
                                            od.CommentNewline
                                        )
                                ).ToList(),
                            o.IsDeleted
                        )
                ).ToList(),
                request.CheckedOrderItems.Select(
                    c => new CheckedSpecialItemOrderItem(
                            c.CheckingType,
                            c.Santei,
                            c.CheckingContent,
                            c.ItemCd,
                            c.SinKouiKbn,
                            c.ItemName,
                            c.InOutKbn
                        )
                ).ToList(),
                new KarteItemInputData(
                    request.KarteInf.HpId,
                    request.KarteInf.RaiinNo,
                    request.KarteInf.PtId,
                    request.KarteInf.SinDate,
                    request.KarteInf.Text,
                    request.KarteInf.IsDeleted,
                    request.KarteInf.RichText),
                request.EnabledInputCheck,
                request.EnabledCommentCheck
                );
            var output = _bus.Handle(input);

            var presenter = new CheckedSpecialItemPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckedSpecialItemResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.InitKbnSetting)]
        public ActionResult<Response<InitKbnSettingResponse>> InitKbnSetting([FromBody] InitKbnSettingRequest request)
        {
            var input = new InitKbnSettingInputData(HpId, request.WindowType, request.FrameId, request.IsEnableLoadDefaultVal, request.PtId, request.RaiinNo, request.SinDate, request.OdrInfs.Select(
                o => new OdrInfItemInputData(
                            HpId,
                            o.RaiinNo,
                            o.RpNo,
                            o.RpEdaNo,
                            o.PtId,
                            o.SinDate,
                            o.HokenPid,
                            o.OdrKouiKbn,
                            o.RpName,
                            o.InoutKbn,
                            o.SikyuKbn,
                            o.SyohoSbt,
                            o.SanteiKbn,
                            o.TosekiKbn,
                            o.DaysCnt,
                            o.SortNo,
                            o.Id,
                            o.OdrDetails.Select(
                                    od => new OdrInfDetailItemInputData(
                                            HpId,
                                            od.RaiinNo,
                                            od.RpNo,
                                            od.RpEdaNo,
                                            od.RowNo,
                                            od.PtId,
                                            od.SinDate,
                                            od.SinKouiKbn,
                                            od.ItemCd,
                                            od.ItemName,
                                            od.Suryo,
                                            od.UnitName,
                                            od.UnitSbt,
                                            od.TermVal,
                                            od.KohatuKbn,
                                            od.SyohoKbn,
                                            od.SyohoLimitKbn,
                                            od.DrugKbn,
                                            od.YohoKbn,
                                            od.Kokuji1,
                                            od.Kokuji2,
                                            od.IsNodspRece,
                                            od.IpnCd,
                                            od.IpnName,
                                            od.JissiKbn,
                                            od.JissiDate,
                                            od.JissiId,
                                            od.JissiMachine,
                                            od.ReqCd,
                                            od.Bunkatu,
                                            od.CmtName,
                                            od.CmtOpt,
                                            od.FontColor,
                                            od.CommentNewline
                                        )
                                ).ToList(),
                            o.IsDeleted
                        )
                ).ToList()
                );
            var output = _bus.Handle(input);

            var presenter = new InitKbnSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<InitKbnSettingResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetCheckedOrder)]
        public ActionResult<Response<GetCheckedOrderResponse>> GetCheckedOrder([FromBody] GetCheckedOrderRequest request)
        {
            var input = new GetCheckedOrderInputData(HpId, UserId, request.SinDate, request.HokenId, request.HokenPid, request.PtId, request.IBirthDay, request.RaiinNo, request.SyosaisinKbn, request.OyaRaiinNo, request.TantoId, request.PrimaryDoctor, request.EnabledSanteiCheck, request.OdrInfItems, request.DiseaseItems);
            var output = _bus.Handle(input);
            var presenter = new GetCheckedOrderPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetCheckedOrderResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CheckedAfter327Screen)]
        public ActionResult<Response<CheckedAfter327ScreenResponse>> CheckedAfter327Screen([FromBody] CheckedAfter327ScreenRequest request)
        {
            var input = new CheckedAfter327ScreenInputData(HpId, request.PtId, request.SinDate, request.CheckedOrderModels, request.IsTokysyoOrder, request.IsTokysyosenOrder);
            var output = _bus.Handle(input);
            var presenter = new CheckedAfter327ScreenPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CheckedAfter327ScreenResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.OrderRealtimeChecker)]
        public ActionResult<Response<OrderRealtimeCheckerResponse>> OrderRealtimeChecker([FromBody] OrderRealtimeCheckerRequest request)
        {
            var input = new GetOrderCheckerInputData(request.PtId, HpId, request.SinDay, request.CurrentListOdr, request.ListCheckingOrder, request.SpecialNoteItem, request.PtDiseaseModels, request.FamilyItems, request.IsDataOfDb, request.RealTimeCheckerCondition);
            var output = _bus.Handle(input);
            var presenter = new OrderRealtimeCheckerPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<OrderRealtimeCheckerResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSummaryInf)]
        public ActionResult<Response<SummaryInfResponse>> GetSummaryInf([FromQuery] SummaryInfRequest request)
        {
            var input = new SummaryInfInputData(HpId, request.PtId, request.SinDate, request.RaiinNo, UserId, request.InfoType);
            var output = _bus.Handle(input);
            var presenter = new SummaryInfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SummaryInfResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetMaxAuditTrailLogDateForPrint)]
        public ActionResult<Response<GetMaxAuditTrailLogDateForPrintResponse>> GetMaxAuditTrailLogDateForPrint([FromQuery] GetMaxAuditTrailLogDateForPrintRequest request)
        {
            var input = new GetMaxAuditTrailLogDateForPrintInputData(HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);
            var presenter = new GetMaxAuditTrailLogDateForPrintPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetMaxAuditTrailLogDateForPrintResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDefaultSelectedTime)]
        public ActionResult<Response<GetDefaultSelectedTimeResponse>> GetDefaultSelectedTime([FromQuery] GetDefaultSelectedTimeRequest request)
        {
            var input = new GetDefaultSelectedTimeInputData(request.DayOfWeek, request.UketukeTime, request.FromOutOfSystem, HpId, request.SinDate, request.BirthDay);
            var output = _bus.Handle(input);
            var presenter = new GetDefaultSelectedTimePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetDefaultSelectedTimeResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Calculate)]
        public ActionResult<Response<CalculateResponseOfMedical>> Calculate([FromBody] CalculateRequest request)
        {
            var input = new CalculateInputData(request.RaiinNo, request.FromRcCheck, request.IsSagaku, HpId, request.PtId, request.SinDate, request.SeikyuUp, request.Prefix, UserId);
            var output = _bus.Handle(input);
            var presenter = new CalculatePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CalculateResponseOfMedical>>(presenter.Result);
        }

        [HttpPost("SaveMedical")]
        public async Task<ActionResult<Response<SaveMedicalResponse>>> SaveMedical([FromBody] SaveMedicalRequest request)
        {
            var familyList = ConvertToFamilyInputItem(request.FamilyList);
            var input = new SaveMedicalInputData(HpId, request.PtId, request.RaiinNo, request.SinDate, request.SyosaiKbn, request.JikanKbn, request.HokenPid, request.SanteiKbn, request.TantoId, request.KaId, request.UketukeTime, request.SinStartTime, request.SinEndTime, request.Status, request.OdrInfs.Select(
                    o => new OdrInfItemInputData(
                            HpId,
                            o.RaiinNo,
                            o.RpNo,
                            o.RpEdaNo,
                            o.PtId,
                            o.SinDate,
                            o.HokenPid,
                            o.OdrKouiKbn,
                            o.RpName,
                            o.InoutKbn,
                            o.SikyuKbn,
                            o.SyohoSbt,
                            o.SanteiKbn,
                            o.TosekiKbn,
                            o.DaysCnt,
                            o.SortNo,
                            o.Id,
                            o.OdrDetails.Select(
                                    od => new OdrInfDetailItemInputData(
                                            HpId,
                                            od.RaiinNo,
                                            od.RpNo,
                                            od.RpEdaNo,
                                            od.RowNo,
                                            od.PtId,
                                            od.SinDate,
                                            od.SinKouiKbn,
                                            od.ItemCd,
                                            od.ItemName,
                                            od.Suryo,
                                            od.UnitName,
                                            od.UnitSbt,
                                            od.TermVal,
                                            od.KohatuKbn,
                                            od.SyohoKbn,
                                            od.SyohoLimitKbn,
                                            od.DrugKbn,
                                            od.YohoKbn,
                                            od.Kokuji1,
                                            od.Kokuji2,
                                            od.IsNodspRece,
                                            od.IpnCd,
                                            od.IpnName,
                                            od.JissiKbn,
                                            od.JissiDate,
                                            od.JissiId,
                                            od.JissiMachine,
                                            od.ReqCd,
                                            od.Bunkatu,
                                            od.CmtName,
                                            od.CmtOpt,
                                            od.FontColor,
                                            od.CommentNewline
                                        )
                                ).ToList(),
                            o.IsDeleted
                        )
                ).ToList(),
                new KarteItemInputData(
                    HpId,
                    request.KarteItem.RaiinNo,
                    request.KarteItem.PtId,
                    request.KarteItem.SinDate,
                    request.KarteItem.Text,
                    request.KarteItem.IsDeleted,
                    request.KarteItem.RichText),
                UserId,
                request.IsSagaku,
                request.AutoSaveKensaIrai,
                new FileItemInputItem(request.FileItem.IsUpdateFile, request.FileItem.ListFileItems),
                familyList,
                request.NextOrderItems,
                request.SpecialNoteItem,
                request.DiseaseListItems.Select(r => new UpsertPtDiseaseListInputItem(
                                                    r.Id,
                                                    r.PtId,
                                                    r.SortNo,
                                                    r.PrefixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                                                    r.SuffixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                                                    r.Byomei,
                                                    r.StartDate,
                                                    r.TenkiKbn,
                                                    r.TenkiDate,
                                                    r.SyubyoKbn,
                                                    r.SikkanKbn,
                                                    r.NanByoCd,
                                                    r.HosokuCmt,
                                                    r.HokenPid,
                                                    r.IsNodspRece,
                                                    r.IsNodspKarte,
                                                    r.SeqNo,
                                                    r.IsImportant,
                                                    r.IsDeleted,
                                                    r.ByomeiCd,
                                                    HpId
                                )).ToList(),
                request.FlowSheetItems,
                request.Monshin,
                request.StateChanged
            );
            var output = _bus.Handle(input);

            if (output.Status == SaveMedicalStatus.Successed)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.ReceptionInfos, output.SameVisitList));
            }

            var presenter = new SaveMedicalPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveMedicalResponse>>(presenter.Result);
        }

        private List<FamilyItem> ConvertToFamilyInputItem(List<FamilyRequestItem> listFamilyRequest)
        {
            var result = listFamilyRequest.Select(family => new FamilyItem(
                                                                family.FamilyId,
                                                                family.PtId,
                                                                family.ZokugaraCd,
                                                                family.FamilyPtId,
                                                                family.Name,
                                                                family.KanaName,
                                                                family.Sex,
                                                                family.Birthday,
                                                                family.IsDead,
                                                                family.IsSeparated,
                                                                family.Biko,
                                                                family.SortNo,
                                                                family.IsDeleted,
                                                                family.PtFamilyRekiList
                                                                      .Select(reki => new FamilyRekiItem(
                                                                                          reki.Id,
                                                                                          reki.ByomeiCd,
                                                                                          reki.Byomei,
                                                                                          reki.Cmt,
                                                                                          reki.SortNo,
                                                                                          reki.IsDeleted))
                                                                      .ToList(),
                                                                 family.IsRevertItem))
                                          .ToList();
            return result;
        }


        [HttpGet(ApiPath.GetHistoryFollowSinDate)]
        public ActionResult<Response<GetHistoryFollowSindateResponse>> GetHistoryFollowSinDate([FromQuery] GetHistoryFollowSindateRequest request)
        {
            var input = new GetHistoryFollowSindateInputData(request.PtId, HpId, UserId, request.SinDate, request.DeleteConditon, request.RaiinNo, request.Flag, request.IsShowApproval);
            var output = _bus.Handle(input);

            var presenter = new GetHistoryFollowSindatePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetHistoryFollowSindateResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetOrderSheetGroup)]
        public ActionResult<Response<GetOrderSheetGroupResponse>> GetOrderSheetGroup([FromQuery] GetOrderSheetGroupRequest request)
        {
            var input = new GetOrderSheetGroupInputData(HpId, UserId, request.PtId, request.SelectDefOnLoad);
            var output = _bus.Handle(input);
            var presenter = new GetOrderSheetGroupPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetOrderSheetGroupResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetOrdersForOneOrderSheetGroup)]
        public ActionResult<Response<GetOrdersForOneOrderSheetGroupResponse>> GetOrdersForOneOrderSheetGroup([FromQuery] GetOrdersForOneOrderSheetGroupRequest request)
        {
            var input = new GetOrdersForOneOrderSheetGroupInputData(request.PtId, HpId, request.SinDate, request.OdrKouiKbn, request.GrpKouiKbn, request.Offset, request.Limit);
            var output = _bus.Handle(input);
            var presenter = new GetOrdersForOneOrderSheetGroupPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetOrdersForOneOrderSheetGroupResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.TrialAccounting)]
        public ActionResult<Response<GetTrialAccountingResponse>> TrialAccounting([FromBody] GetTrialAccountingRequest request)
        {
            var input = new GetTrialAccountingInputData(HpId, UserId, request.PtId, request.SinDate, request.RaiinNo, request.OdrInfItems);
            var output = _bus.Handle(input);
            var presenter = new GetTrialAccountingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTrialAccountingResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CheckTrialAccounting)]
        public ActionResult<Response<CheckOpenTrialAccountingResponse>> CheckTrialAccounting([FromBody] CheckOpenTrialAccountingRequest request)
        {
            var input = new CheckOpenTrialAccountingInputData(HpId, request.PtId, request.RaiinNo, request.SinDate, request.SyosaiKbn, request.AllOdrInfItem.Select(a => new Tuple<string, string>(a.ItemCd, a.ItemName)).ToList(), request.OdrInfHokenPid);
            var output = _bus.Handle(input);
            var presenter = new CheckOpenTrialAccountingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CheckOpenTrialAccountingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetKensaAuditTrailLog)]
        public ActionResult<Response<GetKensaAuditTrailLogResponse>> GetKensaAuditTrailLog([FromQuery] GetKensaAuditTrailLogRequest request)
        {
            var input = new GetKensaAuditTrailLogInputData(HpId, request.RaiinNo, request.SinDate, request.PtId, request.EventCd);
            var output = _bus.Handle(input);
            var presenter = new GetKensaAuditLogTrailPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetKensaAuditTrailLogResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetContainerMst)]
        public ActionResult<Response<GetContainerMstResponse>> GetContainerMst([FromBody] GetContainerMstRequest request)
        {
            var input = new GetContainerMstInputData(HpId, request.SinDate, request.DefaultChecked, request.OdrInfItems);
            var output = _bus.Handle(input);
            var presenter = new GetContainerMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetContainerMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSinkouCountInMonth)]
        public ActionResult<Response<GetSinkouCountInMonthResponse>> GetSinkouCountInMonth([FromQuery] GetSinkouCountInMonthRequest request)
        {
            var input = new GetSinkouCountInMonthInputData(HpId, request.SinDate, request.ItemCd, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new GetSinKouCountInMonthPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetSinkouCountInMonthResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetHeaderVistitDate)]
        public ActionResult<Response<GetHeaderVistitDateResponse>> GetHeaderVistitDate([FromQuery] GetHeaderVistitDateRequest request)
        {
            var input = new GetHeaderVistitDateInputData(HpId, UserId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetHeaderVistitDatePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetHeaderVistitDateResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveKensaIrai)]
        public ActionResult<Response<SaveKensaIraiResponse>> SaveKensaIrai([FromBody] SaveKensaIraiRequest request)
        {
            var input = new SaveKensaIraiInputData(HpId, UserId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);
            var presenter = new SaveKensaIraiPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveKensaIraiResponse>>(presenter.Result);
        }
    }
}
