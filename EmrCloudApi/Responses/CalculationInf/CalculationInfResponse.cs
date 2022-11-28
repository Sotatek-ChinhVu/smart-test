using Domain.Models.CalculationInf;

namespace EmrCloudApi.Responses.CalculationInf
{
    public class CalculationInfResponse
    {
        public CalculationInfResponse(List<CalculationInfModel> listCalculations)
        {
            ListCalculations = listCalculations;
        }

        public List<CalculationInfModel> ListCalculations { get; private set; }
    }
}
