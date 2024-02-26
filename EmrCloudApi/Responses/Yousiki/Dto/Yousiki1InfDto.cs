using Domain.Models.Yousiki;
using Domain.Models.Yousiki.CommonModel;
using Newtonsoft.Json;

namespace EmrCloudApi.Responses.Yousiki.Dto;

public class Yousiki1InfDto
{
    public Yousiki1InfDto(Yousiki1InfModel model, TabYousikiModel tabYousiki)
    {
        PtNum = model.PtNum;
        Name = model.Name;
        PtId = model.PtId;
        SinYm = model.SinYm;
        DataType = model.DataType;
        Status = model.Status;
        StatusDic = model.StatusDic;
        SeqNo = model.SeqNo;
        TabYousiki = tabYousiki;
    }

    [JsonConstructor]
    public Yousiki1InfDto(Yousiki1InfModel model)
    {
        PtNum = model.PtNum;
        Name = model.Name;
        PtId = model.PtId;
        SinYm = model.SinYm;
        DataType = model.DataType;
        Status = model.Status;
        StatusDic = model.StatusDic;
        SeqNo = model.SeqNo;
        TabYousiki = null;
    }

    public long PtNum { get; private set; }

    public string Name { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int DataType { get; private set; }

    public int Status { get; private set; }

    public Dictionary<int, int> StatusDic { get; private set; }

    public int SeqNo { get; private set; }

    public TabYousikiModel? TabYousiki { get; private set; }
}
