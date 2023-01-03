namespace Domain.Models.Accounting
{
    public class AccountingModel
    {
        public AccountingModel(int hpId, long ptId, int sinDate, long raiinNo, int adjustFutan, int nyukinGaku, int paymentMethodCd, int nyukinDate, int uketukeSbt, string nyukinCmt, int nyukinjiTensu, int nyukinjiSeikyu, string nyukinjiDetail, int isDeleted, int nyukinKbn, int seikyuTensu, int seikyuGaku, string seikyuDetail, int newSeikyuTensu, int newAdjustFutan, int newSeikyuGaku, string newSeikyuDetail, int jihiFutan, int jihiOuttax)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            AdjustFutan = adjustFutan;
            NyukinGaku = nyukinGaku;
            PaymentMethodCd = paymentMethodCd;
            NyukinDate = nyukinDate;
            UketukeSbt = uketukeSbt;
            NyukinCmt = nyukinCmt;
            NyukinjiTensu = nyukinjiTensu;
            NyukinjiSeikyu = nyukinjiSeikyu;
            NyukinjiDetail = nyukinjiDetail;
            IsDeleted = isDeleted;
            NyukinKbn = nyukinKbn;
            SeikyuTensu = seikyuTensu;
            SeikyuGaku = seikyuGaku;
            SeikyuDetail = seikyuDetail;
            NewSeikyuTensu = newSeikyuTensu;
            NewAdjustFutan = newAdjustFutan;
            NewSeikyuGaku = newSeikyuGaku;
            NewSeikyuDetail = newSeikyuDetail;
            JihiFutan = jihiFutan;
            JihiOuttax = jihiOuttax;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public int AdjustFutan { get; private set; }

        public int NyukinGaku { get; private set; }

        public int PaymentMethodCd { get; private set; }

        public int NyukinDate { get; private set; }

        public int UketukeSbt { get; private set; }

        public string NyukinCmt { get; private set; }

        public int NyukinjiTensu { get; private set; }

        public int NyukinjiSeikyu { get; private set; }

        public string NyukinjiDetail { get; private set; }

        public int IsDeleted { get; private set; }

        public int NyukinKbn { get; private set; }

        public int SeikyuTensu { get; private set; }

        public int SeikyuGaku { get; private set; }

        public string SeikyuDetail { get; private set; }

        public int NewSeikyuTensu { get; private set; }

        public int NewAdjustFutan { get; private set; }

        public int NewSeikyuGaku { get; private set; }

        public string NewSeikyuDetail { get; private set; }

        public int JihiFutan { get; set; }

        public int JihiOuttax { get; set; }

        public int DebitBalance
        {
            get => SeikyuGaku - (NyukinGaku + AdjustFutan);
        }

        public int TotalSelfExpense
        {
            get => JihiFutan + JihiOuttax;
        }
    }
}
