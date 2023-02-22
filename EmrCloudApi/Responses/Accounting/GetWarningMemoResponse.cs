using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetWarningMemoResponse
    {
        public GetWarningMemoResponse(List<WarningMemoDto> warningMemoDtos)
        {
            WarningMemoDtos = warningMemoDtos;
        }

        public List<WarningMemoDto> WarningMemoDtos { get; private set; }
    }
}
