using Domain.Constant;
using Domain.Models.PatientInfor;
using Helper.Extension;
using UseCase.PatientInfor.CheckValidSamePatient;

namespace Interactor.PatientInfor
{
    public class CheckValidSamePatientInteractor : ICheckValidSamePatientInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        public CheckValidSamePatientInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public CheckValidSamePatientOutputData Handle(CheckValidSamePatientInputData inputData)
        {
            if (inputData.PtId < 0)
                return new CheckValidSamePatientOutputData(CheckValidSamePatientStatus.InvalidPtId, string.Empty);

            if (inputData.HpId < 0)
                return new CheckValidSamePatientOutputData(CheckValidSamePatientStatus.InvalidHpId, string.Empty);

            if(inputData.Birthday < 0)
                return new CheckValidSamePatientOutputData(CheckValidSamePatientStatus.InvalidBirthday, string.Empty);

            if (inputData.Sex < 0)
                return new CheckValidSamePatientOutputData(CheckValidSamePatientStatus.InvalidSex, string.Empty);

            if (string.IsNullOrEmpty(inputData.KanjiName))
                return new CheckValidSamePatientOutputData(CheckValidSamePatientStatus.InvalidKanjiName, string.Empty);

            try
            {

                var samePatientInf = _patientInforRepository.FindSamePatient(inputData.HpId, inputData.KanjiName, inputData.Sex, inputData.Birthday).Where(item => item.PtId != inputData.PtId).ToList();
                if (samePatientInf.Count > 0)
                {
                    string msg = "同姓同名の患者。";
                    samePatientInf.ForEach(ptInf =>
                    {
                        if (!string.IsNullOrEmpty(msg))
                        {
                            msg = msg + Environment.NewLine;
                        }

                        msg = msg + "患者番号：" + string.Format("{0,-9}", ptInf.PtNum.AsString());
                    });
                    string message = string.Format(ErrorMessage.MessageType_mEnt00020, msg);
                    return new CheckValidSamePatientOutputData(CheckValidSamePatientStatus.IsInValid, message);
                }
                else
                {
                    return new CheckValidSamePatientOutputData(CheckValidSamePatientStatus.IsValid, string.Empty);
                }

            }
            finally
            {
                _patientInforRepository.ReleaseResource();
            }
        }
    }
}
