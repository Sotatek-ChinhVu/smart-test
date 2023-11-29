using Domain.Models.TodayOdr;

namespace EmrCloudApi.Responses.LastDayInformation.Dto;

public class OdrDateInfDto
{
    public OdrDateInfDto(OdrDateInfModel model)
    {
        GrpId = model.GrpId;
        SortNo = model.SortNo;
        GrpName = model.GrpName;
        SinDateBinding = model.SinDateBinding;
        OdrDateDetailList = model.OdrDateDetailList.Select(item => new OdrDateDetailDto(item)).ToList();
    }

    public int GrpId { get; private set; }

    public int SortNo { get; private set; }

    public string GrpName { get; private set; }

    public string SinDateBinding { get; private set; }

    public List<OdrDateDetailDto> OdrDateDetailList { get; private set; }
}
