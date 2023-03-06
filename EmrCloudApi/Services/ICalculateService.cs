using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.Recaculate;

namespace EmrCloudApi.Services
{
    public interface ICalculateService
    {
        RecaculateTestResponse Recaculate(string apiUrl, RecaculationInputData request);
    }
}
