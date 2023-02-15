using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetListSyoukiInfResponse
{
    public GetListSyoukiInfResponse(List<SyoukiInfItem> syoukiInfList, List<SyoukiKbnMstItem> syoukiKbnMstList)
    {
        SyoukiInfList = syoukiInfList.Select(item => new SyoukiInfDto(item)).ToList();
        SyoukiKbnMstList = syoukiKbnMstList.Select(item => new SyoukiKbnMstDto(item)).ToList();
    }

    public List<SyoukiInfDto> SyoukiInfList { get; private set; }

    public List<SyoukiKbnMstDto> SyoukiKbnMstList { get; private set; }
}
