using Helper.Constants;

namespace EmrCloudApi.Tenant.Responses.KarteInf
{
    public class ValidationKarteInfListItemResponse
    {
        public ValidationKarteInfListItemResponse(int position, TodayKarteConst.TodayKarteValidationStatus status, string validationMessage)
        {
            Status = status;
            Position = position;
            ValidationMessage = validationMessage;
        }

        public TodayKarteConst.TodayKarteValidationStatus Status { get; private set; }
        public string ValidationMessage { get; private set; }
        public int Position { get; private set; }
    }
}
