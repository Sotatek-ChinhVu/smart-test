namespace UseCase.SetMst.ReorderSetMstList;

public class ReorderSetMstInputItem
{
    public ReorderSetMstInputItem(int hpId, int setCd, int level1, int level2, int level3)
    {
        HpId = hpId;
        SetCd = setCd;
        Level1 = level1;
        Level2 = level2;
        Level3 = level3;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
    public int Level1 { get; private set; }
    public int Level2 { get; private set; }
    public int Level3 { get; private set; }
}
