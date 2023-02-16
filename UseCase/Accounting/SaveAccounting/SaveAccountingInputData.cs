using Domain.Models.AccountDue;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.SaveAccounting
{
    public class SaveAccountingInputData : IInputData<SaveAccountingOutputData>
    {
        public SaveAccountingInputData(int hpId, long ptId, int userId, int sumAdjust, int thisWari, int thisCredit, int payType, string comment, List<SyunoSeikyuModel> syunoSeikyuModels)
        {
            HpId = hpId;
            PtId = ptId;
            UserId = userId;
            SumAdjust = sumAdjust;
            ThisWari = thisWari;
            ThisCredit = thisCredit;
            PayType = payType;
            Comment = comment;
            SyunoSeikyuModels = syunoSeikyuModels;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int UserId { get; set; }
        public int SumAdjust { get; private set; }
        public int ThisWari { get; private set; }
        public int ThisCredit { get; private set; }
        public int PayType { get; private set; }
        public string Comment { get; private set; }
        public List<SyunoSeikyuModel> SyunoSeikyuModels { get; private set; }
    }
}
