using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class SaveSyoukiInfListResponse
{
    public SaveSyoukiInfListResponse(bool status, List<SyoukiInfItem> syoukiInfInvalidList)
    {
        SyoukiInfInvalidList = syoukiInfInvalidList.Select(item => new SyoukiInfDto(item)).ToList();
        Status = status;
    }

    public bool Status { get; private set; }

    public List<SyoukiInfDto> SyoukiInfInvalidList { get; private set; }
}
