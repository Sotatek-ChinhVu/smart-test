using Domain.Models.CalculateModel;
using UseCase.Accounting.GetMeiHoGai;
using UseCase.Accounting.Recaculate;
using UseCase.MedicalExamination.Calculate;
using UseCase.MedicalExamination.GetCheckedOrder;
using UseCase.Receipt.GetListReceInf;
using UseCase.Receipt.GetSinMeiInMonthList;
using UseCase.Receipt.Recalculation;

namespace Interactor.CalculateService
{
    public interface ICalculateService
    {
        SinMeiDataModelDto GetSinMeiList(GetSinMeiDtoInputData inputData);

        bool RunCalculate(RecaculationInputDto inputData);

        ReceInfModelDto GetListReceInf(GetInsuranceInfInputData inputData);

        RunTraialCalculateResponse RunTrialCalculate(RunTraialCalculateRequest inputData);

        bool RunCalculateOne(CalculateOneRequest inputData);

        // function calls ReceFutanCalculateMain to other functions
        bool ReceFutanCalculateMain(ReceCalculateRequest inputData);

        // function calls ReceFutanCalculateMain only to calculate runs in month Rece
        Task<bool> ReceFutanCalculateMain(ReceCalculateRequest inputData, CancellationToken cancellationToken);

        // function calls RunCalculateMonth to other functions
        bool RunCalculateMonth(CalculateMonthRequest inputData);

        // function calls RunCalculateMonth only to calculate runs in month Rece
        Task<bool> RunCalculateMonth(CalculateMonthRequest inputData, CancellationToken cancellationToken);

        SinMeiDataModelDto GetSinMeiInMonthList(GetSinMeiDtoInputData inputData);

        void ReleaseSource();
    }
}
