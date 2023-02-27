using Domain.Models.SystemConf;

namespace EmrCloudApi.Responses.SystemConf;

public class GetSystemConfListResponse
{
    public GetSystemConfListResponse(List<SystemConfModel> systemConfList)
    {
        SystemConfList = systemConfList.Select(item => new SystemConfDto(item)).ToList();
    }

    public List<SystemConfDto> SystemConfList { get; private set; }

}
