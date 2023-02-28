using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.ConvertInputItemToTodayOdr
{
    public class ConvertInputItemToTodayOrdInputData : IInputData<ConvertInputItemToTodayOrdOutputData>
    {
        public ConvertInputItemToTodayOrdInputData(int hpId, int sinDate, Dictionary<string, string> detailInfs)
        {
            HpId = hpId;
            SinDate = sinDate;
            DetailInfs = detailInfs;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public Dictionary<string, string> DetailInfs { get; private set; }
    }
}
