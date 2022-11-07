using Domain.Models.AccountDue;

namespace EmrCloudApi.Tenant.Responses.AccountDue;

public class GetAccountDueListResponse
{
    public GetAccountDueListResponse(AccountDueListModel accountDueModel)
    {
        AccountDueModel = accountDueModel;
    }

    public AccountDueListModel AccountDueModel { get; private set; }
}
