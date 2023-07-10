namespace EmrCloudApi.Responses.AccountDue;

public class SaveAccountDueListResponse
{
    public SaveAccountDueListResponse(List<AccountDueDto> accountDueList)
    {
        AccountDueList = accountDueList;
    }

    public List<AccountDueDto> AccountDueList { get; private set; }
}
