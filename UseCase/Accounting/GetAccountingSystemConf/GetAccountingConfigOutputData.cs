using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetAccountingSystemConf
{
    public class GetAccountingConfigOutputData : IOutputData
    {
        public GetAccountingConfigOutputData(AccountingConfigDto accountingConfigDto, GetAccountingConfigStatus status)
        {
            AccountingConfigDto = accountingConfigDto;
            Status = status;
        }

        public AccountingConfigDto AccountingConfigDto { get; private set; }
        public GetAccountingConfigStatus Status { get; private set; }
    }
}
