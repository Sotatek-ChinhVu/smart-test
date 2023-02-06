﻿using EmrCloudApi.Constants;
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
using EmrCloudApi.Services;
using EmrCloudApi.Tenant.Presenters.Reception;
using EmrCloudApi.Tenant.Requests.Reception;
using EmrCloudApi.Tenant.Responses.Reception;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Insurance.ValidPatternExpirated;
using UseCase.MaxMoney.GetMaxMoney;
using UseCase.MaxMoney.SaveMaxMoney;
using UseCase.RaiinKbn.GetPatientRaiinKubunList;
using UseCase.Reception.Get;
using UseCase.Reception.GetDefaultSelectedTime;
using UseCase.Reception.GetLastRaiinInfs;
using UseCase.Reception.GetListRaiinInfs;
using UseCase.Reception.GetReceptionDefault;
using UseCase.Reception.InitDoctorCombo;
using UseCase.Reception.Insert;
using UseCase.Reception.ReceptionComment;
using UseCase.Reception.Update;
using UseCase.Reception.UpdateTimeZoneDayInf;
using UseCase.ReceptionInsurance.Get;
using UseCase.ReceptionSameVisit.Get;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class ReceptionController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IWebSocketService _webSocketService;
        public ReceptionController(UseCaseBus bus, IWebSocketService webSocketService, IUserService userService) : base(userService)
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
            var input = new GetReceptionInputData(request.RaiinNo);
            var output = _bus.Handle(input);

            var presenter = new GetReceptionPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetReceptionResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetLastRaiinInfs)]
        public ActionResult<Response<GetLastRaiinInfsResponse>> GetLastRaiinInfs([FromQuery] GetLastRaiinInfsRequest request)
        {
            var input = new GetLastRaiinInfsInputData(HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetLastRaiinInfsPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetLastRaiinInfsResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Insert)]
        public async Task<ActionResult<Response<InsertReceptionResponse>>> InsertAsync([FromBody] InsertReceptionRequest request)
        {
            var input = new InsertReceptionInputData(request.Dto, HpId, UserId);
            var output = _bus.Handle(input);
            if (output.Status == InsertReceptionStatus.Success)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged,
                    new CommonMessage { SinDate = input.Dto.Reception.SinDate, RaiinNo = output.RaiinNo });
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
                await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged,
                    new CommonMessage { SinDate = input.Dto.Reception.SinDate, RaiinNo = input.Dto.Reception.RaiinNo });
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
                                                           request.KohiHokenMstEndDate3, request.IsEmptyKohi4, request.IsKohiHaveHokenMst4, request.KohiConfirmDate4, request.KohiHokenMstDisplayTextMaster4, request.KohiHokenMstStartDate4, request.KohiHokenMstEndDate4, request.PatientInfBirthday, request.PatternHokenKbn);
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
            var input = new GetListRaiinInfsInputData(HpId, req.PtId, req.PageIndex, req.PageSize);
            var output = _bus.Handle(input);
            var presenter = new GetListRaiinInfPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
