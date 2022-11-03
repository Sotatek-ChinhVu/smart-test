using Domain.Models.AccountDue;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.AccountDue;
using UseCase.AccountDue.GetAccountDueList;

namespace EmrCloudApi.Tenant.Presenters.AccountDue;

public class GetAccountDueListPresenter : IGetAccountDueListOutputPort
{
    public Response<GetAccountDueListResponse> Result { get; private set; } = new();

    public void Complete(GetAccountDueListOutputData output)
    {
        Result.Data = new GetAccountDueListResponse(
                                                        ConvertToListAccountDueDto(output.AccountDueModel.AccountDueList), 
                                                        output.AccountDueModel.ListPaymentMethod,
                                                        output.AccountDueModel.ListUketsukeSbt
                                                    );
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetAccountDueListStatus status) => status switch
    {
        GetAccountDueListStatus.Successed => ResponseMessage.Success,
        GetAccountDueListStatus.Failed => ResponseMessage.Failed,
        GetAccountDueListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetAccountDueListStatus.InvalidPtId => ResponseMessage.PtInfNotFould,
        GetAccountDueListStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
        GetAccountDueListStatus.InvalidpageSize => ResponseMessage.InvalidPageSize,
        GetAccountDueListStatus.InvalidpageIndex => ResponseMessage.InvalidPageIndex,
        _ => string.Empty
    };

    private List<AccountDueDto> ConvertToListAccountDueDto(List<AccountDueModel> accountDueModels)
    {
        return accountDueModels.Select(item => new AccountDueDto(
                                                 item.HpId,
                                                 item.PtId,
                                                 item.SeikyuSinDate,
                                                 item.Month,
                                                 item.RaiinNo,
                                                 item.HokenPid,
                                                 item.OyaRaiinNo,
                                                 item.NyukinKbn,
                                                 item.SeikyuTensu,
                                                 item.SeikyuGaku,
                                                 item.AdjustFutan,
                                                 item.NyukinGaku,
                                                 item.PaymentMethodCd,
                                                 item.NyukinDate,
                                                 item.UketukeSbt,
                                                 item.NyukinCmt,
                                                 item.UnPaid,
                                                 item.NewSeikyuGaku,
                                                 item.NewAdjustFutan,
                                                 item.KaDisplay,
                                                 item.HokenPatternName,
                                                 item.IsSeikyuRow,
                                                 item.SortNo,
                                                 item.SeqNo,
                                                 item.SeikyuDetail,
                                                 item.RaiinInfStatus,
                                                 item.SeikyuAdjustFutan
                                            )).ToList();
    }
}
