namespace EmrCloudApi.Requests.PatientInfor;

public class GetPatientInfoBetweenTimesListRequest
{
    public int SinYm { get; set; }

    public int StartDateD { get; set; }

    public int StartTimeH { get; set; }

    public int StartTimeM { get; set; }

    public int EndDateD { get; set; }

    public int EndTimeH { get; set; }

    public int EndTimeM { get; set; }
}
