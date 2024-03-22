using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Responses.TimeZoneConf;
using UseCase.TimeZoneConf.GetTimeZoneConfGroup;
using EmrCloudApi.Presenters.TimeZoneConf;
using UseCase.TimeZoneConf.SaveTimeZoneConf;
using EmrCloudApi.Requests.TimeZoneConf;
using Domain.Models.TimeZone;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeZoneConfController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;

        public TimeZoneConfController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetTimeZoneConfGroup)]
        public ActionResult<Response<GetTimeZoneConfGroupResponse>> GetTimeZoneConfGroup()
        {
            var input = new GetTimeZoneConfGroupInputData(HpId, UserId);
            var output = _bus.Handle(input);
            var presenter = new GetTimeZoneConfGroupPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTimeZoneConfGroupResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveTimeZoneConf)]
        public ActionResult<Response<SaveTimeZoneConfResponse>> SaveTimeZoneConf(SaveTimeZoneConfRequest request)
        {
            var input = new SaveTimeZoneConfInputData(HpId, UserId, request.TimeZoneConfs.Select(x=> new TimeZoneConfModel(HpId,
                                                                                                                         x.SortNo,
                                                                                                                         x.YoubiKbn,
                                                                                                                         x.StartTime,
                                                                                                                         x.EndTime,
                                                                                                                         x.SeqNo,
                                                                                                                         x.TimeKbn,
                                                                                                                         x.IsDelete,
                                                                                                                         x.ModelModified)).ToList());
            var output = _bus.Handle(input);
            var presenter = new SaveTimeZoneConfPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveTimeZoneConfResponse>>(presenter.Result);
        }
    }
}
