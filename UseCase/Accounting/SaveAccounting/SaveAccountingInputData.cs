using Domain.Models.AccountDue;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.SaveAccounting
{
    public class SaveAccountingInputData : IInputData<SaveAccountingOutputData>
    {
        public SaveAccountingInputData(int sumAdjust, int thisWari, int thisCredit, int payType, string comment, int uketukeSbt, List<SyunoSeikyuModel> syunoSeikyuModels)
        {
            SumAdjust = sumAdjust;
            ThisWari = thisWari;
            ThisCredit = thisCredit;
            PayType = payType;
            Comment = comment;
            UketukeSbt = uketukeSbt;
            SyunoSeikyuModels = syunoSeikyuModels;
        }

        public int SumAdjust { get; set; }
        public int ThisWari { get; set; }
        public int ThisCredit { get; set; }
        public int PayType { get; set; }
        public string Comment { get; set; }
        public int UketukeSbt { get; set; }
        public List<SyunoSeikyuModel> SyunoSeikyuModels { get; private set; }
    }
}
