using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Domain.Models.PtGroupMst;
using EmrCloudApi.Constants;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.CalculationInf;
using EmrCloudApi.Presenters.GroupInf;
using EmrCloudApi.Presenters.HokenMst;
using EmrCloudApi.Presenters.Insurance;
using EmrCloudApi.Presenters.InsuranceList;
using EmrCloudApi.Presenters.InsuranceMst;
using EmrCloudApi.Presenters.KohiHokenMst;
using EmrCloudApi.Presenters.MaxMoney;
using EmrCloudApi.Presenters.PatientInfor;
using EmrCloudApi.Presenters.PatientInfor.InsuranceMasterLinkage;
using EmrCloudApi.Presenters.PatientInfor.PtKyusei;
using EmrCloudApi.Presenters.PatientInformation;
using EmrCloudApi.Presenters.PtGroupMst;
using EmrCloudApi.Presenters.SwapHoken;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.CalculationInf;
using EmrCloudApi.Requests.GroupInf;
using EmrCloudApi.Requests.HokenMst;
using EmrCloudApi.Requests.Insurance;
using EmrCloudApi.Requests.InsuranceMst;
using EmrCloudApi.Requests.KohiHokenMst;
using EmrCloudApi.Requests.MaxMoney;
using EmrCloudApi.Requests.PatientInfor;
using EmrCloudApi.Requests.PatientInfor.InsuranceMasterLinkage;
using EmrCloudApi.Requests.PatientInfor.PtKyuseiInf;
using EmrCloudApi.Requests.PtGroupMst;
using EmrCloudApi.Requests.SwapHoken;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.CalculationInf;
using EmrCloudApi.Responses.GroupInf;
using EmrCloudApi.Responses.HokenMst;
using EmrCloudApi.Responses.Insurance;
using EmrCloudApi.Responses.InsuranceMst;
using EmrCloudApi.Responses.KohiHokenMst;
using EmrCloudApi.Responses.MaxMoney;
using EmrCloudApi.Responses.PatientInfor;
using EmrCloudApi.Responses.PatientInfor.InsuranceMasterLinkage;
using EmrCloudApi.Responses.PatientInfor.PtKyuseiInf;
using EmrCloudApi.Responses.PatientInformaiton;
using EmrCloudApi.Responses.PtGroupMst;
using EmrCloudApi.Responses.SwapHoken;
using EmrCloudApi.Tenant.Presenters.PatientInfor;
using EmrCloudApi.Tenant.Requests.PatientInfor;
using EmrCloudApi.Tenant.Responses.PatientInfor;
using Helper.Messaging;
using Helper.Messaging.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UseCase.CalculationInf;
using UseCase.Core.Sync;
using UseCase.GroupInf.GetList;
using UseCase.HokenMst.GetDetail;
using UseCase.Insurance.GetKohiPriorityList;
using UseCase.Insurance.GetList;
using UseCase.Insurance.HokenPatternUsed;
using UseCase.Insurance.ValidateInsurance;
using UseCase.Insurance.ValidateRousaiJibai;
using UseCase.Insurance.ValidHokenInfAllType;
using UseCase.Insurance.ValidKohi;
using UseCase.Insurance.ValidMainInsurance;
using UseCase.InsuranceMst.Get;
using UseCase.InsuranceMst.GetHokenSyaMst;
using UseCase.InsuranceMst.SaveHokenSyaMst;
using UseCase.KohiHokenMst.Get;
using UseCase.MaxMoney.GetMaxMoneyByPtId;
using UseCase.PatientGroupMst.GetList;
using UseCase.PatientGroupMst.SaveList;
using UseCase.PatientInfor;
using UseCase.PatientInfor.CheckAllowDeletePatientInfo;
using UseCase.PatientInfor.CheckValidSamePatient;
using UseCase.PatientInfor.DeletePatient;
using UseCase.PatientInfor.GetInsuranceMasterLinkage;
using UseCase.PatientInfor.GetListPatient;
using UseCase.PatientInfor.GetPatientInfoBetweenTimesList;
using UseCase.PatientInfor.GetPtInfByRefNo;
using UseCase.PatientInfor.GetPtInfModelsByName;
using UseCase.PatientInfor.GetPtInfModelsByRefNo;
using UseCase.PatientInfor.GetTokiMstList;
using UseCase.PatientInfor.GetVisitTimesManagementModels;
using UseCase.PatientInfor.PatientComment;
using UseCase.PatientInfor.PtKyuseiInf.GetList;
using UseCase.PatientInfor.Save;
using UseCase.PatientInfor.SaveInsuranceMasterLinkage;
using UseCase.PatientInfor.SavePtKyusei;
using UseCase.PatientInfor.SearchAdvanced;
using UseCase.PatientInfor.SearchEmptyId;
using UseCase.PatientInfor.SearchPatientInfoByPtIdList;
using UseCase.PatientInfor.SearchPatientInfoByPtNum;
using UseCase.PatientInfor.SearchSimple;
using UseCase.PatientInfor.UpdateVisitTimesManagement;
using UseCase.PatientInfor.UpdateVisitTimesManagementNeedSave;
using UseCase.PatientInformation.GetById;
using UseCase.PtGroupMst.CheckAllowDelete;
using UseCase.PtGroupMst.GetGroupNameMst;
using UseCase.PtGroupMst.SaveGroupNameMst;
using UseCase.SearchHokensyaMst.Get;
using UseCase.SwapHoken.Calculation;
using UseCase.SwapHoken.Save;
using UseCase.SwapHoken.Validate;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class PatientInforController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IWebSocketService _webSocketService;
        private readonly IMessenger _messenger;
        private CancellationToken? _cancellationToken;

        public PatientInforController(UseCaseBus bus, IWebSocketService webSocketService, IMessenger messenger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
            _webSocketService = webSocketService;
            _messenger = messenger;
        }

        [HttpGet(ApiPath.Get + "PatientComment")]
        public ActionResult<Response<GetPatientCommentResponse>> GetListPatientComment([FromQuery] GetPatientCommentRequest request)
        {
            var input = new GetPatientCommentInputData(HpId, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new GetPatientCommentPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpGet("GetPatientById")]
        public ActionResult<Response<GetPatientInforByIdResponse>> GetPatientById([FromQuery] GetByIdRequest request)
        {
            var input = new GetPatientInforByIdInputData(HpId, request.PtId, request.SinDate, request.RaiinNo, request.IsShowKyuSeiName, request.ListStatus);
            var output = _bus.Handle(input);

            var present = new GetPatientInforByIdPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetPatientInforByIdResponse>>(present.Result);
        }

        [HttpGet("GetListPatientGroup")]
        public ActionResult<Response<GetListGroupInfResponse>> GetListPatientGroup([FromQuery] GetListGroupInfRequest request)
        {
            var input = new GetListGroupInfInputData(HpId, request.PtId);
            var output = _bus.Handle(input);

            var present = new GetListGroupInfPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetListGroupInfResponse>>(present.Result);
        }

        [HttpGet("InsuranceListByPtId")]
        public ActionResult<Response<GetInsuranceListResponse>> GetInsuranceListByPtId([FromQuery] GetInsuranceListRequest request)
        {
            var input = new GetInsuranceListInputData(HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetInsuranceListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceListResponse>>(presenter.Result);
        }

        [HttpGet("SearchSimple")]
        public ActionResult<Response<SearchPatientInforSimpleResponse>> SearchSimple([FromQuery] SearchPatientInfoSimpleRequest request)
        {
            var input = new SearchPatientInfoSimpleInputData(request.Keyword, request.IsContainMode, HpId, request.PageIndex, request.PageSize, ConvertToSortData(request.SortData));
            var output = _bus.Handle(input);

            var present = new SearchPatientInfoSimplePresenter();
            present.Complete(output);

            return new ActionResult<Response<SearchPatientInforSimpleResponse>>(present.Result);
        }

        [HttpPost("SearchAdvanced")]
        public ActionResult<Response<SearchPatientInfoAdvancedResponse>> SearchAdvanced([FromBody] SearchPatientInfoAdvancedRequest request)
        {
            var input = new SearchPatientInfoAdvancedInputData(ConvertToPatientAdvancedSearchInput(request), HpId, request.PageIndex, request.PageSize, request.SortData);
            var output = _bus.Handle(input);
            var presenter = new SearchPatientInfoAdvancedPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SearchPatientInfoAdvancedResponse>>(presenter.Result);
        }

        [HttpGet("GetListCalculationPatient")]
        public ActionResult<Response<CalculationInfResponse>> GetListCalculationPatient([FromQuery] CalculationInfRequest request)
        {
            var input = new CalculationInfInputData(HpId, request.PtId);
            var output = _bus.Handle(input);

            var present = new CalculationInfPresenter();
            present.Complete(output);

            return new ActionResult<Response<CalculationInfResponse>>(present.Result);
        }

        [HttpGet("GetPatientGroupMst")]
        public ActionResult<Response<GetListPatientGroupMstResponse>> GetPatientGroupMst()
        {
            var input = new GetListPatientGroupMstInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetListPatientGroupMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListPatientGroupMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SavePatientGroupMst)]
        public ActionResult<Response<SaveListPatientGroupMstResponse>> SavePatientGroupMst([FromBody] SaveListPatientGroupMstRequest request)
        {
            var input = new SaveListPatientGroupMstInputData(HpId, UserId, ConvertToListInput(request.SaveListPatientGroupMsts));
            var output = _bus.Handle(input);

            var presenter = new SaveListPatientGroupMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveListPatientGroupMstResponse>>(presenter.Result);
        }

        [HttpGet("GetInsuranceMst")]
        [ResponseCache(Duration = 1800, Location = ResponseCacheLocation.Any, NoStore = true)]
        public ActionResult<Response<GetInsuranceMstResponse>> GetInsuranceMst([FromQuery] GetInsuranceMstRequest request)
        {
            var input = new GetInsuranceMstInputData(HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetInsuranceMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceMstResponse>>(presenter.Result);
        }

        [HttpGet("SearchHokensyaMst")]
        public ActionResult<Response<SearchHokensyaMstResponse>> SearchHokensyaMst([FromQuery] SearchHokensyaMstRequest request)
        {
            var input = new SearchHokensyaMstInputData(HpId, request.SinDate, request.Keyword);
            var output = _bus.Handle(input);

            var presenter = new SearchHokenMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchHokensyaMstResponse>>(presenter.Result);
        }

        [HttpGet("GetHokenMstByFutansyaNo")]
        public ActionResult<Response<GetKohiHokenMstResponse>> GetHokenMstByFutansyaNo([FromQuery] GetKohiHokenMstRequest request)
        {

            var input = new GetKohiHokenMstInputData(HpId, request.SinDate, request.FutansyaNo);
            var output = _bus.Handle(input);

            var presenter = new GetKohiHokenMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetKohiHokenMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ValidateKohi)]
        public ActionResult<Response<ValidateKohiResponse>> ValidateOneKohi([FromBody] ValidateKohiRequest request)
        {
            var input = new ValidKohiInputData(request.SinDate,
                                               request.PtBirthday,
                                               request.IsKohiEmptyModel,
                                               request.IsSelectedKohiMst,
                                               request.SelectedKohiFutansyaNo,
                                               request.SelectedKohiJyukyusyaNo,
                                               request.SelectedKohiTokusyuNo,
                                               request.SelectedKohiStartDate,
                                               request.SelectedKohiEndDate,
                                               request.SelectedKohiConfirmDate,
                                               request.SelectedKohiHokenNo,
                                               request.SelectedKohiIsAddNew,
                                               request.SelectedHokenPatternIsExpirated,
                                               request.KohiMasterIsFutansyaNoCheck,
                                               request.KohiMasterIsJyukyusyaNoCheck,
                                               request.KohiMasterIsTokusyuNoCheck,
                                               request.KohiMasterStartDate,
                                               request.KohiMasterEndDate,
                                               request.KohiMasterDisplayTextMaster,
                                               request.KohiMasterJyukyuCheckDigit,
                                               request.KohiMasterCheckDigit,
                                               request.KohiMasterHoubetu,
                                               request.KohiMasterAgeStart,
                                               request.KohiMasterAgeEnd);
            var output = _bus.Handle(input);

            var presenter = new ValidateKohiPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<ValidateKohiResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ValidateRousaiJibai)]
        public ActionResult<Response<ValidateRousaiJibaiResponse>> ValidateRousaiJibai([FromBody] ValidateRousaiJibaiRequest request)
        {
            var input = new ValidateRousaiJibaiInputData(HpId, request.HokenKbn, request.SinDate, request.IsSelectedHokenInf, request.SelectedHokenInfRodoBango,
                request.ListRousaiTenki, request.SelectedHokenInfRousaiSaigaiKbn, request.SelectedHokenInfRousaiSyobyoDate, request.SelectedHokenInfRousaiSyobyoCd,
                request.SelectedHokenInfRyoyoStartDate, request.SelectedHokenInfRyoyoEndDate, request.SelectedHokenInfStartDate, request.SelectedHokenInfEndDate,
                request.SelectedHokenInfIsAddNew, request.SelectedHokenInfNenkinBango, request.SelectedHokenInfKenkoKanriBango, request.SelectedHokenInfConfirmDate, request.SelectedHokenInfHokenMasterModelIsNull);
            var output = _bus.Handle(input);

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
        public ActionResult<Response<SaveHokenSyaMstResponse>> SaveHokenSyaMst([FromBody] SaveHokenSyaMstRequest request)
        {
            var input = new SaveHokenSyaMstInputData(HpId
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
               , UserId);

            var output = _bus.Handle(input);
            var presenter = new SaveHokenSyaMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveHokenSyaMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.SearchEmptyId)]
        public ActionResult<Response<SearchEmptyIdResponse>> SearchEmptyId([FromQuery] SearchEmptyIdResquest request)
        {
            var input = new SearchEmptyIdInputData(HpId, request.PtNum, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new SearchEmptyIdPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchEmptyIdResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDetailHokenMst)]
        public ActionResult<Response<GetDetailHokenMstResponse>> GetDetailHokenMst([FromQuery] GetDetailHokenMstRequest request)
        {
            var input = new GetDetailHokenMstInputData(HpId, request.HokenNo, request.HokenEdaNo, request.PrefNo, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetDetailHokenMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetDetailHokenMstResponse>>(presenter.Result);
        }

        [HttpPost("ValidateMainInsurance")]
        public ActionResult<Response<ValidateMainInsuranceReponse>> ValidateMainInsurance([FromBody] ValidateMainInsuranceRequest request)
        {
            var input = new ValidMainInsuranceInputData(HpId, request.SinDate, request.PtBirthday, request.HokenKbn, request.HokenSyaNo, request.IsSelectedHokenPattern,
                request.IsSelectedHokenInf, request.IsSelectedHokenMst, request.SelectedHokenInfHoubetu, request.SelectedHokenInfHokenNo, request.SelectedHokenInfHokenEdra, request.SelectedHokenInfIsAddNew, request.SelectedHokenInfIsJihi,
                request.SelectedHokenInfStartDate, request.SelectedHokenInfEndDate, request.SelectedHokenInfKigo, request.SelectedHokenInfBango,
                request.SelectedHokenInfHonkeKbn, request.SelectedHokenInfTokureiYm1, request.SelectedHokenInfTokureiYm2, request.SelectedHokenInfIsShahoOrKokuho, request.SelectedHokenInfIsExpirated,
                request.SelectedHokenInfIsIsNoHoken, request.SelectedHokenInfConfirmDate, request.SelectedHokenInfIsAddHokenCheck, request.SelectedHokenInfTokki1, request.SelectedHokenInfTokki2,
                request.SelectedHokenInfTokki3, request.SelectedHokenInfTokki4, request.SelectedHokenInfTokki5, request.SelectedHokenPatternIsEmptyKohi1, request.SelectedHokenPatternIsEmptyKohi2, request.SelectedHokenPatternIsEmptyKohi3,
                request.SelectedHokenPatternIsEmptyKohi4, request.SelectedHokenPatternIsExpirated, request.SelectedHokenPatternIsEmptyHoken, request.SelectedHokenPatternIsAddNew, request.HokenInfIsNoHoken, request.SelectedHokenInfHokenChecksCount);
            var output = _bus.Handle(input);

            var presenter = new ValidateMainInsurancePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidateMainInsuranceReponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetInsuranceMasterLinkage)]
        public ActionResult<Response<GetInsuranceMasterLinkageResponse>> GetInsuranceMasterLinkage([FromQuery] GetInsuranceMasterLinkageRequest request)
        {
            var input = new GetInsuranceMasterLinkageInputData(HpId, request.FutansyaNo);
            var output = _bus.Handle(input);

            var presenter = new GetInsuranceMasterLinkagePresenter();

            presenter.Complete(output);
            return new ActionResult<Response<GetInsuranceMasterLinkageResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetPtKyuseiInf)]
        public ActionResult<Response<GetPtKyuseiInfResponse>> GetPtKyuseiInf([FromQuery] GetPtKyuseiInfRequest request)
        {
            var input = new GetPtKyuseiInfInputData(HpId, request.PtId, request.IsDeleted);
            var output = _bus.Handle(input);

            var presenter = new GetPtKyuseiInfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPtKyuseiInfResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveInsuranceMasterLinkage)]
        public ActionResult<Response<SaveInsuranceMasterLinkageResponse>> SaveInsuranceMasterLinkage([FromBody] SaveInsuranceMasterLinkageRequest request)
        {
            var input = new SaveInsuranceMasterLinkageInputData(request.DefHokenNoModels, HpId, UserId);
            var output = _bus.Handle(input);
            var presenter = new SaveInsuranceMasterLinkagePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveInsuranceMasterLinkageResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SavePatientInfo)]
        public async Task<ActionResult<Response<SavePatientInfoResponse>>> SavePatientInfo([FromForm] SavePatientInfoFromFormRequest request)
        {
            var patientInfo = JsonConvert.DeserializeObject<SavePatientInfoRequest>(request.JsonPt);
            if (patientInfo is null)
                return BadRequest();

            PatientInforSaveModel patient = new PatientInforSaveModel(HpId,
                      patientInfo.Patient.PtId,
                      patientInfo.Patient.PtNum,
                      patientInfo.Patient.KanaName,
                      patientInfo.Patient.Name,
                      patientInfo.Patient.Sex,
                      patientInfo.Patient.Birthday,
                      patientInfo.Patient.IsDead,
                      patientInfo.Patient.DeathDate,
                      patientInfo.Patient.Mail,
                      patientInfo.Patient.HomePost,
                      patientInfo.Patient.HomeAddress1,
                      patientInfo.Patient.HomeAddress2,
                      patientInfo.Patient.Tel1,
                      patientInfo.Patient.Tel2,
                      patientInfo.Patient.Setanusi,
                      patientInfo.Patient.Zokugara,
                      patientInfo.Patient.Job,
                      patientInfo.Patient.RenrakuName,
                      patientInfo.Patient.RenrakuPost,
                      patientInfo.Patient.RenrakuAddress1,
                      patientInfo.Patient.RenrakuAddress2,
                      patientInfo.Patient.RenrakuTel,
                      patientInfo.Patient.RenrakuMemo,
                      patientInfo.Patient.OfficeName,
                      patientInfo.Patient.OfficePost,
                      patientInfo.Patient.OfficeAddress1,
                      patientInfo.Patient.OfficeAddress2,
                      patientInfo.Patient.OfficeTel,
                      patientInfo.Patient.OfficeMemo,
                      patientInfo.Patient.IsRyosyoDetail,
                      patientInfo.Patient.PrimaryDoctor,
                      patientInfo.Patient.IsTester,
                      patientInfo.Patient.MainHokenPid,
                      patientInfo.Patient.ReferenceNo,
                      patientInfo.Patient.LimitConsFlg,
                      patientInfo.Patient.Memo);

            List<HokenInfModel> hokenInfs = patientInfo.HokenInfs.Select(x => new HokenInfModel(HpId,
                                                                           x.PtId,
                                                                           x.HokenId,
                                                                           x.SeqNo,
                                                                           x.HokenNo,
                                                                           x.HokenEdaNo,
                                                                           x.HokenKbn,
                                                                           x.HokensyaNo,
                                                                           (!string.IsNullOrEmpty(x.Kigo) && string.IsNullOrEmpty(x.Kigo.Replace(" ", "").Replace("　", ""))) ? string.Empty : x.Kigo,
                                                                           x.Bango,
                                                                           x.EdaNo,
                                                                           x.HonkeKbn,
                                                                           x.StartDate,
                                                                           x.EndDate,
                                                                           x.SikakuDate,
                                                                           x.KofuDate,
                                                                           x.ConfirmDates.OrderByDescending(item => item.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0, // get confirm date from confirmDateList
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
                                                                           x.RousaiRoudouCd,
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
                                                                           x.SinDate,
                                                                           x.JibaiHokenName,
                                                                           x.JibaiHokenTanto,
                                                                           x.JibaiHokenTel,
                                                                           x.JibaiJyusyouDate,
                                                                           x.Houbetu,
                                                                           x.ConfirmDates.Select(c => new ConfirmDateModel(
                                                                               c.HokenGrp,
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
                                                                           false)).ToList();

            List<KohiInfModel> hokenKohis = patientInfo.HokenKohis.Select(x => new KohiInfModel(
                                            x.ConfirmDates.Select(c =>
                                                new ConfirmDateModel(
                                                    c.HokenGrp,
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
                                            x.ConfirmDates.OrderByDescending(item => item.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0, // get confirm date from confirmDateList
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
                                            x.SinDate,
                                            false,
                                            x.IsDeleted,
                                            x.SeqNo,
                                            x.IsAddNew)).ToList();

            List<InsuranceModel> insurances = patientInfo.Insurances.Select(x => new InsuranceModel(HpId,
                       x.PtId,
                       0,
                       x.SeqNo,
                       x.HokenSbtCd,
                       x.HokenPid,
                       x.HokenKbn,
                       x.HokenMemo,
                       x.SinDate,
                       x.StartDate,
                       x.EndDate,
                       hokenInfs.FirstOrDefault(h => h.HokenId == x.HokenId) ?? new(),
                       hokenKohis.FirstOrDefault(k => k.HokenId == x.Kohi1Id) ?? new(),
                       hokenKohis.FirstOrDefault(k => k.HokenId == x.Kohi2Id) ?? new(),
                       hokenKohis.FirstOrDefault(k => k.HokenId == x.Kohi3Id) ?? new(),
                       hokenKohis.FirstOrDefault(k => k.HokenId == x.Kohi4Id) ?? new(),
                       x.IsAddNew,
                       x.IsDeleted,
                       x.HokenPatternSelected)).ToList();

            List<GroupInfModel> grpInfs = patientInfo.PtGrps.Select(x => new GroupInfModel(
                                                x.HpPt,
                                                x.PtId,
                                                x.GroupId,
                                                x.GroupCode,
                                                x.GroupName)).ToList();



            var insuranceScans = request.ImageScans.Select(x => new InsuranceScanModel(
                                                                    HpId,
                                                                    0,
                                                                    x.SeqNo,
                                                                    x.HokenGrp,
                                                                    x.HokenId,
                                                                    x.FileName,
                                                                    x.File == null ? Stream.Null : x.File.OpenReadStream(),
                                                                    x.IsDeleted,
                                                                    string.Empty)).ToList();

            List<int> hokenIdList = new();
            if (patientInfo.ReactSave.ConfirmCloneByomei)
            {
                hokenIdList = patientInfo.HokenIdList;
            }

            var input = new SavePatientInfoInputData(patient,
                 patientInfo.PtKyuseis.Select(item => new PtKyuseiModel(HpId, item.PtId, item.SeqNo, item.KanaName, item.Name, item.EndDate)).ToList(),
                 patientInfo.PtSanteis,
                 insurances,
                 hokenInfs,
                 hokenKohis,
                 grpInfs,
                 patientInfo.ReactSave,
                 patientInfo.MaxMoneys,
                 insuranceScans,
                 hokenIdList,
                 UserId,
                 HpId
                 );
            var output = _bus.Handle(input);

            if (output.Status == SavePatientInfoStatus.Successful)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.PatientInfChanged, new PatientInforMessage(output.PatientInforModel));
            }

            var presenter = new SavePatientInfoPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SavePatientInfoResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.DeletePatientInfo)]
        public async Task<ActionResult<Response<DeletePatientInfoResponse>>> DeletePatientInfo([FromBody] DeletePatientInfoRequest request)
        {
            var input = new DeletePatientInfoInputData(HpId, request.PtId, UserId);
            var output = _bus.Handle(input);

            if (output.Status == DeletePatientInfoStatus.Successful)
            {
                await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.ReceptionInfos, output.SameVisitList));
            }

            var presenter = new DeletePatientInfoPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<DeletePatientInfoResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ValidateListPattern)]
        public ActionResult<Response<ValidateListInsuranceResponse>> ValidateListPattern([FromBody] ValidateInsuranceRequest request)
        {
            var input = new ValidateInsuranceInputData(HpId, request.SinDate, request.PtBirthday, request.ListInsurance);
            var output = _bus.Handle(input);
            var presenter = new ValidateInsurancePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<ValidateListInsuranceResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SwapHoken)]
        public ActionResult<Response<SaveSwapHokenResponse>> SwapHokenParttern([FromBody] SaveSwapHokenRequest request)
        {
            var input = new SaveSwapHokenInputData(HpId,
                                                   request.PtId,
                                                   request.HokenIdBefore,
                                                   request.HokenNameBefore,
                                                   request.HokenIdAfter,
                                                   request.HokenNameAfter,
                                                   request.HokenPidBefore,
                                                   request.HokenPidAfter,
                                                   request.StartDate,
                                                   request.EndDate,
                                                   request.IsHokenPatternUsed,
                                                   request.ConfirmInvalidIsShowConversionCondition,
                                                   request.ConfirmSwapHoken,
                                                   UserId,
                                                   request.IsReCalculation,
                                                   request.IsReceCalculation,
                                                   request.IsReceCheckError);
            var output = _bus.Handle(input);
            var presenter = new SaveSwapHokenPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveSwapHokenResponse>>(presenter.Result);
        }

        private PatientAdvancedSearchInput ConvertToPatientAdvancedSearchInput(SearchPatientInfoAdvancedRequest request)
        {
            return new PatientAdvancedSearchInput(
                    request.FromPtNum,
                    request.ToPtNum,
                    request.Name,
                    request.Sex,
                    request.FromAge,
                    request.ToAge,
                    request.FromBirthDay,
                    request.ToBirthDay,
                    request.PostalCode1,
                    request.PostalCode2,
                    request.Address,
                    request.PhoneNum,
                    request.FromVisitDate,
                    request.ToVisitDate,
                    request.FromLastVisitDate,
                    request.ToLastVisitDate,
                    request.FromInsuranceNum,
                    request.ToInsuranceNum,
                    request.FromPublicExpensesNum,
                    request.ToPublicExpensesNum,
                    request.FromSpecialPublicExpensesNum,
                    request.ToSpecialPublicExpensesNum,
                    request.HokenNum,
                    request.Kohi1Num,
                    request.Kohi1EdaNo,
                    request.Kohi2Num,
                    request.Kohi2EdaNo,
                    request.Kohi3Num,
                    request.Kohi3EdaNo,
                    request.Kohi4Num,
                    request.Kohi4EdaNo,
                    request.PatientGroups,
                    request.OrderLogicalOperator,
                    request.DepartmentId,
                    request.DoctorId,
                    request.ByomeiLogicalOperator,
                    request.Byomeis,
                    request.TenMsts,
                    request.ByomeiStartDate,
                    request.ByomeiEndDate,
                    request.ResultKbn,
                    request.IsSuspectedDisease,
                    request.IsOrderOr
                );
        }

        [HttpPost(ApiPath.ValidHokenInfAllType)]
        public ActionResult<Response<ValidHokenInfAllTypeResponse>> ValidHokenInfAllType([FromBody] ValidHokenInfAllTypeRequest request)
        {
            var input = new ValidHokenInfAllTypeInputData(HpId,
                                            request.HokenKbn,
                                            request.SinDate,
                                            request.IsSelectedHokenInf,
                                            request.SelectedHokenInfRodoBango,
                                            request.ListRousaiTenki,
                                            request.SelectedHokenInfRousaiSaigaiKbn,
                                            request.SelectedHokenInfRousaiSyobyoDate,
                                            request.SelectedHokenInfRousaiSyobyoCd,
                                            request.SelectedHokenInfRyoyoStartDate,
                                            request.SelectedHokenInfRyoyoEndDate,
                                            request.SelectedHokenInfStartDate,
                                            request.SelectedHokenInfEndDate,
                                            request.SelectedHokenInfIsAddNew,
                                            request.SelectedHokenInfNenkinBango,
                                            request.SelectedHokenInfKenkoKanriBango,
                                            request.SelectedHokenInfConfirmDate,
                                            request.SelectedHokenInfHokenMasterModelIsNull,
                                            request.SelectedHokenInf,
                                            request.SelectedHokenInfTokki1,
                                            request.SelectedHokenInfTokki2,
                                            request.SelectedHokenInfTokki3,
                                            request.SelectedHokenInfTokki4,
                                            request.SelectedHokenInfTokki5,
                                            request.SelectedHokenInfHoubetu,
                                            request.SelectedHokenInfIsJihi,
                                            request.HokenSyaNo,
                                            request.SelectedHokenInfKigo,
                                            request.SelectedHokenInfBango,
                                            request.SelectedHokenInfTokureiYm1,
                                            request.SelectedHokenInfTokureiYm2,
                                            request.SelectedHokenInfisShahoOrKokuho,
                                            request.SelectedHokenInfisExpirated,
                                            request.SelectedHokenInfHokenNo,
                                            request.SelectedHokenInfHokenEdraNo,
                                            request.IsSelectedHokenMst,
                                            request.SelectedHokenInfHonkeKbn,
                                            request.PtBirthday,
                                            request.SelectedHokenInfIsAddHokenCheck,
                                            request.SelectedHokenInfHokenChecksCount,
                                            request.HokenInfIsNoHoken,
                                            request.HokenInfConfirmDate);

            var output = _bus.Handle(input);

            var presenter = new ValidHokenInfAllTypePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidHokenInfAllTypeResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CheckHokenPatternUsed)]
        public ActionResult<Response<CheckHokenPatternUsedResponse>> CheckHokenPatternUsed([FromBody] CheckHokenPatternUsedRequest request)
        {
            var input = new HokenPatternUsedInputData(HpId,
               request.PtId,
               request.HokenPid);
            var output = _bus.Handle(input);
            var presenter = new CheckHokenPatternUsedPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CheckHokenPatternUsedResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetKohiPriorityList)]
        public ActionResult<Response<GetKohiPriorityListResponse>> GetKohiPriorityList()
        {
            var input = new GetKohiPriorityListInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetKohiPriorityListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetKohiPriorityListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetHokenSyaMst)]
        public ActionResult<Response<GetHokenSyaMstResponse>> GetHokenSyaMst([FromQuery] GetHokenSyaMstRequest request)
        {
            var input = new GetHokenSyaMstInputData(HpId, request.HokensyaNo, request.HokenKbn);
            var output = _bus.Handle(input);
            var presenter = new GetHokenSyaMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetHokenSyaMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveGroupNameMst)]
        public ActionResult<Response<SaveGroupNameMstResponse>> SaveGroupNameMst([FromBody] SaveGroupNameMstRequest request)
        {
            var inputModel = request.PtGroupMsts.Select(x => new GroupNameMstModel(
                                                            x.GrpId,
                                                            x.SortNo,
                                                            x.GrpName,
                                                            0,
                                                            x.GroupItems.Select(m => new GroupItemModel(
                                                                m.GrpId,
                                                                m.GrpCode,
                                                                m.SeqNo,
                                                                m.GrpCodeName,
                                                                m.SortNo,
                                                                0)).ToList())).ToList();

            var input = new SaveGroupNameMstInputData(UserId, HpId, inputModel);
            var output = _bus.Handle(input);
            var presenter = new SaveGroupNameMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveGroupNameMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetListPatientInfoResponse>> GetList([FromQuery] GetListPatientInfoRequest req)
        {
            var input = new GetListPatientInfoInputData(HpId, req.PtId, req.PageIndex, req.PageSize);
            var output = _bus.Handle(input);
            var presenter = new GetListPatientInfoPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListPatientInfoResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetMaxMoneyByPtId)]
        public ActionResult<Response<GetMaxMoneyByPtIdResponse>> GetMaxMoneyByPtId([FromQuery] GetMaxMoneyByPtIdRequest request)
        {
            var input = new GetMaxMoneyByPtIdInputData(HpId, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new GetMaxMoneyByPtIdPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetMaxMoneyByPtIdResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ValidateSwapHoken)]
        public ActionResult<Response<ValidateSwapHokenResponse>> ValidateSwapHoken([FromBody] ValidateSwapHokenRequest request)
        {
            var input = new ValidateSwapHokenInputData(HpId,
               request.PtId,
               request.HokenPid,
               request.StartDate,
               request.EndDate);
            var output = _bus.Handle(input);
            var presenter = new ValidateSwapHokenPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<ValidateSwapHokenResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetGroupNameMst)]
        public ActionResult<Response<GetGroupNameMstResponse>> GetGroupNameMst()
        {
            var input = new GetGroupNameMstInputData(HpId, true);
            var output = _bus.Handle(input);
            var presenter = new GetGroupNameMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetGroupNameMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetListGroupInfo)]
        public ActionResult<Response<GetGroupNameMstResponse>> GetListGroupInfo()
        {
            var input = new GetGroupNameMstInputData(HpId, false);
            var output = _bus.Handle(input);
            var presenter = new GetGroupNameMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetGroupNameMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CheckAllowDeleteGroupMst)]
        public ActionResult<Response<CheckAllowDeleteGroupMstResponse>> CheckAllowDeleteGroupMst([FromBody] CheckAllowDeleteGroupMstRequest request)
        {
            var input = new CheckAllowDeleteGroupMstInputData(HpId, request.GroupId, request.GroupCode, request.CheckAllowDeleteGroupName);
            var output = _bus.Handle(input);
            var presenter = new CheckAllowDeleteGroupMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CheckAllowDeleteGroupMstResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetPatientInfoBetweenTimesList)]
        public ActionResult<Response<GetPatientInfoBetweenTimesListResponse>> GetPatientInfoBetweenTimesList([FromQuery] GetPatientInfoBetweenTimesListRequest request)
        {
            var input = new GetPatientInfoBetweenTimesListInputData(HpId, request.SinYm, request.StartDateD, request.StartTimeH, request.StartTimeM, request.EndDateD, request.EndTimeH, request.EndTimeM);
            var output = _bus.Handle(input);
            var presenter = new GetPatientInfoBetweenTimesListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetPatientInfoBetweenTimesListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.SearchPatientInfoByPtNum)]
        public ActionResult<Response<SearchPatientInfoByPtNumResponse>> SearchPatientInfoByPtNum([FromQuery] SearchPatientInfoByPtNumRequest request)
        {
            var input = new SearchPatientInfoByPtNumInputData(HpId, request.PtNum, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new SearchPatientInfoByPtNumPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SearchPatientInfoByPtNumResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetTokkiMstList)]
        public ActionResult<Response<GetTokkiMstListResponse>> GetTokkiMstList([FromQuery] GetTokkiMstListRequest request)
        {
            var input = new GetTokkiMstListInputData(request.SeikyuYm);
            var output = _bus.Handle(input);
            var presenter = new GetTokkiMstListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTokkiMstListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetPtInfByRefNo)]
        public ActionResult<Response<GetPtInfByRefNoResponse>> GetPtInfByRefNo([FromQuery] GetPtInfByRefNoRequest request)
        {
            var input = new GetPtInfByRefNoInputData(HpId, request.RefNo);
            var output = _bus.Handle(input);
            var presenter = new GetPtInfByRefNoPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetPtInfByRefNoResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetPtInfModelsByName)]
        public ActionResult<Response<GetPtInfModelsByNameResponse>> GetPtInfModelsByName([FromQuery] GetPtInfModelsByNameRequest request)
        {
            var input = new GetPtInfModelsByNameInputData(HpId, request.KanaName, request.Name, request.BirthDate, request.Sex1, request.Sex2);
            var output = _bus.Handle(input);
            var presenter = new GetPtInfModelsByNamePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetPtInfModelsByNameResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetPtInfModelsByRefNo)]
        public ActionResult<Response<GetPtInfModelsByRefNoResponse>> GetPtInfModelsByRefNo([FromQuery] GetPtInfModelsByRefNoRequest request)
        {
            var input = new GetPtInfModelsByRefNoInputData(HpId, request.RefNo);
            var output = _bus.Handle(input);
            var presenter = new GetPtInfModelsByRefNoPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetPtInfModelsByRefNoResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CalculationSwapHoken)]
        public void CalculationSwapHoken([FromBody] CalculationSwapHokenRequest request, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            try
            {
                _messenger.Register<CalculationSwapHokenMessageStatus>(this, UpdateCalculationSwapHokenStatus);
                _messenger.Register<StopCalcStatus>(this, StopCalculationCaculaleSwapHoken);
                HttpContext.Response.ContentType = "application/json";

                var input = new CalculationSwapHokenInputData(HpId, UserId, request.SeikyuYms, request.PtId, request.IsReCalculation, request.IsReceCalculation, request.IsReceCheckError, _messenger);
                var output = _bus.Handle(input);
                if (output.Status == CalculationSwapHokenStatus.Successful)
                    UpdateCalculationSwapHokenStatus(new CalculationSwapHokenMessageStatus(string.Empty, 100, true, true));
                else
                    UpdateCalculationSwapHokenStatus(new CalculationSwapHokenMessageStatus(output.ErrorText, 100, true, false));

            }
            finally
            {
                _messenger.Deregister<CalculationSwapHokenMessageStatus>(this, UpdateCalculationSwapHokenStatus);
                _messenger.Deregister<StopCalcStatus>(this, StopCalculationCaculaleSwapHoken);
            }
        }

        [HttpPost(ApiPath.CheckValidSamePatient)]
        public ActionResult<Response<CheckValidSamePatientResponse>> CheckValidSamePatient([FromBody] CheckValidSamePatientRequest request)
        {
            var input = new CheckValidSamePatientInputData(HpId,
                                                           request.PtId,
                                                           request.KanjiName,
                                                           request.Birthday,
                                                           request.Sex);
            var output = _bus.Handle(input);
            var presenter = new CheckValidSamePatientPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CheckValidSamePatientResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SavePtKyusei)]
        public ActionResult<Response<SavePtKyuseiResponse>> SavePtKyuseiPatient([FromBody] SavePtKyuseiRequest request)
        {
            var input = new SavePtKyuseiInputData(HpId,
                                                  UserId,
                                                  request.PtId,
                                                  request.PtKyuseiList.Select(item => new PtKyuseiItem(
                                                           HpId,
                                                           request.PtId,
                                                           item.SeqNo,
                                                           item.KanaName,
                                                           item.Name,
                                                           item.EndDate,
                                                           item.IsDeleted)).ToList());
            var output = _bus.Handle(input);
            var presenter = new SavePtKyuseiPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SavePtKyuseiResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CheckAllowDeletePatientInfo)]
        public ActionResult<Response<CheckAllowDeletePatientInfoResponse>> CheckAllowDeletePatientInfo([FromBody] CheckAllowDeletePatientInfoRequest request)
        {
            var input = new CheckAllowDeletePatientInfoInputData(HpId, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new CheckAllowDeletePatientInfoPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CheckAllowDeletePatientInfoResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SearchPatientInfoByPtIdList)]
        public ActionResult<Response<SearchPatientInfoByPtIdListResponse>> SearchPatientInfoByPtIdList([FromBody] SearchPatientInfoByPtIdListRequest request)
        {
            var input = new SearchPatientInfoByPtIdListInputData(HpId, request.PtIdList);
            var output = _bus.Handle(input);
            var presenter = new SearchPatientInfoByPtIdListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SearchPatientInfoByPtIdListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetVisitTimesManagementModels)]
        public ActionResult<Response<GetVisitTimesManagementModelsResponse>> GetVisitTimesManagementModels([FromQuery] GetVisitTimesManagementModelsRequest request)
        {
            var input = new GetVisitTimesManagementModelsInputData(HpId, request.SinYm, request.PtId, request.KohiId);
            var output = _bus.Handle(input);
            var presenter = new GetVisitTimesManagementModelsPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetVisitTimesManagementModelsResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateVisitTimesManagement)]
        public ActionResult<Response<UpdateVisitTimesManagementResponse>> UpdateVisitTimesManagement([FromBody] UpdateVisitTimesManagementRequest request)
        {
            var input = new UpdateVisitTimesManagementInputData(HpId, UserId, request.SinYm, request.PtId, request.KohiId, ConvertToVisitTimesManagementList(request));
            var output = _bus.Handle(input);
            var presenter = new UpdateVisitTimesManagementPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateVisitTimesManagementResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.UpdateVisitTimesManagementNeedSave)]
        public ActionResult<Response<UpdateVisitTimesManagementNeedSaveResponse>> UpdateVisitTimesManagementNeedSave([FromBody] UpdateVisitTimesManagementNeedSaveRequest request)
        {
            var input = new UpdateVisitTimesManagementNeedSaveInputData(HpId, UserId, request.PtId, ConvertToVisitTimesManagementList(request));
            var output = _bus.Handle(input);
            var presenter = new UpdateVisitTimesManagementNeedSavePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateVisitTimesManagementNeedSaveResponse>>(presenter.Result);
        }

        private List<VisitTimesManagementModel> ConvertToVisitTimesManagementList(UpdateVisitTimesManagementRequest request)
        {
            var result = request.VisitTimesManagementList.Select(item => new VisitTimesManagementModel(
                                                                             request.PtId,
                                                                             item.SinDate,
                                                                             item.HokenPid,
                                                                             request.KohiId,
                                                                             item.SeqNo,
                                                                             item.SortKey,
                                                                             item.IsDeleted
                                                          )).ToList();
            return result;
        }

        private List<VisitTimesManagementModel> ConvertToVisitTimesManagementList(UpdateVisitTimesManagementNeedSaveRequest request)
        {
            var result = request.VisitTimesManagementList.Select(item => new VisitTimesManagementModel(
                                                                             request.PtId,
                                                                             item.SinDate,
                                                                             0,
                                                                             item.KohiId,
                                                                             0,
                                                                             item.SortKey,
                                                                             false
                                                          )).ToList();
            return result;
        }

        private void StopCalculationCaculaleSwapHoken(StopCalcStatus stopCalcStatus)
        {
            if (!_cancellationToken.HasValue)
            {
                stopCalcStatus.CallFailCallback(false);
            }
            else
            {
                stopCalcStatus.CallSuccessCallback(_cancellationToken!.Value.IsCancellationRequested);
            }
        }

        private void UpdateCalculationSwapHokenStatus(CalculationSwapHokenMessageStatus status)
        {
            StringBuilder titleProgressbar = new();
            titleProgressbar.Append("\n{ displayText: \"");
            titleProgressbar.Append(status.DisplayText);
            titleProgressbar.Append("\", percent: ");
            titleProgressbar.Append(status.Percent);
            titleProgressbar.Append(", complete: ");
            titleProgressbar.Append(status.Complete.ToString().ToLower());
            titleProgressbar.Append(", completeSuccess: ");
            titleProgressbar.Append(status.CompleteSuccess.ToString().ToLower());
            titleProgressbar.Append(" }");
            var resultForFrontEnd = Encoding.UTF8.GetBytes(titleProgressbar.ToString());
            HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
            HttpContext.Response.Body.FlushAsync();
        }

        private Dictionary<string, string> ConvertToSortData(List<SortCol> sortColList)
        {
            Dictionary<string, string> result = new();
            foreach (var item in sortColList)
            {
                if (!result.ContainsKey(item.Col))
                {
                    result.Add(item.Col, item.Type);
                    continue;
                }
                result[item.Col] = item.Type;
            }
            return result;
        }
    }
}
