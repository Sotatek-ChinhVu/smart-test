using Domain.Models.PatientInfor;
using UseCase.PatientInfor.SavePtKyusei;

namespace Interactor.PatientInfor;

public class SavePtKyuseiInteractor : ISavePtKyuseiInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public SavePtKyuseiInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public SavePtKyuseiOutputData Handle(SavePtKyuseiInputData inputData)
    {
        throw new NotImplementedException();
    }
}
