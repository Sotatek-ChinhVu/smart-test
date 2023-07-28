namespace EmrCloudApi.Responses.Receipt
{
    public class CheckExisReceInfEditResponse
    {
        public CheckExisReceInfEditResponse(bool receInfEdit)
        {
            ReceInfEdit = receInfEdit;
        }

        public bool ReceInfEdit { get; private set; }
    }
}
