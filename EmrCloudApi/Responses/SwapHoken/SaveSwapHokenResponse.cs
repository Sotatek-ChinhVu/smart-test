using UseCase.SwapHoken.Save;

namespace EmrCloudApi.Responses.SwapHoken
{
    public class SaveSwapHokenResponse
    {
        public SaveSwapHokenResponse(SaveSwapHokenStatus state, string message, int typeMessage)
        {
            State = state;
            Message = message;
            TypeMessage = typeMessage;
        }

        public SaveSwapHokenStatus State { get; private set; }

        public string Message { get; private set; }

        public int TypeMessage { get; private set; }
    }
}