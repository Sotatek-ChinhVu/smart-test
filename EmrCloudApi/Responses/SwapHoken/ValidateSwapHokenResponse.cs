using UseCase.SwapHoken.Validate;

namespace EmrCloudApi.Responses.SwapHoken
{
    public class ValidateSwapHokenResponse
    {
        public ValidateSwapHokenResponse(ValidateSwapHokenStatus state)
        {
            State = state;
        }

        public ValidateSwapHokenStatus State { get; private set; }
    }
}
