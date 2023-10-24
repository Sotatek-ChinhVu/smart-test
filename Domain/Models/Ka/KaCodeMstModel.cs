namespace Domain.Models.Ka;

public class KaCodeMstModel
{
    public KaCodeMstModel(string receKaCd, int sortNo, string kaSname, string receYousikiKaCd)
    {
        ReceKaCd = receKaCd;
        SortNo = sortNo;
        KaSname = kaSname;
        ReceYousikiKaCd = receYousikiKaCd;
    }

    public string ReceKaCd { get; private set; }

    public int SortNo { get; private set; }

    public string KaSname { get; private set; }

    public string ReceYousikiKaCd { get; private set; }
}
