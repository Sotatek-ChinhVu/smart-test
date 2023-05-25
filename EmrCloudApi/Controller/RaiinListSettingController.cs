using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Responses.RaiinListSetting;
using UseCase.RaiinListSetting.GetDocCategory;
using EmrCloudApi.Presenters.RaiinListSetting;
using UseCase.RaiinListSetting.GetRaiiinListSetting;
using Microsoft.AspNetCore.Authorization;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class RaiinListSettingController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public RaiinListSettingController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList + "DocCategoryRaiin")]
        public ActionResult<Response<GetDocCategoryRaiinResponse>> GetDocCategoryRaiin()
        {
            var input = new GetDocCategoryRaiinInputData(HpId);
            var output = _bus.Handle(input);
            var presenter = new GetDocCategoryRaiinPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetDocCategoryRaiinResponse>>(presenter.Result);
        }

        [AllowAnonymous]
        [HttpGet(ApiPath.GetList + "RaiiinListSetting")]
        public ActionResult<Response<GetRaiiinListSettingResponse>> RaiiinListSetting()
        {
            var input = new GetRaiiinListSettingInputData(1);
            var output = _bus.Handle(input);
            var presenter = new GetRaiiinListSettingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetRaiiinListSettingResponse>>(presenter.Result);
        }
    }
}
