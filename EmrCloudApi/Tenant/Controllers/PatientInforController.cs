using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.CalculationInf;
using EmrCloudApi.Tenant.Presenters.GroupInf;
using EmrCloudApi.Tenant.Presenters.HokenMst;
using EmrCloudApi.Tenant.Presenters.InsuranceList;
using EmrCloudApi.Tenant.Presenters.InsuranceMst;
using EmrCloudApi.Tenant.Presenters.KohiHokenMst;
using EmrCloudApi.Tenant.Presenters.PatientInfor;
using EmrCloudApi.Tenant.Presenters.PatientInformation;
using EmrCloudApi.Tenant.Requests.CalculationInf;
using EmrCloudApi.Tenant.Requests.GroupInf;
using EmrCloudApi.Tenant.Requests.HokenMst;
using EmrCloudApi.Tenant.Requests.Insurance;
using EmrCloudApi.Tenant.Requests.InsuranceMst;
using EmrCloudApi.Tenant.Requests.KohiHokenMst;
using EmrCloudApi.Tenant.Requests.PatientInfor;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.CalculationInf;
using EmrCloudApi.Tenant.Responses.GroupInf;
using EmrCloudApi.Tenant.Responses.HokenMst;
using EmrCloudApi.Tenant.Responses.InsuranceList;
using EmrCloudApi.Tenant.Responses.InsuranceMst;
using EmrCloudApi.Tenant.Responses.KohiHokenMst;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using Microsoft.AspNetCore.Mvc;
using UseCase.CalculationInf;
using UseCase.Core.Sync;
using UseCase.GroupInf.GetList;
using UseCase.HokenMst.GetDetail;
using UseCase.Insurance.GetList;
using UseCase.InsuranceMst.Get;
using UseCase.InsuranceMst.SaveHokenSyaMst;
using UseCase.KohiHokenMst.Get;
using UseCase.PatientGroupMst.GetList;
using UseCase.PatientGroupMst.SaveList;
using UseCase.PatientInfor.PatientComment;
using UseCase.PatientInfor.SearchAdvanced;
using UseCase.PatientInfor.SearchEmptyId;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInformation.GetById;
using UseCase.SearchHokensyaMst.Get;

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

        [HttpGet(ApiPath.Get + "PatientComment")]
        public ActionResult<Response<GetPatientCommentResponse>> GetList([FromQuery] GetPatientCommentRequest request)
        {
            var input = new GetPatientCommentInputData(request.HpId, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new GetPatientCommentPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
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

        [HttpPost(ApiPath.SavePatientGroupMst)]
        public ActionResult<Response<SaveListPatientGroupMstResponse>> SavePatientGroupMst([FromBody] SaveListPatientGroupMstRequest request)
        {
            var input = new SaveListPatientGroupMstInputData(request.HpId, request.UserId, ConvertToListInput(request.SaveListPatientGroupMsts));
            var output = _bus.Handle(input);

            var presenter = new SaveListPatientGroupMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveListPatientGroupMstResponse>>(presenter.Result);
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

        private List<SaveListPatientGroupMstInputItem> ConvertToListInput(List<SaveListPatientGroupMstRequestItem> requestItems)
        {
            List<SaveListPatientGroupMstInputItem> listDatas = new();
            foreach (var item in requestItems)
            {
                listDatas.Add(new SaveListPatientGroupMstInputItem(
                        item.GroupId,
                        item.GroupName,
                        item.Details.Select(detail => new SaveListPatientGroupDetailMstInputItem(
                                item.GroupId,
                                detail.GroupCode,
                                detail.SeqNo,
                                detail.GroupDetailName
                            )).ToList()
                    ));
            }
            return listDatas;
        }

        [HttpPost("SaveHokenSyaMst")]
        public ActionResult<Response<SaveHokenSyaMstResponse>> SaveHokenSyaMst([FromBody] SaveHokenSyaMstRequest request)
        {
            var input = new SaveHokenSyaMstInputData(request.HpId
                                                   , request.Name
                                                   , request.KanaName
                                                   , request.HoubetuKbn
                                                   , request.Houbetu
                                                   , request.HokenKbn
                                                   , request.PrefNo
                                                   , request.HokensyaNo
                                                   , request.Kigo
                                                   , request.Bango
                                                   , request.RateHonnin
                                                   , request.RateKazoku
                                                   , request.PostCode
                                                   , request.Address1
                                                   , request.Address2
                                                   , request.Tel1);

            var output = _bus.Handle(input);
            var presenter = new SaveHokenSyaMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveHokenSyaMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.SearchEmptyId)]
        public ActionResult<Response<SearchEmptyIdResponse>> SearchEmptyId([FromQuery] SearchEmptyIdResquest request)
        {
            var input = new SearchEmptyIdInputData(request.HpId, request.PtNum, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new SearchEmptyIdPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchEmptyIdResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDetailHokenMst)]
        public ActionResult<Response<GetDetailHokenMstResponse>> GetDetailHokenMst([FromQuery] GetDetailHokenMstRequest request)
        {
            var input = new GetDetailHokenMstInputData(request.HpId, request.HokenNo, request.HokenEdaNo, request.PrefNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetDetailHokenMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetDetailHokenMstResponse>>(presenter.Result);
        }
    }
}