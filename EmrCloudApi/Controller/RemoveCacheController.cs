using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Cache;
using EmrCloudApi.Requests.Cache;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Cache;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Cache;
using UseCase.Cache.RemoveAllCache;
using UseCase.Core.Sync;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class RemoveCacheController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public RemoveCacheController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.RemoveCache)]
        public ActionResult<Response<RemoveCacheResponse>> RemoveCache([FromBody] RemoveCacheRequest request)
        {
            var input = new RemoveCacheInputData(request.Key);
            var output = _bus.Handle(input);
            var presenter = new RemoveCachePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<RemoveCacheResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.RemoveAllCache)]
        public ActionResult<Response<RemoveAllCacheResponse>> RemoveAllCache()
        {
            var input = new RemoveAllCacheInputData();
            var output = _bus.Handle(input);
            var presenter = new RemoveAllCachePresenter();
            presenter.Complete(output);
            return new ActionResult<Response<RemoveAllCacheResponse>>(presenter.Result);
        }
    }
}
