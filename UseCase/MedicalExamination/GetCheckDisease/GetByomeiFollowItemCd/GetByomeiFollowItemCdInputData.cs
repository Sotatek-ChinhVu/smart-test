using Domain.Models.TodayOdr;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetByomeiFollowItemCd
{
    public class GetByomeiFollowItemCdInputData : IInputData<GetByomeiFollowItemCdOutputData>
    {
        public GetByomeiFollowItemCdInputData(bool isGridStyle, int hpId, string itemCd, int sinDate, List<CheckedDiseaseModel> todayByomeis)
        {
            IsGridStyle = isGridStyle;
            HpId = hpId;
            ItemCd = itemCd;
            SinDate = sinDate;
            TodayByomeis = todayByomeis;
        }

        public bool IsGridStyle { get; private set; }

        public int HpId { get; private set; }

        public string ItemCd { get; private set; }

        public int SinDate { get; private set; }

        public List<CheckedDiseaseModel> TodayByomeis { get; private set; }
    }
}
