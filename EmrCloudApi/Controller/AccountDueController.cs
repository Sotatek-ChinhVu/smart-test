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
using UseCase.AccountDue.IsNyukinExisted;
using UseCase.AccountDue.SaveAccountDueList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class AccountDueController : BaseParamControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IWebSocketService _webSocketService;
    public AccountDueController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor, IWebSocketService webSocketService) : base(httpContextAccessor)
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

    [HttpGet(ApiPath.IsNyukinExisted)]
    public ActionResult<Response<IsNyukinExistedResponse>> IsNyukinExisted([FromQuery] IsNyukinExistedRequest request)
    {
        var input = new IsNyukinExistedInputData(HpId, request.PtId, request.RaiinNo, request.SinDate);
        var output = _bus.Handle(input);

        var presenter = new IsNyukinExistedPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<IsNyukinExistedResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList)]
    public async Task<ActionResult<Response<SaveAccountDueListResponse>>> SaveList([FromBody] SaveAccountDueListRequest request)
    {
        var input = new SaveAccountDueListInputData(HpId, request.UserId, request.PtId, request.SinDate, request.KaikeiTime, ConvertToListSyunoNyukinInputItem(request));
        var output = _bus.Handle(input);

        if (output.Status == SaveAccountDueListStatus.Successed)
        {
            await _webSocketService.SendMessageAsync(FunctionCodes.ReceptionChanged, new ReceptionChangedMessage(output.ReceptionInfos, output.SameVisitList));
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
