using Domain.Models.AccountDue;

namespace EmrCloudApi.Responses.AccountDue;

public class GetAccountDueListResponse
{
    public GetAccountDueListResponse(List<AccountDueDto> accountDueList, Dictionary<int, string> listPaymentMethod, Dictionary<int, string> listUketsukeSbt)
    {
        AccountDueList = accountDueList;
        ListPaymentMethod = listPaymentMethod;
        ListUketsukeSbt = listUketsukeSbt;
    }

    public List<AccountDueDto> AccountDueList { get; private set; }

    public Dictionary<int, string> ListPaymentMethod { get; private set; }

    public Dictionary<int, string> ListUketsukeSbt { get; private set; }
}
