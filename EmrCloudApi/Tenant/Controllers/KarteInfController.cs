using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.KarteInf;
using EmrCloudApi.Presenters.KarteInfs;
using EmrCloudApi.Requests.KarteInf;
using EmrCloudApi.Requests.KarteInfs;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.KarteInf;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.KarteInf.ConvertTextToRichText;
using UseCase.KarteInf.GetList;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class KarteInfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public KarteInfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetListKarteInfResponse>> GetList([FromQuery] GetListKarteInfRequest request)
        {
            var input = new GetListKarteInfInputData(HpId, request.PtId, request.RaiinNo, request.SinDate, request.IsDeleted);
            var output = _bus.Handle(input);

            var presenter = new GetListKarteInfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListKarteInfResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.ConvertTextToRichText)]
        public ActionResult<Response<ConvertTextToRichTextResponse>> ConvertTextToRichText([FromBody] ConvertTextToRichTextRequest request)
        {
            var input = new ConvertTextToRichTextInputData(HpId, request.PtId);
            var output = _bus.Handle(input);

            var presenter = new ConvertTextToRichTextPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ConvertTextToRichTextResponse>>(presenter.Result);
        }
    }
}
