using UseCase.SwapHoken.Save;

namespace EmrCloudApi.Responses.SwapHoken
{
    public class SaveSwapHokenResponse
    {
        public SaveSwapHokenResponse(SaveSwapHokenStatus state, string message, int typeMessage, List<int> seikyuYm)
        {
            State = state;
            Message = message;
            TypeMessage = typeMessage;
            SeikyuYm = seikyuYm;
        }

        public SaveSwapHokenStatus State { get; private set; }

        public string Message { get; private set; }

        public int TypeMessage { get; private set; }

        public List<int> SeikyuYm { get; private set; }
    }
}