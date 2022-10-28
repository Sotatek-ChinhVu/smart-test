using UseCase.SwapHoken.Save;

namespace EmrCloudApi.Tenant.Responses.SwapHoken
{
    public class SaveSwapHokenResponse
    {
        public SaveSwapHokenResponse(SaveSwapHokenStatus state)
        {
            State = state;
        }

        public SaveSwapHokenStatus State { get; private set; }
    }
}