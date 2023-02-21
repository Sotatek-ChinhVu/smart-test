namespace Domain.Models.Receipt;

public class SyoukiKbnMstModel
{
    public SyoukiKbnMstModel(int syoukiKbn, int startYm)
    {
        SyoukiKbn = syoukiKbn;
        StartYm = startYm;
        EndYm = 0;
        Name = string.Empty;
    }

    public SyoukiKbnMstModel(int syoukiKbn, int startYm, int endYm, string name)
    {
        SyoukiKbn = syoukiKbn;
        StartYm = startYm;
        EndYm = endYm;
        Name = name;
    }

    public int SyoukiKbn { get; private set; }

    public int StartYm { get; private set; }

    public int EndYm { get; private set; }

    public string Name { get; private set; }
}
