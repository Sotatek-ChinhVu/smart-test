using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetSyobyoKeikaListResponse
{
    public GetSyobyoKeikaListResponse(List<SyobyoKeikaItem> syobyoKeikaList)
    {
        SyobyoKeikaList = syobyoKeikaList.Select(item => new SyobyoKeikaDto(item)).ToList();
    }

    public List<SyobyoKeikaDto> SyobyoKeikaList { get; private set; }
}
