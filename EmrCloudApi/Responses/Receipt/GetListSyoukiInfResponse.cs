using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class GetListSyoukiInfResponse
{
    public GetListSyoukiInfResponse(List<SyoukiInfItem> listSyoukiInf, List<SyoukiKbnMstItem> listSyoukiKbnMst)
    {
        ListSyoukiInf = listSyoukiInf.Select(item => new SyoukiInfDto(item)).ToList();
        ListSyoukiKbnMst = listSyoukiKbnMst.Select(item => new SyoukiKbnMstDto(item)).ToList();
    }

    public List<SyoukiInfDto> ListSyoukiInf { get; private set; }

    public List<SyoukiKbnMstDto> ListSyoukiKbnMst { get; private set; }
}
