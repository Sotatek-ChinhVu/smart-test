using Helper.Common;
using Helper.Extension;

namespace Domain.Models.SpecialNote.PatientInfo
{
    public class PtPregnancyModel
    {
        public PtPregnancyModel(long id, int hpId, long ptId, int seqNo, int startDate, int endDate, int periodDate, int periodDueDate, int ovulationDate, int ovulationDueDate, int isDeleted, DateTime updateDate, int updateId, string updateMachine, int sinDate)
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
            OvulationDueDate = ovulationDueDate;
            IsDeleted = isDeleted;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
            SinDate = sinDate;
        }

        public PtPregnancyModel()
        {
            UpdateMachine = string.Empty;
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

        public DateTime UpdateDate { get; private set; }

        public int UpdateId { get; private set; }

        public string UpdateMachine { get; private set; }

        public int SinDate { get; private set; }

        public string PeriodWeek
        {
            get
            {
                int startDate = PeriodDate;
                int endDate = EndDate;
                if (PeriodDate == 0 && PeriodDueDate == 0)
                {
                    var periodDueDate = CIUtil.IntToDate(PeriodDueDate);
                    if (periodDueDate.Year > 1)
                        startDate = periodDueDate.AddDays(-280).ToString("yyyyMMdd").AsInteger();
                }
                return GetPeriodWeek(startDate, 0, endDate);
            }
        }

        private string GetPeriodWeek(int startDay, int ovulation, int endDay = 0)
        {
            if (startDay == 0) return string.Empty;
            if (startDay >= SinDate)
            {
                return string.Empty;
            }
            if (endDay != 0 && endDay < startDay)
            {
                return string.Empty;
            }

            DateTime dtStartDay = CIUtil.IntToDate(startDay);
            dtStartDay = dtStartDay.AddDays(-14 * ovulation);

            DateTime dtToDay;
            if (SinDate > endDay && endDay > 0)
            {
                dtToDay = CIUtil.IntToDate(endDay);
            }
            else
            {
                dtToDay = CIUtil.IntToDate(SinDate);
            }

            int countDays = dtToDay.Subtract(dtStartDay).Days;
            if (countDays < 0)
            {
                countDays *= -1;
            }
            return (countDays / 7) + "W" + (countDays % 7) + "D";
        }

        public string OvulationWeek
        {
            get
            {
                int startDate = OvulationDate;
                int endDate = EndDate;
                var ovulationDueDate = CIUtil.IntToDate(OvulationDueDate);
                if (OvulationDate != 0 && OvulationDueDate != 0)
                {
                    startDate = ovulationDueDate.AddDays(-266).ToString("yyyyMMdd").AsInteger();
                }
                return GetPeriodWeek(startDate, 1, endDate);
            }
        }
    }
}
