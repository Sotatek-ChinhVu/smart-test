using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidMainInsurance
{
    public class ValidMainInsuranceOutputData : IOutputData
    {
        public bool Result { get; private set; }

        public string Message { get; private set; }

        public int TypeMessage { get; private set; }

        public ValidMainInsuranceStatus Status { get; private set; }

        public ValidMainInsuranceOutputData(bool result, string message, int typeMessage, ValidMainInsuranceStatus status)
        {
            Result = result;
            Message = message;
            TypeMessage = typeMessage;
            Status = status;
        }
    }
}
