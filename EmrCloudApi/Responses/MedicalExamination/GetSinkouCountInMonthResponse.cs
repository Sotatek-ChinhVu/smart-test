
using Domain.Models.Medical;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetSinkouCountInMonthResponse
    {
        public GetSinkouCountInMonthResponse(List<SinKouiCountModel> sinKouiModels)
        {
            SinKouiModels = sinKouiModels;
        }

        public List<SinKouiCountModel> SinKouiModels { get; private set; }
    }
}
