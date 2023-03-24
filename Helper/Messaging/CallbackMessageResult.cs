namespace Helper.Messaging
{
    public class CallbackMessageResult<T>
    {
        public CallbackMessageResult(bool success, T result)
        {
            Success = success;
            Result = result;
        }

        public bool Success { get; private set; }
        public T Result { get; private set; }
    }
}
