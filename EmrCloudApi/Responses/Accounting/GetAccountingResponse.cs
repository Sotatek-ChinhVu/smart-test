using Domain.Models.Accounting;
using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(List<RaiinInfModel> raiinInfModels, List<AccountingModel> accountingModels, AccountingInfModel accountingInfModel, List<WarningMemoModel> warningMemoModels, List<PtByomeiModel> ptByomeiModels, List<PaymentMethodMstModel> paymentMethodMstModels)
        {
            RaiinInfModels = raiinInfModels;
            AccountingModels = accountingModels;
            AccountingInfModel = accountingInfModel;
            WarningMemoModels = warningMemoModels;
            PtByomeiModels = ptByomeiModels;
            PaymentMethodMstModels = paymentMethodMstModels;
        }

        public List<RaiinInfModel> RaiinInfModels { get; private set; }
        public List<AccountingModel> AccountingModels { get; private set; }
        public AccountingInfModel AccountingInfModel { get; private set; }
        public List<WarningMemoModel> WarningMemoModels { get; private set; }
        public List<PtByomeiModel> PtByomeiModels { get; private set; }
        public List<PaymentMethodMstModel> PaymentMethodMstModels { get; private set; }
    }
}
