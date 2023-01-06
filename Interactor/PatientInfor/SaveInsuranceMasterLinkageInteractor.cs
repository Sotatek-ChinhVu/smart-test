using Domain.Models.HokenMst;
using Domain.Models.PatientInfor;
using UseCase.PatientInfor.SaveInsuranceMasterLinkage;
using static Helper.Constants.DefHokenNoConst;

namespace Interactor.PatientInfor
{
    public class SaveInsuranceMasterLinkageInteractor : ISaveInsuranceMasterLinkageInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IHokenMstRepository _hokenMstRepository;
        public SaveInsuranceMasterLinkageInteractor(IPatientInforRepository patientInforRepository, IHokenMstRepository hokenMstRepository)
        {
            _patientInforRepository = patientInforRepository;
            _hokenMstRepository = hokenMstRepository;
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
                            return new SaveInsuranceMasterLinkageOutputData(validationStatus);

                        if (item.HokenEdaNo == 0 && item.HokenNo == 0)
                            continue;

                        var listHokenEdaNo = _hokenMstRepository.CheckExistHokenEdaNo(item.HokenNo, inputData.HpId);

                        var checkExistsHokenEda = listHokenEdaNo.Where(x => x.HokenNo == item.HokenNo && x.HokenEdaNo == item.HokenEdaNo);

                        if (!checkExistsHokenEda.Any())
                            return new SaveInsuranceMasterLinkageOutputData(ValidationStatus.InvalidHokenEdaNo);
                    }
                }
                else
                    return new SaveInsuranceMasterLinkageOutputData(ValidationStatus.InputDataNull);
                bool success = _patientInforRepository.SaveInsuranceMasterLinkage(inputData.DefHokenNoModels, inputData.HpId, inputData.UserId);
                var status = success ? ValidationStatus.Success : ValidationStatus.Failed;
                return new SaveInsuranceMasterLinkageOutputData(status);
            }
            catch (Exception)
            {
                return new SaveInsuranceMasterLinkageOutputData(ValidationStatus.Failed);
            }
            finally
            {
                _hokenMstRepository.ReleaseResource();
                _patientInforRepository.ReleaseResource();
            }
        }
    }
}
