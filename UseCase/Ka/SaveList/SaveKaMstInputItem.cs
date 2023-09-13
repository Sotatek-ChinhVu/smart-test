namespace UseCase.Ka.SaveList;

public class SaveKaMstInputItem
{
    public SaveKaMstInputItem(long id, int kaId, string receKaCd, string kaSname, string kaName, string yousikiKaCd)
    {
        Id = id;
        KaId = kaId;
        ReceKaCd = receKaCd;
        KaSname = kaSname;
        KaName = kaName;
        YousikiKaCd = yousikiKaCd;
    }

    public long Id { get; private set; }
    public int KaId { get; private set; }
    public string ReceKaCd { get; private set; }
    public string KaSname { get; private set; }
    public string KaName { get; private set; }
    public string YousikiKaCd { get; private set; }
}
