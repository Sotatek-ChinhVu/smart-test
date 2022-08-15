using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PtPregnancy
{
    public class PtPregnancyModel
    {
        public PtPregnancyModel(long id, int hpId, long ptId, int seqNo, int startDate, int endDate, int periodDate, int periodDueDate, int ovulationDate, int ovulationDueDate, int isDeleted, DateTime updateDate, int updateId, string updateMachine)
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
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
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
    }
}
