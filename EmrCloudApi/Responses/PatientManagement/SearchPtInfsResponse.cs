using Domain.Models.PatientInfor;

namespace EmrCloudApi.Responses.PatientManagement
{
    public class SearchPtInfsResponse
    {
        public SearchPtInfsResponse(int totalCount, List<PatientInforModel> ptInfs)
        {
            TotalCount = totalCount;
            PtInfs = ptInfs;
        }

        public int TotalCount { get; private set; }

        public List<PatientInforModel> PtInfs { get; private set; }
    }
}
