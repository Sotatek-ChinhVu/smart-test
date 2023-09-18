using Domain.Models.MainMenu;
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
using UseCase.PatientManagement.SaveStaConf;
using UseCase.PatientManagement.SearchPtInfs;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class PatientManagementController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public PatientManagementController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.SearchPtInfs)]
        public ActionResult<Response<SearchPtInfsResponse>> SearchPtInfs([FromBody] SearchPtInfsRequest request)
        {
            var input = new SearchPtInfsInputData(HpId, request.OutputOrder, request.PageIndex, request.PageCount, request.CoSta9000PtConf, request.CoSta9000HokenConf,
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

        private StatisticMenuModel ConvertStatisticMenuModel(SaveStaConfMenuRequestItem staConf)
        {
            return new StatisticMenuModel(
                                          staConf.StaConfMenu.MenuId,
                                          staConf.StaConfMenu.GrpId,
                                          staConf.StaConfMenu.ReportId,
                                          staConf.StaConfMenu.SortNo,
                                          staConf.StaConfMenu.IsDeleted,
                                          staConf.StaConfMenu.IsModified,
                                          staConf.StaConfMenu.PatientManagement
                                          );
        }
    }
}
