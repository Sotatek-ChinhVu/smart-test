using UseCase.Accounting.GetAccountingSystemConf;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingConfigResponse
    {
        public GetAccountingConfigResponse(AccountingConfigDto accountingConfig)
        {
            AccountingConfig = accountingConfig;
        }

        public AccountingConfigDto AccountingConfig { get; set; }
    }
}
