using Domain.Models.Accounting;
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
                var sinMeiVm = new SinMeiViewModel(SinMeiMode.Kaikei, includeOutDrg: false, Session.HospitalID, PtId, SinDate, listRaiinNo);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }
    }
}
