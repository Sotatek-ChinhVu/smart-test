using EmrCloudApi.Constants;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.MaxMoney;
using EmrCloudApi.Presenters.RaiinKubun;
using EmrCloudApi.Presenters.Reception;
using EmrCloudApi.Presenters.ReceptionInsurance;
using EmrCloudApi.Presenters.ReceptionSameVisit;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.MaxMoney;
using EmrCloudApi.Requests.RaiinKubun;
using EmrCloudApi.Requests.Reception;
using EmrCloudApi.Requests.ReceptionInsurance;
using EmrCloudApi.Requests.ReceptionSameVisit;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MaxMoney;
using EmrCloudApi.Responses.RaiinKubun;
using EmrCloudApi.Responses.Reception;
using EmrCloudApi.Responses.ReceptionInsurance;
using EmrCloudApi.Responses.ReceptionSameVisit;
using EmrCloudApi.Tenant.Presenters.Reception;
using EmrCloudApi.Tenant.Requests.Reception;
using EmrCloudApi.Tenant.Responses.Reception;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Insurance.ValidPatternExpirated;
using UseCase.MaxMoney.GetMaxMoney;
using UseCase.MaxMoney.SaveMaxMoney;
using UseCase.RaiinKbn.GetPatientRaiinKubunList;
using UseCase.Reception.Delete;
using UseCase.Reception.Get;
using UseCase.Reception.GetDefaultSelectedTime;
using UseCase.Reception.GetHpInf;
using UseCase.Reception.GetLastKarute;
using UseCase.Reception.GetLastRaiinInfs;
using UseCase.Reception.GetListRaiinInf;
using UseCase.Reception.GetOutDrugOrderList;
using UseCase.Reception.GetRaiinInfBySinDate;
using UseCase.Reception.GetRaiinListWithKanInf;
using UseCase.Reception.GetReceptionDefault;
using UseCase.Reception.GetYoyakuRaiinInf;
using UseCase.Reception.InitDoctorCombo;
using UseCase.Reception.Insert;
using UseCase.Reception.ReceptionComment;
using UseCase.Reception.RevertDeleteNoRecept;
using UseCase.Reception.Update;
using UseCase.Reception.UpdateTimeZoneDayInf;
using UseCase.ReceptionInsurance.Get;
using UseCase.ReceptionSameVisit.Get;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class ReceptionController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IWebSocketService _webSocketService;
        public ReceptionController(UseCaseBus bus, IWebSocketService webSocketService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
            _webSocketService = webSocketService;
        }

        [HttpGet(ApiPath.Get + "ReceptionComment")]
        public ActionResult<Response<GetReceptionCommentResponse>> GetReceptionComment([FromQuery] GetReceptionCommentRequest request)
        {
            var input = new GetReceptionCommentInputData(HpId, request.RaiinNo);
            var output = _bus.Handle(input);
            var presenter = new GetReceptionCommentPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.Get)]
        public ActionResult<Response<GetReceptionResponse>> Get([FromQuery] GetReceptionRequest request)
        {
            var input = new GetReceptionInputData(HpId, request.RaiinNo, request.Flag);
            var output = _bus.Handle(input);

            var presenter = new GetReceptionPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetReceptionResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetLastRaiinInfs)]
        public ActionResult<Response<GetLastRaiinInfsResponse>> GetLastRaiinInfs([FromQuery] GetLastRaiinInfsRequest request)
        {
            var input = new GetLastRaiinInfsInputData(HpId, request.PtId, request.SinDate, request.IsLastVisit);
            var output = _bus.Handle(input);

            var presenter = new GetLastRaiinInfsPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetLastRaiinInfsResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetOutDrugOrderList)]
        public ActionResult<Response<GetOutDrugOrderListResponse>> GetOutDrugOrderList([FromQuery] GetOutDrugOrderListRequest request)
        {
            var input = new GetOutDrugOrderListInputData(HpId, request.IsPrintPrescription, request.IsPrintAccountingCard, request.FromDate, request.ToDate, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetOutDrugOrderListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetOutDrugOrderListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Insert)]
        public async Task<ActionResult<Response<InsertReceptionResponse>>> InsertAsync([FromBody] InsertReceptionRequest request)
        {
            var input = new InsertReceptionInputData(request.Dto, HpId, UserId);
            var output = _bus.Handle(input);
            if (output.Status == InsertReceptionStatus.Success)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.ReceptionInfos, output.SameVisitList));
            }

            var presenter = new InsertReceptionPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<InsertReceptionResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Update)]
        public async Task<ActionResult<Response<UpdateReceptionResponse>>> UpdateAsync([FromBody] UpdateReceptionRequest request)
        {
            var input = new UpdateReceptionInputData(request.Dto, HpId, UserId);
            var output = _bus.Handle(input);
            if (output.Status == UpdateReceptionStatus.Success)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.ReceptionInfos, output.SameVisitList));
            }

            var presenter = new UpdateReceptionPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpdateReceptionResponse>>(presenter.Result);
        }

        [HttpGet("GetPatientRaiinKubun")]
        public ActionResult<Response<GetPatientRaiinKubunListResponse>> GetPatientRaiinKubun([FromQuery] GetPatientRaiinKubunListRequest request)
        {
            var input = new GetPatientRaiinKubunListInputData(HpId, request.PtId, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetPatientRaiinKubunListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPatientRaiinKubunListResponse>>(presenter.Result);
        }

        [HttpGet("GetReceptionInsurance")]
        public ActionResult<Response<ReceptionInsuranceResponse>> GetReceptionInsurance([FromQuery] ReceptionInsuranceRequest request)
        {
            var input = new GetReceptionInsuranceInputData(HpId, request.PtId, request.SinDate, request.IsShowExpiredReception);
            var output = _bus.Handle(input);

            var presenter = new ReceptionInsurancePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ReceptionInsuranceResponse>>(presenter.Result);
        }

        [HttpGet("GetListSameVisit")]
        public ActionResult<Response<GetReceptionSameVisitResponse>> GetListSameVisit([FromQuery] GetReceptionSameVisitRequest request)
        {
            var input = new GetReceptionSameVisitInputData(HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetReceptionSameVisitPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetReceptionSameVisitResponse>>(presenter.Result);
        }

        [HttpGet("GetMaxMoneyData")]
        public ActionResult<Response<GetMaxMoneyResponse>> GetMaxMoney([FromQuery] GetMaxMoneyRequest request)
        {
            var input = new GetMaxMoneyInputData(request.PtId, HpId, request.HokenKohiId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetMaxMoneyPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetMaxMoneyResponse>>(presenter.Result);
        }

        [HttpPost("SaveMaxMoneyData")]
        public ActionResult<Response<SaveMaxMoneyResponse>> SaveMaxMoney([FromBody] SaveMaxMoneyRequest request)
        {
            var input = new SaveMaxMoneyInputData(request.ListLimits, HpId, request.PtId, request.KohiId, request.SinYM, UserId);
            var output = _bus.Handle(input);

            var presenter = new SaveMaxMoneyPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveMaxMoneyResponse>>(presenter.Result);
        }

        [HttpPost("CheckPatternSelectedExpirated")]
        public ActionResult<Response<ValidPatternExpiratedResponse>> CheckPatternSelectedExpirated([FromBody] ValidPatternExpiratedRequest request)
        {
            var input = new ValidPatternExpiratedInputData(HpId, request.PtId, request.SinDate, request.PatternHokenPid, request.PatternIsExpirated, request.HokenInfIsJihi, request.HokenInfIsNoHoken, request.PatternConfirmDate,
                                                           request.HokenInfStartDate, request.HokenInfEndDate, request.IsHaveHokenMst, request.HokenMstStartDate, request.HokenMstEndDate, request.HokenMstDisplayTextMaster, request.IsEmptyKohi1,
                                                           request.IsKohiHaveHokenMst1, request.KohiConfirmDate1, request.KohiHokenMstDisplayTextMaster1, request.KohiHokenMstStartDate1, request.KohiHokenMstEndDate1,
                                                           request.IsEmptyKohi2, request.IsKohiHaveHokenMst2, request.KohiConfirmDate2, request.KohiHokenMstDisplayTextMaster2, request.KohiHokenMstStartDate2,
                                                           request.KohiHokenMstEndDate2, request.IsEmptyKohi3, request.IsKohiHaveHokenMst3, request.KohiConfirmDate3, request.KohiHokenMstDisplayTextMaster3, request.KohiHokenMstStartDate3,
                                                           request.KohiHokenMstEndDate3, request.IsEmptyKohi4, request.IsKohiHaveHokenMst4, request.KohiConfirmDate4, request.KohiHokenMstDisplayTextMaster4, request.KohiHokenMstStartDate4, request.KohiHokenMstEndDate4, request.PatientInfBirthday, request.PatternHokenKbn
                                                           , request.SelectedHokenInfIsEmptyModel);
            var output = _bus.Handle(input);

            var presenter = new ValidPatternExpiratedPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidPatternExpiratedResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDataReceptionDefault)]
        public ActionResult<Response<GetReceptionDefaultResponse>> GetDataReceptionDefault([FromQuery] GetReceptionDefaultRequest request)
        {
            var input = new GetReceptionDefaultInputData(HpId, request.PtId, request.Sindate, request.DefaultDoctorSetting);
            var output = _bus.Handle(input);

            var presenter = new GetReceptionDefaultPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetReceptionDefaultResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDefaultSelectedTime)]
        public ActionResult<Response<GetDefaultSelectedTimeResponse>> GetDefaultSelectedTime([FromQuery] GetDefaultSelectedTimeRequest request)
        {
            var input = new GetDefaultSelectedTimeInputData(HpId, request.UketukeTime, request.SinDate, request.BirthDay);
            var output = _bus.Handle(input);

            var presenter = new GetDefaultSelectedTimePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDefaultSelectedTimeResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateTimeZoneDayInf)]
        public ActionResult<Response<UpdateTimeZoneDayInfResponse>> UpdateTimeZoneDayInf([FromBody] UpdateTimeZoneDayInfRequest request)
        {
            var input = new UpdateTimeZoneDayInfInputData(HpId, UserId, request.SinDate, request.CurrentTimeKbn, request.BeforeTimeKbn, request.UketukeTime);
            var output = _bus.Handle(input);

            var presenter = new UpdateTimeZoneDayInfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpdateTimeZoneDayInfResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.InitDoctorCombo)]
        public ActionResult<Response<InitDoctorComboResponse>> InitDoctorCombo([FromQuery] InitDoctorComboRequest request)
        {
            var input = new InitDoctorComboInputData(UserId, request.TantoId, request.PtId, HpId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new InitDoctorComboPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<InitDoctorComboResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetListRaiinInfResponse>> GetList([FromQuery] GetListRaiinInfRequest req)
        {
            var input = new GetListRaiinInfInputData(HpId, req.PtId, req.PageIndex, req.PageSize, req.IsDeleted, req.IsAll);
            var output = _bus.Handle(input);
            var presenter = new GetListRaiinInfPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.GetRaiinListWithKanInf)]
        public ActionResult<Response<GetRaiinListWithKanInfResponse>> GetList([FromQuery] GetRaiinListWithKanInfRequest request)
        {
            var input = new GetRaiinListWithKanInfInputData(HpId, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new GetRaiinListWithKanInfPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.Delete)]
        public async Task<ActionResult<Response<DeleteReceptionResponse>>> Delete([FromBody] DeleteReceptionRequest req)
        {
            var input = new DeleteReceptionInputData(HpId, req.PtId, req.SinDate, req.Flag, UserId, req.RaiinNos);
            var output = _bus.Handle(input);
            var deleteFirst = output.DeleteReceptionItems.FirstOrDefault();
            if (output.Status == DeleteReceptionStatus.Successed && deleteFirst != null)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.ReceptionInfos, output.SameVisitList));
            }

            var presenter = new DeleteReceptionPresenter();
            presenter.Complete(output);

            return Ok(presenter.Result);
        }

        [HttpGet(ApiPath.GetLastKarute)]
        public ActionResult<Response<GetLastKaruteResponse>> GetLastKarute([FromQuery] GetLastKaruteRequest request)
        {
            var input = new GetLastKaruteInputData(HpId, request.PtNum);
            var output = _bus.Handle(input);
            var presenter = new GetLastKarutePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetLastKaruteResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetYoyakuRaiinInf)]
        public ActionResult<Response<GetYoyakuRaiinInfResponse>> GetYoyakuRaiinInf([FromQuery] GetYoyakuRaiinInfRequest request)
        {
            var input = new GetYoyakuRaiinInfInputData(HpId, request.SinDate, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new GetYoyakuRaiinInfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetYoyakuRaiinInfResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetRaiinInfBySinDate)]
        public ActionResult<Response<GetRaiinInfBySinDateResponse>> GetRaiinInfBySinDate([FromQuery] GetRaiinInfBySinDateRequest request)
        {
            var input = new GetRaiinInfBySinDateInputData(HpId, request.SinDate, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new GetRaiinInfBySinDatePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetRaiinInfBySinDateResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetHpInf)]
        public ActionResult<Response<GetHpInfResponse>> GetHpInf([FromQuery] GetHpInfRequest request)
        {
            var input = new GetHpInfInputData(HpId, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetHpInfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetHpInfResponse>>(presenter.Result);
        }

        [HttpPut(ApiPath.RevertDeleteNoRecept)]
        public async Task<ActionResult<Response<RevertDeleteNoReceptResponse>>> RevertDeleteNoRecept(RevertDeleteNoReceptRequest request)
        {
            var input = new RevertDeleteNoReceptInputData(HpId, request.RaiinNo, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            if (output.Status == RevertDeleteNoReceptStatus.Success)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.receptionModel, new()));
            }

            var presenter = new RevertDeleteNoReceptPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<RevertDeleteNoReceptResponse>>(presenter.Result);
        }
    }
}
