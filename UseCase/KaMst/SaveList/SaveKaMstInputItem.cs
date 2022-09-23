namespace UseCase.KaMst.SaveList;

public class SaveKaMstInputItem
{
    public SaveKaMstInputItem(long id, int kaId, string receKaCd, string kaSname, string kaName)
    {
        Id = id;
        KaId = kaId;
        ReceKaCd = receKaCd;
        KaSname = kaSname;
        KaName = kaName;
    }

    public long Id { get; private set; }
    public int KaId { get; private set; }
    public string ReceKaCd { get; private set; }
    public string KaSname { get; private set; }
    public string KaName { get; private set; }
}
