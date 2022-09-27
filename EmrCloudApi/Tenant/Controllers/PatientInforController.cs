﻿﻿using EmrCloudApi.Tenant.Presenters.CalculationInf;
using EmrCloudApi.Tenant.Presenters.PatientInformation;
using EmrCloudApi.Tenant.Requests.CalculationInf;
using EmrCloudApi.Tenant.Requests.PatientInfor;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.CalculationInf;
using EmrCloudApi.Tenant.Presenters.InsuranceList;
using EmrCloudApi.Tenant.Presenters.PatientInfor;
using EmrCloudApi.Tenant.Requests.Insurance;
using EmrCloudApi.Tenant.Responses.InsuranceList;
﻿﻿using EmrCloudApi.Tenant.Presenters.GroupInf;
using EmrCloudApi.Tenant.Requests.GroupInf;
using EmrCloudApi.Tenant.Responses.GroupInf;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using Microsoft.AspNetCore.Mvc;
using UseCase.CalculationInf;
using UseCase.Core.Sync;
using UseCase.GroupInf.GetList;
using UseCase.Insurance.GetList;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using UseCase.PatientGroupMst.GetList;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInformation.GetById;
using EmrCloudApi.Tenant.Responses.InsuranceMst;
using UseCase.InsuranceMst.Get;
using EmrCloudApi.Tenant.Presenters.InsuranceMst;
using EmrCloudApi.Tenant.Requests.InsuranceMst;
using UseCase.SearchHokensyaMst.Get;
using UseCase.PatientInfor.SearchAdvanced;
using UseCase.KohiHokenMst.Get;
using EmrCloudApi.Tenant.Presenters.KohiHokenMst;
using EmrCloudApi.Tenant.Responses.KohiHokenMst;
using EmrCloudApi.Tenant.Requests.KohiHokenMst;
using EmrCloudApi.Tenant.Responses.Insurance;
using UseCase.Insurance.ValidKohi;
using EmrCloudApi.Tenant.Presenters.Insurance;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientInforController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public PatientInforController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet("GetPatientById")]
        public ActionResult<Response<GetPatientInforByIdResponse>> GetPatientById([FromQuery] GetByIdRequest request)
        {
            var input = new GetPatientInforByIdInputData(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = _bus.Handle(input);

            var present = new GetPatientInforByIdPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetPatientInforByIdResponse>>(present.Result);
        }

        [HttpGet("GetListPatientGroup")]
        public ActionResult<Response<GetListGroupInfResponse>> GetListPatientGroup([FromQuery] GetListGroupInfRequest request)
        {
            var input = new GetListGroupInfInputData(request.HpId, request.PtId);
            var output = _bus.Handle(input);

            var present = new GetListGroupInfPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetListGroupInfResponse>>(present.Result);
        }

        [HttpGet("InsuranceListByPtId")]
        public ActionResult<Response<GetInsuranceListResponse>> GetInsuranceListByPtId([FromQuery] GetInsuranceListRequest request)
        {
            var input = new GetInsuranceListInputData(request.HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetInsuranceListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceListResponse>>(presenter.Result);
        }
        
        [HttpGet("SearchSimple")]
        public ActionResult<Response<SearchPatientInforSimpleResponse>> SearchSimple([FromQuery] SearchPatientInfoSimpleRequest request)
        {
            var input = new SearchPatientInfoSimpleInputData(request.Keyword, request.IsContainMode);
            var output = _bus.Handle(input);

            var present = new SearchPatientInfoSimplePresenter();
            present.Complete(output);

            return new ActionResult<Response<SearchPatientInforSimpleResponse>>(present.Result);
        }

        [HttpPost("SearchAdvanced")]
        public ActionResult<Response<SearchPatientInfoAdvancedResponse>> GetList([FromBody] SearchPatientInfoAdvancedRequest request)
        {
            var input = new SearchPatientInfoAdvancedInputData(request.SearchInput);
            var output = _bus.Handle(input);
            var presenter = new SearchPatientInfoAdvancedPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SearchPatientInfoAdvancedResponse>>(presenter.Result);
        }

        [HttpGet("GetListCalculationPatient")]
        public ActionResult<Response<CalculationInfResponse>> GetListCalculationPatient([FromQuery] CalculationInfRequest request)
        {
            var input = new CalculationInfInputData(request.HpId, request.PtId);
            var output = _bus.Handle(input);

            var present = new CalculationInfPresenter();
            present.Complete(output);

            return new ActionResult<Response<CalculationInfResponse>>(present.Result);
        }
        
        [HttpGet("GetPatientGroupMst")]
        public ActionResult<Response<GetListPatientGroupMstResponse>> GetPatientGroupMst()
        {
            var input = new GetListPatientGroupMstInputData();
            var output = _bus.Handle(input);

            var presenter = new GetListPatientGroupMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListPatientGroupMstResponse>>(presenter.Result);
        }

        [HttpGet("GetInsuranceMst")]
        public ActionResult<Response<GetInsuranceMstResponse>> GetInsuranceMst([FromQuery] GetInsuranceMstRequest request)
        {
            var input = new GetInsuranceMstInputData(request.HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetInsuranceMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceMstResponse>>(presenter.Result);
        }

        [HttpGet("SearchHokensyaMst")]
        public ActionResult<Response<SearchHokensyaMstResponse>> SearchHokensyaMst([FromQuery] SearchHokensyaMstRequest request)
        {
            var input = new SearchHokensyaMstInputData(request.HpId, request.PageIndex, request.PageCount, request.SinDate, request.Keyword);
            var output = _bus.Handle(input);

            var presenter = new SearchHokenMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchHokensyaMstResponse>>(presenter.Result);
        }

        [HttpGet("GetHokenMstByFutansyaNo")]
        public ActionResult<Response<GetKohiHokenMstResponse>> GetHokenMstByFutansyaNo([FromQuery] GetKohiHokenMstRequest request)
        {
            var input = new GetKohiHokenMstInputData(request.HpId, request.SinDate, request.FutansyaNo);
            var output = _bus.Handle(input);

            var presenter = new GetKohiHokenMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetKohiHokenMstResponse>>(presenter.Result);
        }

        [HttpPost("ValidateKohi")]
        public ActionResult<Response<ValidateKohiResponse>> ValidateKohi([FromBody] ValidateKohiRequest request)
        {
            var input = new ValidKohiInputData(request.SinDate, request.PtBirthday, request.IsKohiEmptyModel1, request.IsSelectedKohiMst1, request.SelectedKohiFutansyaNo1, request.SelectedKohiJyukyusyaNo1, 
                request.SelectedKohiTokusyuNo1, request.SelectedKohiStartDate1, request.SelectedKohiEndDate1, request.SelectedKohiConfirmDate1, request.SelectedKohiHokenNo1, request.SelectedKohiIsAddNew1, 
                request.SelectedKohiMstFutansyaCheckFlag1, request.SelectedKohiMstJyukyusyaCheckFlag1, request.SelectedKohiMstJyuKyuCheckDigit1, request.SelectedKohiMst1TokusyuCheckFlag1, 
                request.SelectedKohiMstStartDate1, request.SelectedKohiMstEndDate1, request.SelectedKohiMstDisplayText1, request.SelectedKohiMstHoubetu1, request.SelectedKohiMstCheckDigit1,
                request.SelectedKohiMstAgeStart1, request.SelectedKohiMstAgeEnd1, request.IsKohiEmptyModel2, request.IsSelectedKohiMst2, request.SelectedKohiFutansyaNo2, request.SelectedKohiJyukyusyaNo2, 
                request.SelectedKohiTokusyuNo2, request.SelectedKohiStartDate2, request.SelectedKohiEndDate2, request.SelectedKohiConfirmDate2, request.SelectedKohiHokenNo2, request.SelectedKohiIsAddNew2, 
                request.SelectedKohiMstFutansyaCheckFlag2, request.SelectedKohiMstJyukyusyaCheckFlag2, request.SelectedKohiMstJyuKyuCheckDigit2, request.SelectedKohiMst2TokusyuCheckFlag2, request.SelectedKohiMstStartDate2, 
                request.SelectedKohiMstEndDate2, request.SelectedKohiMstDisplayText2, request.SelectedKohiMstHoubetu2, request.SelectedKohiMstCheckDigit2, request.SelectedKohiMstAgeStart2, request.SelectedKohiMstAgeEnd2, 
                request.IsKohiEmptyModel3, request.IsSelectedKohiMst3, request.SelectedKohiFutansyaNo3, request.SelectedKohiJyukyusyaNo3, request.SelectedKohiTokusyuNo3, request.SelectedKohiStartDate3, request.SelectedKohiEndDate3,
                request.SelectedKohiConfirmDate3, request.SelectedKohiHokenNo3, request.SelectedKohiIsAddNew3, request.SelectedKohiMstFutansyaCheckFlag3, request.SelectedKohiMstJyukyusyaCheckFlag3,
                request.SelectedKohiMstJyuKyuCheckDigit3, request.SelectedKohiMst3TokusyuCheckFlag3, request.SelectedKohiMstStartDate3, request.SelectedKohiMstEndDate3, request.SelectedKohiMstDisplayText3, 
                request.SelectedKohiMstHoubetu3, request.SelectedKohiMstCheckDigit3, request.SelectedKohiMstAgeStart3, request.SelectedKohiMstAgeEnd3, request.IsKohiEmptyModel4, request.IsSelectedKohiMst4, 
                request.SelectedKohiFutansyaNo4, request.SelectedKohiJyukyusyaNo4, request.SelectedKohiTokusyuNo4, request.SelectedKohiStartDate4, request.SelectedKohiEndDate4, request.SelectedKohiConfirmDate4, request.SelectedKohiHokenNo4,
                request.SelectedKohiIsAddNew4, request.SelectedKohiMstFutansyaCheckFlag4, request.SelectedKohiMstJyukyusyaCheckFlag4, request.SelectedKohiMstJyuKyuCheckDigit4, request.SelectedKohiMst4TokusyuCheckFlag4, request.SelectedKohiMstStartDate4, 
                request.SelectedKohiMstEndDate4, request.SelectedKohiMstDisplayText4, request.SelectedKohiMstHoubetu4, request.SelectedKohiMstCheckDigit4, request.SelectedKohiMstAgeStart4, request.SelectedKohiMstAgeEnd4);
            var output = _bus.Handle(input);
            
            var presenter = new ValidateKohiPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidateKohiResponse>>(presenter.Result);
        }

    }
}