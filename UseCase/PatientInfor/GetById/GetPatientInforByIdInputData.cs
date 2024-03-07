using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInformation.GetById
{
    public class GetPatientInforByIdInputData : IInputData<GetPatientInforByIdOutputData>
    {
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public bool IsShowKyuSeiName { get; private set; }

        public  List<int> ListStatus { get; private set; }

        public GetPatientInforByIdInputData(int hpId, long ptId, int sinDate, long raiinNo, bool isShowKyuSeiName, List<int> listStatus)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            IsShowKyuSeiName = isShowKyuSeiName;
            ListStatus = listStatus;
        }
    }
}