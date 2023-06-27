﻿using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.CheckAccountingStatus
{
    public class CheckAccountingStatusInputData : IInputData<CheckAccountingStatusOutputData>
    {
        public CheckAccountingStatusInputData(int hpId, long ptId, int sinDate, long raiinNo, int debitBalance, int sumAdjust, int credit, int wari, bool isDisCharge, bool isSaveAccounting, List<SyunoSeikyuDto> syunoSeikyuDtos, List<SyunoSeikyuDto> allSyunoSeikyuDtos)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            DebitBalance = debitBalance;
            SumAdjust = sumAdjust;
            Credit = credit;
            Wari = wari;
            IsDisCharge = isDisCharge;
            IsSaveAccounting = isSaveAccounting;
            SyunoSeikyuDtos = syunoSeikyuDtos;
            AllSyunoSeikyuDtos = allSyunoSeikyuDtos;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public long RaiinNo { get; private set; }
        public int DebitBalance { get; private set; }
        public int SumAdjust { get; private set; }
        public int Credit { get; private set; }
        public int Wari { get; private set; }
        public bool IsDisCharge { get; private set; }
        public bool IsSaveAccounting { get; private set; }
        public List<SyunoSeikyuDto> SyunoSeikyuDtos { get; private set; }
        public List<SyunoSeikyuDto> AllSyunoSeikyuDtos { get; private set; }
    }
}
