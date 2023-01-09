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
            SyunoSeikyu = new SyunoSeikyuModel();
            RaiinInfModel = new SyunoRaiinInfModel();
            SyunoNyukinModels = new List<SyunoNyukinModel>();
            KaikeiInfModels = new List<KaikeiInfModel>();
        }

        public SyunoSeikyuModel SyunoSeikyu { get; private set; }

        public SyunoRaiinInfModel RaiinInfModel { get; private set; }

        public List<SyunoNyukinModel> SyunoNyukinModels { get; private set; }

        public List<KaikeiInfModel> KaikeiInfModels { get; private set; }

        public int HokenId { get; set; }


        public int PtFutan
        {
            get => KaikeiInfModels.Sum(item => item.PtFutan);
        }

        public int JihiFutan
        {
            get => KaikeiInfModels.Sum(item => item.JihiFutan);
        }

        public int JihiOuttax
        {
            get => KaikeiInfModels.Sum(item => item.JihiOuttax);
        }

        public int AdjustRound
        {
            get => KaikeiInfModels.Sum(item => item.AdjustFutan);
        }

        public int JihiTax
        {
            get => KaikeiInfModels.Sum(item => item.JihiTax);
        }

        public int AdjFutan
        {
            get => KaikeiInfModels.Sum(item => item.AdjustFutan);
        }

    }
}
