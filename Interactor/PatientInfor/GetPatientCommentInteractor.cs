using Domain.Models.PatientInfor;
using UseCase.PatientInfor.PatientComment;

namespace Interactor.PatientInfor
{
    public class GetPatientCommentInteractor : IGetPatientCommentInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public GetPatientCommentInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public GetPatientCommentOutputData Handle(GetPatientCommentInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetPatientCommentOutputData(GetPatientCommentStatus.InvalidHpId);

                if (inputData.PtId <= 0)
                    return new GetPatientCommentOutputData(GetPatientCommentStatus.InvalidPtId);

                var data = _patientInforRepository.PatientCommentModels(inputData.HpId, inputData.PtId);

                return new GetPatientCommentOutputData(data, GetPatientCommentStatus.Success);
            }
            catch (Exception)
            {
                return new GetPatientCommentOutputData(GetPatientCommentStatus.Failed);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
            }
        }
    }
}
