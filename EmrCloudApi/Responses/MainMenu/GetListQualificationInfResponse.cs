using Domain.Models.MainMenu;

namespace EmrCloudApi.Responses.MainMenu
{
    public class GetListQualificationInfResponse
    {
        public GetListQualificationInfResponse(List<QualificationInfModel> qualificationInfs)
        {
            QualificationInfs = qualificationInfs;
        }

        public List<QualificationInfModel> QualificationInfs { get; private set; }
    }
}
