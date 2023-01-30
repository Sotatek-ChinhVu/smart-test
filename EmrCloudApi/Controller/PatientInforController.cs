using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.CalculationInf;
using EmrCloudApi.Presenters.GroupInf;
using EmrCloudApi.Presenters.HokenMst;
using EmrCloudApi.Presenters.Insurance;
using EmrCloudApi.Presenters.InsuranceList;
using EmrCloudApi.Presenters.InsuranceMst;
using EmrCloudApi.Presenters.KohiHokenMst;
using EmrCloudApi.Presenters.PatientInfor;
using EmrCloudApi.Presenters.PatientInfor.InsuranceMasterLinkage;
using EmrCloudApi.Presenters.PatientInfor.PtKyusei;
using EmrCloudApi.Presenters.PatientInformation;
using EmrCloudApi.Presenters.PtGroupMst;
using EmrCloudApi.Presenters.SwapHoken;
using EmrCloudApi.Requests.CalculationInf;
using EmrCloudApi.Requests.GroupInf;
using EmrCloudApi.Requests.HokenMst;
using EmrCloudApi.Requests.Insurance;
using EmrCloudApi.Requests.InsuranceMst;
using EmrCloudApi.Requests.KohiHokenMst;
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
using EmrCloudApi.Responses.PatientInfor;
using EmrCloudApi.Responses.PatientInfor.InsuranceMasterLinkage;
using EmrCloudApi.Responses.PatientInfor.PtKyuseiInf;
using EmrCloudApi.Responses.PatientInformaiton;
using EmrCloudApi.Responses.PtGroupMst;
using EmrCloudApi.Responses.SwapHoken;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
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
using UseCase.Insurance.ValidPatternOther;
using UseCase.InsuranceMst.Get;
using UseCase.InsuranceMst.GetHokenSyaMst;
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
using UseCase.PtGroupMst.SaveGroupNameMst;
using UseCase.SearchHokensyaMst.Get;
using UseCase.SwapHoken.Save;
using EmrCloudApi.Responses.MaxMoney;
using EmrCloudApi.Requests.MaxMoney;
using UseCase.MaxMoney.GetMaxMoneyByPtId;
using EmrCloudApi.Presenters.MaxMoney;
using Domain.Models.PtGroupMst;
using UseCase.PtGroupMst.GetGroupNameMst;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class PatientInforController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public PatientInforController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.Get + "PatientComment")]
        public ActionResult<Response<GetPatientCommentResponse>> GetList([FromQuery] GetPatientCommentRequest request)
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
            var input = new GetPatientInforByIdInputData(HpId, request.PtId, request.SinDate, request.RaiinNo);
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
            var input = new SearchPatientInfoSimpleInputData(request.Keyword, request.IsContainMode, HpId, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var present = new SearchPatientInfoSimplePresenter();
            present.Complete(output);

            return new ActionResult<Response<SearchPatientInforSimpleResponse>>(present.Result);
        }

        [HttpPost("SearchAdvanced")]
        public ActionResult<Response<SearchPatientInfoAdvancedResponse>> GetList([FromBody] SearchPatientInfoAdvancedRequest request)
        {
            var input = new SearchPatientInfoAdvancedInputData(ConvertToPatientAdvancedSearchInput(request), HpId, request.PageIndex, request.PageSize);
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
            var input = new GetListPatientGroupMstInputData();
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
            var input = new ValidKohiInputData(request.SinDate, request.PtBirthday, request.IsKohiEmptyModel, request.IsSelectedKohiMst, request.SelectedKohiFutansyaNo, request.SelectedKohiJyukyusyaNo,
                request.SelectedKohiTokusyuNo, request.SelectedKohiStartDate, request.SelectedKohiEndDate, request.SelectedKohiConfirmDate, request.SelectedKohiHokenNo, request.SelectedKohiHokenEdraNo, request.SelectedKohiIsAddNew, request.SelectedHokenPatternIsExpirated);
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

        [HttpPost(ApiPath.ValidateInsuranceOther)]
        public ActionResult<Response<ValidInsuranceOtherResponse>> ValidateInsuranceOther([FromBody] ValidInsuranceOtherRequest request)
        {
            var input = new ValidInsuranceOtherInputData(request.ValidModel);
            var output = _bus.Handle(input);

            var presenter = new ValidInsuranceOtherPresenter();

            presenter.Complete(output);
            return new ActionResult<Response<ValidInsuranceOtherResponse>>(presenter.Result);
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
        public ActionResult<Response<SavePatientInfoResponse>> SavePatientInfo([FromBody] SavePatientInfoRequest request)
        {
            PatientInforSaveModel patient = new PatientInforSaveModel(HpId,
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

            List<InsuranceModel> insurances = request.Insurances.Select(x => new InsuranceModel(HpId,
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
                       x.HokenId,
                       x.Kohi1Id,
                       x.Kohi2Id,
                       x.Kohi3Id,
                       x.Kohi4Id,
                       x.IsAddNew,
                       x.IsDeleted,
                       x.HokenPatternSelected)).ToList();

            List<GroupInfModel> grpInfs = request.PtGrps.Select(x => new GroupInfModel(
                                                x.HpPt,
                                                x.PtId,
                                                x.GroupId,
                                                x.GroupCode,
                                                x.GroupName)).ToList();

            List<HokenInfModel> hokenInfs = request.HokenInfs.Select(x => new HokenInfModel(HpId,
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
                   0,
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

            List<KohiInfModel> hokenKohis = request.HokenKohis.Select(x => new KohiInfModel(
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
                 grpInfs,
                 request.ReactSave,
                 request.MaxMoneys,
                 UserId);
            var output = _bus.Handle(input);
            var presenter = new SavePatientInfoPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SavePatientInfoResponse>>(presenter.Result);
        }

        [HttpPost("DeletePatientInfo")]
        public ActionResult<Response<DeletePatientInfoResponse>> DeletePatientInfo([FromBody] DeletePatientInfoRequest request)
        {
            var input = new DeletePatientInfoInputData(HpId, request.PtId, UserId);
            var output = _bus.Handle(input);
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
               request.HokenIdAfter,
               request.HokenPidBefore,
               request.HokenPidAfter,
               request.StartDate,
               request.EndDate,
               UserId);
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
                    request.OrderItemCodes,
                    request.DepartmentId,
                    request.DoctorId,
                    request.ByomeiLogicalOperator,
                    request.Byomeis,
                    request.ByomeiStartDate,
                    request.ByomeiEndDate,
                    request.ResultKbn,
                    request.IsSuspectedDisease
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
            var input = new GetKohiPriorityListInputData();
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


        [HttpGet(ApiPath.GetMaxMoneyByPtId)]
        public ActionResult<Response<GetMaxMoneyByPtIdResponse>> GetMaxMoneyByPtId([FromQuery] GetMaxMoneyByPtIdRequest request)
        {
            var input = new GetMaxMoneyByPtIdInputData(HpId, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new GetMaxMoneyByPtIdPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetMaxMoneyByPtIdResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetGroupNameMst)]
        public ActionResult<Response<GetGroupNameMstResponse>> GetGroupNameMst()
        {
            var input = new GetGroupNameMstInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetGroupNameMstPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetGroupNameMstResponse>>(presenter.Result);
        }
    }
}