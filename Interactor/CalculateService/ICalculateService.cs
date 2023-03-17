using Domain.Models.CalculateModel;
using UseCase.Accounting.GetMeiHoGai;
using UseCase.Accounting.Recaculate;
using UseCase.MedicalExamination.Calculate;
using UseCase.MedicalExamination.GetCheckedOrder;
using UseCase.Receipt.GetListReceInf;
using UseCase.Receipt.MedicalDetail;
using UseCase.Receipt.Recalculation;

namespace Interactor.CalculateService
{
    public interface ICalculateService
    {
        SinMeiDataModelDto GetSinMeiList(GetSinMeiDtoInputData inputData);

        bool RunCalculate(RecaculationInputDto inputData);

        ReceInfModelDto GetListReceInf(GetInsuranceInfInputData inputData);

        List<string> RunTrialCalculate(RunTraialCalculateRequest inputData);

        bool RunCalculateOne(CalculateOneRequest inputData);

        bool ReceFutanCalculateMain(ReceCalculateRequest inputData);

        bool RunCalculateMonth(CalculateMonthRequest inputData);
    }
}
