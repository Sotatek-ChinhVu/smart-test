using EmrCloudApi.Constants;
using EmrCloudApi.Controller;
using EmrCloudApi.Presenters.Todo;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Todo;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Todo.GetListTodoKbn;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class TodoKbnController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;
        public TodoKbnController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }


        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetTodoKbnResponse>> GetList()
        {
            var input = new GetTodoKbnInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new GetTodoKbnPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetTodoKbnResponse>>(presenter.Result);
        }
    }
}