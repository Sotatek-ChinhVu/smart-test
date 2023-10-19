using Domain.Models.MaxMoney;
using UseCase.MaxMoney.SaveMaxMoney;

namespace Interactor.MaxMoney
{
    public class SaveMaxMoneyInteractor : ISaveMaxMoneyInputPort
    {
        private readonly IMaxmoneyReposiory _maxmoneyReposiory;

        public SaveMaxMoneyInteractor(IMaxmoneyReposiory maxmoneyReposiory)
        {
            _maxmoneyReposiory = maxmoneyReposiory;
        }

        public SaveMaxMoneyOutputData Handle(SaveMaxMoneyInputData inputData)
        {
            try
            {
                if (inputData.PtId < 0)
                    return new SaveMaxMoneyOutputData(SaveMaxMoneyStatus.InvalidPtId);

                if (inputData.HpId < 0)
                    return new SaveMaxMoneyOutputData(SaveMaxMoneyStatus.InvalidHpId);

                if (inputData.KohiId <= 0)
                    return new SaveMaxMoneyOutputData(SaveMaxMoneyStatus.InvalidKohiId);


                bool reuslt = _maxmoneyReposiory.SaveMaxMoney(inputData.ListLimits, inputData.HpId, inputData.PtId, inputData.KohiId, inputData.SinYM, inputData.UserId);
                if (reuslt)
                    return new SaveMaxMoneyOutputData(SaveMaxMoneyStatus.Successful);

                else return new SaveMaxMoneyOutputData(SaveMaxMoneyStatus.Failed);
            }
            finally
            {
                _maxmoneyReposiory.ReleaseResource();
            }
        }
    }
}
