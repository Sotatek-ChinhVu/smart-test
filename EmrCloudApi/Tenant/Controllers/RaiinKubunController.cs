using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.RaiinKubun;
using EmrCloudApi.Tenant.Requests.RaiinKubun;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinKubun;
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
    public class RaiinKubunController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public RaiinKubunController(UseCaseBus bus)
        {
            _bus = bus;
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
            var input = new LoadDataKubunSettingInputData(request.HpId);
            var output = _bus.Handle(input);

            var presenter = new LoadDataKubunSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<LoadDataKubunSettingResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveList + "KubunSetting")]
        public async Task<ActionResult<Response<SaveDataKubunSettingResponse>>> SaveDataKubunSetting([FromBody] SaveDataKubunSettingRequest request)
        {
            var input = new SaveDataKubunSettingInputData(request.RaiinKubunMstRequest.Select(x => x.Map()).ToList());
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SaveDataKubunSettingPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveDataKubunSettingResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetColumnName)]
        public async Task<ActionResult<Response<GetColumnNameListResponse>>> GetColumnName([FromQuery] GetColumnNameListRequest request)
        {
            var input = new GetColumnNameListInputData(request.HpId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetColumnNameListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetColumnNameListResponse>>(presenter.Result);
        }
    }
}
