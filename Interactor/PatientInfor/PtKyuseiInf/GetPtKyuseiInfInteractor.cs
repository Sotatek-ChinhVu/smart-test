using Domain.Models.PatientInfor;
using UseCase.PatientInfor.PtKyuseiInf.GetList;

namespace Interactor.PatientInfor.PtKyuseiInf
{
    public class GetPtKyuseiInfInteractor : IGetPtKyuseiInfInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public GetPtKyuseiInfInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public GetPtKyuseiInfOutputData Handle(GetPtKyuseiInfInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetPtKyuseiInfOutputData(new List<PtKyuseiInfModel>(), GetPtKyuseiInfStatus.InValidHpId);

                if (inputData.PtId <= 0)
                    return new GetPtKyuseiInfOutputData(new List<PtKyuseiInfModel>(), GetPtKyuseiInfStatus.InValidPtId);

                var listPtKyuseiInf = _patientInforRepository.PtKyuseiInfModels(inputData.HpId, inputData.PtId, inputData.IsDeleted);

                if (!listPtKyuseiInf.Any())
                    return new GetPtKyuseiInfOutputData(new List<PtKyuseiInfModel>(), GetPtKyuseiInfStatus.NoData);

                return new GetPtKyuseiInfOutputData(listPtKyuseiInf, GetPtKyuseiInfStatus.Success);
            }
            catch (Exception)
            {
                return new GetPtKyuseiInfOutputData(new List<PtKyuseiInfModel>(), GetPtKyuseiInfStatus.Failed);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
            }
        }
    }
}
