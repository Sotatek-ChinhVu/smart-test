using UseCase.PatientManagement.SaveStaConf;

namespace EmrCloudApi.Responses.PatientManagement
{
    public class SaveStaConfMenuResponse
    {
        public SaveStaConfMenuResponse(SaveStaConfMenuStatus status)
        {
            Status = status;
        }

        public SaveStaConfMenuStatus Status { get; private set; }
    }
}
