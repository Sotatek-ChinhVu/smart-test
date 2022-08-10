namespace Domain.Models.SystemConf;

public class SystemConfModel
{
    public SystemConfModel(int grpCd, int grpEdaNo,
        double val, string param, string biko)
    {
        GrpCd = grpCd;
        GrpEdaNo = grpEdaNo;
        Val = val;
        Param = param;
        Biko = biko;
    }

    public int GrpCd { get; private set; }
    public int GrpEdaNo { get; private set; }
    public double Val { get; private set; }
    public string Param { get; private set; }
    public string Biko { get; private set; }
}
