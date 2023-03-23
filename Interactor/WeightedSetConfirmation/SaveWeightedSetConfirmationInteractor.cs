using Domain.Models.SpecialNote.PatientInfo;
using UseCase.WeightedSetConfirmation.Save;

namespace Interactor.WeightedSetConfirmation
{
    public class SaveWeightedSetConfirmationInteractor : ISaveWeightedSetConfirmationInputPort
    {
        private readonly IPatientInfoRepository _patientInfoRepository;

        public SaveWeightedSetConfirmationInteractor(IPatientInfoRepository patientInfoRepository) => _patientInfoRepository = patientInfoRepository;

        public SaveWeightedSetConfirmationOutputData Handle(SaveWeightedSetConfirmationInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new SaveWeightedSetConfirmationOutputData(SaveWeightedSetConfirmationStatus.InvalidHpId);

                if (inputData.PtId <= 0)
                    return new SaveWeightedSetConfirmationOutputData(SaveWeightedSetConfirmationStatus.InvalidPtId);

                if(inputData.RaiinNo <= 0)
                    return new SaveWeightedSetConfirmationOutputData(SaveWeightedSetConfirmationStatus.InvalidRaiinNo);

                if(inputData.SinDate <= 0)
                    return new SaveWeightedSetConfirmationOutputData(SaveWeightedSetConfirmationStatus.InvalidSinDate);

                bool result = _patientInfoRepository.SaveKensaInfWeightedConfirmation(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.Weithgt, inputData.SinDate, inputData.UserId);
                if(result)
                    return new SaveWeightedSetConfirmationOutputData(SaveWeightedSetConfirmationStatus.Successful);
                else
                    return new SaveWeightedSetConfirmationOutputData(SaveWeightedSetConfirmationStatus.Failed);
            }
            finally
            {
                _patientInfoRepository.ReleaseResource();
            }
        }
    }
}
