using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SytemGenerationConf;
using EmrCloudApi.Requests.SystemGenerationConf;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemGenerationConf;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemGenerationConf;
using UseCase.SystemGenerationConf.Get;
using UseCase.SystemGenerationConf.GetList;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SystemGenerationConfController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;
        public SystemGenerationConfController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetSettingValue)]
        public ActionResult<Response<GetSystemGenerationConfResponse>> GetSettingValue([FromQuery] GetSystemGenerationConfRequest request)
        {
            var input = new GetSystemGenerationConfInputData(HpId, request.GrpCd, request.GrpEdaNo, request.PresentDate, request.DefaultValue, request.DefaultParam);
            var output = _bus.Handle(input);

            var presenter = new GetSystemGenerationConfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemGenerationConfResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetSystemGenerationConfListResponse>> GetList()
        {
            var input = new GetSystemGenerationConfListInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetSystemGenerationConfListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemGenerationConfListResponse>>(presenter.Result);
        }
    }
}
