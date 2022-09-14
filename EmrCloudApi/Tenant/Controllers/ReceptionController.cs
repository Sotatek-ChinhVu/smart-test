﻿using Domain.Models.Reception;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.PatientRaiinKubun;
using EmrCloudApi.Tenant.Presenters.Reception;
using EmrCloudApi.Tenant.Presenters.ReceptionSameVisit;
using EmrCloudApi.Tenant.Presenters.ReceptionInsurance;
using EmrCloudApi.Tenant.Requests.PatientRaiinKubun;
using EmrCloudApi.Tenant.Requests.Reception;
using EmrCloudApi.Tenant.Requests.ReceptionInsurance;
using EmrCloudApi.Tenant.Requests.ReceptionSameVisit;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientRaiinKubun;
using EmrCloudApi.Tenant.Responses.Reception;
using EmrCloudApi.Tenant.Responses.ReceptionSameVisit;
using EmrCloudApi.Tenant.Responses.ReceptionInsurance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.PatientRaiinKubun.Get;
using UseCase.Reception.Get;
using UseCase.Reception.GetList;
using UseCase.ReceptionInsurance.Get;
using UseCase.ReceptionSameVisit.Get;
using UseCase.MaxMoney.GetMaxMoney;
using EmrCloudApi.Tenant.Requests.MaxMoney;
using EmrCloudApi.Tenant.Presenters.MaxMoney;
using EmrCloudApi.Tenant.Responses.MaxMoney;
using UseCase.MaxMoney.SaveMaxMoney;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public ReceptionController(UseCaseBus bus)
        {
            _bus = bus;
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

        [HttpGet("GetPatientRaiinKubun")]
        public ActionResult<Response<GetPatientRaiinKubunResponse>> GetPatientRaiinKubun([FromQuery] PatientRaiinKubunRequest request)
        {
            var input = new GetPatientRaiinKubunInputData(request.HpId, request.PtId, request.RaiinNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetPatientRaiinKubunPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPatientRaiinKubunResponse>>(presenter.Result);
        }

        [HttpGet("GetReceptionInsurance")]
        public ActionResult<Response<ReceptionInsuranceResponse>> GetReceptionInsurance([FromQuery] ReceptionInsuranceRequest request)
        {
            var input = new GetReceptionInsuranceInputData(request.HpId, request.PtId, request.SinDate, request.IsShowExpiredReception);
            var output = _bus.Handle(input);

            var presenter = new ReceptionInsurancePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ReceptionInsuranceResponse>>(presenter.Result);
        }

        [HttpGet("GetListSameVisit")]
        public ActionResult<Response<GetReceptionSameVisitResponse>> GetListSameVisit([FromQuery] GetReceptionSameVisitRequest request)
        {
            var input = new GetReceptionSameVisitInputData(request.HpId, request.PtId,  request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetReceptionSameVisitPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetReceptionSameVisitResponse>>(presenter.Result);
        }

        [HttpGet("GetMaxMoneyData")]
        public ActionResult<Response<GetMaxMoneyResponse>> GetMaxMoney([FromQuery] GetMaxMoneyRequest request)
        {
            var input = new GetMaxMoneyInputData(request.PtId, request.HpId, request.HokenKohiId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetMaxMoneyPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetMaxMoneyResponse>>(presenter.Result);
        }

        [HttpPost("SaveMaxMoneyData")]
        public ActionResult<Response<SaveMaxMoneyResponse>> SaveMaxMoney([FromBody] SaveMaxMoneyRequest request)
        {
            var input = new SaveMaxMoneyInputData(request.ListLimits, request.HpId, request.PtId, request.KohiId, request.SinYM);
            var output = _bus.Handle(input);

            var presenter = new SaveMaxMoneyPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveMaxMoneyResponse>>(presenter.Result);
        }
    }
}
