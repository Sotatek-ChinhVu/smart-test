using Domain.Models.CalculateModel;
using UseCase.Accounting.GetMeiHoGai;
using UseCase.Accounting.Recaculate;
using UseCase.Receipt.GetListReceInf;

namespace Interactor.CalculateService
{
    public interface ICalculateService
    {
        SinMeiDataModelDto GetSinMeiList(GetSinMeiDtoInputData inputData);
        bool RunCalculate(RecaculationInputDto inputData);
        ReceInfModelDto GetListReceInf(GetInsuranceInfInputData inputData);
    }
}
