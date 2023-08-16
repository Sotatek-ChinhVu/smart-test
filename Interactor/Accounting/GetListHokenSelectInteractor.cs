using Domain.Models.Accounting;
using Helper.Extension;
using UseCase.Accounting.GetListHokenSelect;

namespace Interactor.Accounting
{
    public class GetListHokenSelectInteractor : IGetListHokenSelectInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetListHokenSelectInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public GetListHokenSelectOutputData Handle(GetListHokenSelectInputData inputData)
        {
            try
            {
                var raiinInf = _accountingRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo).FirstOrDefault();

                if (raiinInf == null)
                    return new(new(), GetListHokenSelectStatus.NoData);

                var listKaikeiInf = raiinInf.KaikeiInfModels.Where(item =>
                   !item.HokenSbtCd.AsString().StartsWith("1") &&
                   !item.HokenSbtCd.AsString().StartsWith("5") &&
                   !item.ReceSbt.StartsWith("8")).ToList();

                var kaikeiInfSbt = raiinInf.KaikeiInfModels.FirstOrDefault(item => item.HokenSbtCd.AsString().StartsWith("1"));
                if (kaikeiInfSbt != null)
                {
                    listKaikeiInf.Add(kaikeiInfSbt);
                }
                else
                {
                    kaikeiInfSbt = raiinInf.KaikeiInfModels.FirstOrDefault(item => item.HokenSbtCd.AsString().StartsWith("5"));
                    if (kaikeiInfSbt != null)
                    {
                        listKaikeiInf.Add(kaikeiInfSbt);
                    }
                }

                var listHoken = _accountingRepository.GetListHokenSelect(inputData.HpId, listKaikeiInf, inputData.PtId);

                if (listHoken == null || listHoken.Count <= 0)
                    return new GetListHokenSelectOutputData(new(), GetListHokenSelectStatus.NoData);

                var hokenSelects = new List<ListHokenSelectDto>();
                foreach (var item in listHoken)
                {
                    hokenSelects.Add(new ListHokenSelectDto(item));
                }

                return new GetListHokenSelectOutputData(hokenSelects, GetListHokenSelectStatus.Success);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }

        }
    }
}
