namespace Domain.Models.Yousiki;

public class Yousiki1InfModel
{
    public Yousiki1InfModel(long ptNum, string name, long ptId, int sinYm, int dataType, int status, Dictionary<int, int> statusDic, int seqNo, List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        PtNum = ptNum;
        Name = name;
        PtId = ptId;
        SinYm = sinYm;
        DataType = dataType;
        Status = status;
        StatusDic = statusDic;
        SeqNo = seqNo;
        Yousiki1InfDetailList = yousiki1InfDetailList;
    }

    public Yousiki1InfModel(long ptId, int sinYm, int dataType, int seqNo, int isDeleted, int status)
    {
        PtId = ptId;
        SinYm = sinYm;
        DataType = dataType;
        SeqNo = seqNo;
        IsDeleted = isDeleted;
        Status = status;
        Name = string.Empty;
        StatusDic = new();
        Yousiki1InfDetailList = new();
    }

    public Yousiki1InfModel(long ptId, int sinYm, int dataType, int seqNo, int isDeleted, int status, long ptNum, string name)
    {
        PtId = ptId;
        SinYm = sinYm;
        DataType = dataType;
        SeqNo = seqNo;
        IsDeleted = isDeleted;
        Status = status;
        PtNum = ptNum;
        Name = name;
        StatusDic = new();
        Yousiki1InfDetailList = new();
    }

    public Yousiki1InfModel ChangeStatusDic(Dictionary<int, int> statusDic)
    {
        StatusDic = statusDic;
        return this;
    }

    public long PtNum { get; private set; }

    public string Name { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int DataType { get; private set; }

    public int Status { get; private set; }

    public Dictionary<int, int> StatusDic { get; private set; }

    public int SeqNo { get; private set; }

    public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

    public int IsDeleted { get; private set; }
}
