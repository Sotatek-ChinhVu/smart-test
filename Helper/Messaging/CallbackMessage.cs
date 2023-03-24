namespace Helper.Messaging
{
    public abstract class CallbackMessage<T>
    {
        public bool IsHandled { get; private set; }

        private readonly TaskCompletionSource<CallbackMessageResult<T>> _taskSource = new TaskCompletionSource<CallbackMessageResult<T>>();

        /// <summary>Gets the task to await for the callback call. </summary>
        public Task<CallbackMessageResult<T>> Task
        {
            get { return _taskSource.Task; }
        }

        /// <summary>Gets or sets the callback which is called when the processing of the message was successful. </summary>
        public Action<T> SuccessCallback { get; set; }

        /// <summary>Gets or sets the callback which is called when the processing of the message failed. </summary>
        public Action<T> FailCallback { get; set; }

        /// <summary>Calls the success callback. </summary>
        public void CallSuccessCallback(T result)
        {
            try
            {
                if (SuccessCallback != null)
                    SuccessCallback(result);

                _taskSource.SetResult(new CallbackMessageResult<T>(true, result));
            }
            finally
            {
                IsHandled = true;
            }
        }

        /// <summary>Calls the fail callback. </summary>
        public void CallFailCallback(T result)
        {
            try
            {
                if (FailCallback != null)
                    FailCallback(result);

                _taskSource.SetResult(new CallbackMessageResult<T>(false, result));
            }
            finally
            {
                IsHandled = true;
            }
        }
    }
}
