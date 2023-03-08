using Domain.Models.CalculateModel;

namespace Interactor.CalculateService
{
    public interface ICalculateService
    {
        SinMeiDataModelDto GetSinMeiList(object inputData);
        string RunCalculate(object inputData);
    }
}
