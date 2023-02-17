using Domain.Models.Receipt;

namespace UseCase.Receipt;

public class SyoukiKbnMstItem
{
    public SyoukiKbnMstItem(SyoukiKbnMstModel model)
    {
        SyoukiKbn = model.SyoukiKbn;
        StartYm = model.StartYm;
        EndYm = model.EndYm;
        Name = model.Name;
    }

    public int SyoukiKbn { get; private set; }

    public int StartYm { get; private set; }

    public int EndYm { get; private set; }

    public string Name { get; private set; }
}
