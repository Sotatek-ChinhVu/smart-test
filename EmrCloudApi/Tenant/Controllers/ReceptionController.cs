using Domain.Models.Reception;
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
using UseCase.Insurance.ValidPatternExpirated;

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

        [HttpPost("CheckPatternSelectedExpirated")]
        public ActionResult<Response<ValidPatternExpiratedResponse>> CheckPatternSelectedExpirated([FromBody] ValidPatternExpiratedRequest request)
        {
            var input = new ValidPatternExpiratedInputData(request.HpId, request.PtId, request.SinDate, request.PatternHokenPid, request.PatternIsExpirated, request.HokenInfIsJihi, request.HokenInfIsNoHoken, request.PatternConfirmDate,
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
    }
}
