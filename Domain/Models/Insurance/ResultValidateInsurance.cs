namespace Domain.Models.Insurance
{
    public class ResultValidateInsurance<T> 
    {
        public ResultValidateInsurance(T status, string message, int typeMessage)
        {
            Status = status;
            Message = message;
            TypeMessage = typeMessage;
        }

        public T Status { get; private set; }

        public string Message { get; set; }

        public int TypeMessage { get; private set; }
    }
}
