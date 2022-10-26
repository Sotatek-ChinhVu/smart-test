using Domain.Models.AccountDue;

namespace EmrCloudApi.Tenant.Responses.AccountDue;

public class GetAccountDueListResponse
{
    public GetAccountDueListResponse(AccountDueModel accountDueModel)
    {
        AccountDueModel = accountDueModel;
    }

    public AccountDueModel AccountDueModel { get; private set; }
}
