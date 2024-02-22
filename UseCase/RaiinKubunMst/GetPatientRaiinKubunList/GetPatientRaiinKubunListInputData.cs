using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKbn.GetPatientRaiinKubunList
{
    public class GetPatientRaiinKubunListInputData : IInputData<GetPatientRaiinKubunListOutputData>
    {
        public GetPatientRaiinKubunListInputData(int hpId, long ptId, long raiinNo, int sinDate)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long RaiinNo { get; private set; }

        public int SinDate { get; private set; }


    }
}
