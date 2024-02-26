using Domain.Models.MainMenu;
using Domain.Models.PatientInfor;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.HokenMst;
using EmrCloudApi.Presenters.PatientManagement;
using EmrCloudApi.Requests.PatientManagement;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.HokenMst;
using EmrCloudApi.Responses.PatientManagement;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.HokenMst.GetHokenMst;
using UseCase.PatientManagement.GetStaConf;
using UseCase.PatientManagement.SaveStaConf;
using UseCase.PatientManagement.SearchPtInfs;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class PatientManagementController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;

        public PatientManagementController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.SearchPtInfs)]
        public ActionResult<Response<SearchPtInfsResponse>> SearchPtInfs([FromBody] SearchPtInfsRequest request)
        {
            var input = new SearchPtInfsInputData(HpId, request.OutputOrder, request.CoSta9000PtConf, request.CoSta9000HokenConf,
                                                   request.CoSta9000ByomeiConf, request.CoSta9000RaiinConf, request.CoSta9000SinConf, request.CoSta9000KarteConf, request.CoSta9000KensaConf);
            var output = _bus.Handle(input);

            var presenter = new SearchPtInfsPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SearchPtInfsResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetHokenMst)]
        public ActionResult<Response<GetHokenMstResponse>> GetHokenMst()
        {
            var input = new GetHokenMstInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetHokenMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetHokenMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveStaConfMenu)]
        public ActionResult<Response<SaveStaConfMenuResponse>> SaveStaConfMenu([FromBody] SaveStaConfMenuRequest request)
        {
            var input = new SaveStaConfMenuInputData(HpId, UserId, ConvertStatisticMenuModel(request.StaConfMenu));
            var output = _bus.Handle(input);

            var presenter = new SaveStaConfMenuPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveStaConfMenuResponse>>(presenter.Result);
        }

        private StatisticMenuModel ConvertStatisticMenuModel(StaConfMenuRequestItem staConf)
        {
            var ptManagement = staConf.PatientManagement;
            return new StatisticMenuModel(
                                          staConf.MenuId,
                                          staConf.GrpId,
                                          staConf.ReportId,
                                          staConf.MenuName ?? string.Empty,
                                          staConf.SortNo,
                                          staConf.IsDeleted,
                                          staConf.IsModified,
                                          new PatientManagementModel(
                                              ptManagement.OutputOrder1,
                                              ptManagement.OutputOrder2,
                                              ptManagement.OutputOrder3,
                                              ptManagement.ReportType,
                                              ptManagement.PtNumFrom,
                                              ptManagement.PtNumTo,
                                              ptManagement.KanaName,
                                              ptManagement.Name,
                                              ptManagement.BirthDayFrom,
                                              ptManagement.BirthDayTo,
                                              ptManagement.AgeFrom,
                                              ptManagement.AgeTo,
                                              ptManagement.AgeRefDate,
                                              ptManagement.Sex,
                                              ptManagement.HomePost,
                                              ptManagement.ZipCD1,
                                              ptManagement.ZipCD2,
                                              ptManagement.Address,
                                              ptManagement.PhoneNumber,
                                              ptManagement.IncludeTestPt,
                                              ptManagement.ListPtNums,
                                              ptManagement.RegistrationDateFrom,
                                              ptManagement.RegistrationDateTo,
                                              ptManagement.GroupSelected,
                                              ptManagement.HokensyaNoFrom,
                                              ptManagement.HokensyaNoTo,
                                              ptManagement.Kigo,
                                              ptManagement.Bango,
                                              ptManagement.EdaNo,
                                              ptManagement.HokenKbn,
                                              ptManagement.KohiFutansyaNoFrom,
                                              ptManagement.KohiFutansyaNoTo,
                                              ptManagement.KohiTokusyuNoFrom,
                                              ptManagement.KohiTokusyuNoTo,
                                              ptManagement.ExpireDateFrom,
                                              ptManagement.ExpireDateTo,
                                              ptManagement.HokenSbt,
                                              ptManagement.Houbetu1,
                                              ptManagement.Houbetu2,
                                              ptManagement.Houbetu3,
                                              ptManagement.Houbetu4,
                                              ptManagement.Houbetu5,
                                              ptManagement.Kogaku,
                                              ptManagement.KohiHokenNoFrom,
                                              ptManagement.KohiHokenEdaNoFrom,
                                              ptManagement.KohiHokenNoTo,
                                              ptManagement.KohiHokenEdaNoTo,
                                              ptManagement.ValidOrExpired, // 有効/期限切れ
                                              ptManagement.StartDateFrom,
                                              ptManagement.StartDateTo,
                                              ptManagement.TenkiDateFrom,
                                              ptManagement.TenkiDateTo,
                                              ptManagement.IsDoubt,
                                              ptManagement.SearchWord,
                                              ptManagement.SearchWordMode,
                                              ptManagement.ByomeiCdOpt,
                                              ptManagement.SindateFrom,
                                              ptManagement.SindateTo,
                                              ptManagement.LastVisitDateFrom,
                                              ptManagement.LastVisitDateTo,
                                              ptManagement.IsSinkan,
                                              ptManagement.RaiinAgeFrom,
                                              ptManagement.RaiinAgeTo,
                                              ptManagement.DataKind,
                                              ptManagement.ItemCdOpt,
                                              ptManagement.MedicalSearchWord,
                                              ptManagement.WordOpt,
                                              ptManagement.KarteWordOpt,
                                              ptManagement.StartIraiDate,
                                              ptManagement.EndIraiDate,
                                              ptManagement.KensaItemCdOpt,
                                              ptManagement.TenkiKbns,
                                              ptManagement.SikkanKbns,
                                              ptManagement.ByomeiCds,
                                              ptManagement.FreeByomeis,
                                              ptManagement.NanbyoCds,
                                              ptManagement.Statuses,
                                              ptManagement.UketukeSbtId,
                                              ptManagement.ItemCds,
                                              ptManagement.KaMstId,
                                              ptManagement.UserMstId,
                                              ptManagement.JikanKbns,
                                              ptManagement.ItemCmts,
                                              ptManagement.KarteKbns,
                                              ptManagement.KarteSearchWords,
                                              ptManagement.KensaItemCds
                                              )
                                          );
        }

        [HttpGet(ApiPath.GetStaConfMenu)]
        public ActionResult<Response<GetStaConfMenuResponse>> GetStaConfMenu()
        {
            var input = new GetStaConfMenuInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetStaConfMenuPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetStaConfMenuResponse>>(presenter.Result);
        }
    }
}
