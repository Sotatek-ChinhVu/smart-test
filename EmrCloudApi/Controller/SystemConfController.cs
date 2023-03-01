using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SytemConf;
using EmrCloudApi.Requests.SystemConf;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SystemConf.GetSystemConf;
using UseCase.SystemConf.GetSystemConfList;
using UseCase.SystemConf.Get;
using UseCase.SystemConf.GetSystemConfForPrint;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SystemConfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SystemConfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.Get)]
        public ActionResult<Response<GetSystemConfResponse>> GetByGrpCd([FromQuery] GetSystemConfRequest request)
        {
            var input = new GetSystemConfInputData(HpId, request.GrpCd, request.GrpEdaNo);
            var output = _bus.Handle(input);

            var presenter = new GetSystemConfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemConfResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetSystemConfListResponse>> GetSystemConfList()
        {
            var input = new GetSystemConfListInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetSystemConfListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemConfListResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetSystemConfForPrint)]
        public ActionResult<Response<GetSystemConfForPrintResponse>> GetSystemConfForPrint()
        {
            var input = new GetSystemConfForPrintInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetSystemConfForPrintPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSystemConfForPrintResponse>>(presenter.Result);
        }
    }
}
