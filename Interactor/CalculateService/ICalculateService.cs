using Domain.Models.CalculateModel;

namespace Interactor.CalculateService
{
    public interface ICalculateService
    {
        SinMeiDataModelDto GetSinMeiList(object inputData);
        bool RunCalculate(object inputData);
    }
}
