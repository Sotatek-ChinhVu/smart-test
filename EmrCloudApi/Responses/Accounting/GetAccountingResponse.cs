using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(List<AccountingModel> accountingModels, AccountingInfModel accountingInfModel, List<PtByomeiModel> ptByomeiModels)
        {
            AccountingModels = accountingModels;
            AccountingInfModel = accountingInfModel;
            PtByomeiModels = ptByomeiModels;
        }

        public List<AccountingModel> AccountingModels { get; private set; }
        public AccountingInfModel AccountingInfModel { get; private set; }
        public List<PtByomeiModel> PtByomeiModels { get; private set; }
    }
}
