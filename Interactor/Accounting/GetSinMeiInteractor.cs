using Domain.Models.Accounting;
using EmrCalculateApi.Receipt.Constants;
using EmrCalculateApi.Receipt.ViewModels;
using Helper.Constants;
using UseCase.Accounting.GetSinMei;

namespace Interactor.Accounting
{
    public class GetSinMeiInteractor : IGetSinMeiInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetSinMeiInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }
        public GetSinMeiOutputData Handle(GetSinMeiInputData inputData)
        {
            try
            {
                var raiinNos = _accountingRepository.
                var sinMeiVm = new SinMeiViewModel(SinMeiMode.Kaikei, includeOutDrg: false, inputData.HpId, inputData.PtId, inputData.SinDate, listRaiinNo);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }
    }
}
