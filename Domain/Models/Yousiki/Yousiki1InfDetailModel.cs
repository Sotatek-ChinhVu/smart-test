namespace Domain.Models.Yousiki;

public class Yousiki1InfDetailModel
{
    public Yousiki1InfDetailModel(long ptId, int sinYm, int dataType, int seqNo, string codeNo, int rowNo, int newRowNo, int payload, string value)
    {
        PtId = ptId;
        SinYm = sinYm;
        DataType = dataType;
        SeqNo = seqNo;
        CodeNo = codeNo;
        RowNo = rowNo;
        NewRowNo = newRowNo;
        Payload = payload;
        Value = value;
    }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int DataType { get; private set; }

    public int SeqNo { get; private set; }

    public string CodeNo { get; private set; }

    public int RowNo { get; private set; }

    public int NewRowNo { get; private set; }

    public int Payload { get; private set; }

    public string Value { get; private set; }
}
