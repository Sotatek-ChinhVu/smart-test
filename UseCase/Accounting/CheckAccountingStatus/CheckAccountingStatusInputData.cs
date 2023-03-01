using Domain.Models.AccountDue;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.CheckAccountingStatus
{
    public class CheckAccountingStatusInputData : IInputData<CheckAccountingStatusOutputData>
    {
        public CheckAccountingStatusInputData(int hpId, long ptId, int sinDate, long raiinNo, int debitBalance, int sumAdjust, int thisCredit, int wari, bool isDisCharge, bool isDeletedSyuno, bool isSaveAccounting, List<SyunoSeikyuModel> syunoSeikyus)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            DebitBalance = debitBalance;
            SumAdjust = sumAdjust;
            ThisCredit = thisCredit;
            Wari = wari;
            IsDisCharge = isDisCharge;
            IsDeletedSyuno = isDeletedSyuno;
            SyunoSeikyus = syunoSeikyus;
            IsSaveAccounting = isSaveAccounting;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public long RaiinNo { get; private set; }
        public int DebitBalance { get; private set; }
        public int SumAdjust { get; private set; }
        public int ThisCredit { get; private set; }
        public int Wari { get; private set; }
        public bool IsDisCharge { get; private set; }
        public bool IsDeletedSyuno { get; private set; }
        public bool IsSaveAccounting { get; private set; }
        public List<SyunoSeikyuModel> SyunoSeikyus { get; private set; }
    }
}
