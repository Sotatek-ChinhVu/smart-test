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
        public int DeleteConditon { get; private set; }
        public int KarteDeleteHistory { get; private set; }

        public GetMedicalExaminationHistoryInputData(long ptId, int hpId, int sinDate, int pageIndex, int pageSize, int deleteConditon, int karteDeleteHistory)
        {
            PtId = ptId;
            HpId = hpId;
            SinDate = sinDate;
            PageIndex = pageIndex;
            PageSize = pageSize;
            DeleteConditon = deleteConditon;
            KarteDeleteHistory = karteDeleteHistory;
        }
    }
}
