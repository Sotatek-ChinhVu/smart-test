using UseCase.SpecialNote.AddAlrgyDrugList;

namespace EmrCloudApi.Tenant.Responses.SpecialNote
{
    public class AddAlrgyDrugListItemResponse
    {
        public AddAlrgyDrugListItemResponse(AddAlrgyDrugListStatus status, int position, string validationMessage)
        {
            Status = status;
            Position = position;
            ValidationMessage = validationMessage;
        }

        public AddAlrgyDrugListStatus Status { get; private set; }
        public int Position { get; private set; }
        public string ValidationMessage { get; private set; }
    }
}
