namespace Domain.Models.MstItem;

public class RenkeiPathConfModel
{
    public RenkeiPathConfModel(long id, int renkeiId, int seqNo, int edaNo, string path, string machine, int charCd, string workPath, int interval, string param, string user, string passWord, int isInvalid, string biko, bool isDeleted)
    {
        Id = id;
        RenkeiId = renkeiId;
        SeqNo = seqNo;
        EdaNo = edaNo;
        Path = path;
        Machine = machine;
        CharCd = charCd;
        WorkPath = workPath;
        Interval = interval;
        Param = param;
        User = user;
        PassWord = passWord;
        IsInvalid = isInvalid;
        Biko = biko;
        IsDeleted = isDeleted;
    }

    public long Id { get; private set; }

    public int RenkeiId { get; private set; }

    public int SeqNo { get; private set; }

    public int EdaNo { get; private set; }

    public string Path { get; private set; }

    public string Machine { get; private set; }

    public int CharCd { get; private set; }

    public string WorkPath { get; private set; }

    public int Interval { get; private set; }

    public string Param { get; private set; }

    public string User { get; private set; }

    public string PassWord { get; private set; }

    public int IsInvalid { get; private set; }

    public string Biko { get; private set; }

    public bool IsDeleted { get; private set; }

    public string PassWordDisplay
    {
        get => string.IsNullOrEmpty(PassWord) ? "" : new String('●', PassWord.Length);
    }

    public string CharCdName
    {
        get
        {
            if (CharCd == 1)
            {
                return "UTF-8";
            }

            return "Shift-JIS";
        }
    }
}
