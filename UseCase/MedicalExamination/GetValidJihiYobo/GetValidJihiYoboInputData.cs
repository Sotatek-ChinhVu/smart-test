using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetValidJihiYobo
{
    public class GetValidJihiYoboInputData : IInputData<GetValidJihiYoboOutputData>
    {
        public GetValidJihiYoboInputData(int hpId, int syosaiKbn, int sinDate, List<string> itemCds)
        {
            HpId = hpId;
            SyosaiKbn = syosaiKbn;
            SinDate = sinDate;
            ItemCds = itemCds;
        }

        public int HpId { get; private set; }
        public int SyosaiKbn { get; private set; }
        public int SinDate { get; private set; }
        public List<string> ItemCds { get; private set; }
    }
}
