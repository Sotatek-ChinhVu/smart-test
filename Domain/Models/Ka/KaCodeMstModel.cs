namespace Domain.Models.Ka;

public class KaCodeMstModel
{
    public KaCodeMstModel(string receKaCd, int sortNo, string kaSname)
    {
        ReceKaCd = receKaCd;
        SortNo = sortNo;
        KaSname = kaSname;
    }

    public string ReceKaCd { get; private set; }
    public int SortNo { get; private set; }
    public string KaSname { get; private set; }
}
