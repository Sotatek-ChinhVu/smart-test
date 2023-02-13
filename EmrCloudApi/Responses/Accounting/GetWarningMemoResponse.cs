using Domain.Models.Accounting;
using Domain.Models.Reception;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetWarningMemoResponse
    {
        public GetWarningMemoResponse(List<WarningMemoModel> warningMemoModels, List<ReceptionDto> receptionDtos)
        {
            WarningMemoModels = warningMemoModels;
            ReceptionDtos = receptionDtos;
        }

        public List<WarningMemoModel> WarningMemoModels { get; private set; }
        public List<ReceptionDto> ReceptionDtos { get; private set; }
    }
}
