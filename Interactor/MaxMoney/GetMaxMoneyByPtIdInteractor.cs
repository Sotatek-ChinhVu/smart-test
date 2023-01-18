using Domain.Models.MaxMoney;
using UseCase.MaxMoney.GetMaxMoneyByPtId;

namespace Interactor.MaxMoney
{
    public class GetMaxMoneyByPtIdInteractor : IGetMaxMoneyByPtIdInputPort
    {
        private readonly IMaxmoneyReposiory _maxmoneyReposiory;

        public GetMaxMoneyByPtIdInteractor(IMaxmoneyReposiory maxmoneyReposiory) => _maxmoneyReposiory = maxmoneyReposiory;

        public GetMaxMoneyByPtIdOutputData Handle(GetMaxMoneyByPtIdInputData inputData)
        {
            var datas = new List<LimitListModel>();
            try
            {
                if (inputData.PtId < 0)
                    return new GetMaxMoneyByPtIdOutputData(datas, GetMaxMoneyByPtIdStatus.InvalidPtId);

                if (inputData.HpId < 0)
                    return new GetMaxMoneyByPtIdOutputData(datas, GetMaxMoneyByPtIdStatus.InvalidHpId);

                datas = _maxmoneyReposiory.GetListLimitModel(inputData.PtId, inputData.HpId);

                if (datas == null || !datas.Any())
                    return new GetMaxMoneyByPtIdOutputData(new List<LimitListModel>(), GetMaxMoneyByPtIdStatus.Successed);
                else
                    return new GetMaxMoneyByPtIdOutputData(datas, GetMaxMoneyByPtIdStatus.DataNotFound);
            }
            finally
            {
                _maxmoneyReposiory.ReleaseResource();
            }
        }
    }
}
