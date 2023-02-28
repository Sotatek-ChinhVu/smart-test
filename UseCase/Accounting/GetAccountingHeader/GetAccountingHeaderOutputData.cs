using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetAccountingHeader
{
    public class GetAccountingHeaderOutputData : IOutputData
    {
        public GetAccountingHeaderOutputData(int personNumber, List<HeaderDto> headerDtos, GetAccountingHeaderStatus status)
        {
            PersonNumber = personNumber;
            HeaderDtos = headerDtos;
            Status = status;
        }

        public int PersonNumber { get; private set; }
        public List<HeaderDto> HeaderDtos { get; private set; }
        public GetAccountingHeaderStatus Status { get; private set; }
    }
}
