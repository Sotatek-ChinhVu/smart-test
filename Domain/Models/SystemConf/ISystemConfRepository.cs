namespace Domain.Models.SystemConf;

public interface ISystemConfRepository
{
    List<SystemConfModel> GetList(int fromGrpCd, int toGrpCd);

    SystemConfModel GetByGrpCd(int hpId, int grpCd);

    double GetSettingValue(int groupCd, int grpEdaNo, int hpId);

    string GetSettingParams(int groupCd, int grpEdaNo, int hpId);
}
