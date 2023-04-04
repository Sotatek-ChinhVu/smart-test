namespace UseCase.SpecialNote.Save
{
    public class PtPregnancyItem
    {
        public PtPregnancyItem(long id, int hpId, long ptId, int seqNo, int startDate, int endDate, int periodDate, int periodDueDate, int ovulationDate, int ovulationDueDate, int isDeleted, int sinDate)
        {
            Id = id;
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            StartDate = startDate;
            EndDate = endDate;
            PeriodDate = periodDate;
            PeriodDueDate = periodDueDate;
            OvulationDate = ovulationDate;
            OvulationDueDate = ovulationDueDate;
            IsDeleted = isDeleted;
            SinDate = sinDate;
        }

        public long Id { get; private set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SeqNo { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int PeriodDate { get; private set; }

        public int PeriodDueDate { get; private set; }

        public int OvulationDate { get; private set; }

        public int OvulationDueDate { get; private set; }

        public int IsDeleted { get; private set; }

        public int SinDate { get; private set; }
    }
}
