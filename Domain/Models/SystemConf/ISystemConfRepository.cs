namespace Domain.Models.SystemConf;

public interface ISystemConfRepository
{
    List<SystemConfModel> GetList(int fromGrpCd, int toGrpCd);
}
