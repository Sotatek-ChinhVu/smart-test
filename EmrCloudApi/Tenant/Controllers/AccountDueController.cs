using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.AccountDue;
using EmrCloudApi.Tenant.Requests.AccountDue;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.AccountDue;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.AccountDue.GetAccountDueList;
using UseCase.AccountDue.SaveAccountDueList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountDueController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;
    public AccountDueController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.GetList)]
    public async Task<ActionResult<Response<GetAccountDueListResponse>>> GetList([FromQuery] GetAccountDueListRequest request)
    {
        var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
        if (!validateToken)
        {
            return new ActionResult<Response<GetAccountDueListResponse>>(new Response<GetAccountDueListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
        }
        var input = new GetAccountDueListInputData(hpId, request.PtId, request.SinDate, request.IsUnpaidChecked, request.PageIndex, request.PageSize);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new GetAccountDueListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetAccountDueListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList)]
    public async Task<ActionResult<Response<SaveAccountDueListResponse>>> SaveList([FromBody] SaveAccountDueListRequest request)
    {
        var input = new SaveAccountDueListInputData(request.HpId,  request.UserId, request.PtId, request.SinDate, ConvertToListSyunoNyukinInputItem(request));
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new SaveAccountDueListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveAccountDueListResponse>>(presenter.Result);
    }

    private List<SyunoNyukinInputItem> ConvertToListSyunoNyukinInputItem(SaveAccountDueListRequest request)
    {
        return request.ListAccountDues.Select(item => new SyunoNyukinInputItem(
                                                item.NyukinKbn,
                                                item.RaiinNo,
                                                item.SortNo,
                                                item.AdjustFutan,
                                                item.NyukinGaku,
                                                item.PaymentMethodCd,
                                                item.NyukinDate,
                                                item.UketukeSbt,
                                                item.NyukinCmt,
                                                item.SeikyuGaku,
                                                item.SeikyuTensu,
                                                item.SeikyuDetail,
                                                item.IsUpdated,
                                                item.SeqNo,
                                                item.RaiinInfStatus,
                                                item.SeikyuAdjustFutan,
                                                item.SeikyuSinDate,
                                                item.IsDelete
                                            )).ToList();
    }
}
