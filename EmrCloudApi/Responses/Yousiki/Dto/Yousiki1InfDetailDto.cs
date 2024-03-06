using Domain.Models.Yousiki;

namespace EmrCloudApi.Responses.Yousiki.Dto;

public class Yousiki1InfDetailDto
{
    public Yousiki1InfDetailDto(Yousiki1InfDetailModel model)
    {
        PtId = model.PtId;
        SinYm = model.SinYm;
        DataType = model.DataType;
        SeqNo = model.SeqNo;
        CodeNo = model.CodeNo;
        RowNo = model.RowNo;
        Payload = model.Payload;
        Value = model.Value;
    }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int DataType { get; private set; }

    public int SeqNo { get; private set; }

    public string CodeNo { get; private set; }

    public int RowNo { get; private set; }

    public int Payload { get; private set; }

    public string Value { get; private set; }
}
