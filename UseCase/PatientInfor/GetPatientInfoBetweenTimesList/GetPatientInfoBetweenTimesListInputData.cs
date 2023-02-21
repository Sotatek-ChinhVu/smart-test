using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetPatientInfoBetweenTimesList;

public class GetPatientInfoBetweenTimesListInputData : IInputData<GetPatientInfoBetweenTimesListOutputData>
{
    public GetPatientInfoBetweenTimesListInputData(int hpId, int sinYm, int startDateD, int startTimeH, int startTimeM, int endDateD, int endTimeH, int endTimeM)
    {
        HpId = hpId;
        SinYm = sinYm;
        StartDateD = startDateD;
        StartTimeH = startTimeH;
        StartTimeM = startTimeM;
        EndDateD = endDateD;
        EndTimeH = endTimeH;
        EndTimeM = endTimeM;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public int StartDateD { get; private set; }

    public int StartTimeH { get; private set; }

    public int StartTimeM { get; private set; }

    public int EndDateD { get; private set; }

    public int EndTimeH { get; private set; }

    public int EndTimeM { get; private set; }
}
