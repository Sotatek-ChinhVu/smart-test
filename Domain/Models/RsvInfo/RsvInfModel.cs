using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.RsvInfo
{
    public class RsvInfModel
    {
        public RsvInfModel(int hpId, int rsvFrameId, int sinDate, int startTime, long raiinNo, long ptId, int rsvSbt, int tantoId, int kaId, DateTime createDate, int createId, string? createMachine, DateTime updateDate, int updateId, string? updateMachine)
        {
            HpId = hpId;
            RsvFrameId = rsvFrameId;
            SinDate = sinDate;
            StartTime = startTime;
            RaiinNo = raiinNo;
            PtId = ptId;
            RsvSbt = rsvSbt;
            TantoId = tantoId;
            KaId = kaId;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
        }

        public int HpId { get; set; }
        public int RsvFrameId { get; set; }
        public int SinDate { get; set; }
        public int StartTime { get; set; }
        public long RaiinNo { get; set; }
        public long PtId { get; set; }
        public int RsvSbt { get; set; }
        public int TantoId { get; set; }
        public int KaId { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public string? CreateMachine { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateId { get; set; }
        public string? UpdateMachine { get; set; }

    }
}
