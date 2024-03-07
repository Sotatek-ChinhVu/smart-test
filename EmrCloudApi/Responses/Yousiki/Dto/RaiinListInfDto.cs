using Domain.Models.Yousiki;

namespace EmrCloudApi.Responses.Yousiki.Dto;

public class RaiinListInfDto
{
    public RaiinListInfDto(RaiinListInfModel model)
    {
        PtId = model.PtId;
        SinDate = model.SinDate;
        RaiinNo = model.RaiinNo;
        GrpId = model.GrpId;
        GrpName = model.GrpName;
        KbnCd = model.KbnCd;
        KbnName = model.KbnName;
        ColorCd = model.ColorCd;
        IsContainsFile = model.IsContainsFile;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int GrpId { get; private set; }

    public string GrpName { get; private set; }

    public int KbnCd { get; private set; }

    public string KbnName { get; private set; }

    public string ColorCd { get; private set; }

    public bool IsContainsFile { get; private set; }
}
