using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.CheckExisReceInfEdit
{
    public class CheckExisReceInfEditOutputData : IOutputData
    {
        public CheckExisReceInfEditOutputData(CheckExisReceInfEditStatus status, bool isExisted)
        {
            Status = status;
            IsExisted = isExisted;
        }

        public CheckExisReceInfEditStatus Status { get; private set; }

        public bool IsExisted { get; private set; }
    }
}
