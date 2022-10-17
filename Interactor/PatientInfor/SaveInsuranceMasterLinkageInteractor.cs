using Domain.Models.PatientInfor;
using UseCase.PatientInfor.SaveInsuranceMasterLinkage;
using static Helper.Constants.DefHokenNoConst;

namespace Interactor.PatientInfor
{
    public class SaveInsuranceMasterLinkageInteractor : ISaveInsuranceMasterLinkageInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public SaveInsuranceMasterLinkageInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public SaveInsuranceMasterLinkageOutputData Handle(SaveInsuranceMasterLinkageInputData inputData)
        {
            try
            {
                if (inputData.DefHokenNoModels.Any())
                {
                    foreach (var item in inputData.DefHokenNoModels)
                    {
                        var validationStatus = item.Validation();
                        if (validationStatus != ValidationStatus.Valid)
                            return new SaveInsuranceMasterLinkageOutputData(ConvertStatus(validationStatus));
                    }
                }
                else
                    return new SaveInsuranceMasterLinkageOutputData(SaveInsuranceMasterLinkageStatus.InputDataNull);
                bool success = _patientInforRepository.SaveInsuranceMasterLinkage(inputData.DefHokenNoModels);
                var status = success ? SaveInsuranceMasterLinkageStatus.Success : SaveInsuranceMasterLinkageStatus.Failed;
                return new SaveInsuranceMasterLinkageOutputData(status);
            }
            catch (Exception)
            {
                return new SaveInsuranceMasterLinkageOutputData(SaveInsuranceMasterLinkageStatus.Failed);
            }
        }

        private SaveInsuranceMasterLinkageStatus ConvertStatus(ValidationStatus status)
        {
            if (status == ValidationStatus.InvalidHpId)
                return SaveInsuranceMasterLinkageStatus.InvalidHpId;

            if (status == ValidationStatus.InvalidDigit1)
                return SaveInsuranceMasterLinkageStatus.InvalidDigit1;

            if (status == ValidationStatus.InvalidDigit2)
                return SaveInsuranceMasterLinkageStatus.InvalidDigit2;

            if (status == ValidationStatus.InvalidDigit3)
                return SaveInsuranceMasterLinkageStatus.InvalidDigit3;

            if (status == ValidationStatus.InvalidDigit4)
                return SaveInsuranceMasterLinkageStatus.InvalidDigit4;

            if (status == ValidationStatus.InvalidDigit5)
                return SaveInsuranceMasterLinkageStatus.InvalidDigit5;

            if (status == ValidationStatus.InvalidDigit6)
                return SaveInsuranceMasterLinkageStatus.InvalidDigit6;

            if (status == ValidationStatus.InvalidDigit7)
                return SaveInsuranceMasterLinkageStatus.InvalidDigit7;

            if (status == ValidationStatus.InvalidDigit8)
                return SaveInsuranceMasterLinkageStatus.InvalidDigit8;

            return SaveInsuranceMasterLinkageStatus.Success;
        }
    }
}
