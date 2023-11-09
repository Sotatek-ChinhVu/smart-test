using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Online.SaveOnlineConfirmation
{
    public class UpdateOnlineConfirmationOutputData : IOutputData
    {
        public UpdateOnlineConfirmationOutputData(List<ReceptionRowModel> receptions, UpdateOnlineConfirmationStatus status, string message)
        {
            Status = status;
            Message = message;
            Receptions = receptions;
        }

        public List<ReceptionRowModel> Receptions { get; private set; }

        public UpdateOnlineConfirmationStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
