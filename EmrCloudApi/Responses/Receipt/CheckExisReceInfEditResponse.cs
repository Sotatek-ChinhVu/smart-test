using UseCase.Receipt.CheckExisReceInfEdit;

namespace EmrCloudApi.Responses.Receipt
{
    public class CheckExisReceInfEditResponse
    {
        private CheckExisReceInfEditStatus Status;

        public CheckExisReceInfEditResponse(CheckExisReceInfEditStatus status)
        {
            Status = status;
        }
    }
}
