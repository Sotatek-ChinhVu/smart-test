namespace Domain.Models.UserConf;

public interface IUserConfRepository
{
    List<UserConfModel> GetList(int userId, int fromGrpCd, int toGrpCd);
}
