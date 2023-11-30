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

        bool ReceFutanCalculateMain(ReceCalculateRequest inputData);

        Task<bool> ReceFutanCalculateMain(ReceCalculateRequest inputData, CancellationToken cancellationToken);

        bool RunCalculateMonth(CalculateMonthRequest inputData);

        Task<bool> RunCalculateMonth(CalculateMonthRequest inputData, CancellationToken cancellationToken);

        SinMeiDataModelDto GetSinMeiInMonthList(GetSinMeiDtoInputData inputData);

        void ReleaseSource();
    }
}
