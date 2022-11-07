using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.KarteFilter;
using EmrCloudApi.Tenant.Requests.KarteFilter;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteFilter;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.KarteFilter.GetListKarteFilter;
using UseCase.KarteFilter.SaveListKarteFilter;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KarteFilterController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public KarteFilterController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetList)]
        public async Task<ActionResult<Response<GetKarteFilterMstResponse>>> GetList()
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetKarteFilterMstResponse>>(new Response<GetKarteFilterMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetKarteFilterMstResponse>>(new Response<GetKarteFilterMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetKarteFilterInputData(hpId, userId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetKarteFilterMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetKarteFilterMstResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.SaveList)]
        public async Task<ActionResult<Response<SaveKarteFilterMstResponse>>> SaveList([FromBody] SaveKarteFilterMstRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveKarteFilterMstResponse>>(new Response<SaveKarteFilterMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<SaveKarteFilterMstResponse>>(new Response<SaveKarteFilterMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new SaveKarteFilterInputData(request.KarteFilters, hpId, userId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new SaveKarteFilterMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<SaveKarteFilterMstResponse>>(presenter.Result);
        }
    }
}
