using Domain.Models.Accounting;
using UseCase.Accounting.SaveAccounting;

namespace Interactor.Accounting
{
    public class SaveAccountingInteractor : ISaveAccountingInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public SaveAccountingInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public SaveAccountingOutputData Handle(SaveAccountingInputData inputData)
        {
            try
            {

                var save = _accountingRepository.SaveAccounting(inputData.SyunoSeikyuModels, inputData.SumAdjust, inputData.ThisWari, inputData.ThisCredit,
                                                                inputData.PayType, inputData.Comment, inputData.UketukeSbt);
                if (save)
                {
                    return new SaveAccountingOutputData(SaveAccountingStatus.Success);
                }
                return new SaveAccountingOutputData(SaveAccountingStatus.Failed);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }
    }
}
