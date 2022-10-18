using Domain.Models.TodayOdr;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetByomeiFollowItemCd
{
    public class GetByomeiFollowItemCdOutputData : IOutputData
    {
        public GetByomeiFollowItemCdOutputData(List<CheckedDiseaseModel> byomeis, GetByomeiFollowItemCdStatus status)
        {
            Byomeis = byomeis;
            Status = status;
        }

        public List<CheckedDiseaseModel> Byomeis { get; private set; }
        public GetByomeiFollowItemCdStatus Status { get; private set; }
    }
}
