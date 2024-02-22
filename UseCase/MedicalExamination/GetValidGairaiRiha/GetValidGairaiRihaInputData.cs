using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetValidGairaiRiha
{
    public class GetValidGairaiRihaInputData : IInputData<GetValidGairaiRihaOutputData>
    {
        public GetValidGairaiRihaInputData(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, List<Tuple<string, string>> allOdrInfItem)
        {
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
            SyosaiKbn = syosaiKbn;
            AllOdrInfItem = allOdrInfItem;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long RaiinNo { get; private set; }
        public int SinDate { get; private set; }
        public int SyosaiKbn { get; private set; }
        public List<Tuple<string, string>> AllOdrInfItem { get; private set; }
    }
}
