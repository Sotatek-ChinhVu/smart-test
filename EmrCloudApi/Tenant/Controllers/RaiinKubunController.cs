using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.RaiinKubun;
using EmrCloudApi.Tenant.Requests.RaiinKubun;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinKubun;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.RaiinKubunMst.GetList;
using UseCase.RaiinKubunMst.GetListColumnName;
using UseCase.RaiinKubunMst.LoadData;
using UseCase.RaiinKubunMst.Save;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class RaiinKubunController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public RaiinKubunController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList + "Mst")]
        public ActionResult<Response<GetRaiinKubunMstListResponse>> GetRaiinKubunMstList([FromQuery] GetRaiinKubunMstListRequest request)
        {
            var input = new GetRaiinKubunMstListInputData(request.IsDeleted);
            var output = _bus.Handle(input);

            var presenter = new GetRaiinKubunMstListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetRaiinKubunMstListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList + "KubunSetting")]
        public ActionResult<Response<LoadDataKubunSettingResponse>> LoadDataKubunSetting([FromQuery] LoadDataKubunSettingRequest request)
        {
            var input = new LoadDataKubunSettingInputData(HpId, UserId);
            var output = _bus.Handle(input);

            var presenter = new LoadDataKubunSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<LoadDataKubunSettingResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveList + "KubunSetting")]
        public ActionResult<Response<SaveDataKubunSettingResponse>> SaveDataKubunSetting([FromBody] SaveDataKubunSettingRequest request)
        {
            var input = new SaveDataKubunSettingInputData(request.RaiinKubunMstRequest.Select(x => x.Map()).ToList(), UserId);
            var output = _bus.Handle(input);

            var presenter = new SaveDataKubunSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveDataKubunSettingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetColumnName)]
        public ActionResult<Response<GetColumnNameListResponse>> GetColumnName([FromQuery] GetColumnNameListRequest request)
        {
            var input = new GetColumnNameListInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetColumnNameListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetColumnNameListResponse>>(presenter.Result);
        }
    }
}
