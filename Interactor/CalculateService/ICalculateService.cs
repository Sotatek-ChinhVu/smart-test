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

        /// <summary>
        /// function calls ReceFutanCalculateMain to other functions
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        bool ReceFutanCalculateMain(ReceCalculateRequest inputData);

        /// <summary>
        /// function calls ReceFutanCalculateMain only to calculate runs in month Rece
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ReceFutanCalculateMain(ReceCalculateRequest inputData, CancellationToken cancellationToken);

        /// <summary>
        /// function calls RunCalculateMonth to other functions
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        bool RunCalculateMonth(CalculateMonthRequest inputData);

        /// <summary>
        /// function calls RunCalculateMonth only to calculate runs in month Rece
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> RunCalculateMonth(CalculateMonthRequest inputData, CancellationToken cancellationToken);

        SinMeiDataModelDto GetSinMeiInMonthList(GetSinMeiDtoInputData inputData);

        void ReleaseSource();
    }
}
