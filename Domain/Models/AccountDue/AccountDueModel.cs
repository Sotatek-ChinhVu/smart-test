namespace Domain.Models.AccountDue;

public class AccountDueModel
{
    public AccountDueModel(List<AccountDueItemModel> accountDueList, Dictionary<int, string> listPaymentMethod, Dictionary<int, string> listUketsukeSbt)
    {
        AccountDueList = accountDueList;
        ListPaymentMethod = listPaymentMethod;
        ListUketsukeSbt = listUketsukeSbt;
    }

    public List<AccountDueItemModel> AccountDueList { get; private set; }

    public Dictionary<int, string> ListPaymentMethod { get; private set; }

    public Dictionary<int, string> ListUketsukeSbt { get; private set; }
}
