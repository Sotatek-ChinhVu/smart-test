using UseCase.Accounting.GetAccountingHeader;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingHeaderResponse
    {
        public GetAccountingHeaderResponse(int personNumber, List<HeaderDto> headerDtos)
        {
            PersonNumber = personNumber;
            HeaderDtos = headerDtos;
        }

        public int PersonNumber { get; private set; }
        public List<HeaderDto> HeaderDtos { get; private set; }
    }
}
