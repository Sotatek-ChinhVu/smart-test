using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionSameVisit
{
    public class ReceptionSameVisitModel
    {
        public ReceptionSameVisitModel(int hpId, long ptId, int uketukeNo, string kaName, string hokenPidName, string uketukeTime, int status, string timePeriod, string yoyakuInfo, string doctorName, string comment, long oyaRaiinNo, int hokenPid, int kaId, int doctorId, int raiinInfSyosaisinKbn, int raiinInfJikanKbn, int raiinInfSanteiKbn)
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
            KaId = kaId;
            DoctorId = doctorId;
            RaiinInfSyosaisinKbn = raiinInfSyosaisinKbn;
            RaiinInfJikanKbn = raiinInfJikanKbn;
            RaiinInfSanteiKbn = raiinInfSanteiKbn;
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

        public int KaId { get; private set; }

        public int DoctorId { get; private set; }

        public int RaiinInfSyosaisinKbn { get; private set; }

        public int RaiinInfJikanKbn { get; private set; }

        public int RaiinInfSanteiKbn { get; private set; }
    }
}
