using Domain.Models.Diseases;

namespace EmrCloudApi.Responses.Yousiki
{
    public class GetByomeisInMonthResponse
    {
        public GetByomeisInMonthResponse(List<PtDiseaseModel> byomeisInMonth)
        {
            ByomeisInMonth = byomeisInMonth;
        }

        public List<PtDiseaseModel> ByomeisInMonth { get; private set; }
    }
}
