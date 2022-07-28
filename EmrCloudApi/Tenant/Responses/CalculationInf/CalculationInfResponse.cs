using Domain.Models.CalculationInf;

namespace EmrCloudApi.Tenant.Responses.CalculationInf
{
    public class CalculationInfResponse
    {
        public List<CalculationInfModel> ListCalculations { get; set; } = new List<CalculationInfModel>();
    }
}
