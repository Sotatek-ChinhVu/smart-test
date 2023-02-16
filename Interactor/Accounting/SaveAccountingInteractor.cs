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

                var save = _accountingRepository.SaveAccounting(inputData.SyunoSeikyuModels, inputData.HpId, inputData.PtId, inputData.UserId, inputData.SumAdjust, inputData.ThisWari, inputData.ThisCredit,
                                                                inputData.PayType, inputData.Comment);
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
