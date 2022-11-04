namespace Domain.Models.AccountDue;

public class AccountDueListModel
{
    public AccountDueListModel(List<AccountDueModel> accountDueList, Dictionary<int, string> listPaymentMethod, Dictionary<int, string> listUketsukeSbt)
    {
        AccountDueList = accountDueList;
        ListPaymentMethod = listPaymentMethod;
        ListUketsukeSbt = listUketsukeSbt;
    }

    public List<AccountDueModel> AccountDueList { get; private set; }

    public Dictionary<int, string> ListPaymentMethod { get; private set; }

    public Dictionary<int, string> ListUketsukeSbt { get; private set; }
}
