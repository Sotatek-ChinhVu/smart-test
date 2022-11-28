using static Helper.Constants.RsvkrtByomeiConst;

namespace EmrCloudApi.Responses.NextOrder
{
    public class UpsertByomeiItemResponse
    {
        public UpsertByomeiItemResponse(int nextOrderPosition, List<UpsertByomeiValidationItemResponse> validationByomeis)
        {
            NextOrderPosition = nextOrderPosition;
            ValidationByomeis = validationByomeis;
        }

        public int NextOrderPosition { get; private set; }
        public List<UpsertByomeiValidationItemResponse> ValidationByomeis { get; private set; }
    }

    public class UpsertByomeiValidationItemResponse
    {
        public UpsertByomeiValidationItemResponse(RsvkrtByomeiStatus status, int position, string validationMessage)
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
