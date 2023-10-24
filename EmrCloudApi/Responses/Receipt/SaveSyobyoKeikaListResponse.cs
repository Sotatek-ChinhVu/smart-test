using EmrCloudApi.Responses.Receipt.Dto;
using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt;

public class SaveSyobyoKeikaListResponse
{
    public SaveSyobyoKeikaListResponse(bool status, List<SyobyoKeikaItem> syobyoKeikaInvalidList)
    {
        Status = status;
        SyobyoKeikaInvalidList = syobyoKeikaInvalidList.Select(item => new SyobyoKeikaDto(item)).ToList();
    }

    public bool Status { get; private set; }

    public List<SyobyoKeikaDto> SyobyoKeikaInvalidList { get; private set; }
}
