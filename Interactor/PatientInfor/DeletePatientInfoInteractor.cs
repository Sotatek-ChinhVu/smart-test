using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Helper.Constants;
using UseCase.PatientInfor.DeletePatient;

namespace Interactor.PatientInfor
{
    public class DeletePatientInfoInteractor : IDeletePatientInfoInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IReceptionRepository _receptionRepository;
        public DeletePatientInfoInteractor(IPatientInforRepository patientInforRepository, IReceptionRepository receptionRepository)
        {
            _patientInforRepository = patientInforRepository;
            _receptionRepository = receptionRepository;
        }

        public DeletePatientInfoOutputData Handle(DeletePatientInfoInputData inputData)
        {
            if (inputData.PtId < 0)
                return new DeletePatientInfoOutputData(DeletePatientInfoStatus.InvalidPtId);

            if (inputData.HpId < 0)
                return new DeletePatientInfoOutputData(DeletePatientInfoStatus.InvalidHpId);

            if(!_patientInforRepository.IsAllowDeletePatient(inputData.HpId, inputData.PtId))
                return new DeletePatientInfoOutputData(DeletePatientInfoStatus.NotAllowDeletePatient);

            try
            {
                bool result = _patientInforRepository.DeletePatientInfo(inputData.PtId, inputData.HpId, inputData.UserId);

                if (result)
                {
                    var receptionInfos = _receptionRepository.GetList(inputData.HpId, 0, CommonConstants.InvalidId, inputData.PtId, true);
                    var sameVisitList = _receptionRepository.GetListSameVisit(inputData.HpId, inputData.PtId, 0);
                    return new DeletePatientInfoOutputData(DeletePatientInfoStatus.Successful, receptionInfos, sameVisitList);
                }
                else
                    return new DeletePatientInfoOutputData(DeletePatientInfoStatus.Failed);
            }
            catch
            {
                return new DeletePatientInfoOutputData(DeletePatientInfoStatus.Failed);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
            }
        }
    }
}
