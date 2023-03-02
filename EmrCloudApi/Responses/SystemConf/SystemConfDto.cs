using Domain.Models.SystemConf;

namespace EmrCloudApi.Responses.SystemConf;

public class SystemConfDto
{
    public SystemConfDto(SystemConfModel model)
    {
        GrpCd = model.GrpCd;
        GrpEdaNo = model.GrpEdaNo;
        Val = model.Val;
        Param = model.Param;
        Biko = model.Biko;
    }

    public int GrpCd { get; private set; }

    public int GrpEdaNo { get; private set; }

    public double Val { get; private set; }

    public string Param { get; private set; }

    public string Biko { get; private set; }
}
