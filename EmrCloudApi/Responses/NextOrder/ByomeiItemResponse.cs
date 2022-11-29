using static Helper.Constants.RsvkrtByomeiConst;

namespace EmrCloudApi.Responses.NextOrder
{
    public class ByomeiItemResponse
    {
        public ByomeiItemResponse(int nextOrderPosition, List<ByomeiValidationItemResponse> validationByomeis)
        {
            NextOrderPosition = nextOrderPosition;
            ValidationByomeis = validationByomeis;
        }

        public int NextOrderPosition { get; private set; }
        public List<ByomeiValidationItemResponse> ValidationByomeis { get; private set; }
    }

    public class ByomeiValidationItemResponse
    {
        public ByomeiValidationItemResponse(RsvkrtByomeiStatus status, int position, string validationMessage)
        {
            Status = status;
            Position = position;
            ValidationMessage = validationMessage;
        }

        public RsvkrtByomeiStatus Status { get; private set; }
        public int Position { get; private set; }
        public string ValidationMessage { get; private set; }
    }
}
