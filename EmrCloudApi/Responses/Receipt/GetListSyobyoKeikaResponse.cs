using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetListSyobyoKeikaResponse
{
    public GetListSyobyoKeikaResponse(List<SyobyoKeikaItem> listSyobyoKeika)
    {
        ListSyobyoKeika = listSyobyoKeika.Select(item => new SyobyoKeikaDto(item)).ToList();
    }

    public List<SyobyoKeikaDto> ListSyobyoKeika { get; private set; }
}
