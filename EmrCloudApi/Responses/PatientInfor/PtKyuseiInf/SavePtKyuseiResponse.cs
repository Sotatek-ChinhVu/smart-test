namespace EmrCloudApi.Responses.PatientInfor.PtKyuseiInf;

public class SavePtKyuseiResponse
{
    public SavePtKyuseiResponse(bool successd)
    {
        Successd = successd;
    }

    public bool Successd { get; private set; }
}
