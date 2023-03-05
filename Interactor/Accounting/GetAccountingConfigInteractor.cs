using Domain.Models.Accounting;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using UseCase.Accounting.GetAccountingSystemConf;

namespace Interactor.Accounting
{
    public class GetAccountingConfigInteractor : IGetAccountingConfigInputPort
    {
        private readonly IAccountingRepository _accountingRepository;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IPatientInforRepository _patientInforRepository;

        public GetAccountingConfigInteractor(IAccountingRepository accountingRepository, ISystemConfRepository systemConfRepository, IPatientInforRepository patientInforRepository)
        {
            _accountingRepository = accountingRepository;
            _systemConfRepository = systemConfRepository;
            _patientInforRepository = patientInforRepository;
        }

        public GetAccountingConfigOutputData Handle(GetAccountingConfigInputData inputData)
        {
            try
            {
                var raiinNos = _accountingRepository.GetRaiinNos(inputData.HpId, inputData.PtId, inputData.RaiinNo);
                if (!raiinNos.Any()) return new GetAccountingConfigOutputData(new(), GetAccountingConfigStatus.NoData);

                return GetSystemConfigPrints(inputData.HpId, inputData.PtId, raiinNos, inputData.SumAdjust);

            }
            finally
            {
                _accountingRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
                _patientInforRepository.ReleaseResource();
            }
        }
        private GetAccountingConfigOutputData GetSystemConfigPrints(int hpId, long ptId, List<long> raiinNos, int sumAdjust)
        {
            _accountingRepository.CheckOrdInfInOutDrug(hpId, ptId, raiinNos, out bool isVisiblePrintDrgLabel, out bool isVisiblePrintOutDrg);

            var isCheckedPrintReceipt = (int)_systemConfRepository.GetSettingValue(93001, 0, hpId) == 1;
            var isCheckedPrintDetail = (int)_systemConfRepository.GetSettingValue(93002, 0, hpId) == 1;
            if (sumAdjust == 0)
            {
                isCheckedPrintReceipt = (int)_systemConfRepository.GetSettingValue(93001, 3, hpId) == 1;
                isCheckedPrintDetail = (int)_systemConfRepository.GetSettingValue(93002, 3, hpId) == 1;
            }
            var isRyosyoDetail = _patientInforRepository.IsRyosyoFuyou(hpId, ptId);
            if (isRyosyoDetail)
            {
                isCheckedPrintDetail = false;
            }

            var configOutDrg = (int)_systemConfRepository.GetSettingValue(92003, 0, hpId);

            var isCheckedPrintOutDrg = false;
            if (isVisiblePrintOutDrg)
            {
                isVisiblePrintOutDrg = configOutDrg > 0;
                isCheckedPrintOutDrg = configOutDrg == 5;
            }

            var configDrgLabel = (int)_systemConfRepository.GetSettingValue(92005, 0, hpId);
            var configDrgInf = (int)_systemConfRepository.GetSettingValue(92004, 0, hpId);
            var configDrgNote = (int)_systemConfRepository.GetSettingValue(92006, 0, hpId);

            var isCheckedPrintDrgLabel = false;
            var isVisiblePrintDrgLabelSize = false;
            var isVisiblePrintDrgInf = false;
            var isCheckedPrintDrgInf = false;
            var isVisiblePrintDrgNote = false;
            var isCheckedPrintDrgNote = false;

            if (isVisiblePrintDrgLabel)
            {
                isVisiblePrintDrgLabel = configDrgLabel > 0;
                isCheckedPrintDrgLabel = configDrgLabel == 5;
                isVisiblePrintDrgLabelSize = (int)_systemConfRepository.GetSettingValue(92005, 2, hpId) == 0;

                isVisiblePrintDrgInf = configDrgInf > 0;
                isCheckedPrintDrgInf = configDrgInf == 5;

                isVisiblePrintDrgNote = configDrgNote > 0;
                isCheckedPrintDrgNote = configDrgNote == 5;
            }

            var AccountingConfig = new AccountingConfigDto(
                                                            isVisiblePrintDrgLabel,
                                                            isCheckedPrintDrgLabel,
                                                            isVisiblePrintOutDrg,
                                                            isCheckedPrintOutDrg,
                                                            isCheckedPrintReceipt,
                                                            isCheckedPrintDetail,
                                                            isVisiblePrintDrgLabelSize,
                                                            isVisiblePrintDrgInf,
                                                            isCheckedPrintDrgInf,
                                                            isVisiblePrintDrgNote,
                                                            isCheckedPrintDrgNote
                                                            );

            return new GetAccountingConfigOutputData(AccountingConfig, GetAccountingConfigStatus.Successed);
        }
    }
}
