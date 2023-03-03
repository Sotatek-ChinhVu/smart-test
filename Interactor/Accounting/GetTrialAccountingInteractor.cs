using EmrCalculateApi.Interface;
using UseCase.Accounting.TrialCalculate;

namespace Interactor.Accounting
{
    public class GetTrialAccountingInteractor : IGetTrialCalculateInputPort
    {
        private readonly IIkaCalculateViewModel _ikaCalculate;

        public GetTrialAccountingInteractor(IIkaCalculateViewModel ikaCalculate)
        {
            _ikaCalculate = ikaCalculate;
        }

    }
}
