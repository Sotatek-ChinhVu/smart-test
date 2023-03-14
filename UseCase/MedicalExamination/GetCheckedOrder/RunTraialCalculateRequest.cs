namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class RunTraialCalculateRequest
    {
        public RunTraialCalculateRequest(int hpId, long ptId, int sinDate, long raiinNo, List<OdrInfItem> orderInfoList, ReceptionItem reception, bool calcFutan)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            OrderInfoList = orderInfoList;
            Reception = reception;
            CalcFutan = calcFutan;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public List<OdrInfItem> OrderInfoList { get; private set; } = new List<OdrInfItem>();

        public ReceptionItem Reception { get; private set; }

        public bool CalcFutan { get; private set; }
    }
}
