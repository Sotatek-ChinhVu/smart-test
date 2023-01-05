using Domain.Models.AccountDue;

namespace Domain.Models.Accounting
{
    public class AccountingModel
    {
        public AccountingModel(SyunoSeikyuModel syunoSeikyu, SyunoRaiinInfModel raiinInfModel, List<SyunoNyukinModel> syunoNyukinModels, List<KaikeiInfModel> kaikeiInfModels, int hokenId)
        {
            SyunoSeikyu = syunoSeikyu;
            RaiinInfModel = raiinInfModel;
            SyunoNyukinModels = syunoNyukinModels;
            KaikeiInfModels = kaikeiInfModels;
            HokenId = hokenId;
        }

        public AccountingModel()
        {
        }

        public SyunoSeikyuModel SyunoSeikyu { get; }

        public SyunoRaiinInfModel RaiinInfModel { get; }

        public List<SyunoNyukinModel> SyunoNyukinModels { get; }

        public List<KaikeiInfModel> KaikeiInfModels { get; }

        public int HokenId { get; }

    }
}
