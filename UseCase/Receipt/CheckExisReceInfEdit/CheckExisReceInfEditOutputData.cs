using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.CheckExisReceInfEdit
{
    public class CheckExisReceInfEditOutputData : IOutputData
    {
        public CheckExisReceInfEditOutputData(CheckExisReceInfEditStatus status)
        {
            Status = status;
        }

        public CheckExisReceInfEditStatus Status { get; private set; }
    }
}
