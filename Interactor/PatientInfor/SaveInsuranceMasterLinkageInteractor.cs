using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using UseCase.PatientInfor.SaveInsuranceMasterLinkage;
using static Helper.Constants.DefHokenNoConst;

namespace Interactor.PatientInfor
{
    public class SaveInsuranceMasterLinkageInteractor : ISaveInsuranceMasterLinkageInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IMstItemRepository _mstItemRepository;

        public SaveInsuranceMasterLinkageInteractor(IPatientInforRepository patientInforRepository, IMstItemRepository mstItemRepository)
        {
            _patientInforRepository = patientInforRepository;
            _mstItemRepository = mstItemRepository;
        }

        public SaveInsuranceMasterLinkageOutputData Handle(SaveInsuranceMasterLinkageInputData inputData)
        {
            try
            {
                if (inputData.DefHokenNoModels.Any())
                {
                    foreach (var item in inputData.DefHokenNoModels)
                    {
                        if (!_mstItemRepository.CheckExistHokenEdaNo(item.HokenNo, item.HokenEdaNo))
                            return new SaveInsuranceMasterLinkageOutputData(ValidationStatus.InvalidHokenEdaNo);

                        var validationStatus = item.Validation();
                        if (validationStatus != ValidationStatus.Valid)
                            return new SaveInsuranceMasterLinkageOutputData(validationStatus);
                    }
                }
                else
                    return new SaveInsuranceMasterLinkageOutputData(ValidationStatus.InputDataNull);
                bool success = _patientInforRepository.SaveInsuranceMasterLinkage(inputData.DefHokenNoModels);
                var status = success ? ValidationStatus.Success : ValidationStatus.Failed;
                return new SaveInsuranceMasterLinkageOutputData(status);
            }
            catch (Exception)
            {
                return new SaveInsuranceMasterLinkageOutputData(ValidationStatus.Failed);
            }
        }
    }
}
