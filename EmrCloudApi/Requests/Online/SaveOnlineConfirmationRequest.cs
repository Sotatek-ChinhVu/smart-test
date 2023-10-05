using Domain.Models.MainMenu;
using Helper.Constants;

namespace EmrCloudApi.Requests.Online
{
    public class SaveOnlineConfirmationRequest
    {
        public QualificationInfModel QualificationInf { get; set; } = new();

        public ModelStatus ModelStatus { get; set; } = ModelStatus.None;
    }
}
