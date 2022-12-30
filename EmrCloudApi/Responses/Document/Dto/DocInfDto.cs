using Domain.Models.Document;
using Helper.Common;

namespace EmrCloudApi.Responses.Document.Dto;

public class DocInfDto
{
    public DocInfDto(DocInfModel model)
    {
        HpId = model.HpId;
        PtId = model.PtId;
        SinDate = CIUtil.SDateToShowSDate(model.SinDate);
        RaiinNo = model.RaiinNo;
        SeqNo = model.SeqNo;
        CategoryCd = model.CategoryCd;
        CategoryName = model.CategoryName;
        FileName = model.FileName;
        DisplayFileName = model.DisplayFileName;
        UpdateDate = CIUtil.GetCIDateTimeStr(model.UpdateDate);
        FileLink = model.FileLink;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public string SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int SeqNo { get; private set; }

    public int CategoryCd { get; private set; }

    public string CategoryName { get; private set; }

    public string FileName { get; private set; }

    public string DisplayFileName { get; private set; }

    public string UpdateDate { get; private set; }

    public string FileLink { get; private set; }
}
