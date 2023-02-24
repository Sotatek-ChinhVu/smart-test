namespace UseCase.SystemConf.GetSystemConfList;

public class GetSystemConfListInputItem
{
    public GetSystemConfListInputItem(int grpCd, int grpEdaNo)
    {
        GrpCd = grpCd;
        GrpEdaNo = grpEdaNo;
    }

    public int GrpCd { get;private set; }

    public int GrpEdaNo { get; private set; }
}
