using Domain.Models.CalculateModel;

namespace Interactor.CalculateService
{
    public interface ICalculateService
    {
        Task<string> CallCalculate(string apiUrl, object inputData);
        SinMeiDataModelDto GetSinMeiList(string apiUrl, object inputData);
    }
}
