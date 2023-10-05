using Domain.Models.MainMenu;
using Helper.Constants;
using UseCase.Core.Sync.Core;

namespace UseCase.Online.SaveOnlineConfirmation
{
    public class SaveOnlineConfirmationInputData : IInputData<SaveOnlineConfirmationOutputData>
    {
        public SaveOnlineConfirmationInputData(int userId, QualificationInfModel qualificationInf, ModelStatus modelStatus)
        {
            UserId = userId;
            QualificationInf = qualificationInf;
            ModelStatus = modelStatus;
        }
        public int UserId { get; set; }

        public QualificationInfModel QualificationInf { get; private set; }

        public ModelStatus ModelStatus { get; private set; }
    }
}
