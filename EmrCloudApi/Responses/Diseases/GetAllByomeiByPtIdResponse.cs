using Domain.Models.Diseases;

namespace EmrCloudApi.Responses.Diseases;

public class GetAllByomeiByPtIdResponse
{
    public GetAllByomeiByPtIdResponse(List<PtDiseaseModel> byomeiList)
    {
        ByomeiList = byomeiList.Select(item => new ByomeiByPtIdDto(item)).ToList();
    }

    public List<ByomeiByPtIdDto> ByomeiList { get; private set; }
}
