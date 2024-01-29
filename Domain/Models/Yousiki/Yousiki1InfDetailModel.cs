namespace Domain.Models.Yousiki;

public class Yousiki1InfDetailModel
{
    public Yousiki1InfDetailModel(long ptId, int sinYm, int dataType, int seqNo, string codeNo, int rowNo, int payload, string value)
    {
        PtId = ptId;
        SinYm = sinYm;
        DataType = dataType;
        SeqNo = seqNo;
        CodeNo = codeNo;
        RowNo = rowNo;
        Payload = payload;
        Value = value;
    }

    /// <summary>
    /// update value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Yousiki1InfDetailModel ChangeValue(string value)
    {
        Value = value;
        return this;
    }

    public Yousiki1InfDetailModel()
    { 
        CodeNo = string.Empty;
        Value = string.Empty;
    }

    public Yousiki1InfDetailModel(long ptId, int sinYm, int dataType, int seqNo, string codeNo, int rowNo, int payload, string value, int isDeleted)
    {
        PtId = ptId;
        SinYm = sinYm;
        DataType = dataType;
        SeqNo = seqNo;
        CodeNo = codeNo;
        RowNo = rowNo;
        Payload = payload;
        Value = value;
        IsDeleted = isDeleted;
    }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int DataType { get; private set; }

    public int SeqNo { get; private set; }

    public string CodeNo { get; private set; }

    public int RowNo { get; private set; }

    public int Payload { get; private set; }

    public string Value { get; private set; }

    public int IsDeleted {  get; private set; }
}
