using Domain.Models.Online;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetListQualification
{
    public class GetListQualificationInfOutputData : IOutputData
    {
        public GetListQualificationInfOutputData(List<QualificationInfModel> qualificationInfs, GetListQualificationInfStatus status)
        {
            QualificationInfs = qualificationInfs;
            Status = status;
        }

        public List<QualificationInfModel> QualificationInfs { get; private set; }

        public GetListQualificationInfStatus Status { get; private set; }
    }
}
