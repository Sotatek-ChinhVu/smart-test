using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidKohi
{
    public class ValidKohiOutputData : IOutputData
    {
        public bool Result { get; private set; }

        public string Message { get; private set; }

        public int TypeMessage { get; private set; }

        public ValidKohiStatus Status { get; private set; }

        public ValidKohiOutputData(bool result, string message, int typeMessage, ValidKohiStatus status)
        {
            Result = result;
            Message = message;
            TypeMessage = typeMessage;
            Status = status;
        }
    }
}
