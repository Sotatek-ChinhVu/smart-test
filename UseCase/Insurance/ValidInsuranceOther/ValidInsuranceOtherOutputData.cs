using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidPatternOther
{
    public class ValidInsuranceOtherOutputData : IOutputData
    {
        public bool Result { get; private set; }

        public string Message { get; private set; }

        public int TypeMessage { get; private set; }

        public ValidInsuranceOtherStatus Status { get; private set; }

        public ValidInsuranceOtherOutputData(bool result, string message, int typeMessage, ValidInsuranceOtherStatus status)
        {
            Result = result;
            Message = message;
            TypeMessage = typeMessage;
            Status = status;
        }
    }
}
