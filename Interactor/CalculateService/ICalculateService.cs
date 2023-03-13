using Domain.Models.CalculateModel;
using UseCase.Accounting.GetMeiHoGai;
using UseCase.Accounting.Recaculate;
using UseCase.MedicalExamination.Calculate;
using UseCase.Receipt.MedicalDetail;

namespace Interactor.CalculateService
{
    public interface ICalculateService
    {
        SinMeiDataModelDto GetSinMeiList(GetSinMeiDtoInputData inputData);

        bool RunCalculate(RecaculationInputDto inputData);

        bool RunCalculateOne(CalculateOneRequest inputData);

        bool ReceFutanCalculateMain(ReceCalculateRequest inputData);

        SinMeiDataModelDto GetSinMeiAccountingCard(GetSinMeiAccountingCardDtoInputData inputData);
    }
}
