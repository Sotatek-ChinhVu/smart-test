using Domain.Models.PatientInfor;
using UseCase.PatientInfor.GetPatientInfoBetweenTimesList;

namespace Interactor.PatientInfor;

public class GetPatientInfoBetweenTimesListInteractor : IGetPatientInfoBetweenTimesListInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;

    public GetPatientInfoBetweenTimesListInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public GetPatientInfoBetweenTimesListOutputData Handle(GetPatientInfoBetweenTimesListInputData inputData)
    {
        try
        {
            int startDate = inputData.SinYm * 100 + inputData.StartDateD;
            int endDate = inputData.SinYm * 100 + inputData.EndDateD;
            string startTime = inputData.StartTimeH.ToString() + inputData.StartTimeM.ToString().PadLeft(2, '0');
            string endTime = inputData.EndTimeH.ToString() + inputData.EndTimeM.ToString().PadLeft(2, '0');
            var ptInfList = _patientInforRepository.SearchPatient(inputData.HpId, startDate, startTime, endDate, endTime)
                                                   .Select(item => new PatientInfoOutputItem(
                                                                       item.PtId,
                                                                       item.PtNum,
                                                                       item.KanaName,
                                                                       item.Name,
                                                                       item.Sex))
                                                   .ToList();

            return new GetPatientInfoBetweenTimesListOutputData(ptInfList, GetPatientInfoBetweenTimesListStatus.Success);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }
}
