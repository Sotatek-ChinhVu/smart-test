using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetSinkouCountInMonth
{
    public class GetSinkouCountInMonthInputData : IInputData<GetSinkouCountInMonthOutputData>
    {
        public GetSinkouCountInMonthInputData(int hpId, int sinDate, string itemCd, long ptId)
        {
            HpId = hpId;
            SinDate = sinDate;
            ItemCd = itemCd;
            PtId = ptId;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public string ItemCd { get; private set; }

        public long PtId { get; private set; }
    }
}
