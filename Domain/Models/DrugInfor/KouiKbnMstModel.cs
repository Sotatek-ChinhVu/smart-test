namespace Domain.Models.DrugInfor;

public class KouiKbnMstModel
{
    public KouiKbnMstModel(int kouiKbnId, int kouiKbn1, int kouiKbn2, string kouiName, int excKouiKbn, int oyaKouiKbnId)
    {
        KouiKbnId = kouiKbnId;
        KouiKbn1 = kouiKbn1;
        KouiKbn2 = kouiKbn2;
        KouiName = kouiName;
        ExcKouiKbn = excKouiKbn;
        OyaKouiKbnId = oyaKouiKbnId;
    }

    public int KouiKbnId { get; private set; }

    public int KouiKbn1 { get; private set; }

    public int KouiKbn2 { get; private set; }

    public string KouiName { get; private set; }

    public int ExcKouiKbn { get; private set; }

    public int OyaKouiKbnId { get; private set; }
}
