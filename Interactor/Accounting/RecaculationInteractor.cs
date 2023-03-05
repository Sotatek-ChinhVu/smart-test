using EmrCalculateApi.Ika.ViewModels;
using EmrCalculateApi.Interface;
using Infrastructure.Interfaces;
using UseCase.Accounting.Recaculate;

namespace Interactor.Accounting
{
    public class RecaculationInteractor : IRecaculationInputPort
    {
        private readonly IFutancalcViewModel _futancalcViewModel;
        private readonly ITenantProvider _tenantProvider;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        public RecaculationInteractor(IFutancalcViewModel futancalcViewModel, ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _futancalcViewModel = futancalcViewModel;
            _tenantProvider = tenantProvider;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        public RecaculationOutputData Handle(RecaculationInputData inputData)
        {
            IkaCalculateViewModel ikaCalculateViewModel = new IkaCalculateViewModel(_futancalcViewModel, _tenantProvider, _systemConfigProvider, _emrLogger);
            ikaCalculateViewModel.RunCalculate(inputData.HpId, inputData.PtId, inputData.SinDate, 0, "SAI_");

            return new RecaculationOutputData(RecaculationStatus.Successed);
        }
    }
}
