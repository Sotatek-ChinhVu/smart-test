using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.RaiinKubun;
using EmrCloudApi.Tenant.Requests.RaiinKubun;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinKubun;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.RaiinKubunMst.GetList;
using UseCase.RaiinKubunMst.GetListColumnName;
using UseCase.RaiinKubunMst.LoadData;
using UseCase.RaiinKubunMst.Save;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RaiinKubunController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public RaiinKubunController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetList + "Mst")]
        public async Task<ActionResult<Response<GetRaiinKubunMstListResponse>>> GetRaiinKubunMstList([FromQuery] GetRaiinKubunMstListRequest request)
        {
            var input = new GetRaiinKubunMstListInputData(request.IsDeleted);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetRaiinKubunMstListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetRaiinKubunMstListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList + "KubunSetting")]
        public async Task<ActionResult<Response<LoadDataKubunSettingResponse>>> LoadDataKubunSetting([FromQuery] LoadDataKubunSettingRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            int userId = _userService.GetLoginUser().UserId;
            var input = new LoadDataKubunSettingInputData(hpId, userId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new LoadDataKubunSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<LoadDataKubunSettingResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveList + "KubunSetting")]
        public async Task<ActionResult<Response<SaveDataKubunSettingResponse>>> SaveDataKubunSetting([FromBody] SaveDataKubunSettingRequest request)
        {
            int userId = _userService.GetLoginUser().UserId;
            var input = new SaveDataKubunSettingInputData(request.RaiinKubunMstRequest.Select(x => x.Map()).ToList(), userId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SaveDataKubunSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveDataKubunSettingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetColumnName)]
        public async Task<ActionResult<Response<GetColumnNameListResponse>>> GetColumnName([FromQuery] GetColumnNameListRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new GetColumnNameListInputData(hpId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetColumnNameListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetColumnNameListResponse>>(presenter.Result);
        }
    }
}
