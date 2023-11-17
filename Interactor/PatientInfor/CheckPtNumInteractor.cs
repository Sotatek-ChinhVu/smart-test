using Domain.Models.PatientInfor;
using UseCase.PatientInfor.CheckPtNum;
using UseCase.PatientInfor.Save;

namespace Interactor.PatientInfor
{
    public class CheckPtNumInteractor : ICheckPtNumInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private const byte retryNumber = 50;

        public CheckPtNumInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public CheckPtNumOutputData Handle(CheckPtNumInputData inputData)
        {

            try
            {
                long ptNum = 0;
                var count = 0;
                while (ptNum  == 0 && count < retryNumber)
                {
                    ptNum = _patientInforRepository.CheckInsertPtNum(inputData.HpId, inputData.PtNum);
                    count++;
                }

                return new CheckPtNumOutputData(ptNum);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
                GC.Collect();

                // Wait for all finalizers to complete before continuing.
                GC.WaitForPendingFinalizers();fi
            }
        }


    }
}