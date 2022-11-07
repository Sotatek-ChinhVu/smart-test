using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.CalculationInf;
using EmrCloudApi.Tenant.Presenters.GroupInf;
using EmrCloudApi.Tenant.Presenters.HokenMst;
using EmrCloudApi.Tenant.Presenters.Insurance;
using EmrCloudApi.Tenant.Presenters.InsuranceList;
using EmrCloudApi.Tenant.Presenters.InsuranceMst;
using EmrCloudApi.Tenant.Presenters.KohiHokenMst;
using EmrCloudApi.Tenant.Presenters.PatientInfor;
using EmrCloudApi.Tenant.Presenters.PatientInfor.InsuranceMasterLinkage;
using EmrCloudApi.Tenant.Presenters.PatientInfor.PtKyusei;
using EmrCloudApi.Tenant.Presenters.PatientInformation;
using EmrCloudApi.Tenant.Requests.CalculationInf;
using EmrCloudApi.Tenant.Requests.GroupInf;
using EmrCloudApi.Tenant.Requests.HokenMst;
using EmrCloudApi.Tenant.Requests.Insurance;
using EmrCloudApi.Tenant.Requests.InsuranceMst;
using EmrCloudApi.Tenant.Requests.KohiHokenMst;
using EmrCloudApi.Tenant.Requests.PatientInfor;
using EmrCloudApi.Tenant.Requests.PatientInfor.InsuranceMasterLinkage;
using EmrCloudApi.Tenant.Requests.PatientInfor.PtKyuseiInf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.CalculationInf;
using EmrCloudApi.Tenant.Responses.GroupInf;
using EmrCloudApi.Tenant.Responses.HokenMst;
using EmrCloudApi.Tenant.Responses.Insurance;
using EmrCloudApi.Tenant.Responses.InsuranceMst;
using EmrCloudApi.Tenant.Responses.KohiHokenMst;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using EmrCloudApi.Tenant.Responses.PatientInfor.InsuranceMasterLinkage;
using EmrCloudApi.Tenant.Responses.PatientInfor.PtKyuseiInf;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using Microsoft.AspNetCore.Mvc;
using UseCase.CalculationInf;
using UseCase.Core.Sync;
using UseCase.GroupInf.GetList;
using UseCase.HokenMst.GetDetail;
using UseCase.Insurance.GetList;
using UseCase.Insurance.ValidateInsurance;
using UseCase.Insurance.ValidateRousaiJibai;
using UseCase.Insurance.ValidKohi;
using UseCase.Insurance.ValidMainInsurance;
using UseCase.Insurance.ValidPatternOther;
using UseCase.InsuranceMst.Get;
using UseCase.InsuranceMst.SaveHokenSyaMst;
using UseCase.KohiHokenMst.Get;
using UseCase.PatientGroupMst.GetList;
using UseCase.PatientGroupMst.SaveList;
using UseCase.PatientInfor.DeletePatient;
using UseCase.PatientInfor.GetInsuranceMasterLinkage;
using UseCase.PatientInfor.PatientComment;
using UseCase.PatientInfor.PtKyuseiInf.GetList;
using UseCase.PatientInfor.Save;
using UseCase.PatientInfor.SaveInsuranceMasterLinkage;
using UseCase.PatientInfor.SearchAdvanced;
using UseCase.PatientInfor.SearchEmptyId;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInformation.GetById;
using UseCase.SearchHokensyaMst.Get;
using EmrCloudApi.Tenant.Requests.SwapHoken;
using UseCase.SwapHoken.Save;
using EmrCloudApi.Tenant.Presenters.SwapHoken;
using EmrCloudApi.Tenant.Responses.SwapHoken;
using Domain.Models.PatientInfor;
using Domain.Models.InsuranceInfor;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientInforController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public PatientInforController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.Get + "PatientComment")]
        public async Task<ActionResult<Response<GetPatientCommentResponse>>> GetList([FromQuery] GetPatientCommentRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetPatientCommentResponse>>(new Response<GetPatientCommentResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetPatientCommentInputData(hpId, request.PtId);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new GetPatientCommentPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet("GetPatientById")]
        public async Task<ActionResult<Response<GetPatientInforByIdResponse>>> GetPatientById([FromQuery] GetByIdRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetPatientInforByIdResponse>>(new Response<GetPatientInforByIdResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetPatientInforByIdInputData(hpId, request.PtId, request.SinDate, request.RaiinNo);
            var output = await Task.Run(() => _bus.Handle(input));

            var present = new GetPatientInforByIdPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetPatientInforByIdResponse>>(present.Result);
        }

        [HttpGet("GetListPatientGroup")]
        public async Task<ActionResult<Response<GetListGroupInfResponse>>> GetListPatientGroup([FromQuery] GetListGroupInfRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetListGroupInfResponse>>(new Response<GetListGroupInfResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetListGroupInfInputData(hpId, request.PtId);
            var output = await Task.Run(() => _bus.Handle(input));

            var present = new GetListGroupInfPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetListGroupInfResponse>>(present.Result);
        }

        [HttpGet("InsuranceListByPtId")]
        public async Task<ActionResult<Response<GetInsuranceListResponse>>> GetInsuranceListByPtId([FromQuery] GetInsuranceListRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetInsuranceListResponse>>(new Response<GetInsuranceListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetInsuranceListInputData(hpId, request.PtId, request.SinDate);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetInsuranceListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceListResponse>>(presenter.Result);
        }

        [HttpGet("SearchSimple")]
        public async Task<ActionResult<Response<SearchPatientInforSimpleResponse>>> SearchSimple([FromQuery] SearchPatientInfoSimpleRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SearchPatientInforSimpleResponse>>(new Response<SearchPatientInforSimpleResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SearchPatientInfoSimpleInputData(request.Keyword, request.IsContainMode, hpId);
            var output = await Task.Run(() => _bus.Handle(input));

            var present = new SearchPatientInfoSimplePresenter();
            present.Complete(output);

            return new ActionResult<Response<SearchPatientInforSimpleResponse>>(present.Result);
        }

        [HttpPost("SearchAdvanced")]
        public async Task<ActionResult<Response<SearchPatientInfoAdvancedResponse>>> GetList([FromBody] SearchPatientInfoAdvancedRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SearchPatientInfoAdvancedResponse>>(new Response<SearchPatientInfoAdvancedResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SearchPatientInfoAdvancedInputData(request.SearchInput, hpId);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new SearchPatientInfoAdvancedPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SearchPatientInfoAdvancedResponse>>(presenter.Result);
        }

        [HttpGet("GetListCalculationPatient")]
        public async Task<ActionResult<Response<CalculationInfResponse>>> GetListCalculationPatient([FromQuery] CalculationInfRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<CalculationInfResponse>>(new Response<CalculationInfResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new CalculationInfInputData(hpId, request.PtId);
            var output = await Task.Run(() => _bus.Handle(input));

            var present = new CalculationInfPresenter();
            present.Complete(output);

            return new ActionResult<Response<CalculationInfResponse>>(present.Result);
        }

        [HttpGet("GetPatientGroupMst")]
        public async Task<ActionResult<Response<GetListPatientGroupMstResponse>>> GetPatientGroupMst()
        {
            var input = new GetListPatientGroupMstInputData();
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetListPatientGroupMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListPatientGroupMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SavePatientGroupMst)]
        public async Task<ActionResult<Response<SaveListPatientGroupMstResponse>>> SavePatientGroupMst([FromBody] SaveListPatientGroupMstRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveListPatientGroupMstResponse>>(new Response<SaveListPatientGroupMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveListPatientGroupMstResponse>>(new Response<SaveListPatientGroupMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SaveListPatientGroupMstInputData(hpId, userId, ConvertToListInput(request.SaveListPatientGroupMsts));
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SaveListPatientGroupMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveListPatientGroupMstResponse>>(presenter.Result);
        }

        [HttpGet("GetInsuranceMst")]
        public async Task<ActionResult<Response<GetInsuranceMstResponse>>> GetInsuranceMst([FromQuery] GetInsuranceMstRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetInsuranceMstResponse>>(new Response<GetInsuranceMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetInsuranceMstInputData(hpId, request.PtId, request.SinDate);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetInsuranceMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceMstResponse>>(presenter.Result);
        }

        [HttpGet("SearchHokensyaMst")]
        public async Task<ActionResult<Response<SearchHokensyaMstResponse>>> SearchHokensyaMst([FromQuery] SearchHokensyaMstRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SearchHokensyaMstResponse>>(new Response<SearchHokensyaMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SearchHokensyaMstInputData(hpId, request.SinDate, request.Keyword);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SearchHokenMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchHokensyaMstResponse>>(presenter.Result);
        }

        [HttpGet("GetHokenMstByFutansyaNo")]
        public async Task<ActionResult<Response<GetKohiHokenMstResponse>>> GetHokenMstByFutansyaNo([FromQuery] GetKohiHokenMstRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetKohiHokenMstResponse>>(new Response<GetKohiHokenMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetKohiHokenMstInputData(hpId, request.SinDate, request.FutansyaNo);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetKohiHokenMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetKohiHokenMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ValidateKohi)]
        public async Task<ActionResult<Response<ValidateKohiResponse>>> ValidateKohi([FromBody] ValidateKohiRequest request)
        {
            var input = new ValidKohiInputData(request.SinDate, request.PtBirthday, request.IsKohiEmptyModel1, request.IsSelectedKohiMst1, request.SelectedKohiFutansyaNo1, request.SelectedKohiJyukyusyaNo1,
                request.SelectedKohiTokusyuNo1, request.SelectedKohiStartDate1, request.SelectedKohiEndDate1, request.SelectedKohiConfirmDate1, request.SelectedKohiHokenNo1, request.SelectedKohiHokenEdraNo1, request.SelectedKohiIsAddNew1,
                request.IsKohiEmptyModel2, request.IsSelectedKohiMst2, request.SelectedKohiFutansyaNo2, request.SelectedKohiJyukyusyaNo2,
                request.SelectedKohiTokusyuNo2, request.SelectedKohiStartDate2, request.SelectedKohiEndDate2, request.SelectedKohiConfirmDate2, request.SelectedKohiHokenNo2, request.SelectedKohiHokenEdraNo2, request.SelectedKohiIsAddNew2,
                request.IsKohiEmptyModel3, request.IsSelectedKohiMst3, request.SelectedKohiFutansyaNo3, request.SelectedKohiJyukyusyaNo3, request.SelectedKohiTokusyuNo3, request.SelectedKohiStartDate3, request.SelectedKohiEndDate3,
                request.SelectedKohiConfirmDate3, request.SelectedKohiHokenNo3, request.SelectedKohiHokenEdraNo3, request.SelectedKohiIsAddNew3,
                request.IsKohiEmptyModel4, request.IsSelectedKohiMst4, request.SelectedKohiFutansyaNo4, request.SelectedKohiJyukyusyaNo4, request.SelectedKohiTokusyuNo4, request.SelectedKohiStartDate4, request.SelectedKohiEndDate4,
                request.SelectedKohiConfirmDate4, request.SelectedKohiHokenNo4, request.SelectedKohiHokenEdraNo4, request.SelectedKohiIsAddNew4);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new ValidateKohiPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidateKohiResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ValidateRousaiJibai)]
        public async Task<ActionResult<Response<ValidateRousaiJibaiResponse>>> ValidateRousaiJibai([FromBody] ValidateRousaiJibaiRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<ValidateRousaiJibaiResponse>>(new Response<ValidateRousaiJibaiResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new ValidateRousaiJibaiInputData(hpId, request.HokenKbn, request.SinDate, request.IsSelectedHokenInf, request.SelectedHokenInfRodoBango,
                request.ListRousaiTenki, request.SelectedHokenInfRousaiSaigaiKbn, request.SelectedHokenInfRousaiSyobyoDate, request.SelectedHokenInfRousaiSyobyoCd,
                request.SelectedHokenInfRyoyoStartDate, request.SelectedHokenInfRyoyoEndDate, request.SelectedHokenInfStartDate, request.SelectedHokenInfEndDate,
                request.SelectedHokenInfIsAddNew, request.SelectedHokenInfNenkinBango, request.SelectedHokenInfKenkoKanriBango, request.SelectedHokenInfConfirmDate);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new ValidateRousaiJibaiPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidateRousaiJibaiResponse>>(presenter.Result);
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
        public async Task<ActionResult<Response<SaveHokenSyaMstResponse>>> SaveHokenSyaMst([FromBody] SaveHokenSyaMstRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveHokenSyaMstResponse>>(new Response<SaveHokenSyaMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveHokenSyaMstResponse>>(new Response<SaveHokenSyaMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SaveHokenSyaMstInputData(hpId
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
                                                   , request.Tel1
                                                   , request.IsKigoNa
                                                   , userId);

            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new SaveHokenSyaMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveHokenSyaMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.SearchEmptyId)]
        public async Task<ActionResult<Response<SearchEmptyIdResponse>>> SearchEmptyId([FromQuery] SearchEmptyIdResquest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SearchEmptyIdResponse>>(new Response<SearchEmptyIdResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SearchEmptyIdInputData(hpId, request.PtNum, request.PageIndex, request.PageSize);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SearchEmptyIdPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchEmptyIdResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDetailHokenMst)]
        public async Task<ActionResult<Response<GetDetailHokenMstResponse>>> GetDetailHokenMst([FromQuery] GetDetailHokenMstRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetDetailHokenMstResponse>>(new Response<GetDetailHokenMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetDetailHokenMstInputData(hpId, request.HokenNo, request.HokenEdaNo, request.PrefNo, request.SinDate);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetDetailHokenMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetDetailHokenMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ValidateInsuranceOther)]
        public async Task<ActionResult<Response<ValidInsuranceOtherResponse>>> ValidateInsuranceOther([FromBody] ValidInsuranceOtherRequest request)
        {
            var input = new ValidInsuranceOtherInputData(request.ValidModel);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new ValidInsuranceOtherPresenter();

            presenter.Complete(output);
            return new ActionResult<Response<ValidInsuranceOtherResponse>>(presenter.Result);
        }

        [HttpPost("ValidateMainInsurance")]
        public async Task<ActionResult<Response<ValidateMainInsuranceReponse>>> ValidateMainInsurance([FromBody] ValidateMainInsuranceRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<ValidateMainInsuranceReponse>>(new Response<ValidateMainInsuranceReponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new ValidMainInsuranceInputData(hpId, request.SinDate, request.PtBirthday, request.HokenKbn, request.HokenSyaNo, request.IsSelectedHokenPattern,
                request.IsSelectedHokenInf, request.IsSelectedHokenMst, request.SelectedHokenInfHoubetu, request.SelectedHokenInfHokenNo, request.SelectedHokenInfHokenEdra, request.SelectedHokenInfIsAddNew, request.SelectedHokenInfIsJihi,
                request.SelectedHokenInfStartDate, request.SelectedHokenInfEndDate, request.SelectedHokenInfKigo, request.SelectedHokenInfBango,
                request.SelectedHokenInfHonkeKbn, request.SelectedHokenInfTokureiYm1, request.SelectedHokenInfTokureiYm2, request.SelectedHokenInfIsShahoOrKokuho, request.SelectedHokenInfIsExpirated,
                request.SelectedHokenInfIsIsNoHoken, request.SelectedHokenInfConfirmDate, request.SelectedHokenInfIsAddHokenCheck, request.SelectedHokenInfTokki1, request.SelectedHokenInfTokki2,
                request.SelectedHokenInfTokki3, request.SelectedHokenInfTokki4, request.SelectedHokenInfTokki5, request.SelectedHokenPatternIsEmptyKohi1, request.SelectedHokenPatternIsEmptyKohi2, request.SelectedHokenPatternIsEmptyKohi3,
                request.SelectedHokenPatternIsEmptyKohi4, request.SelectedHokenPatternIsExpirated, request.SelectedHokenPatternIsEmptyHoken);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new ValidateMainInsurancePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidateMainInsuranceReponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetInsuranceMasterLinkage)]
        public async Task<ActionResult<Response<GetInsuranceMasterLinkageResponse>>> GetInsuranceMasterLinkage([FromQuery] GetInsuranceMasterLinkageRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetInsuranceMasterLinkageResponse>>(new Response<GetInsuranceMasterLinkageResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetInsuranceMasterLinkageInputData(hpId, request.FutansyaNo);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetInsuranceMasterLinkagePresenter();

            presenter.Complete(output);
            return new ActionResult<Response<GetInsuranceMasterLinkageResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetPtKyuseiInf)]
        public async Task<ActionResult<Response<GetPtKyuseiInfResponse>>> GetPtKyuseiInf([FromQuery] GetPtKyuseiInfRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetPtKyuseiInfResponse>>(new Response<GetPtKyuseiInfResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetPtKyuseiInfInputData(hpId, request.PtId, request.IsDeleted);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetPtKyuseiInfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPtKyuseiInfResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveInsuranceMasterLinkage)]
        public async Task<ActionResult<Response<SaveInsuranceMasterLinkageResponse>>> SaveInsuranceMasterLinkage([FromBody] SaveInsuranceMasterLinkageRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveInsuranceMasterLinkageResponse>>(new Response<SaveInsuranceMasterLinkageResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveInsuranceMasterLinkageResponse>>(new Response<SaveInsuranceMasterLinkageResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SaveInsuranceMasterLinkageInputData(request.DefHokenNoModels, hpId, userId);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new SaveInsuranceMasterLinkagePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveInsuranceMasterLinkageResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SavePatientInfo)]
        public async Task<ActionResult<Response<SavePatientInfoResponse>>> SavePatientInfo([FromBody] SavePatientInfoRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SavePatientInfoResponse>>(new Response<SavePatientInfoResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<SavePatientInfoResponse>>(new Response<SavePatientInfoResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            PatientInforSaveModel patient = new PatientInforSaveModel(hpId,
                                                                      request.Patient.PtId,
                                                                      request.Patient.PtNum,
                                                                      request.Patient.KanaName,
                                                                      request.Patient.Name,
                                                                      request.Patient.Sex,
                                                                      request.Patient.Birthday,
                                                                      request.Patient.IsDead,
                                                                      request.Patient.DeathDate,
                                                                      request.Patient.Mail,
                                                                      request.Patient.HomePost,
                                                                      request.Patient.HomeAddress1,
                                                                      request.Patient.HomeAddress2,
                                                                      request.Patient.Tel1,
                                                                      request.Patient.Tel2,
                                                                      request.Patient.Setanusi,
                                                                      request.Patient.Zokugara,
                                                                      request.Patient.Job,
                                                                      request.Patient.RenrakuName,
                                                                      request.Patient.RenrakuPost,
                                                                      request.Patient.RenrakuAddress1,
                                                                      request.Patient.RenrakuAddress2,
                                                                      request.Patient.RenrakuTel,
                                                                      request.Patient.RenrakuMemo,
                                                                      request.Patient.OfficeName,
                                                                      request.Patient.OfficePost,
                                                                      request.Patient.OfficeAddress1,
                                                                      request.Patient.OfficeAddress2,
                                                                      request.Patient.OfficeTel,
                                                                      request.Patient.OfficeMemo,
                                                                      request.Patient.IsRyosyoDetail,
                                                                      request.Patient.PrimaryDoctor,
                                                                      request.Patient.IsTester,
                                                                      request.Patient.MainHokenPid,
                                                                      request.Patient.ReferenceNo,
                                                                      request.Patient.LimitConsFlg,
                                                                      request.Patient.Memo);

            List<InsuranceModel> insurances = request.Insurances.Select(x => new InsuranceModel(hpId,
                                                                                               x.PtId,
                                                                                               0,
                                                                                               x.SeqNo,
                                                                                               x.HokenSbtCd,
                                                                                               x.HokenPid,
                                                                                               x.HokenKbn,
                                                                                               x.HokenMemo,
                                                                                               0,
                                                                                               x.StartDate,
                                                                                               x.EndDate,
                                                                                               x.HokenId,
                                                                                               x.Kohi1Id,
                                                                                               x.Kohi2Id,
                                                                                               x.Kohi3Id,
                                                                                               x.Kohi4Id,
                                                                                               x.IsAddNew,
                                                                                               x.IsDeleted)).ToList();

            List<HokenInfModel> hokenInfs = request.HokenInfs.Select(x => new HokenInfModel(hpId,
                                                                                           x.PtId,
                                                                                           x.HokenId,
                                                                                           x.SeqNo,
                                                                                           x.HokenNo,
                                                                                           x.HokenEdaNo,
                                                                                           x.HokenKbn,
                                                                                           x.HokensyaNo,
                                                                                           x.Kigo,
                                                                                           x.Bango,
                                                                                           x.EdaNo,
                                                                                           x.HonkeKbn,
                                                                                           x.StartDate,
                                                                                           x.EndDate,
                                                                                           x.SikakuDate,
                                                                                           x.KofuDate,
                                                                                           0,
                                                                                           x.KogakuKbn,
                                                                                           x.TasukaiYm,
                                                                                           x.TokureiYm1,
                                                                                           x.TokureiYm2,
                                                                                           x.GenmenKbn,
                                                                                           x.GenmenRate,
                                                                                           x.GenmenGaku,
                                                                                           x.SyokumuKbn,
                                                                                           x.KeizokuKbn,
                                                                                           x.Tokki1,
                                                                                           x.Tokki2,
                                                                                           x.Tokki3,
                                                                                           x.Tokki4,
                                                                                           x.Tokki5,
                                                                                           x.RousaiKofuNo,
                                                                                           string.Empty,
                                                                                           x.RousaiRoudouCd,
                                                                                           string.Empty,
                                                                                           x.RousaiSaigaiKbn,
                                                                                           x.RousaiKantokuCd,
                                                                                           x.RousaiSyobyoDate,
                                                                                           x.RyoyoStartDate,
                                                                                           x.RyoyoEndDate,
                                                                                           x.RousaiSyobyoCd,
                                                                                           x.RousaiJigyosyoName,
                                                                                           x.RousaiPrefName,
                                                                                           x.RousaiCityName,
                                                                                           x.RousaiReceCount,
                                                                                           string.Empty,
                                                                                           string.Empty,
                                                                                           string.Empty,
                                                                                           0,
                                                                                           x.JibaiHokenName,
                                                                                           x.JibaiHokenTanto,
                                                                                           x.JibaiHokenTel,
                                                                                           x.JibaiJyusyouDate,
                                                                                           x.Houbetu,
                                                                                           x.ConfirmDates.Select(c => new ConfirmDateModel(c.HokenGrp,
                                                                                                                                           c.HokenId,
                                                                                                                                           c.SeqNo,
                                                                                                                                           c.CheckId,
                                                                                                                                           c.CheckName,
                                                                                                                                           c.CheckComment,
                                                                                                                                           c.ConfirmDate)).ToList(),
                                                                                           x.RousaiTenkis.Select(m => new RousaiTenkiModel(m.RousaiTenkiSinkei,
                                                                                                                                           m.RousaiTenkiTenki,
                                                                                                                                           m.RousaiTenkiEndDate,
                                                                                                                                           m.RousaiTenkiIsDeleted,
                                                                                                                                           m.SeqNo)).ToList(),
                                                                                           false,
                                                                                           x.IsDeleted,
                                                                                           new HokenMstModel(),
                                                                                           new HokensyaMstModel(),
                                                                                           x.IsAddNew,
                                                                                           false,
                                                                                           string.Empty)).ToList();

            List<KohiInfModel> hokenKohis = request.HokenKohis.Select(x => new KohiInfModel(x.ConfirmDates.Select(c => new ConfirmDateModel(c.HokenGrp,
                                                                                                                                            c.HokenId,
                                                                                                                                            c.SeqNo,
                                                                                                                                            c.CheckId,
                                                                                                                                            c.CheckName,
                                                                                                                                            c.CheckComment,
                                                                                                                                            c.ConfirmDate)).ToList(),
                                                                                            x.FutansyaNo,
                                                                                            x.JyukyusyaNo,
                                                                                            x.HokenId,
                                                                                            x.StartDate,
                                                                                            x.EndDate,
                                                                                            0,
                                                                                            x.Rate,
                                                                                            x.GendoGaku,
                                                                                            x.SikakuDate,
                                                                                            x.KofuDate,
                                                                                            x.TokusyuNo,
                                                                                            x.HokenSbtKbn,
                                                                                            x.Houbetu,
                                                                                            new HokenMstModel(),
                                                                                            x.HokenNo,
                                                                                            x.HokenEdaNo,
                                                                                            x.PrefNo,
                                                                                            0,
                                                                                            false,
                                                                                            x.IsDeleted,
                                                                                            x.SeqNo,
                                                                                            x.IsAddNew)).ToList();


            var input = new SavePatientInfoInputData(patient,
                                                     request.PtKyuseis,
                                                     request.PtSanteis,
                                                     insurances,
                                                     hokenInfs,
                                                     hokenKohis,
                                                     request.PtGrps,
                                                     userId);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new SavePatientInfoPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SavePatientInfoResponse>>(presenter.Result);
        }

        [HttpPost("DeletePatientInfo")]
        public async Task<ActionResult<Response<DeletePatientInfoResponse>>> DeletePatientInfo([FromBody] DeletePatientInfoRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<DeletePatientInfoResponse>>(new Response<DeletePatientInfoResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<DeletePatientInfoResponse>>(new Response<DeletePatientInfoResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new DeletePatientInfoInputData(hpId, request.PtId, userId);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new DeletePatientInfoPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<DeletePatientInfoResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ValidateListPattern)]
        public async Task<ActionResult<Response<ValidateListInsuranceResponse>>> ValidateListPattern([FromBody] ValidateInsuranceRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<ValidateListInsuranceResponse>>(new Response<ValidateListInsuranceResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new ValidateInsuranceInputData(hpId, request.SinDate, request.PtBirthday, request.ListInsurance);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new ValidateInsurancePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<ValidateListInsuranceResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SwapHoken)]
        public async Task<ActionResult<Response<SaveSwapHokenResponse>>> SwapHokenParttern([FromBody] SaveSwapHokenRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveSwapHokenResponse>>(new Response<SaveSwapHokenResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveSwapHokenResponse>>(new Response<SaveSwapHokenResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SaveSwapHokenInputData(hpId,
                                                   request.PtId,
                                                   request.HokenIdBefore,
                                                   request.HokenIdAfter,
                                                   request.HokenPidBefore,
                                                   request.HokenPidAfter,
                                                   request.StartDate,
                                                   request.EndDate,
                                                   userId);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new SaveSwapHokenPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveSwapHokenResponse>>(presenter.Result);
        }
    }
}