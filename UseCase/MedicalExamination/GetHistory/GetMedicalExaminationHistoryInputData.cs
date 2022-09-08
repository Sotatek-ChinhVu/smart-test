using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetHistory
{
    public class GetMedicalExaminationHistoryInputData : IInputData<GetMedicalExaminationHistoryOutputData>
    {
        public long PtId { get; private set; }
        public int HpId { get; private set; }
        public int SinDate { get; private set; }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public long FilterId { get; private set; }

        public GetMedicalExaminationHistoryInputData(long ptId, int hpId, int sinDate, int pageIndex, int pageSize, long filterId)
        {
            PtId = ptId;
            HpId = hpId;
            SinDate = sinDate;
            PageIndex = pageIndex;
            PageSize = pageSize;
            FilterId = filterId;
        }
    }
}
