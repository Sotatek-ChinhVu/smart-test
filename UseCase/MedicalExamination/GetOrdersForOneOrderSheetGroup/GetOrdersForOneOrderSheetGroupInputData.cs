using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetOrdersForOneOrderSheetGroup
{
    public class GetOrdersForOneOrderSheetGroupInputData : IInputData<GetOrdersForOneOrderSheetGroupOutputData>
    {

        public GetOrdersForOneOrderSheetGroupInputData(long ptId, int hpId, int sinDate, int odrKouiKbn, int grpKouiKbn, int offset, int limit)
        {
            PtId = ptId;
            HpId = hpId;
            SinDate = sinDate;
            Offset = offset;
            Limit = limit;
            OdrKouiKbn = odrKouiKbn;
            GrpKouiKbn = grpKouiKbn;
        }

        public long PtId { get; private set; }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public int OdrKouiKbn { get; private set; }

        public int GrpKouiKbn { get; private set; }

        public int Offset { get; private set; }

        public int Limit { get; private set; }
    }
}
