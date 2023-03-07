using Domain.Models.CalculateModel;
using Helper.Enum;

namespace Interactor.CalculateService
{
    public interface ICalculateService
    {
        Task<string> CallCalculate(CalculateApiPath path, object inputData);
        SinMeiDataModelDto GetSinMeiList(CalculateApiPath path, object inputData);
    }
}
