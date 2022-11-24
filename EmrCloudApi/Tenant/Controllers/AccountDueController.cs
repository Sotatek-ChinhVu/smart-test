using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.AccountDue;
using EmrCloudApi.Tenant.Requests.AccountDue;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.AccountDue;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.AccountDue.GetAccountDueList;
using UseCase.AccountDue.SaveAccountDueList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
public class AccountDueController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    public AccountDueController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetAccountDueListResponse>> GetList([FromQuery] GetAccountDueListRequest request)
    {
        
        var input = new GetAccountDueListInputData(HpId, request.PtId, request.SinDate, request.IsUnpaidChecked, request.PageIndex, request.PageSize);
        var output = _bus.Handle(input);

        var presenter = new GetAccountDueListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetAccountDueListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList)]
    public ActionResult<Response<SaveAccountDueListResponse>> SaveList([FromBody] SaveAccountDueListRequest request)
    {
        
        var input = new SaveAccountDueListInputData(HpId, request.UserId, request.PtId, request.SinDate, ConvertToListSyunoNyukinInputItem(request));
        var output = _bus.Handle(input);

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
