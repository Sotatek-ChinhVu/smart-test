using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionSameVisit
{
    public class ReceptionSameVisitModel
    {
        public ReceptionSameVisitModel(int hpId, long ptId, int uketukeNo, string kaName, string hokenPidName, string uketukeTime, int status, string timePeriod, string yoyakuInfo, string doctorName, string comment, long oyaRaiinNo, int hokenPid)
        {
            HpId = hpId;
            PtId = ptId;
            UketukeNo = uketukeNo;
            KaName = kaName;
            HokenPidName = hokenPidName;
            UketukeTime = uketukeTime;
            Status = status;
            TimePeriod = timePeriod;
            YoyakuInfo = yoyakuInfo;
            DoctorName = doctorName;
            Comment = comment;
            OyaRaiinNo = oyaRaiinNo;
            HokenPid = hokenPid;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int UketukeNo { get; private set; }

        public string KaName { get; private set; }

        public string HokenPidName { get; private set; }

        public string UketukeTime { get; private set; }

        public int Status { get; private set; }

        public string TimePeriod { get; private set; }

        public string YoyakuInfo { get; private set; }

        public string DoctorName { get; private set; }

        public string Comment { get; private set; }

        public long OyaRaiinNo { get; private set; }

        public int HokenPid { get; private set; }
    }
}
