using EmrCloudApi.Constants;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.AccountDue;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.AccountDue;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.AccountDue;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.AccountDue.GetAccountDueList;
using UseCase.AccountDue.SaveAccountDueList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class AccountDueController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IWebSocketService _webSocketService;
    public AccountDueController(UseCaseBus bus, IUserService userService, IWebSocketService webSocketService) : base(userService)
    {
        _bus = bus;
        _webSocketService = webSocketService;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetAccountDueListResponse>> GetList([FromQuery] GetAccountDueListRequest request)
    {
        var input = new GetAccountDueListInputData(HpId, request.PtId, request.SinDate, request.IsUnpaidChecked);
        var output = _bus.Handle(input);

        var presenter = new GetAccountDueListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetAccountDueListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList)]
    public async Task<ActionResult<Response<SaveAccountDueListResponse>>> SaveList([FromBody] SaveAccountDueListRequest request)
    {
        var input = new SaveAccountDueListInputData(HpId, request.UserId, request.PtId, request.SinDate, ConvertToListSyunoNyukinInputItem(request));
        var output = _bus.Handle(input);

        if (output.Status == SaveAccountDueListStatus.Successed)
        {
            await _webSocketService.SendMessageAsync(FunctionCodes.AccountDueChanged,
                new CommonMessage { PtId = input.PtId, SinDate = input.SinDate, RaiinNo = 0 });
        }

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
